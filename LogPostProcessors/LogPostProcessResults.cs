
using System;

namespace LogScraper.LogPostProcessors
{
    public sealed class LogPostProcessResults
    {
        /// <summary>
        /// Gets the results of the log post-processing operation.
        /// </summary>
        public LogPostProcessResult[] Results { get; } = new LogPostProcessResult[Enum.GetValues<LogPostProcessorKind>().Length];
        /// <summary>
        /// Sets the processing result for the specified log post-processor kind.
        /// </summary>
        /// <param name="kind">The kind of log post-processor for which to set the result.</param>
        /// <param name="result">The result to associate with the specified log post-processor kind.</param>
        public void Set(LogPostProcessorKind kind, LogPostProcessResult result)
        {
            Results[(int)kind] = result;
            LineCountIncludingHeadersAndFooters = CountAllProcessorsLines();
        }

        /// <summary>
        /// Gets the total number of lines, including header and footer lines.
        /// </summary>
        public int LineCountIncludingHeadersAndFooters { get; private set; } = 0;


        /// <summary>
        /// Counts the total number of lines from all processors, including header and footer lines.
        /// </summary>
        /// <returns>The total line count including headers and footers.</returns>
        private int CountAllProcessorsLines()
        {
            int count = 0;
            foreach (var result in Results)
            {
                if (result != null)
                {
                    // Add 2 for the header and footer lines
                    count += result.LineCount + 2;
                }
            }
            return count;
        }
    }
}