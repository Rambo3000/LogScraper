using System.Collections.Generic;
using LogScraper.Log.Collection;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// Represents the result of filtering log metadata.
    /// This class contains the filtered log lines and their associated metadata properties.
    /// </summary>
    internal class LogMetadataFilterResult
    {
        /// <summary>
        /// A list of log lines that match the filtering criteria.
        /// </summary>
        public List<LogLine> LogLines { get; set; }

        /// <summary>
        /// A list of metadata properties and their associated values for the filtered log lines.
        /// </summary>
        public List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesList { get; set; }
    }
}
