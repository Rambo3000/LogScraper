using System.Collections.Generic;
using LogScraper.LogPostProcessors;
using LogScraper.Utilities.Extensions;

namespace LogScraper.Log
{
    /// <summary>
    /// Provides utility methods for calculating visual line indexes for log entries based on their processed visual
    /// representation.
    /// </summary>
    /// <remarks>This class is intended for internal use in scenarios where log entries may span multiple
    /// visual lines due to post-processing, such as pretty-printing XML or JSON content. All members are static and
    /// thread safety is guaranteed as long as the input collections are not modified concurrently.</remarks>
    internal static class LogEntryVisualIndexCalculator
    {
        /// <summary>
        /// Builds a cache mapping each visible log entry to its corresponding visual line index based on
        /// post-processing rules.
        /// </summary>
        /// <remarks>The returned array can be used to efficiently map log entries to their visual
        /// representation after applying post-processing. The length of the returned array matches the number of
        /// visible log entries.</remarks>
        /// <param name="visibleLogEntries">The list of log entries to be processed. The order of entries determines the mapping in the resulting cache.</param>
        /// <param name="logPostProcessCollection">The collection of post-processing rules used to determine the visual line span for each log entry.</param>
        /// <returns>An array of integers where each element represents the starting visual line index for the corresponding log
        /// entry in the input list.</returns>
        public static int[] BuildCache(IList<LogEntry> visibleLogEntries, LogPostProcessCollection logPostProcessCollection)
        {
            int count = visibleLogEntries.Count;
            int[] visualLineIndexes = new int[count];

            int currentVisualLineIndex = 0;

            for (int i = 0; i < count; i++)
            {
                visualLineIndexes[i] = currentVisualLineIndex;

                LogEntry logEntry = visibleLogEntries[i];
                currentVisualLineIndex += GetVisualLineSpan(logEntry, logPostProcessCollection);
            }

            return visualLineIndexes;
        }
        /// <summary>
        /// Attempts to find the visual line index of a specified log entry within a collection of visible log entries.
        /// </summary>
        /// <remarks>The visual line index reflects the position of the log entry as it appears after
        /// applying any post-processing defined in the provided collection. If the target entry is not present in the
        /// visible log entries, the method returns false and the index is set to -1.</remarks>
        /// <param name="visibleLogEntries">The list of log entries that are currently visible. Cannot be null.</param>
        /// <param name="targetEntry">The log entry for which to find the visual line index. Cannot be null.</param>
        /// <param name="logPostProcessCollection">A collection of post-processing operations that may affect the visual representation of log entries. Cannot
        /// be null.</param>
        /// <param name="index">When this method returns, contains the zero-based visual line index of the specified log entry if found;
        /// otherwise, -1. This parameter is passed uninitialized.</param>
        /// <returns>true if the visual line index of the specified log entry is found; otherwise, false.</returns>
        public static bool TryGetVisualLineIndex(IList<LogEntry> visibleLogEntries, LogEntry targetEntry, LogPostProcessCollection logPostProcessCollection, out int index)
        {
            index = -1;
            int currentVisualLineIndex = 0;

            for (int i = 0; i < visibleLogEntries.Count; i++)
            {
                LogEntry logEntry = visibleLogEntries[i];

                if (logEntry == targetEntry)
                {
                    index = currentVisualLineIndex;
                    return true;
                }

                currentVisualLineIndex += GetVisualLineSpan(logEntry, logPostProcessCollection);
            }

            return false;
        }
        /// <summary>
        /// Calculates the total number of visual lines required to display a log entry, including any additional
        /// entries and formatted representations.
        /// </summary>
        /// <remarks>The returned count includes the main log entry, any additional log entries, and extra
        /// lines for XML or JSON pretty-printed representations if available.</remarks>
        /// <param name="logEntry">The log entry for which to determine the visual line span. Cannot be null.</param>
        /// <param name="logPostProcessCollection">A collection of post-processing results used to determine if the log entry has formatted (e.g., XML or JSON)
        /// representations.</param>
        /// <returns>The total number of visual lines needed to display the log entry and its associated content.</returns>
        private static int GetVisualLineSpan(LogEntry logEntry, LogPostProcessCollection logPostProcessCollection)
        {
            int lineCount = 1; // the log entry itself

            if (logEntry.AdditionalLogEntries != null) lineCount += logEntry.AdditionalLogEntries.Count;

            if (logEntry.TryGetPostProcessResult(logPostProcessCollection, LogPostProcessorKind.XmlPrettyPrint, out LogEntryPostProcessResult xmlResult))
            {
                lineCount += xmlResult.LineCount + 2;
            }

            if (logEntry.TryGetPostProcessResult(logPostProcessCollection, LogPostProcessorKind.JsonPrettyPrint, out LogEntryPostProcessResult jsonResult))
            {
                lineCount += jsonResult.LineCount + 2;
            }

            return lineCount;
        }
    }
}