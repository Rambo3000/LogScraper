using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Rendering;

namespace LogScraper.Utilities.UserControls
{
    public partial class SearchResultListControl : UserControl
    {
        #region Private types

        private class LogEntryDisplayObject
        {
            /// <summary>Position of this entry in the source log entries list.</summary>
            public int Index { get; set; }

            public LogEntry OriginalLogEntry { get; set; }

            /// <summary>
            /// The rendered display string — produced by LogRenderer.RenderSingleLogEntry.
            /// This is what is drawn in the listbox row, and what highlight offsets refer to.
            /// </summary>
            public string RenderedText { get; set; }

            /// <summary>Zero-based character index of the search match within RenderedText.</summary>
            public int MatchStart { get; set; }

            /// <summary>Character length of the search match.</summary>
            public int MatchLength { get; set; }

            /// <summary>
            /// Index into AdditionalLogEntries when the match was found there; -1 for the main entry.
            /// </summary>
            public int AdditionalLogLineIndex { get; set; }

            public override string ToString() => RenderedText ?? string.Empty;
        }

        #endregion

        #region Private fields

        private List<LogEntry> logEntries;

        #endregion

        #region Public events

        /// <summary>
        /// Fired when the user selects a result entry. The main form should navigate the log view to this entry.
        /// </summary>
        public event EventHandler<LogEntry> ResultSelected;

        /// <summary>
        /// Fired when the user selects a result entry. The main form should navigate the log view to this entry.
        /// </summary>
        public event EventHandler Close;

        #endregion

        #region Initialization

        public SearchResultListControl()
        {
            InitializeComponent();
            LstSearchResults.DrawMode = DrawMode.OwnerDrawFixed;
            LstSearchResults.DrawItem += LstSearchResults_DrawItem;
        }

        #endregion

        #region Public interface

        /// <summary>
        /// Stores the log entries for use in subsequent searches. Does not trigger a refresh.
        /// Call UpdateSearchResults to refresh the list.
        /// </summary>
        public void UpdateLogEntries(List<LogEntry> entries)
        {
            logEntries = entries;
        }

        /// <summary>
        /// Refreshes the result list based on the provided search settings.
        /// Should be called by the main form in response to UserControlSearch.SearchSettingsChanged.
        /// </summary>
        public void UpdateSearchResults(SearchSettings searchSettings)
        {
            LstSearchResults.ItemHeight = LstSearchResults.Font.Height;

            if (logEntries == null || logEntries.Count == 0 || string.IsNullOrEmpty(searchSettings?.SearchText))
            {
                Clear();
                return;
            }

            // Preserve selection across refresh
            LogEntry previouslySelectedEntry = null;
            if (LstSearchResults.SelectedItem is LogEntryDisplayObject selected)
            {
                previouslySelectedEntry = selected.OriginalLogEntry;
            }

            LstSearchResults.BeginUpdate();
            LstSearchResults.Items.Clear();

            List<LogEntryDisplayObject> results = SearchLogEntries(logEntries, searchSettings);
            LstSearchResults.Items.AddRange([.. results]);

            // Restore previous selection if still present
            if (previouslySelectedEntry != null)
            {
                for (int i = 0; i < LstSearchResults.Items.Count; i++)
                {
                    if (LstSearchResults.Items[i] is LogEntryDisplayObject dto &&
                        Equals(dto.OriginalLogEntry, previouslySelectedEntry))
                    {
                        LstSearchResults.SelectedIndex = i;
                        break;
                    }
                }
            }

            LblResultCount.Text = $"Zoeken op '{searchSettings.SearchText}', {results.Count} resultaten gevonden";

            LstSearchResults.EndUpdate();
        }

        public void Clear()
        {
            LstSearchResults.Items.Clear();
        }

        #endregion

        #region Control events

        private void LstSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstSearchResults.SelectedItem is LogEntryDisplayObject selected)
            {
                ResultSelected?.Invoke(this, selected.OriginalLogEntry);
            }
        }

        /// <summary>
        /// Owner-draw handler. Draws the rendered entry text with a yellow highlight rectangle and bold
        /// font over the matched search term. Uses a darker highlight when the item is selected to
        /// remain visible against the selection background.
        /// TextRenderer.MeasureText with NoPadding is used for accurate glyph-width measurement,
        /// which is more reliable than Graphics.MeasureString for listbox rendering.
        /// </summary>
        private void LstSearchResults_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= LstSearchResults.Items.Count) return;

            if (LstSearchResults.Items[e.Index] is not LogEntryDisplayObject item)
            {
                return;
            }

            Graphics graphics = e.Graphics;
            Rectangle bounds = e.Bounds;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            
            if (isSelected) graphics.FillRectangle(LogScraperBrushes.BlueSelectedLogline, e.Bounds);
            else graphics.FillRectangle(SystemBrushes.Window, e.Bounds);

            string renderedText = item.RenderedText ?? string.Empty;
            int matchStart = item.MatchStart;
            int matchLength = item.MatchLength;

            // Split rendered text into three segments around the match
            bool hasMatch = matchStart >= 0 && matchLength > 0 && matchStart + matchLength <= renderedText.Length;

            string textBefore = hasMatch ? renderedText[..matchStart] : renderedText;
            string textMatch = hasMatch ? renderedText.Substring(matchStart, matchLength) : string.Empty;

            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix;

            int textLeft = bounds.Left + 2;
            int textTop = bounds.Top + (bounds.Height - e.Font.Height) / 2;

            // Measure segment widths before drawing to position the highlight rectangle
            Size beforeSize = TextRenderer.MeasureText(graphics, textBefore, e.Font, bounds.Size, flags);
            Size matchSize = hasMatch ? TextRenderer.MeasureText(graphics, textMatch, e.Font, bounds.Size, flags) : Size.Empty;

            // Draw yellow highlight background under the match segment before any text
            if (hasMatch)
            {
                Rectangle highlightRect = new(
                    textLeft + beforeSize.Width,
                    bounds.Top + 1,
                    matchSize.Width,
                    bounds.Height - 2);

                using Brush highlightBrush = new SolidBrush(Color.Yellow);
                graphics.FillRectangle(highlightBrush, highlightRect);
            }
            TextRenderer.DrawText(graphics, renderedText, e.Font, new Point(textLeft, textTop), Color.Black, flags);
        }

        #endregion

        #region Search logic

        /// <summary>
        /// Searches through the provided log entries for matches against the search settings.
        /// Uses LogRenderer.RenderSingleLogEntry to produce display strings consistent with the main
        /// log view. Match position is found on the rendered string so highlight offsets are correct
        /// even when metadata substitution shifts character positions.
        /// Returns one result per log entry (first matching line wins).
        /// </summary>
        private static List<LogEntryDisplayObject> SearchLogEntries(List<LogEntry> entries, SearchSettings searchSettings)
        {
            List<LogEntryDisplayObject> results = [];

            if (entries == null || string.IsNullOrEmpty(searchSettings.SearchText))
            {
                return results;
            }

            LogRenderSettings renderSettings = searchSettings.LogRenderSettings;

            for (int i = 0; i < entries.Count; i++)
            {
                LogEntry current = entries[i];
                if (current?.Entry == null) continue;

                string renderedText = renderSettings != null
                    ? LogRenderer.RenderSingleLogEntry(current, renderSettings)
                    : current.Entry;

                // When ShowOriginalMetadata is true the rendered string retains the raw metadata prefix,
                // so respect IsMetadataSearchEnabled to decide whether to skip it.
                // When ShowOriginalMetadata is false the metadata has been removed/substituted and the
                // rendered string starts at content — always search from 0.
                int searchStartIndex = 0;
                if (renderSettings != null && renderSettings.ShowOriginalMetadata && !searchSettings.IsMetadataSearchEnabled)
                {
                    searchStartIndex = current.StartIndexContent;
                }

                int matchIndex = FindMatchIndex(renderedText, searchSettings.SearchText, searchSettings.CaseSensitive, searchSettings.WholeWord, searchStartIndex);

                if (matchIndex >= 0)
                {
                    results.Add(BuildDisplayObject(i, current, renderedText, matchIndex, searchSettings.SearchText.Length, -1));
                    continue;
                }

                // Fall through to additional log lines — always searched from position 0
                if (current.AdditionalLogEntries == null) continue;

                for (int additionalIndex = 0; additionalIndex < current.AdditionalLogEntries.Count; additionalIndex++)
                {
                    string additionalLine = current.AdditionalLogEntries[additionalIndex];
                    matchIndex = FindMatchIndex(additionalLine, searchSettings.SearchText, searchSettings.CaseSensitive, searchSettings.WholeWord, 0);

                    if (matchIndex >= 0)
                    {
                        results.Add(BuildDisplayObject(i, current, additionalLine, matchIndex, searchSettings.SearchText.Length, additionalIndex));
                        break;
                    }
                }
            }

            return results;
        }

        private static LogEntryDisplayObject BuildDisplayObject(int index, LogEntry entry, string renderedText, int matchStart, int matchLength, int additionalLogLineIndex)
        {
            return new LogEntryDisplayObject
            {
                Index = index,
                OriginalLogEntry = entry,
                RenderedText = renderedText,
                MatchStart = matchStart,
                MatchLength = matchLength,
                AdditionalLogLineIndex = additionalLogLineIndex
            };
        }

        #endregion

        #region Static text search helpers

        /// <summary>
        /// Finds the index of the first occurrence of searchText within text, starting at contentStartIndex.
        /// Supports case-sensitive and whole-word matching.
        /// </summary>
        public static int FindMatchIndex(string text, string searchText, bool caseSensitive, bool wholeWord, int contentStartIndex)
        {
            if (text == null || searchText == null) return -1;

            StringComparison comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int startPosition = contentStartIndex;

            while (startPosition <= text.Length - searchText.Length)
            {
                int index = text.IndexOf(searchText, startPosition, comparison);
                if (index < 0) return -1;

                if (!wholeWord) return index;

                if (IsWholeWordAt(text, index, searchText.Length)) return index;

                startPosition = index + 1;
            }

            return -1;
        }

        /// <summary>
        /// Returns true if the substring at the given index and length within text is bounded by
        /// non-word characters or string boundaries on both sides.
        /// </summary>
        public static bool IsWholeWordAt(string text, int index, int length)
        {
            if (text == null) return false;

            static bool IsWordChar(char c) => char.IsLetterOrDigit(c) || c == '_';

            int beforeIndex = index - 1;
            if (beforeIndex >= 0 && IsWordChar(text[beforeIndex])) return false;

            int afterIndex = index + length;
            if (afterIndex < text.Length && IsWordChar(text[afterIndex])) return false;

            return true;
        }

        #endregion

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close?.Invoke(this, EventArgs.Empty);
            Clear();
        }
    }
}