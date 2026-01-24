using System.Collections.Generic;
using LogScraper.Log.Content;
using LogScraper.LogPostProcessors;

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
        /// Determines which lines of log content should be styled for each specified content property with custom
        /// coloring.
        /// </summary>
        /// <remarks>The returned dictionary contains an entry for each content property in the input
        /// list, even if no lines require styling for that property. The method uses the provided post-processor kinds
        /// to determine the correct visual line indices.</remarks>
        /// <param name="visibleLogEntries">The list of log entries currently visible in the log view. Can be null.</param>
        /// <param name="contentPropertiesWithCustomColoring">The collection of log content properties for which custom coloring should be applied. Each property will be
        /// <param name="kinds">The list of log post-processor kinds to consider when calculating visual line indices.</param>
        /// evaluated for styling.</param>
        /// <returns>A dictionary mapping each specified log content property to a list of visual line indices representing the
        /// lines that should be styled for that property. If no entries are visible or no properties require styling,
        /// the dictionary will be empty.</returns>
        public static Dictionary<LogContentProperty, List<int>> GetVisualLineIndexesPerContentProperty(List<LogEntry> visibleLogEntries, List<LogContentProperty> contentPropertiesWithCustomColoring, List<LogPostProcessorKind> kinds)
        {
            Dictionary<LogContentProperty, List<int>> logEntriesToStylePerContentProperty = new(contentPropertiesWithCustomColoring.Count);

            if (visibleLogEntries == null) return logEntriesToStylePerContentProperty;

            int[] visualLineIndexPerVisibleEntry = BuildCache(visibleLogEntries, kinds);

            foreach (LogContentProperty logContentProperty in contentPropertiesWithCustomColoring)
            {
                List<int> logEntriesIndexes = [];
                logEntriesToStylePerContentProperty[logContentProperty] = logEntriesIndexes;

                for (int i = 0; i < visibleLogEntries.Count; i++)
                {
                    LogEntry logEntry = visibleLogEntries[i];

                    if (logEntry.LogContentProperties == null || logEntry.LogContentProperties.Count == 0) continue;

                    if (!logEntry.LogContentProperties.ContainsKey(logContentProperty)) continue;

                    logEntriesIndexes.Add(visualLineIndexPerVisibleEntry[i]);
                }
            }

            return logEntriesToStylePerContentProperty;
        }

        /// <summary>
        /// Attempts to find the visual line index of a specified log entry within a collection of visible log entries.
        /// </summary>
        /// <remarks>The visual line index reflects the position of the log entry as it appears after
        /// applying any post-processing defined in the provided collection. If the target entry is not present in the
        /// visible log entries, the method returns false and the index is set to -1.</remarks>
        /// <param name="visibleLogEntries">The list of log entries that are currently visible. Cannot be null.</param>
        /// <param name="targetEntry">The log entry for which to find the visual line index. Cannot be null.</param>
        /// <param name="logPostProcessorKinds">The list of log post-processor kinds to consider when calculating visual line spans.</param>
        /// <param name="index">When this method returns, contains the zero-based visual line index of the specified log entry if found;
        /// otherwise, -1. This parameter is passed uninitialized.</param>
        /// <returns>true if the visual line index of the specified log entry is found; otherwise, false.</returns>
        public static bool TryGetVisualLineIndex(IList<LogEntry> visibleLogEntries, LogEntry targetEntry, List<LogPostProcessorKind> logPostProcessorKinds, out int index)
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

                currentVisualLineIndex += GetVisualLineSpan(logEntry, logPostProcessorKinds);
            }

            return false;
        }

        /// <summary>
        /// Builds a cache mapping each visible log entry to its corresponding visual line index based on
        /// post-processing rules.
        /// </summary>
        /// <remarks>The returned array can be used to efficiently map log entries to their visual
        /// representation after applying post-processing. The length of the returned array matches the number of
        /// visible log entries.</remarks>
        /// <param name="visibleLogEntries">The list of log entries to be processed. The order of entries determines the mapping in the resulting cache.</param>
        /// <param name="logPostProcessorKinds">The list of log post-processor kinds to consider when calculating visual line spans.</param>
        /// <returns>An array of integers where each element represents the starting visual line index for the corresponding log
        /// entry in the input list.</returns>
        private static int[] BuildCache(List<LogEntry> visibleLogEntries, List<LogPostProcessorKind> logPostProcessorKinds)
        {
            int count = visibleLogEntries.Count;
            int[] visualLineIndexes = new int[count];

            int currentVisualLineIndex = 0;

            for (int i = 0; i < count; i++)
            {
                visualLineIndexes[i] = currentVisualLineIndex;

                LogEntry logEntry = visibleLogEntries[i];
                currentVisualLineIndex += GetVisualLineSpan(logEntry, logPostProcessorKinds);
            }

            return visualLineIndexes;
        }

        /// <summary>
        /// Calculates the total number of visual lines required to display a log entry, including any additional
        /// entries and formatted representations.
        /// </summary>
        /// <remarks>The returned count includes the main log entry, any additional log entries, and extra
        /// lines for XML or JSON pretty-printed representations if available.</remarks>
        /// <param name="logEntry">The log entry for which to determine the visual line span. Cannot be null.</param>
        /// <param name="logPostProcessorKinds">The list of log post-processor kinds to consider when calculating visual line spans.</param>
        /// <returns>The total number of visual lines needed to display the log entry and its associated content.</returns>
        private static int GetVisualLineSpan(LogEntry logEntry, List<LogPostProcessorKind> logPostProcessorKinds)
        {
            int lineCount = 1; // the log entry itself

            if (logEntry.AdditionalLogEntries != null) lineCount += logEntry.AdditionalLogEntries.Count;

            if (logEntry.LogPostProcessResults == null) return lineCount;

            foreach (LogPostProcessorKind kind in logPostProcessorKinds)
            {
                LogPostProcessResult result = logEntry.LogPostProcessResults.Results[(int)kind];
                if (result == null) continue;
                lineCount += logEntry.LogPostProcessResults.Results[(int)kind].LineCount + 2;
            }

            return lineCount;
        }
    }
}