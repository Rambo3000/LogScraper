using System.Collections.Generic;

namespace LogScraper.Log.Rendering
{
    /// <summary>
    /// Represents a mapping of log entries to their rendered visual lines in the log view.
    /// </summary>
    public sealed class LogEntriesRenderMap
    {

        public LogEntriesRenderMap(List<LogEntry> visibleLogEntries, int[] visualLineIndexPerEntry, int entireCollectionSize)
        {
            VisibleLogEntries = visibleLogEntries;
            VisualLineIndexPerEntry = visualLineIndexPerEntry;
            VisualLineIndexPerEntryEntireCollection = new int[entireCollectionSize];
            for (int i = 0; i < entireCollectionSize; i++)
            {
                VisualLineIndexPerEntryEntireCollection[i] = -1;
            }
            for (int i = 0; i < visibleLogEntries.Count; i++)
            {
                int entryIndex = visibleLogEntries[i].Index;
                VisualLineIndexPerEntryEntireCollection[entryIndex] = visualLineIndexPerEntry[i];
            }
        }

        /// <summary>
        /// The list of log entries that are visible in the rendered view.
        /// </summary>
        public List<LogEntry> VisibleLogEntries { get; private set; }

        /// <summary>
        /// An array mapping each visible log entry to its starting visual line index in the rendered view.
        /// </summary>
        public int[] VisualLineIndexPerEntry { get; private set; }

        /// <summary>
        /// An array mapping each visible log entry to its starting visual line index in the rendered view.
        /// This array is sized to the entire collection of log entries, with indices for non-visible entries set to -1.
        /// </summary>
        public int[] VisualLineIndexPerEntryEntireCollection { get; private set; }
    }
}