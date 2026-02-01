using System.Collections.Generic;

namespace LogScraper.Log.Rendering
{
    /// <summary>
    /// Represents a mapping of log entries to their rendered visual lines in the log view.
    /// </summary>
    public sealed class LogEntriesRenderMap
    {
        /// <summary>
        /// The list of log entries that are visible in the rendered view.
        /// </summary>
        public List<LogEntry> VisibleLogEntries { get; set; }

        /// <summary>
        /// An array mapping each visible log entry to its starting visual line index in the rendered view.
        /// </summary>
        public int[] VisualLineIndexPerEntry { get; set; }
    }
}