namespace LogScraper.Log.Rendering
{
    /// <summary>
    /// Represents a range of log entries defined by a beginning and an end log entry.
    /// This can be used to specify a subset of log entries for rendering.
    /// </summary>
    public class LogRange
    {
        /// <summary>
        /// The log entry that marks the beginning of the log range.
        /// If null, the range starts from the beginning of the log collection.
        /// </summary>
        public LogEntry Begin { get; set; }

        /// <summary>
        /// The log entry that marks the end of the log range.
        /// If null, the range ends at the beginning of the log collection.
        /// </summary>
        public LogEntry End { get; set; }

        /// <summary>
        /// Indicates whether the log range is constrained by either a beginning or an end log entry.
        /// </summary>
        public bool IsConstrained
        {
            get
            {
                return Begin != null || End != null;
            }
        }
    }
}
