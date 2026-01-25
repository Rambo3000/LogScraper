namespace LogScraper.Utilities.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Counts the number of lines in the specified string.
        /// </summary>
        /// <remarks>A line is defined as a sequence of characters ending with a line feed ('\n') or the
        /// end of the string. This method does not distinguish between different newline conventions (e.g., '\r\n' vs.
        /// '\n').</remarks>
        /// <param name="text">The string in which to count lines. Can be null or empty.</param>
        /// <returns>The total number of lines in the string. Returns 0 if the string is null or empty.</returns>
        public static int CountLines(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int lineCount = 1;
            for (int index = 0; index < text.Length; index++)
            {
                if (text[index] == '\n')
                {
                    lineCount++;
                }
            }
            return lineCount;
        }
    }
}