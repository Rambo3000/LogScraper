﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogScraper.Log.Content;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log
{
    /// <summary>
    /// Classifies log entries based on metadata properties and content properties.
    /// </summary>
    internal class LogEntryClassifier
    {
        /// <summary>
        /// Processes a list of log entries and metadata properties to generate a list of metadata properties
        /// and their available values across the logentries, along with count of each value.
        /// </summary>
        /// <param name="logEntries">The list of log entries to process.</param>
        /// <param name="logMetadataProperties">The metadata properties to extract from the log entries.</param>
        /// <returns>A list of LogMetadataPropertyAndValues objects containing metadata properties and their values.</returns>
        public static List<LogMetadataPropertyAndValues> GetLogEntriesListOfMetadataPropertyAndValues(List<LogEntry> logEntries, List<LogMetadataProperty> logMetadataProperties)
        {
            List<LogMetadataPropertyAndValues> logMetadataPropertyAndValuesList = [];

            if (logEntries == null || logMetadataProperties == null) return logMetadataPropertyAndValuesList;

            // Dictionary to store the count of each value for each metadata property.
            Dictionary<LogMetadataProperty, Dictionary<string, int>> valueCounts = [];

            foreach (LogMetadataProperty logMetadataProperty in logMetadataProperties)
            {
                valueCounts[logMetadataProperty] = [];
            }

            // Iterate through each log entry to populate the value counts.
            foreach (LogEntry logEntry in logEntries)
            {
                foreach (LogMetadataProperty logMetadataProperty in logMetadataProperties)
                {
                    // Try to get the value of the current metadata property from the log entry.
                    if (!logEntry.LogMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string propertyValue))
                    {
                        // If the metadata property is not present in the log entry, skip it.
                        continue;
                    }

                    // Get the dictionary for the current metadata property.
                    Dictionary<string, int> ValueCountDictionary = valueCounts[logMetadataProperty];

                    // Increment the count for the property value or initialize it to 1 if it doesn't exist.
                    if (ValueCountDictionary.TryGetValue(propertyValue, out int value))
                    {
                        ValueCountDictionary[propertyValue] = ++value;
                    }
                    else
                    {
                        ValueCountDictionary[propertyValue] = 1;
                    }
                }
            }

            // Create LogMetadataPropertyAndValues objects based on the value counts.
            foreach (LogMetadataProperty logMetadataProperty in logMetadataProperties)
            {
                LogMetadataPropertyAndValues LogMetadataPropertyAndValues = new()
                {
                    // Convert the value counts into a dictionary of LogMetadataValue objects.
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

        /// <summary>
        /// Classifies the metadata and content properties of log entries in the specified log collection based on the
        /// provided log layout.
        /// </summary>
        /// <remarks>This method processes each log entry in the collection in parallel, applying
        /// classification rules for both metadata and content properties. The classification results are stored within
        /// the log entries.</remarks>
        /// <param name="logLayout">The layout definition that specifies the structure and classification rules for log entries.</param>
        /// <param name="logCollection">The collection of log entries to classify.</param>
        public static void ClassifyMetadataAndContentProperties(LogLayout logLayout, LogCollection logCollection)
        {
            if (logCollection == null) return;

            Parallel.ForEach(logCollection.LogEntries, logEntry =>
            {
                SetLogEntryStartPositionContent(logEntry, logLayout.RemoveMetaDataCriteria.AfterPhrase, logLayout.StartIndexMetadata);
                ClassifyLogEntryMetadataProperties(logEntry, logLayout);
                ClassifyLogEntryContentProperties(logEntry, logLayout, out bool errorFound);
                if (errorFound) Interlocked.Increment(ref logCollection.ErrorCount);
            });
        }

        /// <summary>
        /// Sets the starting position of the content within a log entry based on a specified metadata content
        /// separator.
        /// </summary>
        /// <remarks>If the specified metadata content separator is not found in the log entry starting
        /// from <paramref name="startIndex"/>, the content start position is set to <paramref name="startIndex"/>.
        /// Otherwise, the position is set to the index immediately following the separator.</remarks>
        /// <param name="logEntry">The log entry object whose content start position is to be set.</param>
        /// <param name="metadataContentSeperator">The string used to separate metadata from content within the log entry.</param>
        /// <param name="startIndex">The index at which to begin searching for the metadata content separator.</param>
        private static void SetLogEntryStartPositionContent(LogEntry logEntry, string metadataContentSeperator, int startIndex)
        {
            int index = logEntry.Entry.IndexOf(metadataContentSeperator, startIndex);
            if (index == -1)
            {
                // If the seperator is not found, start searching after the startIndex
                index = startIndex;
            }
            else
            {
                index += metadataContentSeperator.Length;
            }
            logEntry.StartIndexContent = index;
        }
        /// <summary>
        /// Classifies metadata properties for a log entry based on the provided log layout and adds this to the log entry.
        /// </summary>
        /// <param name="logEntry">The log entry to classify metadata properties for.</param>
        /// <param name="logLayout">The layout defining metadata properties and their criteria.</param>
        /// <param name="logCollection">The collection of log entries, the number of errors found is updated here.</param>
        private static void ClassifyLogEntryMetadataProperties(LogEntry logEntry, LogLayout logLayout)
        {
            // Skip log entries that already have metadata properties classified.
            if (logLayout.LogMetadataProperties == null || logEntry.LogMetadataPropertiesWithStringValue != null) return;

            logEntry.LogMetadataPropertiesWithStringValue = new IndexDictionary<LogMetadataProperty, string>(logLayout.LogMetadataProperties.Count);

            // Determine and add metadata properties and their values to the log entry.
            foreach (var logMetadataProperty in logLayout.LogMetadataProperties)
            {
                // Extract the property value based on the criteria.
                string propertyValue = ExtractValue(logEntry.Entry, logMetadataProperty.Criteria, true, logLayout.StartIndexMetadata);

                // Add the property value to the log entry if it exists.
                if (propertyValue != null) logEntry.LogMetadataPropertiesWithStringValue[logMetadataProperty] = propertyValue;
            }
        }

        /// <summary>
        /// Classifies content properties for a log entry based on the provided log layout and adds this to the log entry.
        /// </summary>
        /// <param name="logEntry">The log entry to classify content properties for.</param>
        /// <param name="logLayout">The layout defining content properties and their criteria.</param>
        private static void ClassifyLogEntryContentProperties(LogEntry logEntry, LogLayout logLayout, out bool errorFound)
        {
            errorFound = false;

            // Skip log entries that already have content properties classified.
            if (logLayout.LogMetadataProperties == null || logEntry.LogContentProperties != null) return;

            logEntry.LogContentProperties = new IndexDictionary<LogContentProperty, LogContentValue>(logLayout.LogContentProperties.Count);

            // Determine and add content properties and their values to the log entry.
            foreach (LogContentProperty logContentProperty in logLayout.LogContentProperties)
            {
                string value = null;
                foreach (FilterCriteria filterCriteria in logContentProperty.Criterias)
                {
                    // Extract the content value based on the criteria.
                    value = ExtractValue(logEntry.Entry, filterCriteria, false, logEntry.StartIndexContent);
                    // Add the content value to the log entry if it exists.
                    if (value != null)
                    {
                        logEntry.LogContentProperties[logContentProperty] = new LogContentValue(value.Trim(), logEntry.TimeStamp.ToString("HH:mm:ss"));
                        if (logContentProperty.IsErrorProperty) errorFound = true;
                        break; // Exit the loop after finding a valid value.
                    }
                }
            }
        }

        /// <summary>
        /// Extracts a value from a log entry based on the specified filter criteria.
        /// </summary>
        /// <param name="logEntry">The log entry to extract the value from.</param>
        /// <param name="criteria">The criteria defining the extraction rules.</param>
        /// <param name="afterPhraseManditory">Indicates whether the after phrase is mandatory for extraction.</param>
        /// <param name="startPosition">The starting position for the search.</param>
        /// <returns>The extracted value, or null if the criteria are not met.</returns>
        private static string ExtractValue(string logEntry, FilterCriteria criteria, bool afterPhraseManditory, int startPosition)
        {
            // Return null if the criteria are invalid or mandatory phrases are missing.
            if (criteria == null || string.IsNullOrEmpty(criteria.BeforePhrase) || afterPhraseManditory && string.IsNullOrEmpty(criteria.AfterPhrase)) return null;

            // Find the start index of the before phrase.
            int startIndex = logEntry.IndexOf(criteria.BeforePhrase, startPosition);

            if (startIndex == -1) return null;

            // Move the start index to the end of the before phrase.
            startIndex += criteria.BeforePhrase.Length;

            // Find the end index of the after phrase, if it exists.
            int endIndex = criteria.AfterPhrase == null ? -1 : logEntry.IndexOf(criteria.AfterPhrase, startIndex);

            // Handle cases where the after phrase is mandatory but not found.
            if (afterPhraseManditory && endIndex == -1) return null;

            // Use the end of the log entry if no after phrase is specified.
            if (endIndex == -1) endIndex = logEntry.Length;

            // Return an empty string if the start and end indices are the same.
            if (endIndex == startIndex) return string.Empty;

            // Extract and return the substring between the start and end indices.
            return logEntry[startIndex..endIndex];
        }
    }
}
