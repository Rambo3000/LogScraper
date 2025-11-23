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

namespace LogScraper
{
    public partial class UserControlSearch : UserControl
    {
        //TODO: check resetting of the log content item list
        //TODO: improve text shown in the content listbox (maybe make the search text bold)
        //TODO: search metadata conditionally
        //TODO: use the content item list box for previous/next

        public event Action<string, SearchDirectionUserControl, bool, bool, bool> Search;
        private LogMetadataFilterResult LogMetadataFilterResult;

        private SearchEvent LastSearchEvent = null;

        public enum SearchDirectionUserControl
        {
            Forward,
            Backward
        }


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
                       WholeWord == other.WholeWord;
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

        public void SetResultsFound(bool resultsFound)
        {
            lblNoResults.Visible = !resultsFound;
        }
        public void UpdateLogEntries(LogMetadataFilterResult logMetadataFilterResult)
        {
            LogMetadataFilterResult = logMetadataFilterResult;
            LstLogContent.Items.Clear();
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
                LstLogContent.Items.Clear();
                return;
            }
            SearchEvent searchEventNew =  new()
            {
                FirstLogEntry = LogMetadataFilterResult.LogEntries.FirstOrDefault(),
                LastLogEntry = LogMetadataFilterResult.LogEntries.LastOrDefault(),
                SearchText = txtSearch.Text,
                CaseSensitive = chkCaseSensitive.Checked,
                WholeWord = chkWholeWordsOnly.Checked
            };
            if (searchEventNew.Equals(LastSearchEvent))
            {
                return;
            }
            LstLogContent.Items.Clear();

            if (IsSearchEmpty() || LogMetadataFilterResult == null || LogMetadataFilterResult.LogEntries == null)
            {
                return;
            }
            List<LogEntryDisplayObject> results = SearchLogEntries(LogMetadataFilterResult.LogEntries, txtSearch.Text.Trim(), chkCaseSensitive.Checked, chkWholeWordsOnly.Checked);
            foreach (var result in results)
            {
                LstLogContent.Items.Add(result);
            }

            LastSearchEvent = searchEventNew;
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
            List<LogEntryDisplayObject> results = new List<LogEntryDisplayObject>();

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
                if (current == null)
                {
                    continue;
                }

                string content = current.Entry ?? string.Empty;
                int matchIndex = FindMatchIndex(content, searchText, caseSensitive, wholeWord);
                if (matchIndex >= 0)
                {
                    LogEntryDisplayObject item = new LogEntryDisplayObject();
                    item.Index = i;
                    item.OriginalLogEntry = current;
                    item.SearchTextStartvalue = matchIndex;

                    string snippet = BuildValueTillSentenceEnd(content, matchIndex, searchText.Length, 8);
                    string timeDesc = current.TimeStamp.ToString("HH:mm:ss");
                    item.ContentValue = new LogContentValue(snippet, timeDesc);

                    results.Add(item);
                }
            }

            return results;
        }

        // zoek index van eerste match (geeft -1 als niet gevonden). controleert whole word indien gevraagd.
        public int FindMatchIndex(string text, string searchText, bool caseSensitive, bool wholeWord)
        {
            if (text == null || searchText == null)
            {
                return -1;
            }

            StringComparison comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int startPos = 0;

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
    }
}
