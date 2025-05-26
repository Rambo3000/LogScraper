using System.Collections.Generic;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// Represents the result of filtering log metadata.
    /// This class contains the filtered log entries and their associated metadata properties.
    /// </summary>
    public class LogMetadataFilterResult
    {
        /// <summary>
        /// A list of log entries that match the filtering criteria.
        /// </summary>
        public List<LogEntry> LogEntries { get; set; }

        /// <summary>
        /// A list of metadata properties and their associated values for the filtered log entries.
        /// </summary>
        public List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValues { get; set; }
    }
}
