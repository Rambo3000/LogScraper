using LogScraper.Log.Collection;
using LogScraper.Log.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace LogScraper.Log
{
    /// <summary>
    /// Provides methods to filter log entries based on metadata properties and values.
    /// </summary>
    internal static class LogMetadataFilter
    {
        /// <summary>
        /// Filters the log entries based on the provided metadata properties and values, and updates the count of each metadata value.
        /// </summary>
        /// <param name="allLogEntries">The list of all log entries.</param>
        /// <param name="logMetadataPropertyAndValuesList">The list of metadata properties and values to filter by.</param>
        /// <returns>A result containing the filtered log entries and the updated metadata properties and values.</returns>
        public static LogMetadataFilterResult GetLogMetadataFilterResult(List<LogEntry> allLogEntries, List<LogMetadataPropertyAndValues> logMetadataPropertyAndValuesList)
        {
            LogMetadataFilterResult logMetadataFilterResult = new()
            {
                LogEntries = FilterLogEntriesUsingMetadataProperties(allLogEntries, logMetadataPropertyAndValuesList),
                LogMetadataPropertyAndValuesList = logMetadataPropertyAndValuesList
            };

            UpdateLogMetadataValuesCount(logMetadataFilterResult.LogEntries, logMetadataPropertyAndValuesList);

            return logMetadataFilterResult;
        }

        /// <summary>
        /// Filters the log entries using the specified metadata properties and values.
        /// </summary>
        /// <param name="allLogEntries">The list of all log entries.</param>
        /// <param name="logMetadataPropertyAndValues">The list of metadata properties and values to filter by.</param>
        /// <returns>The list of filtered log entries.</returns>
        private static List<LogEntry> FilterLogEntriesUsingMetadataProperties(List<LogEntry> allLogEntries, List<LogMetadataPropertyAndValues> logMetadataPropertyAndValues)
        {
            if (allLogEntries == null) return [];

            if (logMetadataPropertyAndValues == null || logMetadataPropertyAndValues.Count == 0) return allLogEntries;

            // Create a dictionary containing only the properties and values where to search on
            Dictionary<LogMetadataProperty, HashSet<LogMetadataValue>> enabledFilterPropertiesAndValues = [];
            foreach (LogMetadataPropertyAndValues logMetadataPropertyAndValue in logMetadataPropertyAndValues)
            {
                if (!logMetadataPropertyAndValue.IsFilterEnabled) continue;

                HashSet<LogMetadataValue> logMetadataValues = [];
                foreach (KeyValuePair<LogMetadataValue, string> kvp in logMetadataPropertyAndValue.LogMetadataValues)
                {
                    if (!kvp.Key.IsFilterEnabled) continue;
                    logMetadataValues.Add(kvp.Key);
                }
                enabledFilterPropertiesAndValues[logMetadataPropertyAndValue.LogMetadataProperty] = logMetadataValues;
            }

            if (enabledFilterPropertiesAndValues.Count == 0) return allLogEntries;

            List<LogEntry> filteredLogEntries = allLogEntries;

            // Check for each property on which you can filter
            // Filtering between properties is an AND operation, filtering within the values of a property is an OR operation
            foreach (var kvp in enabledFilterPropertiesAndValues)
            {
                LogMetadataProperty logMetadataProperty = kvp.Key;
                HashSet<LogMetadataValue> logMetadataValues = kvp.Value;

                filteredLogEntries = [.. filteredLogEntries.Where(logEntry =>
                        logEntry.LogMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value) &&
                        logMetadataValues.Any(logMetadataValue => logMetadataValue.Value == value)
                    )];
            }

            return filteredLogEntries;
        }

        /// <summary>
        /// Updates the count of each metadata value based on the filtered log entries.
        /// </summary>
        /// <param name="logEntriesFiltered">The list of filtered log entries.</param>
        /// <param name="logMetadataPropertyAndValuesList">The list of metadata properties and values to update.</param>
        private static void UpdateLogMetadataValuesCount(List<LogEntry> logEntriesFiltered, List<LogMetadataPropertyAndValues> logMetadataPropertyAndValuesList)
        {
            // Get the (new) properties and values available for the filtered logentries
            List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesNewCount = LogEntryClassifier.GetLogEntriesListOfMetadataPropertyAndValues(logEntriesFiltered, [.. logMetadataPropertyAndValuesList.Select(item => item.LogMetadataProperty)]);

            // Loop through all previously existing properties
            foreach (LogMetadataPropertyAndValues logMetadataPropertyAndValues in logMetadataPropertyAndValuesList)
            {
                // Obtain the new property matching the existing one
                LogMetadataPropertyAndValues propertyNewCount = LogMetadataPropertyAndValuesNewCount.FirstOrDefault(item => item.LogMetadataProperty == logMetadataPropertyAndValues.LogMetadataProperty, null);

                // Loop through all values of the existing property
                foreach (LogMetadataValue logMetadataValue in logMetadataPropertyAndValues.LogMetadataValues.Keys)
                {
                    int count = 0;
                    if (propertyNewCount != null)
                    {
                        LogMetadataValue value = propertyNewCount.LogMetadataValues.Keys.FirstOrDefault(item => item.Value == logMetadataValue.Value);

                        if (value != null) count = value.Count;
                    }
                    logMetadataValue.Count = count;
                }
            }
        }
    }
}