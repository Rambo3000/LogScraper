using System.Collections.Generic;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// Represents a filter that enables selection of log entries based on a specific metadata property and its
    /// associated values.
    /// </summary>
    /// <param name="property">The log metadata property on which to base the filter. Determines which metadata attribute is evaluated when
    /// filtering log entries.</param>
    public class LogMetadataFilter(LogMetadataProperty property)
    {
        /// <summary>
        /// Gets the log metadata property associated with this filter.
        /// </summary>
        public LogMetadataProperty Property { get; } = property;

        /// <summary>
        /// Gets the collection of active log metadata values and their associated filter modes.
        /// </summary>
        public Dictionary<LogMetadataValue, FilterMode> ActiveValues { get; } = [];
    }
}
