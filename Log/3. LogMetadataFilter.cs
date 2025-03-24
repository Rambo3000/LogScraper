using LogScraper.Log.Collection;
using LogScraper.Log.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace LogScraper.Log
{
    internal static class LogMetadataFilter
    {
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
        private static List<LogLine> FilterLogLinesUsingMetadataProperties(List<LogLine> allLogLines, List<LogMetadataPropertyAndValues> logMetadataPropertyAndValues)
        {
            if (allLogLines == null) return [];

            if (logMetadataPropertyAndValues == null || logMetadataPropertyAndValues.Count == 0) return allLogLines;

            // Create a dictionary containing only the properties and values where to search on
            Dictionary<LogMetadataProperty, List<LogMetadataValue>> enabledFilterPropertiesAndValues = [];
            foreach (LogMetadataPropertyAndValues logMetadataPropertyAndValue in logMetadataPropertyAndValues)
            {
                if (logMetadataPropertyAndValue.IsFilterEnabled == false) continue;

                List<LogMetadataValue> logMetadataValues = [];
                foreach (KeyValuePair<LogMetadataValue, string> kvp in logMetadataPropertyAndValue.LogMetadataValues)
                {
                    if (kvp.Key.IsFilterEnabled == false) continue;
                    logMetadataValues.Add(kvp.Key);
                }
                enabledFilterPropertiesAndValues.Add(logMetadataPropertyAndValue.LogMetadataProperty, logMetadataValues);
            }

            if (enabledFilterPropertiesAndValues.Count == 0) return allLogLines;

            List<LogLine> filteredLogLines = allLogLines;
            // Check for each property on which you can filter
            // Filtering between properties is an AND operation, filtering within the values of a property is an OR operation
            foreach (LogMetadataProperty logMetadataProperty in enabledFilterPropertiesAndValues.Keys)
            {
                List<LogLine> newlyFilteredLogLines = [];
                List<LogMetadataValue> logMetadataValues = enabledFilterPropertiesAndValues[logMetadataProperty];
                foreach (LogLine logLine in filteredLogLines)
                {
                    // Check if the LogLine has all the specified LogMetadataPropertyAndValuesList and they match the criteria.
                    if (FilterSingleLogLine(logLine, logMetadataProperty, logMetadataValues))
                    {
                        newlyFilteredLogLines.Add(logLine);
                    }
                }

                // Feedback the newly filtered lines as a shortened list for the next property
                filteredLogLines = newlyFilteredLogLines;
            }

            return filteredLogLines;
        }
        private static bool FilterSingleLogLine(LogLine logLine, LogMetadataProperty logMetadataProperty, List<LogMetadataValue> logMetadataValues)
        {
            if (!logLine.LogMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value)) return false;

            // Check if the value of the logline is included in the selected values of the user
            foreach (LogMetadataValue logMetadataValue in logMetadataValues)
            {
                if (value == logMetadataValue.Value)
                {
                    return true;
                }
            }
            return false;
        }
        private static void UpdateLogMetadataValuesCount(List<LogLine> logLinesFiltered, List<LogMetadataPropertyAndValues> logMetadataPropertyAndValuesList)
        {
            // Get the (new) properties and values available for the filtered loglines
            List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesNewCount = LogLineClassifier.GetLogLinesListOfMetadataPropertyAndValues(logLinesFiltered, logMetadataPropertyAndValuesList.Select(item => item.LogMetadataProperty).ToList());

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
