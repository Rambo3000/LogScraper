using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Credentials;
using LogScraper.LogProviders;
using LogScraper.LogProviders.Runtime;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Utilities.Extensions;

namespace LogScraper.Controls.LogProviders
{
    public partial class RuntimeProviderControl : UserControl, ILogProviderUserControl
    {
        private class HtmlLink : IEquatable<HtmlLink>
        {
            public string Name { get; set; }

            /// <summary>
            /// The name used for display in the selection list. Equal to <see cref="Name"/> unless the
            /// containing runtime instance has filtering enabled, in which case the configured words have been
            /// removed to improve readability.
            /// </summary>
            public string NameFiltered { get; set; }
            public string Url { get; set; }

            /// <summary>
            /// The date/time detected near this link in the source HTML (e.g. a "last modified" column in a
            /// table row or list item). Used to restore chronological order when the server returns entries in
            /// a different order than displayed (for example because sorting only happens client-side, via
            /// JavaScript, after the page has loaded).
            /// </summary>
            public DateTime? Timestamp { get; set; }

            public bool Equals(HtmlLink other)
            {
                return other != null && GetHashCode() == other.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                return Equals(obj as HtmlLink);
            }

            public override string ToString()
            {
                return NameFiltered;
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(Name, Url);
            }
        }

        public event EventHandler SourceSelectionChanged;

        public event EventHandler<string, bool> StatusUpdate;
        public event EventHandler<string> UriChanged;
        public event EventHandler<bool> IsSourceValidChanged;

        private List<RuntimeInstance> RuntimeInstances { get; set; }

        public bool IsSourceValid
        {
            get
            {
                if (SelectedRuntimeInstance == null) return false;

                // If the runtime instance has a direct URL, it's valid
                if (!SelectedRuntimeInstance.IsUrlLinkToHtmlFolderList && !SelectedRuntimeInstance.IsUrlLinkToHtmlFileList)
                    return !string.IsNullOrWhiteSpace(SelectedRuntimeInstance.UrlRuntimeLog);

                // If it's a folder list, check for folder+file
                if (SelectedRuntimeInstance.IsUrlLinkToHtmlFolderList)
                    return SelectedFolderLink != null && SelectedFileLink != null;

                // If it's a file list, check for file
                if (SelectedRuntimeInstance.IsUrlLinkToHtmlFileList)
                    return SelectedFileLink != null;

                return false;
            }
        }

        public void UpdateAfterProviderSelected()
        {
            string uri = string.Empty;
            if( SelectedRuntimeInstance != null ) uri += $" {SelectedRuntimeInstance.Description}";
            if (SelectedFolderLink != null) uri += $"/{SelectedFolderLink.Name}";
            if (SelectedFileLink != null) uri += $"/{SelectedFileLink.Name}";

            UriChanged?.Invoke(this, uri);
            IsSourceValidChanged?.Invoke(this, IsSourceValid);
        }

        public RuntimeProviderControl()
        {
            InitializeComponent();
        }

        public void UpdateRuntimeInstances(List<RuntimeInstance> runtimeInstance)
        {
            RuntimeInstances = runtimeInstance;

            PopulateRuntimeInstances();
        }

        public ISourceAdapter GetSourceAdapter()
        {
            if (SelectedRuntimeInstance == null) throw new Exception("De runtime instantie is verplicht");

            if (SelectedRuntimeInstance.IsUrlLinkToHtmlFolderList || SelectedRuntimeInstance.IsUrlLinkToHtmlFileList)
            {
                if (SelectedFileLink == null) throw new Exception("Het log bestand is verplicht. Selecteer een bestand in de lijst.");
                return GetSourceAdapterWithSpecificUrl(SelectedRuntimeInstance, SelectedFileLink.Url);
            }
            return GetSourceAdapterWithSpecificUrl(SelectedRuntimeInstance, SelectedRuntimeInstance.UrlRuntimeLog);
        }

        private ISourceAdapter GetSourceAdapterWithSpecificUrl(RuntimeInstance runtimeInstance, string url = null, bool authenticate = true)
        {
            if (SelectedRuntimeInstance == null) throw new Exception("De runtime instantie is verplicht");

            url ??= SelectedRuntimeInstance.UrlRuntimeLog;

            ISourceAdapter sourceAdapter;
            if (Debugger.IsAttached && SelectedRuntimeInstance.Description[..4] == "Stub")
            {
                sourceAdapter = SourceAdapterFactory.CreateFileSourceAdapter(SelectedRuntimeInstance.UrlRuntimeLog);
            }
            else
            {
                sourceAdapter = SourceAdapterFactory.CreateHttpSourceAdapter(url, CredentialManager.GenerateTargetLogProvider("Runtime", runtimeInstance.Description), ConfigAppState.Instance.GenericConfig.Value.HttpCLientTimeOUtSeconds, runtimeInstance.HttpAuthenticationSettings, TrailType.None, null, authenticate);
            }
            return sourceAdapter;
        }
        private void PopulateRuntimeInstances()
        {
            cboRuntimeInstances.Items.Clear();
            CboFileList.Items.Clear();
            CboFolderList.Items.Clear();
            txtUrl.Text = string.Empty;
            UpdateFolderAndFileControls();

            if (Debugger.IsAttached)
            {
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "Stub1", UrlRuntimeLog = "Stubs/Runtime1.txt" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "Stub2", UrlRuntimeLog = "Stubs/Runtime2.txt" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "Stub3", UrlRuntimeLog = "Stubs/Runtime3.txt" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "StubJSONInvertedExample", UrlRuntimeLog = "Stubs/JSONInvertedExample.log" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "HTTP Folders", UrlRuntimeLog = "https://sharprconsultancy.nl/Folders/", IsUrlLinkToHtmlFolderList = true });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "HTTP Files", UrlRuntimeLog = "https://sharprconsultancy.nl/Files/", IsUrlLinkToHtmlFileList = true });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "HTTP Basic Authentication (authorized/password001)", UrlRuntimeLog = "https://testpages.eviltester.com/styled/auth/basic-auth-results.html" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "HTTP no such host", UrlRuntimeLog = "http://nonexistinghost/" });
            }
            else
            {
                cboRuntimeInstances.Items.AddRange([.. RuntimeInstances]);
            }
        }

        // Method to raise the custom SourceSelectionChanged event.
        protected virtual void OnSourceSelectionChanged(EventArgs e)
        {
            SourceSelectionChanged?.Invoke(this, e);
        }

        protected virtual void OnSourceValidChanged(bool isValid)
        {
            IsSourceValidChanged?.Invoke(this, isValid);
        }

        private void CboRuntimeInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            RuntimeInstance runtimeInstance = SelectedRuntimeInstance;

            if (!runtimeInstance.IsUrlLinkToHtmlFolderList && !SelectedRuntimeInstance.IsUrlLinkToHtmlFileList) txtUrl.Text = runtimeInstance.UrlRuntimeLog;

            CboFolderList.Items.Clear();
            CboFileList.Items.Clear();
            UpdateFolderAndFileControls();

            try
            {
                HttpSourceAdapter sourceAdapter = (HttpSourceAdapter)GetSourceAdapterWithSpecificUrl(runtimeInstance, runtimeInstance.UrlRuntimeLog, false);

                if (sourceAdapter.TryInitiateClientAndAuthenticate(out HttpResponseMessage httpResponseMessage, out string errorMessage))
                {
                    if (runtimeInstance.IsUrlLinkToHtmlFolderList)
                    {
                        PopulateFolders();
                    }
                    if (runtimeInstance.IsUrlLinkToHtmlFileList)
                    {
                        PopulateFiles();
                    }

                    OnStatusUpdate("Ok", true);
                }
                else
                {
                    OnStatusUpdate(errorMessage, false);
                }
                OnSourceSelectionChanged(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                OnStatusUpdate(ex.Message, false);
                ex.LogStackTraceToFile("Error selecting runtime instance");
            }
            UpdateAfterProviderSelected();
            OnSourceValidChanged(IsSourceValid);
        }
        private void PopulateFiles()
        {
            CboFileList.Items.Clear();
            UpdateFolderAndFileControls();
            txtUrl.Text = string.Empty;
            if (SelectedRuntimeInstance == null) return;

            Thread.Sleep(100);
            ISourceAdapter sourceAdapter = null;
            if (SelectedRuntimeInstance.IsUrlLinkToHtmlFolderList)
            {
                if (SelectedFolderLink == null) return;
                sourceAdapter = GetSourceAdapterWithSpecificUrl(SelectedRuntimeInstance, SelectedFolderLink.Url);
            }
            if (SelectedRuntimeInstance.IsUrlLinkToHtmlFileList)
            {
                sourceAdapter = GetSourceAdapterWithSpecificUrl(SelectedRuntimeInstance, SelectedRuntimeInstance.UrlRuntimeLog);
            }

            Thread.Sleep(100);
            string html = sourceAdapter.GetLog();
            List<string> filterUrlNameValues = SelectedRuntimeInstance.FilterUrlName ? SelectedRuntimeInstance.FilterUrlNameValues : null;
            if (TryParseHtmlLinks(html, SelectedRuntimeInstance.UrlRuntimeLog, filterUrlNameValues, out List<HtmlLink> links))
            {
                CboFileList.Items.AddRange([.. links]);
                UpdateFolderAndFileControls();
                OnStatusUpdate(string.Empty, true);
            }
            else
            {
                OnStatusUpdate("No files found in the HTML response.", false);
                return;
            }
        }
        bool PopulationFolders = false;
        private void PopulateFolders()
        {
            PopulationFolders = true;

            CboFolderList.Items.Clear();
            CboFileList.Items.Clear();
            txtUrl.Text = string.Empty;

            Thread.Sleep(100);
            ISourceAdapter sourceAdapter = GetSourceAdapterWithSpecificUrl(SelectedRuntimeInstance, SelectedRuntimeInstance.UrlRuntimeLog);

            Thread.Sleep(100);
            string html = sourceAdapter.GetLog();

            if (TryParseHtmlLinks(html, SelectedRuntimeInstance.UrlRuntimeLog, null, out List<HtmlLink> links))
            {
                CboFolderList.Items.AddRange([.. links]);
                UpdateFolderAndFileControls();
                OnStatusUpdate(string.Empty, true);
            }
            else
            {
                OnStatusUpdate("No folders found in the HTML response.", false);
                PopulationFolders = false;
                return;
            }
            PopulationFolders = false;
        }


        protected virtual void OnStatusUpdate(string message, bool isSucces)
        {
            StatusUpdate?.Invoke(message, isSucces);
        }
        private RuntimeInstance SelectedRuntimeInstance
        {
            get
            {
                if (cboRuntimeInstances.SelectedIndex == -1) return null;

                return ((RuntimeInstance)cboRuntimeInstances.SelectedItem);

            }
        }
        private HtmlLink SelectedFileLink
        {
            get
            {
                if (CboFileList.SelectedIndex == -1) return null;

                return ((HtmlLink)CboFileList.SelectedItem);

            }
        }
        private HtmlLink SelectedFolderLink
        {
            get
            {
                if (CboFolderList.SelectedIndex == -1) return null;

                return ((HtmlLink)CboFolderList.SelectedItem);

            }
        }
        private static bool TryParseHtmlLinks(string html, string baseUrl, List<string> filterUrlNameValues, out List<HtmlLink> links)
        {
            links = [];

            if (string.IsNullOrWhiteSpace(html))
            {
                return false;
            }

            try
            {
                HtmlAgilityPack.HtmlDocument doc = new();
                doc.LoadHtml(html);

                IEnumerable<HtmlAgilityPack.HtmlNode> anchorNodes = doc.DocumentNode.SelectNodes("//a");

                if (anchorNodes == null)
                {
                    return false;
                }

                foreach (HtmlAgilityPack.HtmlNode anchor in anchorNodes)
                {
                    string href = anchor.GetAttributeValue("href", null);
                    if (string.IsNullOrWhiteSpace(href) || href.StartsWith('#') || href.StartsWith("javascript:"))
                    {
                        continue;
                    }

                    // Resolve relative URLs
                    bool isHttpLink = href.Length > 4 && string.Compare("http", href[..4], true) == 0;
                    if (!isHttpLink)
                    {   try
                        {
                            href = new Uri(new Uri(baseUrl), href).ToString();
                        }
                        catch { }                        ;
                    }

                    // Skip the base URL itself
                    if (href.Equals(baseUrl, StringComparison.OrdinalIgnoreCase)) continue;

                    string text = anchor.InnerText.Trim();

                    if (!string.IsNullOrEmpty(href))
                    {
                        // Timestamp detection is a best-effort enhancement only: an unexpected failure for one
                        // row (e.g. unusual markup around that specific link) must never discard an otherwise
                        // valid link, let alone the entire result set.
                        DateTime? timestamp;
                        try
                        {
                            timestamp = TryFindRowTimestamp(anchor);
                        }
                        catch
                        {
                            timestamp = null;
                        }

                        string nameFiltered = text;
                        if (filterUrlNameValues != null && filterUrlNameValues.Count > 0)
                        {
                            foreach (string filterValue in filterUrlNameValues)
                                nameFiltered = nameFiltered.Replace(filterValue, "");
                        }

                        HtmlLink link = new()
                        {
                            Name = text,
                            NameFiltered = nameFiltered,
                            Url = href,
                            Timestamp = timestamp
                        };
                        links.Add(link);
                    }
                }

                // Many servers return entries in an arbitrary (e.g. filesystem) order and only sort them
                // chronologically client-side via JavaScript after the page has loaded. Links without a
                // detected timestamp sort after any dated ones (keeping their relative order), so this is a
                // safe no-op when no dates could be found at all.
                links = [.. links.OrderByDescending(link => link.Timestamp)];

                return links.Count > 0;
            }
            catch
            {
                links = null;
                return false;
            }
        }

        /// <summary>
        /// Maximum number of ancestor elements to inspect, above a link, when looking for a "row" container in
        /// non-tabular markup (lists, div/span based layouts). Keeps the search close to the link so unrelated
        /// dates elsewhere on the page cannot be picked up by accident.
        /// </summary>
        private const int MaxRowContainerDepth = 5;

        /// <summary>
        /// Attempts to find a date/time value associated with the row (or list item) that contains the given
        /// anchor. This is a generic, structure-agnostic alternative to parsing specific CSS classes/columns or
        /// the linked file name: it inspects the text surrounding the link - the enclosing table row, list
        /// item, or a nearby container - and tries to recognize a date/time within it.
        /// </summary>
        private static DateTime? TryFindRowTimestamp(HtmlAgilityPack.HtmlNode anchor)
        {
            // Table rows are structured enough to inspect cell-by-cell, which avoids accidentally merging
            // unrelated column text (e.g. a file size) into a single candidate string.
            HtmlAgilityPack.HtmlNode tableRow = anchor.Ancestors("tr").FirstOrDefault();
            if (tableRow != null)
            {
                HtmlAgilityPack.HtmlNodeCollection cells = tableRow.SelectNodes(".//td");
                if (cells != null)
                {
                    foreach (HtmlAgilityPack.HtmlNode cell in cells)
                    {
                        if (TryExtractTimestamp(cell.InnerText, out DateTime cellTimestamp)) return cellTimestamp;
                    }
                }
                return null;
            }

            // For non-tabular markup (lists, or div/span based rows), climb a limited number of ancestors and
            // stop as soon as a container is reached that still holds only this single link. That container is
            // the most likely "row" scope, and its combined text is searched for a date/time.
            HtmlAgilityPack.HtmlNode candidate = anchor;
            for (int depth = 0; depth < MaxRowContainerDepth && candidate.ParentNode != null; depth++)
            {
                candidate = candidate.ParentNode;
                if (candidate.Name is not ("li" or "div" or "p" or "span")) continue;

                HtmlAgilityPack.HtmlNodeCollection anchorsInCandidate = candidate.SelectNodes(".//a");
                if (anchorsInCandidate == null || anchorsInCandidate.Count > 1) break;

                if (TryExtractTimestamp(candidate.InnerText, out DateTime containerTimestamp)) return containerTimestamp;
            }

            // Classic flat listings (e.g. Apache/Nginx directory indexes) place the timestamp as plain text
            // right after the link instead of wrapping each entry in its own element.
            return TryExtractTimestamp(GetTrailingSiblingText(anchor), out DateTime trailingTimestamp) ? trailingTimestamp : null;
        }

        /// <summary>
        /// Collects the plain text that follows an anchor, up to the next link or line break, so a timestamp
        /// that was not wrapped in its own row/list element can still be recognized.
        /// </summary>
        private static string GetTrailingSiblingText(HtmlAgilityPack.HtmlNode anchor)
        {
            StringBuilder text = new();
            HtmlAgilityPack.HtmlNode sibling = anchor.NextSibling;
            while (sibling != null && sibling.Name != "a" && sibling.Name != "br")
            {
                text.Append(sibling.InnerText).Append(' ');
                sibling = sibling.NextSibling;
            }
            return text.ToString();
        }

        /// <summary>
        /// Common date/time representations that may appear next to a link in a directory or file listing
        /// (e.g. "2026-07-01T06:00:05Z", "30-Jun-2026 14:33", "Mon, 30 Jun 2026 22:00:00 GMT"). Only patterns
        /// with unambiguous separators (-, /, :, or a textual month) are matched, so plain numbers such as file
        /// sizes are never mistaken for a date.
        /// </summary>
        private static readonly Regex TimestampPattern = new(
              @"\d{4}-\d{2}-\d{2}[T ]\d{2}:\d{2}(:\d{2}(\.\d+)?)?(Z|[+-]\d{2}:?\d{2})?"
            + @"|\d{1,2}[-/][A-Za-z]{3}[-/]\d{2,4}([ T]\d{2}:\d{2}(:\d{2})?)?"
            + @"|[A-Za-z]{3},?\s+\d{1,2}\s+[A-Za-z]{3}\s+\d{4}\s+\d{2}:\d{2}:\d{2}\s+GMT"
            + @"|\d{1,2}/\d{1,2}/\d{2,4}(\s+\d{1,2}:\d{2}(:\d{2})?(\s*[AP]M)?)?",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Tries to find and parse the first recognizable date/time within the given text. Matching uses
        /// invariant culture and results are normalized to UTC so timestamps using different formats (with or
        /// without an explicit offset) can be compared consistently.
        /// </summary>
        private static bool TryExtractTimestamp(string text, out DateTime timestamp)
        {
            timestamp = default;
            if (string.IsNullOrWhiteSpace(text)) return false;

            foreach (Match match in TimestampPattern.Matches(text))
            {
                if (DateTime.TryParse(match.Value, CultureInfo.InvariantCulture,
                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out timestamp))
                {
                    return true;
                }
            }
            return false;
        }

        private void CboFolderList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedFolderLink != null && !PopulationFolders)
            {
                try
                {
                    PopulateFiles();
                }
                catch (Exception ex)
                {
                    OnStatusUpdate(ex.Message, false);
                    ex.LogStackTraceToFile("Error selecting folder link");
                }
            }
            UpdateAfterProviderSelected();
            OnSourceValidChanged(IsSourceValid);
        }

        private void CboFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUrl.Text = string.Empty;
            if (SelectedFileLink == null) return;
            txtUrl.Text = SelectedFileLink.Url;
            OnSourceSelectionChanged(EventArgs.Empty);
            UpdateAfterProviderSelected();
            OnSourceValidChanged(IsSourceValid);
        }

        private void UpdateFolderAndFileControls()
        {
            CboFolderList.Enabled = CboFolderList.Items.Count > 0;
            LblFolder.Enabled = CboFolderList.Enabled;

            CboFileList.Enabled = CboFileList.Items.Count > 0;
            LblFile.Enabled = CboFileList.Enabled;
        }
    }
}
