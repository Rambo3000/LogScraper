using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Log.Processing;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log.Filtering
{
    /// <summary>
    /// Provides functionality to filter log entries based on specified metadata filters, and to generate associated log flow trees.
    /// </summary>
    internal static class LogMetadataFilterEngine
    {
        /// <summary>
        /// Filters the log entries based on the provided metadata filters, and computes value counts for the filtered result.
        /// </summary>
        /// <param name="collection">The log collection containing the log entries to filter.</param>
        /// <param name="filters">The list of metadata filters to apply.</param>
        /// <param name="logLayout">The log layout used to build flow trees.</param>
        /// <returns>A result containing the filtered log entries, per-property value counts, and log flow trees.</returns>
        public static LogMetadataFilterResult Apply(LogCollection collection, List<LogMetadataFilter> filters, LogLayout logLayout)
        {
            List<LogEntry> logEntries;
            BitArray filteredLogEntriesMask;

            // Hold the read lock only while iterating the live LogEntries list so a concurrent
            // CommitParsedEntries (write lock) cannot mutate it mid-enumeration.
            collection.AcquireReadAccess();
            try
            {
                logEntries = FilterLogEntries(collection.LogEntries, filters, out filteredLogEntriesMask);
            }
            finally
            {
                collection.ReleaseReadAccess();
            }

            // logEntries is now an independent snapshot — safe to use outside the lock.
            IndexDictionary<LogMetadataProperty, LogMetadataFilterStats> filterStats = LogMetadataStatsBuilder.Build(logEntries, logLayout.LogMetadataProperties);
            IndexDictionary<LogContentProperty, LogFlowTree> logFlowTrees = LogFlowTreeBuilder.Build(logLayout, logEntries);

            return new(logEntries, filteredLogEntriesMask, filters, filterStats, logFlowTrees, collection);
        }

        /// <summary>
        /// Filters log entries by applying all active metadata filters.
        /// Filtering between properties is an AND operation; filtering within values of a property is an OR operation.
        /// Exclude mode removes entries that match the specified values.
        /// </summary>
        /// <param name="allLogEntries">The list of all log entries.</param>
        /// <param name="filters">The list of metadata filters to apply.</param>
        /// <returns>The filtered list of log entries.</returns>
        private static List<LogEntry> FilterLogEntries(IReadOnlyList<LogEntry> allLogEntries, List<LogMetadataFilter> filters, out BitArray filteredLogEntriesMask)
        {
            if (allLogEntries == null)
            {
                filteredLogEntriesMask = new BitArray(0);
                return [];
            }
            if (filters == null || filters.Count == 0)
            {
                filteredLogEntriesMask = new BitArray(allLogEntries.Count, true);
                // Return a copy rather than the live list reference. The caller (Apply) holds the read
                // lock only for the duration of this method. Once the lock is released the background
                // thread may call CommitParsedEntries and mutate _logEntries, which would invalidate
                // any iterator held by the downstream render pipeline. Copying only the references
                // (not the LogEntry objects themselves) costs ~8 bytes per entry — ~8 MB for 1M lines.
                return [.. allLogEntries];
            }

            List<LogMetadataFilter> activeFilters = [.. filters.Where(f => f.ActiveValues.Count > 0)];
            if (activeFilters.Count == 0)
            {
                filteredLogEntriesMask = new BitArray(allLogEntries.Count, true);
                // Same reason as above: return a snapshot copy so the live list is safe to mutate
                // after the read lock is released.
                return [.. allLogEntries];
            }
            

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
            filteredLogEntriesMask = new BitArray(allLogEntries.Count);

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
                if (include)
                {
                    result.Add(logEntry);
                    filteredLogEntriesMask[logEntry.Index] = true;
                }
            }

            return result;
        }
    }
}