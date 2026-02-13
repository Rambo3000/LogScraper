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
        #region Private properties and initialization

        private const string DefaulSearchtText = "Zoeken...";
        public event Action<string, SearchDirectionUserControl, bool, bool, bool> Search;
        private List<LogEntry> LogEntries ;

        private SearchEvent LastSearchEvent = null;
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

        #endregion

        #region Classes and enums
        public enum SearchDirectionUserControl
        {
            Forward,
            Backward
        }
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

        #region Search listbox population and update

        /// <summary>
        /// Updates the search results list to reflect the current search criteria and log entries. Performs either an
        /// incremental update or a full refresh, depending on changes since the last search.
        /// </summary>
        /// <remarks>This method preserves the user's selection in the results list when possible and
        /// updates the displayed result count and status indicators. Incremental updates are performed when only new
        /// log entries have been added and the search criteria remain unchanged, improving performance for large log
        /// sets. A full refresh is performed if the search criteria have changed or the previous selection cannot be
        /// determined. This method should be called whenever the search parameters or log data change to ensure the
        /// results list remains accurate.</remarks>
        private void UpdateSearchResultsList()
        {
            //Reset item height to default in for weird windows scaling cases
            LstLogContent.ItemHeight = LstLogContent.Font.Height;

            if (LogEntries == null || LogEntries.Count == 0)
            {
                Clear();
                return;
            }

            SearchEvent searchEventNew = new()
            {
                FirstLogEntry = LogEntries.FirstOrDefault(),
                LastLogEntry = LogEntries.LastOrDefault(),
                SearchText = txtSearch.Text,
                CaseSensitive = chkCaseSensitive.Checked,
                IsMetadataSearchEnabled = IsMetadataSearchEnabled,
                WholeWord = chkWholeWordsOnly.Checked
            };

            // If the search event has not changed, no need to update
            if (searchEventNew.Equals(LastSearchEvent))
            {
                return;
            }

            // Save the currently selected log entry to restore selection later
            LogEntry previouslySelectedLogEntry = null;
            if (LstLogContent.SelectedItem is LogEntryDisplayObject selectedItem && selectedItem.OriginalLogEntry != null)
            {
                previouslySelectedLogEntry = selectedItem.OriginalLogEntry;
            }

            bool didIncrementalAppend = false;

            // Search requirements for incremental appending:
            // - we have a previous search event
            // - FirstLogEntry, SearchText, CaseSensitive, WholeWord are the same
            // - LastLogEntry is different (newer logs added)
            if (LastSearchEvent != null
                && Equals(LastSearchEvent.FirstLogEntry, searchEventNew.FirstLogEntry)
                && string.Equals(LastSearchEvent.SearchText ?? string.Empty, searchEventNew.SearchText ?? string.Empty, StringComparison.Ordinal)
                && LastSearchEvent.CaseSensitive == searchEventNew.CaseSensitive
                && LastSearchEvent.WholeWord == searchEventNew.WholeWord
                && LastSearchEvent.IsMetadataSearchEnabled == searchEventNew.IsMetadataSearchEnabled
                && !Equals(LastSearchEvent.LastLogEntry, searchEventNew.LastLogEntry))
            {
                // Find the index of the last log entry from the previous search
                int oldLastIndex = LogEntries.FindIndex(delegate (LogEntry le) { return Equals(le, LastSearchEvent.LastLogEntry); });

                if (oldLastIndex >= 0)
                {
                    int startIndex = oldLastIndex + 1;
                    if (startIndex <= LogEntries.Count - 1)
                    {
                        // Only search the new entries that were added since the last search
                        List<LogEntryDisplayObject> newResults = SearchLogEntriesInRange(LogEntries, startIndex, LogEntries.Count - 1, txtSearch.Text.Trim(), chkCaseSensitive.Checked, chkWholeWordsOnly.Checked);
                        LstLogContent.Items.AddRange([.. newResults]);

                        didIncrementalAppend = true;
                    }
                    else
                    {
                        didIncrementalAppend = true;
                    }
                }
            }

            if (!didIncrementalAppend)
            {
                // Full refresh of the search results
                LstLogContent.Items.Clear();

                if (IsSearchEmpty() || LogEntries == null)
                {
                    LastSearchEvent = searchEventNew;
                    return;
                }

                List<LogEntryDisplayObject> results = SearchLogEntries(LogEntries, txtSearch.Text.Trim(), chkCaseSensitive.Checked, chkWholeWordsOnly.Checked);
                LstLogContent.Items.AddRange([..results]);
            }

            // Try to restore the previous selection
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

            LastSearchEvent = searchEventNew;
        }

        #endregion

        #region Search logic

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

        /// <summary>
        /// Searches the specified log entries for occurrences of the given search text, returning a list of display
        /// objects representing matching entries.
        /// </summary>
        /// <remarks>The search is performed on both the main entry and any additional log lines
        /// associated with each log entry. The returned objects include information about the match location and a
        /// content snippet for display purposes.</remarks>
        /// <param name="entries">The collection of log entries to search. If null, an empty list is returned.</param>
        /// <param name="searchText">The text to search for within each log entry. If null or empty, no results are returned.</param>
        /// <param name="caseSensitive">Specifies whether the search should be case-sensitive. If <see langword="true"/>, only exact case matches
        /// are considered.</param>
        /// <param name="wholeWord">Specifies whether only whole word matches should be considered. If <see langword="true"/>, partial matches
        /// are excluded.</param>
        /// <returns>A list of display objects representing log entries that contain the search text. The list is empty if no
        /// matches are found or if the input parameters are invalid.</returns>
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

                //Note that the timestamp is not searched if the metadata search is disabled, this is not in line with the search of the main log window, but it is -for now- the expected behavior for this search control.
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
                        matchIndex = FindMatchIndex(additionalLogLine, searchText, caseSensitive, wholeWord, 0);
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

        /// <summary>
        /// Finds the index of the first occurrence of the specified search text within the given text, with options for
        /// case sensitivity and whole word matching.
        /// </summary>
        /// <remarks>If whole word matching is enabled, the method checks that the found substring is not
        /// part of a larger word. The search begins at the specified start index and proceeds forward.</remarks>
        /// <param name="text">The text to search within. If null, the method returns -1.</param>
        /// <param name="searchText">The substring to search for. If null, the method returns -1.</param>
        /// <param name="caseSensitive">Specifies whether the search should be case-sensitive. If <see langword="true"/>, the search distinguishes
        /// between uppercase and lowercase characters; otherwise, it ignores case.</param>
        /// <param name="wholeWord">Specifies whether to match only whole words. If <see langword="true"/>, the method returns the index only if
        /// the match is a whole word.</param>
        /// <param name="contentStartIndex">The zero-based index in the text at which to begin searching. Must be greater than or equal to 0 and less
        /// than or equal to the length of the text.</param>
        /// <returns>The zero-based index of the first occurrence of the search text that matches the specified criteria;
        /// otherwise, -1 if no match is found or if either input string is null.</returns>
        public static int FindMatchIndex(string text, string searchText, bool caseSensitive, bool wholeWord, int contentStartIndex)
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

        /// <summary>
        /// Determines whether the specified substring within the input text represents a whole word, delimited by
        /// non-word characters or string boundaries.
        /// </summary>
        /// <remarks>A whole word is defined as a sequence of letters, digits, or underscores that is not
        /// immediately preceded or followed by another word character. If text is null, the method returns false. The
        /// method does not validate that the specified index and length are within the bounds of the input
        /// string.</remarks>
        /// <param name="text">The input string to examine for a whole word at the specified position. Can be null.</param>
        /// <param name="index">The zero-based starting index of the substring to check within the input text.</param>
        /// <param name="length">The length of the substring to check for whole word boundaries.</param>
        /// <returns>true if the substring at the specified index and length is a whole word; otherwise, false.</returns>
        public static bool IsWholeWordAt(string text, int index, int length)
        {
            if (text == null)
            {
                return false;
            }

            int beforeIndex = index - 1;
            int afterIndex = index + length;

            static bool IsWordChar(char c)
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
        #endregion

        #region Helper methods

        /// <summary>
        /// Builds a text snippet starting up to a specified number of characters before a match and ending at the
        /// conclusion of the containing sentence.
        /// </summary>
        /// <remarks>The sentence end is determined by the first occurrence of a period ('.'), exclamation
        /// mark ('!'), question mark ('?'), or a line break after the match. If the snippet does not start at the
        /// beginning of the text, it is prefixed with an ellipsis ("..."). Trailing whitespace is removed from the
        /// result.</remarks>
        /// <param name="text">The source text from which to extract the snippet. If <paramref name="text"/> is <see langword="null"/>, an
        /// empty string is returned.</param>
        /// <param name="matchStart">The zero-based index in <paramref name="text"/> where the match begins. Must be within the bounds of the
        /// text.</param>
        /// <param name="matchLength">The length of the match in characters. Must be greater than zero.</param>
        /// <param name="maxBefore">The maximum number of characters to include before the match start in the resulting snippet. If the match is
        /// near the beginning of the text, fewer characters may be included.</param>
        /// <returns>A substring of <paramref name="text"/> that starts up to <paramref name="maxBefore"/> characters before
        /// <paramref name="matchStart"/> and ends at the end of the containing sentence. Returns an empty string if the
        /// input is invalid or no valid snippet can be constructed.</returns>
        public static string BuildValueTillSentenceEnd(string text, int matchStart, int matchLength, int maxBefore)
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

        #endregion

        #region Drawing of listbox items
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
        /// <summary>
        /// Truncates the specified text so that its rendered width does not exceed the given maximum width, appending
        /// an ellipsis if truncation occurs.
        /// </summary>
        /// <remarks>The method uses the current font of the LstLogContent control to measure text width.
        /// If the text is truncated, an ellipsis is appended to indicate omitted content.</remarks>
        /// <param name="text">The text to be truncated if it exceeds the specified maximum width when rendered.</param>
        /// <param name="graphics">The graphics context used to measure the rendered width of the text.</param>
        /// <param name="maxWidth">The maximum allowed width, in pixels, for the rendered text including the ellipsis.</param>
        /// <returns>A string containing the original text if it fits within the maximum width; otherwise, a truncated version of
        /// the text with an ellipsis appended.</returns>
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
        #endregion

        #region Public properties and events
        public void Clear()
        {
            LastSearchEvent = null;
            LstLogContent.Items.Clear();
            lblResults.Text = "";
        }
        public LogEntry SelectedLogEntry
        {
            get
            {
                if (SelectedLogEntryDisplayObject == null) return null;
                return SelectedLogEntryDisplayObject.OriginalLogEntry;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMetadataSearchEnabled { get; set; }

        /// Event to filter log entries based on the selected log content property
        public event EventHandler SelectedItemChanged;
        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            SelectedItemChanged?.Invoke(this, e);
        }
        public void UpdateLogEntries(List<LogEntry> logEntries)
        {
            LogEntries = logEntries;

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
        #endregion

        #region Control events and private properties
        private LogEntryDisplayObject SelectedLogEntryDisplayObject
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogEntryDisplayObject)LstLogContent.SelectedItem);
            }
        }
        private bool IsSearchEmpty()
        {
            string search = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(search) || search == DefaulSearchtText) return true;

            return false;
        }

        private void LstLogContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ClearSelectedLogEntryExternallyInProgress) return;

            OnSelectedItemChanged(EventArgs.Empty);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
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
            UpdateSearchResultsList();
            LstLogContent.MoveSelection(false, chkWrapAround.Checked);
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
        #endregion
    }
}
