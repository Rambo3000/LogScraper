using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log.Processors
{
    /// <summary>
    /// Computes metadata value counts per property across a set of log entries.
    /// Used to populate filter stats after filtering is applied.
    /// </summary>
    internal static class LogMetadataStatsBuilder
    {
        /// <summary>
        /// Builds a list of filter stats for the given log entries and metadata properties,
        /// containing the count of each distinct value per property.
        /// </summary>
        /// <remarks>
        /// Optimized for the hot UI path — uses index-based loops and CollectionsMarshal
        /// to minimize dictionary overhead.
        /// </remarks>
        /// <param name="logEntries">The log entries to count values from.</param>
        /// <param name="logMetadataProperties">The metadata properties to compute stats for.</param>
        /// <returns>A list of LogMetadataFilterStats, one per property.</returns>
        public static IndexDictionary<LogMetadataProperty, LogMetadataFilterStats> Build(List<LogEntry> logEntries, List<LogMetadataProperty> logMetadataProperties)
        {
            if (logEntries == null || logMetadataProperties == null) throw new AggregateException("Log entries and metadata properties cannot be null.");

            int propertyCount = logMetadataProperties.Count;
            IndexDictionary<LogMetadataProperty, LogMetadataFilterStats> result = new(propertyCount);

            // One dictionary per property, indexed by interned LogMetadataValue.
            // Pre-allocated with estimated capacity to reduce rehashing.
            Dictionary<LogMetadataValue, int>[] valueCounts = new Dictionary<LogMetadataValue, int>[propertyCount];
            for (int i = 0; i < propertyCount; i++)
            {
                valueCounts[i] = new Dictionary<LogMetadataValue, int>(32);
            }

            foreach (LogEntry logEntry in logEntries)
            {
                for (int i = 0; i < propertyCount; i++)
                {
                    if (!logEntry.Metadata.TryGetValue(logMetadataProperties[i], out LogMetadataValue metadataValue))
                        continue;

                    // GetValueRefOrAddDefault avoids a double dictionary lookup:
                    // adds with default (0) if missing, then increments via ref.
                    ref int count = ref CollectionsMarshal.GetValueRefOrAddDefault(valueCounts[i], metadataValue, out _);
                    count++;
                }
            }

            for (int i = 0; i < propertyCount; i++)
            {
                LogMetadataFilterStats stats = new(logMetadataProperties[i]);
                foreach (var kvp in valueCounts[i])
                {
                    stats.ValueCounts.Add(new LogMetadataValueCount(kvp.Key, kvp.Value));
                }
                result[i] = stats;
            }

            return result;
        }
    }
}