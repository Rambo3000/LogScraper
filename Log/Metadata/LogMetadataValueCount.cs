namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// Represents a log metadata value along with its associated count of filtered log entries.
    /// </summary>
    /// <param name="value">The log metadata value being represented, which includes the value itself and its associated metadata property.</param>
    /// <param name="filteredCount">The count of log entries that match the specified metadata value after filtering is applied.</param>
    public class LogMetadataValueCount(LogMetadataValue value, int count)
    {
        /// <summary>
        /// Gets the log metadata value being represented, which includes the value itself and its associated metadata property.
        /// </summary>
        public LogMetadataValue Value { get; } = value;

        /// <summary>
        /// Gets or sets the count of log entries that match the specified metadata value after filtering is applied.
        /// </summary>
        public int Count { get; set; } = count;
    }
}
