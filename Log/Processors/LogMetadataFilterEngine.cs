using System.Collections.Generic;
using System.Linq;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log.Processors
{
    /// <summary>
    /// Provides functionality to filter log entries based on specified metadata filters, and to generate associated log flow trees.
    /// </summary>
    internal static class LogMetadataFilterEngine
    {
        /// <summary>
        /// Filters the log entries based on the provided metadata filters, and computes value counts for the filtered result.
        /// </summary>
        /// <param name="allLogEntries">The list of all log entries.</param>
        /// <param name="filters">The list of metadata filters to apply.</param>
        /// <param name="logLayout">The log layout used to build flow trees.</param>
        /// <returns>A result containing the filtered log entries, per-property value counts, and log flow trees.</returns>
        public static LogMetadataFilterResult Apply(List<LogEntry> allLogEntries, List<LogMetadataFilter> filters, LogLayout logLayout)
        {
            LogMetadataFilterResult result = new()
            {
                LogEntries = FilterLogEntries(allLogEntries, filters),
            };

            result.FilterStats = LogMetadataStatsBuilder.Build(result.LogEntries, logLayout.LogMetadataProperties);
            result.LogFlowTrees = PopulateLogFlowTrees(logLayout, result.LogEntries);

            return result;
        }

        /// <summary>
        /// Filters log entries by applying all active metadata filters.
        /// Filtering between properties is an AND operation; filtering within values of a property is an OR operation.
        /// Exclude mode removes entries that match the specified values.
        /// </summary>
        /// <param name="allLogEntries">The list of all log entries.</param>
        /// <param name="filters">The list of metadata filters to apply.</param>
        /// <returns>The filtered list of log entries.</returns>
        private static List<LogEntry> FilterLogEntries(List<LogEntry> allLogEntries, List<LogMetadataFilter> filters)
        {
            if (allLogEntries == null) return [];
            if (filters == null || filters.Count == 0) return allLogEntries;

            // Only consider filters that have active values.
            List<LogMetadataFilter> activeFilters = [.. filters.Where(filter => filter.ActiveValues.Count > 0)];
            if (activeFilters.Count == 0) return allLogEntries;

            List<LogEntry> filteredLogEntries = allLogEntries;

            foreach (LogMetadataFilter filter in activeFilters)
            {
                HashSet<LogMetadataValue> includeValues = [.. filter.ActiveValues.Where(kvp => kvp.Value == FilterMode.Include).Select(kvp => kvp.Key)];
                HashSet<LogMetadataValue> excludeValues = [.. filter.ActiveValues.Where(kvp => kvp.Value == FilterMode.Exclude).Select(kvp => kvp.Key)];

                filteredLogEntries = [.. filteredLogEntries.Where(logEntry =>
                {
                    if (!logEntry.Metadata.TryGetValue(filter.Property, out LogMetadataValue entryValue))
                        return false;

                    if (excludeValues.Contains(entryValue)) return false;
                    if (includeValues.Count > 0 && !includeValues.Contains(entryValue)) return false;

                    return true;
                })];
            }

            return filteredLogEntries;
        }

        /// <summary>
        /// Constructs a collection of log flow trees based on the specified log layout and log entries.
        /// </summary>
        /// <param name="logLayout">The layout configuration that defines the log content properties and their relationships.</param>
        /// <param name="logEntries">A list of log entries to be processed for building the log flow trees.</param>
        /// <returns>An IndexDictionary mapping each flow tree starting property to its corresponding LogFlowTree.</returns>
        private static IndexDictionary<LogContentProperty, LogFlowTree> PopulateLogFlowTrees(LogLayout logLayout, List<LogEntry> logEntries)
        {
            IndexDictionary<LogContentProperty, LogFlowTree> logFlowTrees = new(logLayout.LogContentProperties.Count);

            if (logLayout.LogContentProperties == null || logLayout.LogContentProperties.Count == 0) return logFlowTrees;

            foreach (LogContentProperty logContentProperty in logLayout.LogContentProperties)
            {
                if (logContentProperty.IsBeginFlowTreeFilter && logContentProperty.EndFlowTreeContentProperty != null)
                {
                    logFlowTrees[logContentProperty] = LogFlowTreeBuilder.BuildLogFlowTree(logEntries, logContentProperty, logContentProperty.EndFlowTreeContentProperty);
                }
            }

            return logFlowTrees;
        }
    }
}