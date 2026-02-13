using System.Collections.Generic;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Utilities.IndexDictionary;

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

        /// <summary>
        /// Gets or sets the collection of log flow trees, organized by log content properties.
        /// </summary>
        /// <remarks>This property provides a structured representation of log flow data, allowing for
        /// efficient  access and manipulation of log flow trees based on specific log content properties.</remarks>
        public IndexDictionary<LogContentProperty, LogFlowTree> LogFlowTrees { get; set; }
    }
}
