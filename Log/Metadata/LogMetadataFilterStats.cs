using System.Collections.Generic;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// Represents the statistics for a log metadata filter, including the metadata property and the counts of each value associated with that property.
    /// </summary>
    /// <param name="property">The log metadata property for which the statistics are being collected. This property serves as the basis for categorizing the counts of associated metadata values.</param>
    public class LogMetadataFilterStats(LogMetadataProperty property)
    {
        /// <summary>
        /// Gets the log metadata property for which the statistics are being collected. This property serves as the basis for categorizing the counts of associated metadata values.
        /// </summary>
        public LogMetadataProperty Property { get; } = property;

        /// <summary>
        /// Gets the collection of log metadata values and their associated counts of filtered log entries. Each entry in this collection represents a specific value of the metadata property and the number of log entries that match that value after filtering is applied.
        /// </summary>
        public List<LogMetadataValueCount> ValueCounts { get; } = [];
    }
}
