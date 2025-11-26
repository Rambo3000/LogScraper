using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;
using System.ComponentModel;

namespace LogScraper
{
    public partial class UserControlSearch : UserControl
    {
        //TODO: search metadata conditionally, make sure to search the timestamp
        //TODO: clean up code and comments

        public event Action<string, SearchDirectionUserControl, bool, bool, bool> Search;
        private LogMetadataFilterResult LogMetadataFilterResult;

        private SearchEvent LastSearchEvent = null;

        public enum SearchDirectionUserControl
        {
            Forward,
            Backward
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMetadataSearchEnabled { get; set; }

        /// Event to filter log entries based on the selected log content property
        public event EventHandler SelectedItemChanged;
        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            SelectedItemChanged?.Invoke(this, e);
        }

        #region Private class LogEntryDisplayObject
        private class LogEntryDisplayObject
        {
            public int Index { get; set; }
            public LogEntry OriginalLogEntry { get; set; }
            public int SearchTextStartvalue { get; set; }
            public int SearchTextAdditionalLogLineIndex { get; set; }
            public LogContentValue ContentValue { get; set; }
            public override string ToString()
            { return ContentValue != null ? ContentValue.Value : string.Empty; }
        }
        #endregion

        #region Private class SearchEvent
        private class SearchEvent : IEquatable<SearchEvent>
        {
            public LogEntry FirstLogEntry { get; set; }
            public LogEntry LastLogEntry { get; set; }
            public string SearchText { get; set; }
            public bool CaseSensitive { get; set; }
            public bool WholeWord { get; set; }
            public bool IsMetadataSearchEnabled { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as SearchEvent);
            }
            public bool Equals(SearchEvent other)
            {
                return other != null &&
                       FirstLogEntry == other.FirstLogEntry &&
                       LastLogEntry == other.LastLogEntry &&
                       SearchText == other.SearchText &&
                       CaseSensitive == other.CaseSensitive &&
                       WholeWord == other.WholeWord &&
                       IsMetadataSearchEnabled == other.IsMetadataSearchEnabled;
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(FirstLogEntry, LastLogEntry, SearchText, CaseSensitive, WholeWord);
            }
        }
        #endregion

        public UserControlSearch()
        {
            InitializeComponent();
            TxtSearch_Leave(null, null);
            ToolTip.SetToolTip(chkWholeWordsOnly, "Alleen hele woorden zoeken");
            ToolTip.SetToolTip(chkCaseSensitive, "Hoofdletter gevoelig zoeken");
            ToolTip.SetToolTip(chkWrapAround, "Zoek verder vanaf het begin");
            ToolTip.SetToolTip(btnSearchNext, "Volgende zoeken");
            ToolTip.SetToolTip(btnSearchPrevious, "Vorige zoeken");
        }
        public void UpdateLogEntries(LogMetadataFilterResult logMetadataFilterResult)
        {
            LogMetadataFilterResult = logMetadataFilterResult;

            //Do not search automatically after the log has been cleared
            if (LastSearchEvent == null || LastSearchEvent.FirstLogEntry == null) return;
            UpdateSearchResultsList();
        }
        private bool ClearSelectedLogEntryExternallyInProgress = false;
        public void ClearSelectedLogEntry()
        {
            ClearSelectedLogEntryExternallyInProgress = true;
            LstLogContent.SelectedIndex = -1;
            ClearSelectedLogEntryExternallyInProgress = false;
        }

        private void BtnSearchNext_Click(object sender, EventArgs e)
        {
            Search?.Invoke(txtSearch.Text, SearchDirectionUserControl.Forward, chkCaseSensitive.Checked, chkWholeWordsOnly.Checked, chkWrapAround.Checked);
            UpdateSearchResultsList();
            LstLogContent.MoveSelection(true, chkWrapAround.Checked);
        }

        private void BtnSearchPrevious_Click(object sender, EventArgs e)
        {
            Search?.Invoke(txtSearch.Text, SearchDirectionUserControl.Backward, chkCaseSensitive.Checked, chkWholeWordsOnly.Checked, chkWrapAround.Checked);
            LstLogContent.MoveSelection(false, chkWrapAround.Checked);
        }
        private void UpdateSearchResultsList()
        {
            if (LogMetadataFilterResult?.LogEntries == null || LogMetadataFilterResult.LogEntries.Count == 0)
            {
                Clear();
                return;
            }

            SearchEvent searchEventNew = new()
            {
                FirstLogEntry = LogMetadataFilterResult.LogEntries.FirstOrDefault(),
                LastLogEntry = LogMetadataFilterResult.LogEntries.LastOrDefault(),
                SearchText = txtSearch.Text,
                CaseSensitive = chkCaseSensitive.Checked,
                IsMetadataSearchEnabled = IsMetadataSearchEnabled,
                WholeWord = chkWholeWordsOnly.Checked
            };

            // snelcheck: exact hetzelfde zoek-event als laatst -> niets doen
            if (searchEventNew.Equals(LastSearchEvent))
            {
                return;
            }

            // bewaar huidige selectie (op OriginalLogEntry zodat we die later kunnen terugvinden)
            LogEntry previouslySelectedLogEntry = null;
            if (LstLogContent.SelectedItem is LogEntryDisplayObject selectedItem && selectedItem.OriginalLogEntry != null)
            {
                previouslySelectedLogEntry = selectedItem.OriginalLogEntry;
            }

            List<LogEntry> entries = LogMetadataFilterResult.LogEntries;
            bool didIncrementalAppend = false;

            // Voorwaarde voor incremental append:
            // - we hebben een vorige zoekactie
            // - FirstLogEntry, SearchText, CaseSensitive, WholeWord zijn gelijk
            // - LastLogEntry is anders (nieuwere logs toegevoegd)
            if (LastSearchEvent != null
                && Equals(LastSearchEvent.FirstLogEntry, searchEventNew.FirstLogEntry)
                && string.Equals(LastSearchEvent.SearchText ?? string.Empty, searchEventNew.SearchText ?? string.Empty, StringComparison.Ordinal)
                && LastSearchEvent.CaseSensitive == searchEventNew.CaseSensitive
                && LastSearchEvent.WholeWord == searchEventNew.WholeWord
                && LastSearchEvent.IsMetadataSearchEnabled == searchEventNew.IsMetadataSearchEnabled
                && !Equals(LastSearchEvent.LastLogEntry, searchEventNew.LastLogEntry))
            {
                // probeer de positie van de oude LastLogEntry te vinden
                int oldLastIndex = entries.FindIndex(delegate (LogEntry le) { return Equals(le, LastSearchEvent.LastLogEntry); });

                if (oldLastIndex >= 0)
                {
                    int startIndex = oldLastIndex + 1;
                    if (startIndex <= entries.Count - 1)
                    {
                        // zoek alleen in het nieuwe bereik en voeg toe
                        List<LogEntryDisplayObject> newResults = SearchLogEntriesInRange(entries, startIndex, entries.Count - 1, txtSearch.Text.Trim(), chkCaseSensitive.Checked, chkWholeWordsOnly.Checked);
                        // voeg toe aan de ListBox
                        for (int i = 0; i < newResults.Count; i++)
                        {
                            LstLogContent.Items.Add(newResults[i]);
                        }

                        didIncrementalAppend = true;
                    }
                    else
                    {
                        // geen nieuwe entries om toe te voegen; niks doen behalve updaten van LastSearchEvent verderop
                        didIncrementalAppend = true;
                    }
                }
                // indien oldLastIndex == -1: we kunnen de oude positie niet bepalen -> fallback naar volledige refresh
            }

            if (!didIncrementalAppend)
            {
                // volledige refresh
                LstLogContent.Items.Clear();

                if (IsSearchEmpty() || LogMetadataFilterResult == null || LogMetadataFilterResult.LogEntries == null)
                {
                    LastSearchEvent = searchEventNew;
                    return;
                }

                List<LogEntryDisplayObject> results = SearchLogEntries(entries, txtSearch.Text.Trim(), chkCaseSensitive.Checked, chkWholeWordsOnly.Checked);
                for (int i = 0; i < results.Count; i++)
                {
                    LstLogContent.Items.Add(results[i]);
                }
            }

            // probeer de oude selectie terug te zetten indien die nog aanwezig is
            if (previouslySelectedLogEntry != null)
            {
                for (int i = 0; i < LstLogContent.Items.Count; i++)
                {
                    object itemObj = LstLogContent.Items[i];
                    if (itemObj is LogEntryDisplayObject dto && Equals(dto.OriginalLogEntry, previouslySelectedLogEntry))
                    {
                        LstLogContent.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (LstLogContent.Items.Count == 1) lblResults.Text = "1 resultaat";
            else lblResults.Text = $"{LstLogContent.Items.Count} resultaten";

            if (LstLogContent.Items.Count == 0) lblResults.ForeColor = Color.DarkRed;
            else lblResults.ForeColor = Color.Black;

            // update LastSearchEvent (altijd bijwerken zodat volgende keer vergelijking klopt)
            LastSearchEvent = searchEventNew;
        }

        private void Clear()
        {
            LastSearchEvent = null;
            LstLogContent.Items.Clear();
            lblResults.Text = "";
        }

        /// <summary>
        /// Searches for log entries within the specified index range that match the given search criteria.
        /// </summary>
        /// <remarks>The returned display objects have their index values adjusted to reflect their
        /// position in the original entries list. The search is limited to the valid range between <paramref
        /// name="startIndex"/> and <paramref name="endIndex"/>.</remarks>
        /// <param name="entries">The list of log entries to search. Cannot be null.</param>
        /// <param name="startIndex">The zero-based index of the first entry in the range to search. Must be greater than or equal to 0 and less
        /// than the number of entries.</param>
        /// <param name="endIndex">The zero-based index of the last entry in the range to search. Must be greater than or equal to <paramref
        /// name="startIndex"/>.</param>
        /// <param name="searchText">The text to search for within each log entry. If empty, no entries will match.</param>
        /// <param name="caseSensitive">Specifies whether the search should be case-sensitive. If <see langword="true"/>, the search matches case
        /// exactly; otherwise, it ignores case.</param>
        /// <param name="wholeWord">Specifies whether to match only whole words. If <see langword="true"/>, only entries containing the exact
        /// word are matched; otherwise, partial matches are allowed.</param>
        /// <returns>A list of display objects representing log entries that match the search criteria within the specified
        /// range. Returns an empty list if no entries match or if the input parameters are invalid.</returns>
        private List<LogEntryDisplayObject> SearchLogEntriesInRange(List<LogEntry> entries, int startIndex, int endIndex, string searchText, bool caseSensitive, bool wholeWord)
        {
            List<LogEntryDisplayObject> adjustedResults = [];

            if (entries == null || startIndex < 0 || endIndex < startIndex || startIndex >= entries.Count)
            {
                return adjustedResults;
            }

            int safeEnd = Math.Min(endIndex, entries.Count - 1);
            int count = safeEnd - startIndex + 1;

            List<LogEntry> slice = entries.GetRange(startIndex, count);
            List<LogEntryDisplayObject> partial = SearchLogEntries(slice, searchText, caseSensitive, wholeWord);

            for (int i = 0; i < partial.Count; i++)
            {
                LogEntryDisplayObject item = partial[i];
                item.Index += startIndex;
                adjustedResults.Add(item);
            }

            return adjustedResults;
        }


        private bool IsSearchEmpty()
        {
            string search = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(search) || search == DefaulSearchtText) return true;

            return false;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchNext_Click(sender, e);
                e.Handled = true; // This prevents the system from processing the Enter key further
            }
        }

        private void ChkCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void ChkWholeWordsOnly_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private const string DefaulSearchtText = "Zoeken...";

        private void TxtSearch_Enter(object sender, EventArgs e)
        {

            if (txtSearch.Text == DefaulSearchtText)
            {
                txtSearch.Text = string.Empty;
                txtSearch.ForeColor = SystemColors.ControlText;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = DefaulSearchtText;
                txtSearch.ForeColor = Color.DarkGray;
            }
        }

        private void LstLogContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            // Fetch the item
            if (LstLogContent.Items[e.Index] is not LogEntryDisplayObject item || item.ContentValue == null) return;

            Graphics g = e.Graphics;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            if (isSelected) g.FillRectangle(LogScraperBrushes.BlueSelectedLogline, e.Bounds);
            else g.FillRectangle(SystemBrushes.Window, e.Bounds);

            //Default drawing, without tree
            string truncatedValue = TruncateTextToFit(item.ContentValue.TimeDescription + " " + item.ContentValue.Value, g, e.Bounds.Width);
            e.Graphics.DrawString(truncatedValue, LstLogContent.Font, SystemBrushes.ControlText, e.Bounds);

            e.DrawFocusRectangle();
        }

        private string TruncateTextToFit(string text, Graphics graphics, int maxWidth)
        {
            string truncatedText = text;
            int ellipsisWidth = TextRenderer.MeasureText(graphics, ".....", LstLogContent.Font).Width;

            while (TextRenderer.MeasureText(graphics, truncatedText, LstLogContent.Font).Width + ellipsisWidth > maxWidth)
            {
                if (truncatedText.Length == 0) break;
                truncatedText = truncatedText[..^1];
            }

            if (truncatedText.Length < text.Length)
            {
                truncatedText += "...";
            }

            return truncatedText;
        }

        private List<LogEntryDisplayObject> SearchLogEntries(List<LogEntry> entries, string searchText, bool caseSensitive, bool wholeWord)
        {
            List<LogEntryDisplayObject> results = [];

            if (entries == null)
            {
                return results;
            }

            if (string.IsNullOrEmpty(searchText))
            {
                return results;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                LogEntry current = entries[i];
                if (current?.Entry == null)
                {
                    continue;
                }

                int matchIndex = FindMatchIndex(current.Entry, searchText, caseSensitive, wholeWord, IsMetadataSearchEnabled ? 0 : current.StartIndexContent);
                if (matchIndex >= 0)
                {
                    LogEntryDisplayObject item = new()
                    {
                        Index = i,
                        OriginalLogEntry = current,
                        SearchTextStartvalue = matchIndex,
                        SearchTextAdditionalLogLineIndex = -1
                    };

                    string snippet = BuildValueTillSentenceEnd(current.Entry, matchIndex, searchText.Length, 8);
                    string timeDesc = current.TimeStamp.ToString("HH:mm:ss");
                    item.ContentValue = new LogContentValue(snippet, timeDesc);

                    results.Add(item);
                }
                // If no match is found in the main entry, check additional log entries
                else if (current.AdditionalLogEntries != null && current.AdditionalLogEntries.Count > 0)
                {
                    for (int additionalLogLineIndex = 0; additionalLogLineIndex < current.AdditionalLogEntries.Count; additionalLogLineIndex++)
                    {
                        string additionalLogLine = current.AdditionalLogEntries[additionalLogLineIndex];
                        matchIndex = FindMatchIndex(additionalLogLine, searchText, caseSensitive, wholeWord,0);
                        if (matchIndex >= 0)
                        {
                            LogEntryDisplayObject item = new()
                            {
                                Index = i,
                                OriginalLogEntry = current,
                                SearchTextStartvalue = matchIndex,
                                SearchTextAdditionalLogLineIndex = additionalLogLineIndex
                            };

                            string snippet = BuildValueTillSentenceEnd(additionalLogLine, matchIndex, searchText.Length, 8);
                            string timeDesc = current.TimeStamp.ToString("HH:mm:ss");
                            item.ContentValue = new LogContentValue(snippet, timeDesc);

                            results.Add(item);
                            break;
                        }
                    }
                }
            }

            return results;
        }

        // zoek index van eerste match (geeft -1 als niet gevonden). controleert whole word indien gevraagd.
        public int FindMatchIndex(string text, string searchText, bool caseSensitive, bool wholeWord, int contentStartIndex)
        {
            if (text == null || searchText == null)
            {
                return -1;
            }

            StringComparison comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int startPos = contentStartIndex;

            while (startPos <= text.Length - searchText.Length)
            {
                int idx = text.IndexOf(searchText, startPos, comparison);
                if (idx < 0)
                {
                    return -1;
                }

                if (wholeWord)
                {
                    if (IsWholeWordAt(text, idx, searchText.Length))
                    {
                        return idx;
                    }
                    startPos = idx + 1;
                    continue;
                }
                else
                {
                    return idx;
                }
            }

            return -1;
        }

        // controleert of match op idx een "heel woord" is (letters, digits, underscore)
        public bool IsWholeWordAt(string text, int index, int length)
        {
            if (text == null)
            {
                return false;
            }

            int beforeIndex = index - 1;
            int afterIndex = index + length;

            bool IsWordChar(char c)
            {
                return char.IsLetterOrDigit(c) || c == '_';
            }

            if (beforeIndex >= 0)
            {
                char cBefore = text[beforeIndex];
                if (IsWordChar(cBefore))
                {
                    return false;
                }
            }

            if (afterIndex < text.Length)
            {
                char cAfter = text[afterIndex];
                if (IsWordChar(cAfter))
                {
                    return false;
                }
            }

            return true;
        }

        // bouwt string: vanaf maxBefore tekens vóór match (of begin regel) tot het einde van de zin.
        // zin-einde = eerste '.' of '!' of '?' na match (inclusief die char). Stop ook op newline.
        public string BuildValueTillSentenceEnd(string text, int matchStart, int matchLength, int maxBefore)
        {
            if (text == null)
            {
                return string.Empty;
            }

            int textLength = text.Length;
            if (matchStart < 0 || matchStart >= textLength || matchLength <= 0)
            {
                return string.Empty;
            }

            int snippetStart = matchStart - maxBefore;
            if (snippetStart < 0)
            {
                snippetStart = 0;
            }

            // zoek het einde van de zin: eerst newline of '.', '!', '?'
            int searchPos = matchStart + matchLength;
            if (searchPos < 0)
            {
                searchPos = matchStart;
            }

            int endPos = textLength - 1; // inclusive
            bool foundSentenceEnd = false;
            for (int i = searchPos; i < textLength; i++)
            {
                char c = text[i];
                if (c == '\r' || c == '\n')
                {
                    endPos = i - 1;
                    foundSentenceEnd = true;
                    break;
                }

                if (c == '.' || c == '!' || c == '?')
                {
                    endPos = i; // include punctuation
                                // include trailing quote or ) if present immediately after punctuation
                    if (i + 1 < textLength)
                    {
                        char nextChar = text[i + 1];
                        if (nextChar == '"' || nextChar == '\'' || nextChar == ')' || nextChar == ']')
                        {
                            endPos = i + 1;
                        }
                    }
                    foundSentenceEnd = true;
                    break;
                }
            }

            if (!foundSentenceEnd)
            {
                endPos = textLength - 1;
            }

            int length = endPos - snippetStart + 1;
            if (length <= 0)
            {
                return string.Empty;
            }

            string snippet = text.Substring(snippetStart, length);

            // prefix with "..." when we trimmed the beginning
            if (snippetStart > 0)
            {
                snippet = "..." + snippet;
            }

            // trim trailing whitespace
            snippet = snippet.TrimEnd();

            return snippet;
        }
        private bool ignoreSelectedItemChanged = false;

        public LogEntry SelectedLogEntry
        {
            get
            {
                if (SelectedLogEntryDisplayObject == null) return null;
                return SelectedLogEntryDisplayObject.OriginalLogEntry;
            }
        }
        private LogEntryDisplayObject SelectedLogEntryDisplayObject
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogEntryDisplayObject)LstLogContent.SelectedItem);
            }
        }

        private void LstLogContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ClearSelectedLogEntryExternallyInProgress) return;

            if (ignoreSelectedItemChanged) return;

            OnSelectedItemChanged(EventArgs.Empty);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
