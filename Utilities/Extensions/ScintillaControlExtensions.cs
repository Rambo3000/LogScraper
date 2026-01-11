using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log.Content;
using ScintillaNET;

namespace LogScraper.Utilities.Extensions
{
    public static class ScintillaControlExtensions
    {
        private const int INDICATOR_SEARCH = 20;

        /// <summary>
        /// Initializes custom indicators for highlighting and search in the Scintilla control.
        /// Sets up visual styles for indicators used for line highlighting and search results.
        /// </summary>
        /// <param name="scintillacontrol">The Scintilla control to initialize.</param>
        public static void Initialize(this Scintilla scintillacontrol)
        {
            // Begin/End line indicator
            scintillacontrol.Indicators[10].Style = IndicatorStyle.FullBox;
            scintillacontrol.Indicators[10].ForeColor = Color.Gray;
            scintillacontrol.Indicators[10].Alpha = 50;
            scintillacontrol.Indicators[10].Under = true;

            // Active line indicator
            scintillacontrol.Indicators[11].Style = IndicatorStyle.FullBox;
            scintillacontrol.Indicators[11].ForeColor = Color.FromArgb(60, 132, 196);
            scintillacontrol.Indicators[11].Alpha = 100;
            scintillacontrol.Indicators[11].Under = true;

            Indicator indicator = scintillacontrol.Indicators[INDICATOR_SEARCH];
            indicator.Style = IndicatorStyle.StraightBox;
            indicator.Under = true;
            indicator.ForeColor = Color.Gold;
            indicator.Alpha = 100;
            indicator.OutlineAlpha = 200;
        }

        /// <summary>
        /// Hides unused margin columns in the Scintilla control for a cleaner appearance.
        /// </summary>
        /// <param name="scintillacontrol">The Scintilla control to modify.</param>
        public static void HideUnusedMargins(this ScintillaNET.Scintilla scintillacontrol)
        {
            // Margin 1 is fold markers or bookmarks
            scintillacontrol.Margins[0].Width = 0;

            // Margin 1 is fold markers or bookmarks
            scintillacontrol.Margins[1].Width = 0;

            // Margin 2 is unused by default
            scintillacontrol.Margins[2].Width = 0;
        }

        /// <summary>
        /// Enables and styles the line number margin in the Scintilla control.
        /// </summary>
        /// <param name="scintillacontrol">The Scintilla control to modify.</param>
        public static void EnableLineNumbers(this Scintilla scintillacontrol)
        {
            const int LINE_NUMBER_MARGIN = 0;

            scintillacontrol.Margins[LINE_NUMBER_MARGIN].Type = MarginType.Number;
            scintillacontrol.Margins[LINE_NUMBER_MARGIN].Width = 40; // Adjust width as needed
            scintillacontrol.Margins[LINE_NUMBER_MARGIN].Sensitive = false; // don't react to clicks
            scintillacontrol.Styles[Style.LineNumber].ForeColor = Color.DarkGray;
            scintillacontrol.Styles[Style.LineNumber].BackColor = Color.WhiteSmoke;
        }

        /// <summary>
        /// Sets the default font of the Scintilla control to match a reference control.
        /// </summary>
        /// <param name="scintillacontrol">The Scintilla control to modify.</param>
        /// <param name="referenceControl">The control whose font settings will be used.</param>
        public static void UseDefaultFont(this Scintilla scintillacontrol, Control referenceControl)
        {
            scintillacontrol.Styles[Style.Default].Font = referenceControl.Font.Name;
            scintillacontrol.Styles[Style.Default].Size = (int)referenceControl.Font.Size;
            scintillacontrol.Styles[Style.Default].Bold = referenceControl.Font.Bold;
            scintillacontrol.Styles[Style.Default].Italic = referenceControl.Font.Italic;

            scintillacontrol.StyleClearAll(); // Apply to all styles
        }

        /// <summary>
        /// Highlights specified lines in the Scintilla control using custom indicators.
        /// </summary>
        /// <param name="scintillacontrol">The Scintilla control to modify.</param>
        /// <param name="beginLine">The line number to highlight as the begin line (light gray), or null.</param>
        /// <param name="endLine">The line number to highlight as the end line (light gray), or null.</param>
        /// <param name="activeLine">The line number to highlight as the active line (light blue), or null.</param>
        public static void HighlightLines(this Scintilla scintillacontrol, int? beginLine, int? endLine, int? activeLine)
        {
            // Clear previous highlights
            scintillacontrol.IndicatorCurrent = 10;
            scintillacontrol.IndicatorClearRange(0, scintillacontrol.TextLength);
            scintillacontrol.IndicatorCurrent = 11;
            scintillacontrol.IndicatorClearRange(0, scintillacontrol.TextLength);

            // Apply begin/end line (light gray)
            if (beginLine.HasValue)
                scintillacontrol.HighlightSingleLine(beginLine.Value, 10);
            if (endLine.HasValue && endLine != beginLine)
                scintillacontrol.HighlightSingleLine(endLine.Value, 10);

            // Apply active line (light blue)
            if (activeLine.HasValue)
                scintillacontrol.HighlightSingleLine(activeLine.Value, 11);
        }
        /// <summary>
        /// Applies custom styles to specific lines in a Scintilla control based on the provided log content properties
        /// and their associated line numbers.
        /// </summary>
        /// <remarks>A maximum of 186 log content properties are processed to avoid exceeding style index
        /// limits. Existing styles in the Scintilla control are reset before applying new styles. Only lines associated
        /// with properties that have custom styling enabled are affected.</remarks>
        /// <param name="scintillaControl">The Scintilla control to which the styles will be applied.</param>
        /// <param name="logContentProperties">A list of log content properties that define the custom styles to be used. Only properties with custom
        /// styling enabled are considered.</param>
        /// <param name="contentLinesToStyle">A mapping of log content properties to the lists of line numbers that should be styled using each property's
        /// custom style.</param>
        public static void StyleLines(this Scintilla scintillaControl, List<LogContentProperty> logContentProperties, Dictionary<LogContentProperty, List<int>> contentLinesToStyle)
        {
            scintillaControl.ResetAllStyles();

            // Reorder the properties so the errors are always prioritised and styled last,
            // after sorting on error inverse the order
            List<(LogContentProperty Property, int Index)> orderedProperties = [.. logContentProperties
                .Select((property, index) => (property, index))
                .Where(item => item.index < 186)
                .OrderBy(item => item.property.IsErrorProperty ? 1 : 0)   // non-error first
                .ThenByDescending(item => item.index)];


            foreach ((LogContentProperty logContentProperty, int index) in orderedProperties)
            {
                if (!logContentProperty.IsCustomStyleEnabled) continue;

                // Offset style index to avoid conflicts with default styles
                int styleIndex = index + 64;

                scintillaControl.SetStyleFromLogContentProperty(logContentProperty, styleIndex);

                contentLinesToStyle.TryGetValue(logContentProperty, out List<int> linesToStyle);

                if (linesToStyle == null) continue;

                foreach (int lineNumber in linesToStyle)
                {
                    scintillaControl.StyleSingleLine(lineNumber, styleIndex);
                }
            }
        }
        /// <summary>
        /// Resets all text styling in the specified Scintilla control to the default style.
        /// </summary>
        /// <remarks>This method removes all custom styling from the control's text, restoring the default
        /// appearance. Use this method to clear syntax highlighting or other style customizations.</remarks>
        /// <param name="scintillaControl">The Scintilla control whose text styles will be reset. Cannot be null.</param>
        public static void ResetAllStyles(this Scintilla scintillaControl)
        {
            scintillaControl.StartStyling(0);
            scintillaControl.SetStyling(scintillaControl.TextLength, Style.Default);
        }


        /// <summary>
        /// Highlights a single line in the Scintilla control using the specified indicator.
        /// </summary>
        /// <param name="scintillacontrol">The Scintilla control to modify.</param>
        /// <param name="lineNumber">The line number to highlight.</param>
        /// <param name="indicatorIndex">The indicator index to use for highlighting.</param>
        public static void HighlightSingleLine(this Scintilla scintillacontrol, int lineNumber, int indicatorIndex)
        {
            if (lineNumber < 0 || lineNumber >= scintillacontrol.Lines.Count)
                return;

            var line = scintillacontrol.Lines[lineNumber];
            scintillacontrol.IndicatorCurrent = indicatorIndex;
            scintillacontrol.IndicatorFillRange(line.Position, line.Length);
        }

        /// <summary>
        /// Applies the specified style to a single line in the Scintilla control.
        /// </summary>
        /// <remarks>If the specified line number is outside the valid range, no styling is
        /// applied.</remarks>
        /// <param name="scintillaControl">The Scintilla control to which the style will be applied.</param>
        /// <param name="lineNumber">The zero-based index of the line to style. Must be within the range of existing lines in the control.</param>
        /// <param name="styleIndex">The style index to apply to the specified line.</param>
        public static void StyleSingleLine(this Scintilla scintillaControl, int lineNumber, int styleIndex)
        {
            if (lineNumber < 0 || lineNumber >= scintillaControl.Lines.Count)
                return;

            Line line = scintillaControl.Lines[lineNumber];

            int startPosition = line.Position;
            int length = line.Length;

            scintillaControl.StartStyling(startPosition);
            scintillaControl.SetStyling(length, styleIndex);
        }

        /// <summary>
        /// Applies custom foreground and background colors from a log content property to a specified style in the
        /// given Scintilla control.
        /// </summary>
        /// <param name="scintillaControl">The Scintilla control whose style will be updated.</param>
        /// <param name="logContentProperty">The log content property containing custom text and background color settings to apply.</param>
        /// <param name="styleIndex">The index of the style in the Scintilla control to which the colors will be applied.</param>
        public static void SetStyleFromLogContentProperty(this Scintilla scintillaControl, LogContentProperty logContentProperty, int styleIndex)
        {
            Style style = scintillaControl.Styles[styleIndex];
            if (logContentProperty.CustomTextColor == Color.Empty)
            {
                style.ForeColor = scintillaControl.Styles[Style.Default].ForeColor;
            }
            else
            {
                style.ForeColor = logContentProperty.CustomTextColor;
            }

            if (logContentProperty.CustomBackColor == Color.Empty)
            {
                style.BackColor = scintillaControl.Styles[Style.Default].BackColor;
            }
            else
            {
                style.BackColor = logContentProperty.CustomBackColor;
                style.FillLine = true;
            }
        }

        /// <summary>
        /// Clears all line highlighting from the Scintilla control.
        /// </summary>
        /// <param name="scintillacontrol">The Scintilla control to modify.</param>
        public static void ClearAllLineHighlighting(this Scintilla scintillacontrol)
        {
            scintillacontrol.IndicatorCurrent = 10;
            scintillacontrol.IndicatorClearRange(0, scintillacontrol.TextLength);
            scintillacontrol.IndicatorCurrent = 11;
            scintillacontrol.IndicatorClearRange(0, scintillacontrol.TextLength);
            scintillacontrol.IndicatorCurrent = 12;
            scintillacontrol.IndicatorClearRange(0, scintillacontrol.TextLength);
        }

        /// <summary>
        /// Scrolls the Scintilla control to center the specified line in the visible area and moves the caret to that line.
        /// </summary>
        /// <param name="scintillacontrol">The Scintilla control to scroll.</param>
        /// <param name="lineNumber">The line number to center and select.</param>
        public static void ScrollToLine(this ScintillaNET.Scintilla scintillacontrol, int lineNumber)
        {
            if (lineNumber < 0 || lineNumber >= scintillacontrol.Lines.Count)
                return;

            // How many lines fit in the visible area?
            int visibleLines = scintillacontrol.LinesOnScreen;

            // Calculate the first visible line so that 'lineNumber' is centered
            int firstVisibleLine = lineNumber - visibleLines / 2;

            if (firstVisibleLine < 0)
                firstVisibleLine = 0;

            int maxFirstLine = scintillacontrol.Lines.Count - visibleLines;
            if (maxFirstLine < 0) maxFirstLine = 0; // in case document shorter than visible area

            if (firstVisibleLine > maxFirstLine)
                firstVisibleLine = maxFirstLine;

            // Set the first visible line to scroll
            scintillacontrol.FirstVisibleLine = firstVisibleLine;

            // Also move caret and selection to the line
            scintillacontrol.SetEmptySelection(scintillacontrol.Lines[lineNumber].Position);
        }

        /// <summary>
        /// Specifies the direction for searching text in the <see cref="Scintilla"/> control.
        /// </summary>
        public enum SearchDirection
        {
            Forward,
            Backward
        }

        /// <summary>
        /// Searches for a specified text in the <see cref="Scintilla"/> control and highlights the first match.
        /// Optionally supports whole word, case sensitivity, direction, and wrap-around search.
        /// </summary>
        /// <param name="scintillaControl">The <see cref="Scintilla"/> control to search in.</param>
        /// <param name="searchText">The text to search for.</param>
        /// <param name="direction">The direction to search (forward or backward).</param>
        /// <param name="wholeWord">Indicates whether to match whole words only.</param>
        /// <param name="caseSensitive">Indicates whether the search should be case-sensitive.</param>
        /// <param name="wrapAround">Indicates whether the search should wrap around when reaching the end or beginning.</param>
        /// <returns>True if a match is found; otherwise, false.</returns>
        public static bool Find(this Scintilla scintillaControl, string searchText, SearchDirection direction, bool wholeWord, bool caseSensitive, bool wrapAround)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                scintillaControl.IndicatorCurrent = INDICATOR_SEARCH;
                scintillaControl.IndicatorClearRange(0, scintillaControl.TextLength);
                return false;
            }

            SearchFlags flags = GetSearchFlags(wholeWord, caseSensitive);

            SetInitialSearchRange(scintillaControl, searchText, direction);

            scintillaControl.SearchFlags = flags;
            int pos = scintillaControl.SearchInTarget(searchText);
            if (pos >= 0)
            {
                Highlight(scintillaControl, searchText, wholeWord, caseSensitive);
                scintillaControl.GotoPosition(pos);
                return true;
            }

            if (wrapAround)
            {
                SetWrapAroundRange(scintillaControl, direction);
                scintillaControl.SearchFlags = flags;
                pos = scintillaControl.SearchInTarget(searchText);
                if (pos >= 0)
                {
                    Highlight(scintillaControl, searchText, wholeWord, caseSensitive);
                    scintillaControl.GotoPosition(pos);
                    return true;
                }
            }

            scintillaControl.IndicatorCurrent = INDICATOR_SEARCH;
            scintillaControl.IndicatorClearRange(0, scintillaControl.TextLength);
            return false;
        }

        /// <summary>
        /// Returns the appropriate <see cref="SearchFlags"/> based on whole word and case sensitivity options.
        /// </summary>
        /// <param name="wholeWord">Whether to match whole words only.</param>
        /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
        /// <returns>The combined <see cref="SearchFlags"/>.</returns>
        private static SearchFlags GetSearchFlags(bool wholeWord, bool caseSensitive)
        {
            SearchFlags flags = SearchFlags.None;
            if (wholeWord) flags |= SearchFlags.WholeWord;
            if (caseSensitive) flags |= SearchFlags.MatchCase;
            return flags;
        }

        /// <summary>
        /// Sets the initial search range in the Scintilla control based on the search direction.
        /// </summary>
        /// <param name="scintillaControl">The Scintilla control to set the range for.</param>
        /// <param name="searchText">The text to search for.</param>
        /// <param name="direction">The direction to search.</param>
        private static void SetInitialSearchRange(Scintilla scintillaControl, string searchText, SearchDirection direction)
        {
            if (direction == SearchDirection.Forward)
            {
                int start = scintillaControl.CurrentPosition;
                if (start + searchText.Length <= scintillaControl.TextLength &&
                    scintillaControl.Text.Substring(start, searchText.Length).Equals(searchText, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    start += searchText.Length;
                }
                scintillaControl.TargetStart = start;
                scintillaControl.TargetEnd = scintillaControl.TextLength;
            }
            else
            {
                scintillaControl.TargetStart = scintillaControl.CurrentPosition;
                scintillaControl.TargetEnd = 0;
            }
        }

        /// <summary>
        /// Sets the search range to wrap around the document based on the search direction.
        /// </summary>
        /// <param name="scintillaControl">The Scintilla control to set the range for.</param>
        /// <param name="direction">The direction to search.</param>
        private static void SetWrapAroundRange(Scintilla scintillaControl, SearchDirection direction)
        {
            scintillaControl.TargetStart = direction == SearchDirection.Forward ? 0 : scintillaControl.TextLength;
            scintillaControl.TargetEnd = direction == SearchDirection.Forward ? scintillaControl.TextLength : 0;
        }

        /// <summary>
        /// Highlights all occurrences of the specified text in the Scintilla control using the search indicator.
        /// </summary>
        /// <param name="scintillaControl">The Scintilla control to highlight in.</param>
        /// <param name="searchText">The text to highlight.</param>
        /// <param name="wholeWord">Whether to match whole words only.</param>
        /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
        public static void HighlightAllOccurrences(this Scintilla scintillaControl, string searchText, bool wholeWord = false, bool caseSensitive = false)
        {
            if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 3)
                return;

            SearchFlags flags = GetSearchFlags(wholeWord, caseSensitive);

            scintillaControl.IndicatorCurrent = INDICATOR_SEARCH;
            scintillaControl.IndicatorClearRange(0, scintillaControl.TextLength);

            scintillaControl.TargetStart = 0;
            scintillaControl.TargetEnd = scintillaControl.TextLength;
            scintillaControl.SearchFlags = flags;

            int pos = scintillaControl.SearchInTarget(searchText);
            while (pos >= 0)
            {
                scintillaControl.IndicatorFillRange(scintillaControl.TargetStart, scintillaControl.TargetEnd - scintillaControl.TargetStart);
                scintillaControl.TargetStart = scintillaControl.TargetEnd;
                scintillaControl.TargetEnd = scintillaControl.TextLength;
                pos = scintillaControl.SearchInTarget(searchText);
            }
        }

        /// <summary>
        /// Highlights the current search result or all occurrences depending on the search text length.
        /// </summary>
        /// <param name="scintillaControl">The Scintilla control to highlight in.</param>
        /// <param name="searchText">The text to highlight.</param>
        /// <param name="wholeWord">Whether to match whole words only.</param>
        /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
        private static void Highlight(Scintilla scintillaControl, string searchText, bool wholeWord, bool caseSensitive)
        {
            scintillaControl.IndicatorCurrent = INDICATOR_SEARCH;
            scintillaControl.IndicatorClearRange(0, scintillaControl.TextLength);

            if (searchText.Length <= 3)
            {
                scintillaControl.IndicatorFillRange(scintillaControl.TargetStart, scintillaControl.TargetEnd - scintillaControl.TargetStart);
            }
            else
            {
                HighlightAllOccurrences(scintillaControl, searchText, wholeWord, caseSensitive);
            }
        }

    }
}