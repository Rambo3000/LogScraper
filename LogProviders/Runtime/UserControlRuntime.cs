using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Credentials;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Utilities.Extensions;

namespace LogScraper.LogProviders.Runtime
{
    public partial class UserControlRuntimeLogProvider : UserControl, ILogProviderUserControl
    {
        private class HtmlLink : IEquatable<HtmlLink>
        {
            public string Name { get; set; }
            public string Url { get; set; }

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
                return Name;
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

        public void UpdateUri()
        {
            string uri = string.Empty;
            if( SelectedRuntimeInstance != null ) uri += $" {SelectedRuntimeInstance.Description}";
            if (SelectedFolderLink != null) uri += $"/{SelectedFolderLink.Name}";
            if (SelectedFileLink != null) uri += $"/{SelectedFileLink.Name}";

            UriChanged?.Invoke(this, uri);
        }

        public UserControlRuntimeLogProvider()
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
                sourceAdapter = SourceAdapterFactory.CreateHttpSourceAdapter(url, CredentialManager.GenerateTargetLogProvider("Runtime", runtimeInstance.Description), ConfigurationManager.GenericConfig.HttpCLientTimeOUtSeconds, runtimeInstance.HttpAuthenticationSettings, TrailType.None, null, authenticate);
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
            UpdateUri();
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
            if (TryParseHtmlLinks(html, SelectedRuntimeInstance.UrlRuntimeLog, out List<HtmlLink> links))
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

            if (TryParseHtmlLinks(html, SelectedRuntimeInstance.UrlRuntimeLog, out List<HtmlLink> links))
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
        private static bool TryParseHtmlLinks(string html, string baseUrl, out List<HtmlLink> links)
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
                        HtmlLink link = new()
                        {
                            Name = text,
                            Url = href
                        };
                        links.Add(link);
                    }
                }

                return links.Count > 0;
            }
            catch
            {
                links = null;
                return false;
            }
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
            UpdateUri();
            OnSourceValidChanged(IsSourceValid);
        }

        private void CboFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUrl.Text = string.Empty;
            if (SelectedFileLink == null) return;
            txtUrl.Text = SelectedFileLink.Url;
            OnSourceSelectionChanged(EventArgs.Empty);
            UpdateUri();
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
