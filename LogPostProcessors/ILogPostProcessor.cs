using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{
    /// <summary>
    /// Defines methods for determining whether a log entry can be processed and for processing log entries to produce
    /// formatted output.
    /// </summary>
    public interface ILogPostProcessor
    {
        /// <summary>
        /// Attempts to process the specified log entry and outputs the resulting text if successful.
        /// </summary>
        /// <param name="logEntry">The log entry to process. Cannot be null.</param>
        /// <param name="processedText">When this method returns, contains the processed text if the operation succeeded; otherwise, contains null.</param>
        /// <returns>true if the log entry was successfully processed; otherwise, false.</returns>
        public bool TryProcess(LogEntry logEntry, out string processedText);
    }
}