using System.Collections.Generic;
using System.Linq;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;

namespace LogScraper.Log.Processing
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
            result.LogFlowTrees = LogFlowTreeBuilder.Build(logLayout, result.LogEntries);

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

            List<LogMetadataFilter> activeFilters = [.. filters.Where(f => f.ActiveValues.Count > 0)];
            if (activeFilters.Count == 0) return allLogEntries;

            // Build include/exclude sets once per filter, outside the entry loop.
            int activeFilterCount = activeFilters.Count;
            HashSet<LogMetadataValue>[] includeSets = new HashSet<LogMetadataValue>[activeFilterCount];
            HashSet<LogMetadataValue>[] excludeSets = new HashSet<LogMetadataValue>[activeFilterCount];

            for (int f = 0; f < activeFilterCount; f++)
            {
                includeSets[f] = [];
                excludeSets[f] = [];
                foreach (var kvp in activeFilters[f].ActiveValues)
                {
                    if (kvp.Value == FilterMode.Include) includeSets[f].Add(kvp.Key);
                    else excludeSets[f].Add(kvp.Key);
                }
            }

            List<LogEntry> result = new(allLogEntries.Count);

            foreach (LogEntry logEntry in allLogEntries)
            {
                bool include = true;
                for (int f = 0; f < activeFilterCount; f++)
                {
                    if (!logEntry.Metadata.TryGetValue(activeFilters[f].Property, out LogMetadataValue entryValue))
                    { include = false; break; }

                    if (includeSets[f].Count > 0 && !includeSets[f].Contains(entryValue))
                    { include = false; break; }

                    if (excludeSets[f].Contains(entryValue))
                    { include = false; break; }
                }
                if (include) result.Add(logEntry);
            }

            return result;
        }
    }
}