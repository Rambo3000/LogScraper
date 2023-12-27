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

            // Clear the selection to avoid further typing in highlighted style
            richTextBox.SelectionLength = 0;
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

        public static void FindNext(this RichTextBox richTextBox, string searchText, ref int currentIndex)
        {
            if (currentIndex == -1)
                currentIndex = 0;

            int nextIndex = richTextBox.Text.IndexOf(searchText, currentIndex, StringComparison.CurrentCultureIgnoreCase);
            if (nextIndex >= 0)
            {
                richTextBox.Select(nextIndex, searchText.Length);
                richTextBox.SelectionBackColor = Color.Yellow;
                currentIndex = nextIndex + 1;
            }
            else
            {
                MessageBox.Show("No more matches found.");
                currentIndex = -1;
            }
        }
        public static void FindPrevious(this RichTextBox richTextBox, string searchText, ref int currentIndex)
        {
            if (currentIndex == -1)
                currentIndex = richTextBox.Text.Length;

            int prevIndex = richTextBox.Text.LastIndexOf(searchText, currentIndex - 1, StringComparison.CurrentCultureIgnoreCase);
            if (prevIndex >= 0)
            {
                richTextBox.Select(prevIndex, searchText.Length);
                richTextBox.SelectionBackColor = Color.Yellow;
                currentIndex = prevIndex - 1;
            }
            else
            {
                MessageBox.Show("No previous matches found.");
                currentIndex = -1;
            }
        }
    }
}