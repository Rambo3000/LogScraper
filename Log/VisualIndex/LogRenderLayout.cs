using System.Collections.Generic;

namespace LogScraper.Log.VisualIndex
{
    /// <summary>
    /// Describes the layout of rendered log entries, including which entries are visible and their corresponding visual line indices.
    /// </summary>
    public sealed class LogRenderLayout
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
