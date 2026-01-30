using System;
using System.Collections.Generic;
using System.Reflection;
using LogScraper.Log.Content;
using LogScraper.LogPostProcessors;

namespace LogScraper.Log.VisualIndex
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

        /// <summary>
        /// Attempts to determine the top visible log entry and its offset based on the first visible visual line
        /// </summary>
        /// <remarks>This method uses a binary search algorithm to efficiently locate the log entry that corresponds
        /// to the specified visual line index. If the visual line index falls within the range of visible log
        /// entries, the method returns true and provides the index of the log entry along with the offset into that
        /// entry. If no matching entry is found, the method returns false.</remarks>
        /// <param name="visualLineIndex">The zero-based index of the visual line in the rendered log view.</param>
        /// <param name="renderLayout">The layout of rendered log entries, including visible entries and their visual line indices. Cannot be null.</param>
        /// <param name="visualLineIndexPerVisibleEntry">An array mapping each visible log entry to its starting visual line index. Cannot be null.</param>
        /// <param name="visibleLogEntries">The list of log entries that are currently visible. Cannot be null.</param>
        /// <returns>true if a matching log entry is found; otherwise, false.</returns>
        public static bool TryGetRenderPosition(int visualLineIndex, LogRenderLayout renderLayout, out LogEntryRenderPosition logRenderPosition)
        {
            logRenderPosition = new()
            {
                RenderedLineIndex = visualLineIndex
            };

            if (renderLayout == null || renderLayout.VisualLineIndexPerEntry == null || renderLayout.VisibleLogEntries == null)
            {
                return false;
            }

            int low = 0;
            int high = renderLayout.VisualLineIndexPerEntry.Length - 1;

            while (low <= high)
            {
                int middle = (low + high) / 2;
                int startVisualIndex = renderLayout.VisualLineIndexPerEntry[middle];

                if (startVisualIndex == visualLineIndex)
                {
                    logRenderPosition.LogEntry = renderLayout.VisibleLogEntries[middle];
                    logRenderPosition.OffsetIntoLogEntry = 0;
                    logRenderPosition.VisibleLogEntriesListIndex = middle;
                    return true;
                }

                if (startVisualIndex < visualLineIndex)
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

            logRenderPosition.LogEntry = renderLayout.VisibleLogEntries[high];
            logRenderPosition.OffsetIntoLogEntry = visualLineIndex - renderLayout.VisualLineIndexPerEntry[high];
            logRenderPosition.VisibleLogEntriesListIndex = high;
            return true;
        }

        /// <summary>
        /// Calculates the appropriate scroll position to maintain the visual context of the log view
        /// </summary>
        /// <remarks>This method determines the new scroll position based on the top visible log entry
        /// before and after a re-rendering of the log view. It attempts to preserve the user's context by finding
        /// an anchored position in the new layout that corresponds to the previous top entry.</remarks>
        /// <param name="preRenderTopLogEntry">The log entry render position that was at the top of the view before re-rendering. Cannot be null.</param>
        /// <param name="preRenderLayout">The layout of rendered log entries before re-rendering. Cannot be null.</param>
        /// <param name="postRenderLayout">The layout of rendered log entries after re-rendering. Cannot be null.</param>
        /// <param name="linesOnScreen">The number of visual lines that can be displayed on the screen at once.</param>
        /// <param name="scrollToPosition">When this method returns, contains the calculated scroll position to maintain context.</param>
        /// <returns>true if a scroll position is successfully calculated; otherwise, false.</returns>
        internal static bool TryGetScrollToPosition(LogEntryRenderPosition preRenderTopLogEntry, LogRenderLayout preRenderLayout, LogRenderLayout postRenderLayout, int linesOnScreen, out int scrollToPosition)
        {
            if (preRenderTopLogEntry == null || preRenderTopLogEntry.LogEntry == null || preRenderLayout == null || postRenderLayout == null || preRenderLayout.VisibleLogEntries == null || postRenderLayout.VisibleLogEntries == null || preRenderLayout.VisualLineIndexPerEntry == null || postRenderLayout.VisualLineIndexPerEntry == null)
            {
                scrollToPosition = -1;
                return false;
            }

            // Find the top most log entry in the post render layout
            if (!TryGetPostRenderTopMostPosition(preRenderTopLogEntry, postRenderLayout, out LogEntryRenderPosition postRenderTopLogEntry))
            {
                // Since no log entry is found beyond the previously visible lines, the logical choice is to show the bottom part of the log
                scrollToPosition = postRenderLayout.VisualLineIndexPerEntry[^1];
                return true;
            }

            // The top most entry pre and post render are the same, so the post render offset can be directly calculated
            if (preRenderTopLogEntry.LogEntry == postRenderTopLogEntry.LogEntry)
            {
                // Make sure to retain the pre render offset
                scrollToPosition = postRenderTopLogEntry.RenderedLineIndex + preRenderTopLogEntry.OffsetIntoLogEntry;
                return true;
            }

            // Try to get the first line in the post render layout which also existed in the pre render layout within the visible range
            if (!TryGetAnchoredPosition(preRenderTopLogEntry, postRenderTopLogEntry, preRenderLayout, postRenderLayout, linesOnScreen, out LogEntryRenderPosition preRenderNewAnchorPosition, out LogEntryRenderPosition postRenderNewAnchorPosition))
            {
                // If no matching entry is found in both pre- and post render, the user will scroll further into the log to the first found line
                scrollToPosition = postRenderTopLogEntry.RenderedLineIndex;
                return true;
            }

            // Calculate the distance in the pre render layout from the top log entry to the new anchor position
            int preRenderAnchorDistanceFromTop = Math.Max(0, preRenderNewAnchorPosition.RenderedLineIndex - preRenderTopLogEntry.RenderedLineIndex);

            // Correct the post render scroll position to maintain the same distance from the new anchor position
            int postRenderTopScrollToLine = postRenderNewAnchorPosition.RenderedLineIndex - preRenderAnchorDistanceFromTop;

            // Make sure we set the scroll position between 0 and the last entry
            scrollToPosition = Math.Max(0, Math.Min(postRenderTopScrollToLine, postRenderLayout.VisualLineIndexPerEntry[^1]));

            return true;
        }

        /// <summary>
        /// Attempts to find the topmost log entry in the post-render layout that is at or after the pre-render top log entry.
        /// </summary>
        /// <remarks>This method iterates through the visible log entries in the post-render layout to find
        /// the first entry whose index is greater than or equal to that of the pre-render top log entry. If such an
        /// entry is found, it returns true along with the corresponding render position. If no matching entry is
        /// found, the method returns false.</remarks>
        /// <param name="preRenderTopLogEntry">The log entry render position that was at the top of the view before re-rendering. Cannot be null.</param>
        /// <param name="postRenderLayout">The layout of rendered log entries after re-rendering. Cannot be null.</param>
        /// <param name="postRenderTopMostPosition">When this method returns, contains the topmost log entry position in the post-render layout if found; otherwise, null. This parameter is passed uninitialized.</param>
        /// <returns>true if a topmost position is found; otherwise, false.</returns>
        private static bool TryGetPostRenderTopMostPosition(LogEntryRenderPosition preRenderTopLogEntry, LogRenderLayout postRenderLayout, out LogEntryRenderPosition postRenderTopMostPosition)
        {
            postRenderTopMostPosition = null;
            for (int i = 0; i < postRenderLayout.VisibleLogEntries.Count; i++)
            {
                if (postRenderLayout.VisibleLogEntries[i].Index >= preRenderTopLogEntry.LogEntry.Index)
                {
                    postRenderTopMostPosition = new LogEntryRenderPosition
                    {
                        LogEntry = postRenderLayout.VisibleLogEntries[i],
                        RenderedLineIndex = postRenderLayout.VisualLineIndexPerEntry[i],
                        VisibleLogEntriesListIndex = i,
                        OffsetIntoLogEntry = 0
                    };
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Attempts to find an anchored log entry position that exists in both pre-render and post-render layouts
        /// </summary>
        /// <remarks>This method searches for a log entry that is visible in both the pre-render and post-render layouts
        /// within the range of lines currently displayed on the screen. If such an entry is found, it returns
        /// the corresponding positions in both layouts. This is useful for maintaining scroll position
        /// consistency after re-rendering.</remarks>
        /// <param name="preRenderTopLogEntry">The log entry render position that was at the top of the view before re-rendering. Cannot be null.</param>
        /// <param name="postRenderTopLogEntry">The log entry render position that is at the top of the view after re-rendering. Cannot be null.</param>
        /// <param name="preRenderLayout">The layout of rendered log entries before re-rendering. Cannot be null.</param>
        /// <param name="postRenderLayout">The layout of rendered log entries after re-rendering. Cannot be null.</param>
        /// <param name="linesOnScreen">The number of visual lines that can be displayed on the screen at once.</param>
        /// <param name="anchorPreRenderPosition">When this method returns, contains the anchored log entry position in the pre-render layout if found; otherwise, null. This parameter is passed uninitialized.</param>
        /// <param name="anchorPostRenderPosition">When this method returns, contains the anchored log entry position in the post-render layout if found; otherwise, null. This parameter is passed uninitialized.</param>
        /// <returns>true if an anchored position is found; otherwise, false.</returns>
        private static bool TryGetAnchoredPosition(LogEntryRenderPosition preRenderTopLogEntry, LogEntryRenderPosition postRenderTopLogEntry, LogRenderLayout preRenderLayout, LogRenderLayout postRenderLayout, int linesOnScreen, out LogEntryRenderPosition anchorPreRenderPosition, out LogEntryRenderPosition anchorPostRenderPosition)
        {
            anchorPreRenderPosition = null;
            anchorPostRenderPosition = null;

            int postRenderLogEntriesEndIndex = Math.Min(postRenderTopLogEntry.VisibleLogEntriesListIndex + linesOnScreen, postRenderLayout.VisibleLogEntries.Count - 1);

            // Select each visible log entry in the post render layout within the visible range
            for (int i = postRenderTopLogEntry.VisibleLogEntriesListIndex; i < postRenderLogEntriesEndIndex + linesOnScreen; i++)
            {
                LogEntry newLogEntry = postRenderLayout.VisibleLogEntries[i];

                int preRenderLogEntriesEndIndex = Math.Min(preRenderTopLogEntry.VisibleLogEntriesListIndex + linesOnScreen, preRenderLayout.VisibleLogEntries.Count - 1);

                // Search within visible range of the the pre render layout for any matching log entry
                for (int j = preRenderTopLogEntry.VisibleLogEntriesListIndex; j < preRenderLogEntriesEndIndex; j++)
                {
                    // If a matching log entry is found, set the anchor positions and return true
                    if (preRenderLayout.VisibleLogEntries[j] == newLogEntry)
                    {
                        anchorPreRenderPosition = new()
                        {
                            VisibleLogEntriesListIndex = j,
                            LogEntry = preRenderLayout.VisibleLogEntries[j],
                            RenderedLineIndex = preRenderLayout.VisualLineIndexPerEntry[j]
                        };

                        anchorPostRenderPosition = new()
                        {
                            VisibleLogEntriesListIndex = i,
                            LogEntry = postRenderLayout.VisibleLogEntries[i],
                            RenderedLineIndex = postRenderLayout.VisualLineIndexPerEntry[i]
                        };
                        return true;
                    }
                }
            }
            return false;
        }
    }
}