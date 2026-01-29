using System;
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
        public static Dictionary<LogContentProperty, List<int>> GetVisualLineIndexesPerContentProperty(List<LogEntry> visibleLogEntries, List<LogContentProperty> contentPropertiesWithCustomColoring, List<LogPostProcessorKind> kinds, out int[] visualLineIndexPerVisibleEntry)
        {
            Dictionary<LogContentProperty, List<int>> logEntriesToStylePerContentProperty = new(contentPropertiesWithCustomColoring.Count);
            
            visualLineIndexPerVisibleEntry = null;
            if (visibleLogEntries == null) return logEntriesToStylePerContentProperty;

            visualLineIndexPerVisibleEntry = BuildCache(visibleLogEntries, kinds);

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
        /// Attempts to determine the top visible log entry and its offset based on the first visible visual line
        /// </summary>
        /// <remarks>This method uses a binary search algorithm to efficiently locate the log entry that corresponds
        /// to the specified visual line index. If the visual line index falls within the range of visible log
        /// entries, the method returns true and provides the index of the log entry along with the offset into that
        /// entry. If no matching entry is found, the method returns false.</remarks>
        /// <param name="firstVisibleVisualLine">The zero-based index of the first visible visual line in the log view.</param>
        /// <param name="visualLineIndexPerVisibleEntry">An array mapping each visible log entry to its starting visual line index. Cannot be null.</param>
        /// <param name="visibleLogEntries">The list of log entries that are currently visible. Cannot be null.</param>
        /// <param name="logEntryIndex">When this method returns, contains the index of the top visible log entry if found; otherwise, -1.
        /// This parameter is passed uninitialized.</param>
        /// <param name="offsetIntoEntry">When this method returns, contains the offset into the top visible log entry if found; otherwise, 0.
        /// This parameter is passed uninitialized.</param>
        /// <returns>true if a matching log entry is found; otherwise, false.</returns>
        public static bool TryGetTopVisibleLogEntry(int firstVisibleVisualLine, int[] visualLineIndexPerVisibleEntry, List<LogEntry> visibleLogEntries, out int logEntryIndex, out int offsetIntoEntry)
        {
            logEntryIndex = -1;
            offsetIntoEntry = 0;

            if (visualLineIndexPerVisibleEntry == null || visibleLogEntries == null)
            {
                return false;
            }

            int low = 0;
            int high = visualLineIndexPerVisibleEntry.Length - 1;

            while (low <= high)
            {
                int middle = (low + high) / 2;
                int startVisualIndex = visualLineIndexPerVisibleEntry[middle];

                if (startVisualIndex == firstVisibleVisualLine)
                {
                    logEntryIndex = visibleLogEntries[middle].Index;
                    offsetIntoEntry = 0;
                    return true;
                }

                if (startVisualIndex < firstVisibleVisualLine)
                {
                    low = middle + 1;
                }
                else
                {
                    high = middle - 1;
                }
            }

            if (high < 0)
            {
                return false;
            }

            logEntryIndex = visibleLogEntries[high].Index;
            offsetIntoEntry = firstVisibleVisualLine - visualLineIndexPerVisibleEntry[high];
            return true;
        }
        /// <summary>
        /// Attempts to calculate the visual line index to scroll to in order to maintain the position of a
        /// previously visible log entry after the visible log entries have changed.
        /// </summary>
        /// <remarks>This method searches through the new collection of visible log entries to find the
        /// previously visible log entry. If found, it calculates the appropriate visual line index to scroll to,
        /// taking into account any offset within the entry. If the previous entry is not found, the method
        /// determines the next closest entry or defaults to the last entry in the list.</remarks>
        /// <param name="previousLogEntryIndex">The index of the previously visible log entry.</param>
        /// <param name="previousOffsetIntoEntry">The offset into the previously visible log entry.</param>
        /// <param name="newVisualLineIndexPerVisibleEntry">An array mapping each new visible log entry to its starting visual line index. Cannot be null.</param>
        /// <param name="newVisibleLogEntries">The list of new log entries that are currently visible. Cannot be null.</param>
        /// <param name="scrollToPosition">When this method returns, contains the calculated visual line index to scroll to.
        /// This parameter is passed uninitialized.</param>
        /// <returns>true if a scroll position is successfully calculated; otherwise, false.</returns>

        public static bool TryGetScrollToPosition(int previousLogEntryIndex, int previousOffsetIntoEntry, int[] newVisualLineIndexPerVisibleEntry, List<LogEntry> newVisibleLogEntries, out int scrollToPosition)
        {
            scrollToPosition = -1;

            if (previousLogEntryIndex < 0 || previousOffsetIntoEntry < 0 || newVisualLineIndexPerVisibleEntry == null || newVisualLineIndexPerVisibleEntry.Length == 0 || newVisibleLogEntries == null || newVisibleLogEntries.Count == 0)
            {
                return false;
            }

            int entryCount = newVisibleLogEntries.Count;

            for (int i = 0; i < entryCount; i++)
            {
                // Simple scenario: the top log entry of the old view matches has a match in the new view
                if (newVisibleLogEntries[i].Index == previousLogEntryIndex)
                {
                    int startVisualLine = newVisualLineIndexPerVisibleEntry[i];

                    //Calculate any offset which previously also existed
                    int nextStartVisualLine = i + 1 < entryCount ? newVisualLineIndexPerVisibleEntry[i + 1] : int.MaxValue;
                    int visualSpan = nextStartVisualLine - startVisualLine;
                    int clampedOffset = Math.Min(previousOffsetIntoEntry, Math.Max(visualSpan - 1, 0));

                    scrollToPosition = Math.Max(startVisualLine + clampedOffset, 0);
                    return true;
                }

                //In case the top log entry of the old view does not match in the new view
                if (newVisibleLogEntries[i].Index > previousLogEntryIndex)
                {
                    // TODO: create helper class which contains all variables to make this better understandable. Include logentry object references for easy debugging.
                    // TODO: the old top line cannot be restored at the top. instead we should calculate which non-top line should remain in position
                    // 1 whether the new entry exists (if at all) in the old page. we need number of lines per page here
                    // 2 if it doesnt exist we should find the next new visible log entry which is also available in the old log. This can be optimized to not loop the entire old list all the time
                    //     if no match is found default back to the initially found new visible entry and show that since that is the closest entry
                    // 3 if a match is found, calculate the distance in the old scenario between the old top entry and the new selected entry in the old log. We now know the offset from the top of the window
                    // 4 substract the found offset from the log line index of the matched entry
                    scrollToPosition = newVisualLineIndexPerVisibleEntry[i];
                    return true;
                }
            }

            scrollToPosition = newVisualLineIndexPerVisibleEntry[entryCount - 1];
            return true;
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
        /// <returns>An array where each element at index i represents the visual line index of the log entry at index i in the input list.</returns>
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