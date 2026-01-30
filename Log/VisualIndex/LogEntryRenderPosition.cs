namespace LogScraper.Log.VisualIndex
{
    /// <summary>
    /// Describes a specific position within a rendered log entry.
    /// </summary>
    public sealed class LogEntryRenderPosition
    {
        /// <summary>
        /// The log entry being rendered.
        /// </summary>
        public LogEntry LogEntry { get; set; }

        /// <summary>
        /// The offset from the top of the log entry, this is used for positioning within multi-line log entries.
        /// </summary>
        public int OffsetIntoLogEntry { get; set; }

        /// <summary>
        /// The index of the rendered line within the overall rendered log view.
        /// </summary>
        public int RenderedLineIndex { get; set; }

        /// <summary>
        /// The index of the log entry within the visible log entries list. This is not the same as the LogEntry.Index property,
        /// as the visible log entries list may contain a subset of all possible LogEntries in the LogCollection.
        /// </summary>
        public int VisibleLogEntriesListIndex { get; set; } = -1;
    }

}
