using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;

namespace LogScraper.Utilities.Extensions
{
    public static class ScintillaRichTextBoxExtensions
    {
        private const int INDICATOR_SEARCH = 20;

        public static void Initialize(this Scintilla sci)
        {
            sci.Indicators[10].Style = IndicatorStyle.FullBox;
            sci.Indicators[10].ForeColor = Color.Gray;
            sci.Indicators[10].Alpha = 50;
            sci.Indicators[10].Under = true;

            sci.Indicators[11].Style = IndicatorStyle.FullBox;
            sci.Indicators[11].ForeColor = Color.FromArgb(60, 132, 196);
            sci.Indicators[11].Alpha = 80;
            sci.Indicators[11].Under = true;

            Indicator indicator = sci.Indicators[INDICATOR_SEARCH];
            indicator.Style = IndicatorStyle.StraightBox;
            indicator.Under = true;
            indicator.ForeColor = Color.Gold;
            indicator.Alpha = 100;
            indicator.OutlineAlpha = 200;
        }
        public static void HideUnusedMargins(this ScintillaNET.Scintilla sci)
        {
            // Margin 1 is fold markers or bookmarks
            sci.Margins[0].Width = 0;

            // Margin 1 is fold markers or bookmarks
            sci.Margins[1].Width = 0;

            // Margin 2 is unused by default
            sci.Margins[2].Width = 0;
        }

        public static void EnableLineNumbers(this Scintilla sci)
        {
            const int LINE_NUMBER_MARGIN = 0;

            sci.Margins[LINE_NUMBER_MARGIN].Type = MarginType.Number;
            sci.Margins[LINE_NUMBER_MARGIN].Width = 40; // Adjust width as needed
            sci.Margins[LINE_NUMBER_MARGIN].Sensitive = false; // don't react to clicks
            sci.Styles[Style.LineNumber].ForeColor = Color.DarkGray;
            sci.Styles[Style.LineNumber].BackColor = Color.WhiteSmoke;
        }
        public static void UseDefaultFont(this Scintilla sci, Control referenceControl)
        {
            sci.Styles[Style.Default].Font = referenceControl.Font.Name;
            sci.Styles[Style.Default].Size = (int)referenceControl.Font.Size;
            sci.Styles[Style.Default].Bold = referenceControl.Font.Bold;
            sci.Styles[Style.Default].Italic = referenceControl.Font.Italic;

            sci.StyleClearAll(); // Apply to all styles
        }

        public static void HighlightLines(this Scintilla sci, int? beginLine, int? endLine, int? activeLine)
        {
            // Clear previous
            sci.IndicatorCurrent = 10;
            sci.IndicatorClearRange(0, sci.TextLength);
            sci.IndicatorCurrent = 11;
            sci.IndicatorClearRange(0, sci.TextLength);

            // Apply begin/end line (light gray)
            if (beginLine.HasValue)
                HighlightSingleLine(sci, beginLine.Value, 10);
            if (endLine.HasValue && endLine != beginLine)
                HighlightSingleLine(sci, endLine.Value, 10);

            // Apply active line (light blue)
            if (activeLine.HasValue)
                HighlightSingleLine(sci, activeLine.Value, 11);
        }

        private static void HighlightSingleLine(Scintilla sci, int lineNumber, int indicatorIndex)
        {
            if (lineNumber < 0 || lineNumber >= sci.Lines.Count)
                return;

            var line = sci.Lines[lineNumber];
            sci.IndicatorCurrent = indicatorIndex;
            sci.IndicatorFillRange(line.Position, line.Length);
        }

        public static void ClearAllLineHighlighting(this Scintilla sci)
        {
            sci.IndicatorCurrent = 10;
            sci.IndicatorClearRange(0, sci.TextLength);
            sci.IndicatorCurrent = 11;
            sci.IndicatorClearRange(0, sci.TextLength);
        }
        public static void ScrollToLine(this ScintillaNET.Scintilla sci, int lineNumber)
        {
            if (lineNumber < 0 || lineNumber >= sci.Lines.Count)
                return;

            // How many lines fit in the visible area?
            int visibleLines = sci.LinesOnScreen;

            // Calculate the first visible line so that 'lineNumber' is centered
            int firstVisibleLine = lineNumber - visibleLines / 2;

            if (firstVisibleLine < 0)
                firstVisibleLine = 0;

            int maxFirstLine = sci.Lines.Count - visibleLines;
            if (maxFirstLine < 0) maxFirstLine = 0; // in case document shorter than visible area

            if (firstVisibleLine > maxFirstLine)
                firstVisibleLine = maxFirstLine;

            // Set the first visible line to scroll
            sci.FirstVisibleLine = firstVisibleLine;

            // Also move caret and selection to the line
            sci.SetEmptySelection(sci.Lines[lineNumber].Position);
        }

        /// <summary>
        /// Specifies the direction for searching text in the <see cref="RichTextBox"/>.
        /// </summary>
        public enum SearchDirection
        {
            Forward,
            Backward
        }

        /// <summary>
        /// Searches for a specified text in the <see cref="RichTextBox"/> and highlights the first match.
        /// </summary>
        /// <param name="textBox">The <see cref="RichTextBox"/> to search in.</param>
        /// <param name="searchText">The text to search for.</param>
        /// <param name="direction">The direction to search (forward or backward).</param>
        /// <param name="wholeWord">Indicates whether to match whole words only.</param>
        /// <param name="caseSensitive">Indicates whether the search should be case-sensitive.</param>
        /// <param name="wrapAround">Indicates whether the search should wrap around when reaching the end or beginning.</param>
        /// <returns>True if a match is found; otherwise, false.</returns>
        public static bool Find(this Scintilla textBox, string searchText, SearchDirection direction, bool wholeWord, bool caseSensitive, bool wrapAround)
        {
            // Stel zoekflags in
            var flags = SearchFlags.None;
            if (wholeWord) flags |= SearchFlags.WholeWord;
            if (caseSensitive) flags |= SearchFlags.MatchCase;

            int start, end;

            if (direction == SearchDirection.Forward)
            {
                start = textBox.CurrentPosition;
                end = textBox.TextLength;
                if (textBox.CurrentPosition + searchText.Length <= textBox.TextLength)
                {
                    if (textBox.Text.Substring(start, searchText.Length) == searchText)
                    {
                        start += searchText.Length;
                    }
                }
            }
            else
            {
                start = textBox.CurrentPosition;
                end = 0;
            }

            // Stel target in
            textBox.TargetStart =  start;
            textBox.TargetEnd = end;
            textBox.SearchFlags = flags;

            // Zoek
            int pos = textBox.SearchInTarget(searchText);
            if (pos >= 0)
            {

                // Voeg nieuwe highlight toe
                if (searchText.Length <= 3)
                {
                    // Verwijder vorige zoekmarkering
                    textBox.IndicatorCurrent = INDICATOR_SEARCH;
                    textBox.IndicatorClearRange(0, textBox.TextLength);
                    textBox.IndicatorCurrent = INDICATOR_SEARCH;
                    textBox.IndicatorFillRange(textBox.TargetStart, textBox.TargetEnd - textBox.TargetStart);
                }
                else
                {
                    HighlightAllOccurrences(textBox, searchText, wholeWord, caseSensitive);
                }
                textBox.GotoPosition(pos);
                return true;
            }

            // Wrap-around als nodig
            if (wrapAround)
            {
                textBox.TargetStart = (direction == SearchDirection.Forward) ? 0 : textBox.TextLength;
                textBox.TargetEnd = (direction == SearchDirection.Forward) ? textBox.TextLength : 0;
                textBox.SearchFlags = flags;

                pos = textBox.SearchInTarget(searchText);
                if (pos >= 0)
                {
                    textBox.IndicatorCurrent = INDICATOR_SEARCH;
                    textBox.IndicatorClearRange(0, textBox.TextLength);
                    // Voeg nieuwe highlight toe
                    if (searchText.Length <= 3)
                    {
                        textBox.IndicatorCurrent = INDICATOR_SEARCH;
                        textBox.IndicatorFillRange(textBox.TargetStart, textBox.TargetEnd - textBox.TargetStart);
                    }
                    else
                    {
                        HighlightAllOccurrences(textBox, searchText, wholeWord, caseSensitive);
                    }
                    textBox.GotoPosition(pos);
                    return true;
                }
            }

            return false;
        }

        public static void HighlightAllOccurrences(this ScintillaNET.Scintilla sci, string searchText, bool wholeWord = false, bool caseSensitive = false)
        {
            if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 3)
                return;

            var flags = SearchFlags.None;
            if (wholeWord) flags |= SearchFlags.WholeWord;
            if (caseSensitive) flags |= SearchFlags.MatchCase;

            // Maak oude zoekmarkering leeg
            sci.IndicatorCurrent = INDICATOR_SEARCH;
            sci.IndicatorClearRange(0, sci.TextLength);

            // Zoek alle matches
            sci.TargetStart = 0;
            sci.TargetEnd = sci.TextLength;
            sci.SearchFlags = flags;

            int pos = sci.SearchInTarget(searchText);
            while (pos >= 0)
            {
                sci.IndicatorFillRange(sci.TargetStart, sci.TargetEnd - sci.TargetStart);

                sci.TargetStart = sci.TargetEnd;
                sci.TargetEnd = sci.TextLength;

                pos = sci.SearchInTarget(searchText);
            }
        }
    }
}