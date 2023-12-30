using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LogScraper.Extensions
{
    public static class RichTextBoxExtensions
    {
        public static void HighlightLine(this RichTextBox richTextBox, int lineNumber, Color backColor, Color foreColor)
        {
            if (lineNumber < 0 || lineNumber >= richTextBox.Lines.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number is out of range.");
            }

            int start = richTextBox.GetFirstCharIndexFromLine(lineNumber);
            int length = richTextBox.Lines[lineNumber].Length;

            richTextBox.Select(start, length);
            richTextBox.SelectionBackColor = backColor;
            richTextBox.SelectionColor = foreColor;
        }
        public static void ClearHighlighting(this RichTextBox richTextBox)
        {
            richTextBox.SelectAll();
            richTextBox.SelectionBackColor = richTextBox.BackColor;
        }
        public static void HighlightAll(this RichTextBox richTextBox, string searchText)
        {
            richTextBox.Select(0, richTextBox.TextLength);
            richTextBox.SelectionBackColor = Color.White;

            if (!string.IsNullOrEmpty(searchText))
            {
                RegexOptions options = RegexOptions.IgnoreCase;
                MatchCollection matches = Regex.Matches(richTextBox.Text, Regex.Escape(searchText), options);

                foreach (Match match in matches.Cast<Match>())
                {
                    richTextBox.Select(match.Index, match.Length);
                    richTextBox.SelectionBackColor = Color.Yellow;
                }
            }
        }
        public enum SearchDirection
        {
            Forward,
            Backward
        }

        public static bool Find(this RichTextBox richTextBox, string searchText, SearchDirection direction, bool wholeWord, bool caseSensitive)
        {
            int currentIndex = richTextBox.SelectionStart;
            //int initialSelectionLength = richTextBox.SelectionLength;
            if (currentIndex == -1) currentIndex = 0;

            StringComparison stringComparison = caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

            int nextIndex = FindIndexRichTextBox(richTextBox, direction, searchText, currentIndex, stringComparison, wholeWord);

            if (nextIndex < 0) return false;

            richTextBox.Select(nextIndex, searchText.Length);
            richTextBox.SelectionBackColor = Color.Yellow;
            return true;
        }

        private static int FindIndexRichTextBox(RichTextBox richTextBox, SearchDirection direction, string searchText, int currentIndex, StringComparison stringComparison, bool wholeWord, bool isStartedFromBegining = false)
        {
            int nextIndex;

            if (direction == SearchDirection.Backward)
            {
                nextIndex = richTextBox.Text.LastIndexOf(searchText, currentIndex, stringComparison);
                if (nextIndex == currentIndex && richTextBox.SelectionLength == searchText.Length)
                {
                    if (currentIndex - 1 >= 0) nextIndex = richTextBox.Text.LastIndexOf(searchText, currentIndex - 1, stringComparison);
                }
                // Start from the end if nothing is found:
                if (nextIndex == -1 && isStartedFromBegining == false)
                {
                    if (richTextBox.Text.Length - 1 >= 0) nextIndex = richTextBox.Text.LastIndexOf(searchText, richTextBox.Text.Length - 1, stringComparison);
                    isStartedFromBegining = true;
                }
            }
            else // SearchDirection.Forward
            {
                nextIndex = richTextBox.Text.IndexOf(searchText, currentIndex, stringComparison);
                if (nextIndex == currentIndex && richTextBox.SelectionLength == searchText.Length)
                {
                    if (richTextBox.Text.Length > currentIndex + 1) nextIndex = richTextBox.Text.IndexOf(searchText, currentIndex + 1, stringComparison);
                }
                // Start from the beginning if nothing is found:
                if (nextIndex == -1 && isStartedFromBegining == false)
                {
                    if (richTextBox.Text.Length > currentIndex + searchText.Length) nextIndex = richTextBox.Text.IndexOf(searchText, 0, currentIndex + searchText.Length, stringComparison);
                    isStartedFromBegining = true;
                }
            }

            if (wholeWord && nextIndex >= 0)
            {
                bool isWholeWord = IsWholeWord(richTextBox.Text, nextIndex, searchText.Length);
                if (!isWholeWord)
                {
                    int newSearchIndex = direction == SearchDirection.Forward ? nextIndex + searchText.Length : nextIndex - searchText.Length;
                    // If not a whole word, continue searching
                    return FindIndexRichTextBox(richTextBox, direction, searchText, newSearchIndex, stringComparison, wholeWord, isStartedFromBegining);
                }
            }

            return nextIndex;
        }

        private static bool IsWholeWord(string text, int startIndex, int length)
        {
            if (startIndex > 0 && char.IsLetterOrDigit(text[startIndex - 1]))
            {
                return false;
            }

            int endIndex = startIndex + length - 1;
            if (endIndex < text.Length - 1 && char.IsLetterOrDigit(text[endIndex + 1]))
            {
                return false;
            }

            return true;
        }
    }
}