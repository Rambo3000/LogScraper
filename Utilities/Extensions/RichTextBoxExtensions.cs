using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LogScraper.Utilities.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="RichTextBox"/> control to enhance its functionality.
    /// </summary>
    public static class RichTextBoxExtensions
    {
        /// <summary>
        /// Highlights a specific line in the <see cref="RichTextBox"/> with the specified background and foreground colors.
        /// </summary>
        /// <param name="richTextBox">The <see cref="RichTextBox"/> to apply the highlighting to.</param>
        /// <param name="lineNumber">The line number to highlight (zero-based).</param>
        /// <param name="backColor">The background color to use for highlighting.</param>
        /// <param name="foreColor">The foreground color to use for highlighting.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the line number is out of range.</exception>
        public static void HighlightLine(this RichTextBox richTextBox, int lineNumber, Color backColor, Color foreColor)
        {
            if (lineNumber < 0 || lineNumber >= richTextBox.Lines.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number is out of range.");
            }

            // Get the starting character index and length of the specified line.
            int start = richTextBox.GetFirstCharIndexFromLine(lineNumber);
            int length = richTextBox.Lines[lineNumber].Length;

            // Apply the specified background and foreground colors to the line.
            richTextBox.Select(start, length);
            richTextBox.SelectionBackColor = backColor;
            richTextBox.SelectionColor = foreColor;
        }

        /// <summary>
        /// Clears all highlighting in the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="richTextBox">The <see cref="RichTextBox"/> to clear highlighting from.</param>
        public static void ClearHighlighting(this RichTextBox richTextBox)
        {
            // Select all text and reset the background color to the default.
            richTextBox.SelectAll();
            richTextBox.SelectionBackColor = richTextBox.BackColor;
        }

        /// <summary>
        /// Highlights all occurrences of a specified search text in the <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="richTextBox">The <see cref="RichTextBox"/> to search and highlight in.</param>
        /// <param name="searchText">The text to search for and highlight.</param>
        public static void HighlightAll(this RichTextBox richTextBox, string searchText)
        {
            // Reset all text highlighting.
            richTextBox.Select(0, richTextBox.TextLength);
            richTextBox.SelectionBackColor = Color.White;

            if (!string.IsNullOrEmpty(searchText))
            {
                // Perform a case-insensitive search for the specified text.
                RegexOptions options = RegexOptions.IgnoreCase;
                MatchCollection matches = Regex.Matches(richTextBox.Text, Regex.Escape(searchText), options);

                // Highlight each match with a yellow background.
                foreach (Match match in matches.Cast<Match>())
                {
                    richTextBox.Select(match.Index, match.Length);
                    richTextBox.SelectionBackColor = Color.Yellow;
                }
            }
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
        /// <param name="richTextBox">The <see cref="RichTextBox"/> to search in.</param>
        /// <param name="searchText">The text to search for.</param>
        /// <param name="direction">The direction to search (forward or backward).</param>
        /// <param name="wholeWord">Indicates whether to match whole words only.</param>
        /// <param name="caseSensitive">Indicates whether the search should be case-sensitive.</param>
        /// <param name="wrapAround">Indicates whether the search should wrap around when reaching the end or beginning.</param>
        /// <returns>True if a match is found; otherwise, false.</returns>
        public static bool Find(this RichTextBox richTextBox, string searchText, SearchDirection direction, bool wholeWord, bool caseSensitive, bool wrapAround)
        {
            int currentIndex = richTextBox.SelectionStart;
            if (currentIndex == -1) currentIndex = 0;

            // Determine the string comparison type based on case sensitivity.
            StringComparison stringComparison = caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

            // Find the index of the next occurrence of the search text.
            int nextIndex = FindIndexRichTextBox(richTextBox, direction, searchText, currentIndex, stringComparison, wholeWord, wrapAround);

            if (nextIndex < 0) return false;

            // Highlight the found text.
            richTextBox.Select(nextIndex, searchText.Length);
            richTextBox.SelectionBackColor = Color.Yellow;
            return true;
        }

        /// <summary>
        /// Finds the index of the next occurrence of the search text in the <see cref="RichTextBox"/>.
        /// </summary>
        private static int FindIndexRichTextBox(RichTextBox richTextBox, SearchDirection direction, string searchText, int currentIndex, StringComparison stringComparison, bool wholeWord, bool wrapAround, bool isStartedFromBegining = false)
        {
            int nextIndex;

            if (direction == SearchDirection.Backward)
            {
                // Search backward for the text.
                nextIndex = richTextBox.Text.LastIndexOf(searchText, currentIndex, stringComparison);
                if (nextIndex == currentIndex && richTextBox.SelectionLength == searchText.Length)
                {
                    if (currentIndex - 1 >= 0) nextIndex = richTextBox.Text.LastIndexOf(searchText, currentIndex - 1, stringComparison);
                }
                if (wrapAround && nextIndex == -1 && isStartedFromBegining == false)
                {
                    if (richTextBox.Text.Length - 1 >= 0) nextIndex = richTextBox.Text.LastIndexOf(searchText, richTextBox.Text.Length - 1, stringComparison);
                    isStartedFromBegining = true;
                }
            }
            else
            {
                // Search forward for the text.
                nextIndex = richTextBox.Text.IndexOf(searchText, currentIndex, stringComparison);
                if (nextIndex == currentIndex && richTextBox.SelectionLength == searchText.Length)
                {
                    if (richTextBox.Text.Length > currentIndex + 1) nextIndex = richTextBox.Text.IndexOf(searchText, currentIndex + 1, stringComparison);
                }
                if (wrapAround && nextIndex == -1 && isStartedFromBegining == false)
                {
                    if (richTextBox.Text.Length > currentIndex + searchText.Length) nextIndex = richTextBox.Text.IndexOf(searchText, 0, currentIndex + searchText.Length, stringComparison);
                    isStartedFromBegining = true;
                }
            }

            if (wholeWord && nextIndex >= 0)
            {
                // Ensure the match is a whole word.
                bool isWholeWord = IsWholeWord(richTextBox.Text, nextIndex, searchText.Length);
                if (!isWholeWord)
                {
                    int newSearchIndex = direction == SearchDirection.Forward ? nextIndex + searchText.Length : nextIndex - searchText.Length;
                    return FindIndexRichTextBox(richTextBox, direction, searchText, newSearchIndex, stringComparison, wholeWord, wrapAround, isStartedFromBegining);
                }
            }

            return nextIndex;
        }

        /// <summary>
        /// Determines whether the specified text at the given index is a whole word.
        /// </summary>
        private static bool IsWholeWord(string text, int startIndex, int length)
        {
            // Check if the character before the start index is a letter or digit.
            if (startIndex > 0 && char.IsLetterOrDigit(text[startIndex - 1]))
            {
                return false;
            }

            // Check if the character after the end index is a letter or digit.
            int endIndex = startIndex + length - 1;
            if (endIndex < text.Length - 1 && char.IsLetterOrDigit(text[endIndex + 1]))
            {
                return false;
            }

            return true;
        }
    }
}