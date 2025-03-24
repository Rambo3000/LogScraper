using LogScraper.Log.Collection;
using LogScraper.Log.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace LogScraper.Log
{
    /// <summary>
    /// Provides methods to filter log lines based on metadata properties and values.
    /// </summary>
    internal static class LogMetadataFilter
    {
        /// <summary>
        /// Filters the log lines based on the provided metadata properties and values, and updates the count of each metadata value.
        /// </summary>
        /// <param name="allLogLines">The list of all log lines.</param>
        /// <param name="logMetadataPropertyAndValuesList">The list of metadata properties and values to filter by.</param>
        /// <returns>A result containing the filtered log lines and the updated metadata properties and values.</returns>
        public static LogMetadataFilterResult GetLogMetadataFilterResult(List<LogLine> allLogLines, List<LogMetadataPropertyAndValues> logMetadataPropertyAndValuesList)
        {
            LogMetadataFilterResult logMetadataFilterResult = new()
            {
                LogLines = FilterLogLinesUsingMetadataProperties(allLogLines, logMetadataPropertyAndValuesList),
                LogMetadataPropertyAndValuesList = logMetadataPropertyAndValuesList
            };

            UpdateLogMetadataValuesCount(logMetadataFilterResult.LogLines, logMetadataPropertyAndValuesList);

            return logMetadataFilterResult;
        }

        /// <summary>
        /// Filters the log lines using the specified metadata properties and values.
        /// </summary>
        /// <param name="allLogLines">The list of all log lines.</param>
        /// <param name="logMetadataPropertyAndValues">The list of metadata properties and values to filter by.</param>
        /// <returns>The list of filtered log lines.</returns>
        private static List<LogLine> FilterLogLinesUsingMetadataProperties(List<LogLine> allLogLines, List<LogMetadataPropertyAndValues> logMetadataPropertyAndValues)
        {
            if (allLogLines == null) return [];

            if (logMetadataPropertyAndValues == null || logMetadataPropertyAndValues.Count == 0) return allLogLines;

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

            if (enabledFilterPropertiesAndValues.Count == 0) return allLogLines;

            List<LogLine> filteredLogLines = allLogLines;

            // Check for each property on which you can filter
            // Filtering between properties is an AND operation, filtering within the values of a property is an OR operation
            foreach (var kvp in enabledFilterPropertiesAndValues)
            {
                LogMetadataProperty logMetadataProperty = kvp.Key;
                HashSet<LogMetadataValue> logMetadataValues = kvp.Value;

                filteredLogLines = [.. filteredLogLines.Where(logLine =>
                        logLine.LogMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value) &&
                        logMetadataValues.Any(logMetadataValue => logMetadataValue.Value == value)
                    )];
            }

            return filteredLogLines;
        }

        /// <summary>
        /// Updates the count of each metadata value based on the filtered log lines.
        /// </summary>
        /// <param name="logLinesFiltered">The list of filtered log lines.</param>
        /// <param name="logMetadataPropertyAndValuesList">The list of metadata properties and values to update.</param>
        private static void UpdateLogMetadataValuesCount(List<LogLine> logLinesFiltered, List<LogMetadataPropertyAndValues> logMetadataPropertyAndValuesList)
        {
            // Get the (new) properties and values available for the filtered loglines
            List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesNewCount = LogLineClassifier.GetLogLinesListOfMetadataPropertyAndValues(logLinesFiltered, [.. logMetadataPropertyAndValuesList.Select(item => item.LogMetadataProperty)]);

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