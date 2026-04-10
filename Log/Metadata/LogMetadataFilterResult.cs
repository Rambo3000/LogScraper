using System.Collections;
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
    public class LogMetadataFilterResult (List<LogEntry> logEntries, BitArray filteredLogEntriesMask, List<LogMetadataFilter> activeFilters, IndexDictionary<LogMetadataProperty, LogMetadataFilterStats> filterStats, IndexDictionary<LogContentProperty, LogFlowTree> logFlowTrees, LogCollection sourceLogCollection)
    {
        /// <summary>
        /// A list of log entries that match the filtering criteria.
        /// </summary>
        public List<LogEntry> LogEntries { get; private set; } = logEntries;
        /// <summary>
        /// A BitArray indicating which log entries from the source collection are included in the filtered result.
        /// </summary>
        public BitArray FilteredLogEntriesMask { get; private set; } = filteredLogEntriesMask;
        /// <summary>
        /// Gets the collection of source logs associated with this instance.
        /// </summary>
        public LogCollection SourceLogCollection { get; private set; } = sourceLogCollection;
        /// <summary>
        /// Gets or sets the list of active log metadata filters.
        /// </summary>
        public List<LogMetadataFilter> ActiveFilters { get; private set; } = activeFilters;

        /// <summary>
        /// Gets or sets the collection of statistics for metadata filters applied to log entries.
        /// </summary>
        public IndexDictionary<LogMetadataProperty, LogMetadataFilterStats> FilterStats { get; private set; } = filterStats;

        /// <summary>
        /// Gets or sets the collection of log flow trees, organized by log content properties.
        /// </summary>
        /// <remarks>This property provides a structured representation of log flow data, allowing for
        /// efficient  access and manipulation of log flow trees based on specific log content properties.</remarks>
        public IndexDictionary<LogContentProperty, LogFlowTree> LogFlowTrees { get; private set; } = logFlowTrees;
    }
}
