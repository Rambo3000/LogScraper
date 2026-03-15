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
        /// Gets or sets the collection of statistics for metadata filters applied to log entries.
        /// </summary>
        public IndexDictionary<LogMetadataProperty, LogMetadataFilterStats> FilterStats { get; set; }

        /// <summary>
        /// Gets or sets the collection of active log metadata filters, organized by metadata property.
        /// </summary>
        public IndexDictionary<LogMetadataProperty, LogMetadataFilter> ActiveFilters { get; set; }

        /// <summary>
        /// Gets or sets the collection of log flow trees, organized by log content properties.
        /// </summary>
        /// <remarks>This property provides a structured representation of log flow data, allowing for
        /// efficient  access and manipulation of log flow trees based on specific log content properties.</remarks>
        public IndexDictionary<LogContentProperty, LogFlowTree> LogFlowTrees { get; set; }
    }
}
