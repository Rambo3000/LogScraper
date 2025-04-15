using System.Collections.Generic;
using System.Linq;
using LogScraper.Log.Collection;
using LogScraper.Log.Filter;
using LogScraper.Log.Metadata;

namespace LogScraper.Log
{
    internal class LogLineClassifier
    {
        public static List<LogMetadataPropertyAndValues> GetLogLinesListOfMetadataPropertyAndValues(List<LogLine> logLines, List<LogMetadataProperty> logMetadataProperties)
        {
            List<LogMetadataPropertyAndValues> logMetadataPropertyAndValuesList = [];

            if (logLines == null || logMetadataProperties == null) return logMetadataPropertyAndValuesList;

            Dictionary<LogMetadataProperty, Dictionary<string, int>> valueCounts = [];

            foreach (LogMetadataProperty logMetadataProperty in logMetadataProperties)
            {
                valueCounts[logMetadataProperty] = [];
            }

            // Iterate through each LogLine in the LogCollection.
            foreach (LogLine logLine in logLines)
                {
                foreach (LogMetadataProperty logMetadataProperty in logMetadataProperties)
                {
                    if (!logLine.LogMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string propertyValue))
                    {
                        // LogMetadataProperties is not present in the LogLine, skip it.
                        continue;
                    }

                    Dictionary<string,int> ValueCountDictionary = valueCounts[logMetadataProperty];
                    if (ValueCountDictionary.TryGetValue(propertyValue, out int value))
                    {
                        //Increment the value count
                        ValueCountDictionary[propertyValue] = ++value;
                    }
                    else
                    {
                        //Initialize at 1 
                        ValueCountDictionary[propertyValue] = 1;
                    }
                }
            }

            // Create LogMetadataPropertyAndValues objects based on the value counts.
            foreach (LogMetadataProperty logMetadataProperty in logMetadataProperties)
            {
                LogMetadataPropertyAndValues LogMetadataPropertyAndValues = new()
                {
                    LogMetadataValues = valueCounts[logMetadataProperty].ToDictionary(
                            kvp => new LogMetadataValue(kvp.Key, kvp.Value, false),
                            kvp => kvp.Value.ToString()
                        ),

                    LogMetadataProperty = logMetadataProperty
                };

                logMetadataPropertyAndValuesList.Add(LogMetadataPropertyAndValues);
            }

            return logMetadataPropertyAndValuesList;
        }
        public static void ClassifyLogLineMetadataProperties(LogLayout logLayout, LogCollection logCollection)
        {
            if (logCollection == null || logLayout.LogMetadataProperties == null) return;

            foreach (var logLine in logCollection.LogLines)
            {
                if (logLine.LogMetadataPropertiesWithStringValue != null) continue;

                logLine.LogMetadataPropertiesWithStringValue = [];

                // Determine and add log properties and their values to the LogLine based on LogMetadataProperties.
                foreach (var logMetadataProperty in logLayout.LogMetadataProperties)
                {
                    // Extract and store the property value in the dictionary.
                    string propertyValue = ExtractValue(logLine.Line, logMetadataProperty.Criteria, true, logLayout.StartPosition);
                    if (propertyValue != null) logLine.LogMetadataPropertiesWithStringValue[logMetadataProperty] = propertyValue;
                    if (propertyValue == "ERROR") logCollection.ErrorCount++;
                }
            }
        }
        public static void ClassifyLogLineContentProperties(LogLayout logLayout, LogCollection logCollection)
        {
            if (logCollection == null || logLayout.LogContentBeginEndFilters == null) return;

            foreach (var logLine in logCollection.LogLines)
            {
                if (logLine.LogContentProperties != null) continue;

                logLine.LogContentProperties = [];

                // Determine and add log properties and their values to the LogLine based on LogMetadataProperties.
                foreach (var LogContent in logLayout.LogContentBeginEndFilters)
                {
                    // Extract and store the property value in the dictionary.
                    string value = ExtractValue(logLine.Line, LogContent.Criteria, false, logLayout.StartPosition);
                    if (value != null) logLine.LogContentProperties[LogContent] = logLine.TimeStamp.ToString("HH:mm:ss") + " " + value;
                }
            }
        }
        private static string ExtractValue(string logLine, FilterCriteria criteria, bool afterPhraseManditory, int startPosition)
        {
            if (criteria == null || string.IsNullOrEmpty(criteria.BeforePhrase) || afterPhraseManditory && string.IsNullOrEmpty(criteria.AfterPhrase)) return null;

            int startIndex = logLine.IndexOf(criteria.BeforePhrase, startPosition);

            if (startIndex == -1) return null;

            startIndex += criteria.BeforePhrase.Length;

            int endIndex = criteria.AfterPhrase == null ? -1 : logLine.IndexOf(criteria.AfterPhrase, startIndex);

            if (afterPhraseManditory && endIndex == -1)
            {
                return null;
            }
            else if (endIndex == -1)
            {
                endIndex = logLine.Length;
            }

            if (endIndex == startIndex) return string.Empty;

            return logLine[startIndex..endIndex];
        }
    }
}
