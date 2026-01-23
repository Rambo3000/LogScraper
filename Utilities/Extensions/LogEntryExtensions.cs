using LogScraper.Log;
using LogScraper.LogPostProcessors;

namespace LogScraper.Utilities.Extensions
{
    internal static class LogEntryExtensions
    {
        /// <summary>
        /// Attempts to retrieve the post-processing result for the specified log entry and processor kind from the
        /// given collection.
        /// </summary>
        /// <param name="logEntry">The log entry for which to obtain the post-processing result. Cannot be null.</param>
        /// <param name="collection">The collection containing post-processing results. Cannot be null.</param>
        /// <param name="kind">The kind of post-processor whose result is to be retrieved.</param>
        /// <param name="result">When this method returns, contains the post-processing result associated with the specified log entry and
        /// processor kind, if found; otherwise, the default value.</param>
        /// <returns>true if a post-processing result was found for the specified log entry and processor kind; otherwise, false.</returns>
        public static bool TryGetPostProcessResult(this LogEntry logEntry, LogPostProcessCollection collection, LogPostProcessorKind kind, out LogEntryPostProcessResult result)
        {
            return collection.TryGetResult(kind, logEntry.Index, out result);
        }
    }
}