using System;
using System.Collections.Generic;
using System.Text;
using LogScraper.Export;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log
{
    /// <summary>
    /// This class is responsible for exporting log data based on the filtered log metadata and export settings.
    /// </summary>
    internal class LogDataExporter
    {
        /// <summary>
        /// Creates a single string based on the filtered log metadata, export settings, and display options.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <param name="reduceNumberOfLogEntriesForDisplaying">Whether to reduce the number of log entries for display purposes.</param>
        /// <returns>A single string containing the processed log data.</returns>
        public static string CreateExportedLog(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings, bool reduceNumberOfLogEntriesForDisplaying, out int entryCount)
        {
            entryCount = 0;
            // Calculate the start and end indices based on the begin and end filters and extra nog entries to include.
            (int startIndex, int endIndex) = CalculateExportRange(filterResult, logExportSettings);

            // If the calculated indices are invalid, return an empty ExportedLogData object.
            if (startIndex <= -1 || endIndex <= 0 || startIndex > endIndex) return string.Empty;

            entryCount = endIndex - startIndex;

            return GetLogEntriesAsString(filterResult, reduceNumberOfLogEntriesForDisplaying, startIndex, endIndex, logExportSettings);
        }

        /// <summary>
        /// Determines the start and end indices for the log entries to be exported based on the export settings.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <returns>A tuple containing the start and end indices.</returns>
        private static (int startIndex, int endIndex) CalculateExportRange(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            int numberOfLogEntriesTotal = filterResult.LogEntries.Count;

            int startindex = 0;
            int endindex = numberOfLogEntriesTotal;

            // Adjust the start index based on the LogEntryBegin setting and extra log entries to include.
            if (logExportSettings.LogEntryBegin != null)
            {
                for (int i = 0; i < numberOfLogEntriesTotal; i++)
                {
                    if (filterResult.LogEntries[i] == logExportSettings.LogEntryBegin)
                    {
                        startindex = i;
                        break;
                    }
                }
                startindex -= logExportSettings.ExtraLogEntriesBegin;
                if (startindex < 0) startindex = 0; // Ensure the start index is not negative.
            }

            // Adjust the end index based on the LogEntryEnd setting and extra log entries to include.
            if (logExportSettings.LogEntryEnd != null)
            {
                for (int i = 0; i < numberOfLogEntriesTotal; i++)
                {
                    if (filterResult.LogEntries[i] == logExportSettings.LogEntryEnd)
                    {
                        endindex = i + 1;
                        break;
                    }
                }
                endindex += logExportSettings.ExtraLogEntriesEnd;
                if (endindex > numberOfLogEntriesTotal) endindex = numberOfLogEntriesTotal; // Ensure the end index does not exceed the total log entries.
            }

            return (startindex, endindex);
        }

        /// <summary>
        /// Converts the specified range of log entries into a single string, optionally reducing the number of log entries for display.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="reduceNumberOfLog entriesForDisplaying">Whether to reduce the number of log entries for display purposes.</param>
        /// <param name="startIndex">The starting index of the log entries to include.</param>
        /// <param name="endIndex">The ending index of the log entries to include.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <returns>A string containing the formatted log entries.</returns>
        private static string GetLogEntriesAsString(LogMetadataFilterResult filterResult, bool reduceNumberOfLogEntriesForDisplaying, int startIndex, int endIndex, LogExportSettings logExportSettings)
        {
            StringBuilder stringBuilder = new();
            const int maxNrOfRecordsShown = 1000;
            bool dottedLogEntriesAdded = false;

            for (int i = startIndex; i < endIndex; i++)
            {
                // Skip log entries in the middle if reducing the number of log entries for display.
                if (reduceNumberOfLogEntriesForDisplaying && i - startIndex > maxNrOfRecordsShown && endIndex - i > maxNrOfRecordsShown)
                {
                    if (!dottedLogEntriesAdded)
                    {
                        // Add placeholder log entries to indicate skipped content.
                        for (int j = 0; j < 10; j++)
                        {
                            stringBuilder.AppendLine("... <log is ingekort>");
                        }
                        dottedLogEntriesAdded = true;
                    }
                    continue;
                }

                AppendLogEntryToBuilder(stringBuilder, filterResult.LogEntries[i], logExportSettings);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Appends a log entry and its metadata to the StringBuilder.
        /// </summary>
        /// <param name="stringbuilder">The StringBuilder to append to.</param>
        /// <param name="logEntry">The log entry to append.</param>
        /// <param name="logExportSettingsMetadata">Metadata settings for the log export.</param>
        private static void AppendLogEntryToBuilder(StringBuilder stringbuilder, LogEntry logEntry, LogExportSettings logExportSettings)
        {
            string logEntryMetadataFormatted = logEntry.Entry;

            // Modify the log entry metadata if the original metadata is not to be shown.
            if (!logExportSettings.ShowOriginalMetadata)
            {
                if (logExportSettings.LogLayout.RemoveMetaDataCriteria != null)
                {
                    logEntryMetadataFormatted = RemoveTextByCriteria(logEntry.Entry, logExportSettings.LogLayout.StartIndexMetadata, logEntry.StartIndexContent);
                }
                // Insert metadata at the original metadata position.
                logEntryMetadataFormatted = InsertMetadataIntoLogEntry(logEntryMetadataFormatted, logExportSettings.LogLayout.StartIndexMetadata, logEntry.LogMetadataPropertiesWithStringValue, logExportSettings);
            }

            stringbuilder.AppendLine(logEntryMetadataFormatted);

            // Append any additional log entries associated with the current log entry.
            if (logEntry.AdditionalLogEntries != null)
            {
                for (int j = 0; j < logEntry.AdditionalLogEntries.Count; j++)
                {
                    stringbuilder.AppendLine(logEntry.AdditionalLogEntries[j]);
                }
            }
        }

        /// <summary>
        /// Inserts metadata to a log entry at the specified position.
        /// </summary>
        /// <param name="logEntry">The original log entry.</param>
        /// <param name="startIndex">The position to insert the metadata.</param>
        /// <param name="logMetadataPropertiesWithStringValue">The metadata properties and their string values.</param>
        /// <param name="logExportSettingsMetadata">Metadata settings for the log export.</param>
        /// <returns>The log entry with added metadata.</returns>
        private static string InsertMetadataIntoLogEntry(string logEntry, int startIndex, IndexDictionary<LogMetadataProperty, string> logMetadataPropertiesWithStringValue, LogExportSettings logExportSettings)
        {
            if (startIndex <= 0 ||
                logExportSettings.SelectedMetadataProperties == null ||
                logExportSettings.SelectedMetadataProperties.Count == 0 ||
                logMetadataPropertiesWithStringValue == null ||
                logMetadataPropertiesWithStringValue.Count == 0)
            {
                return logEntry;
            }

            List<string> values = [];
            foreach (LogMetadataProperty logMetadataProperty in logExportSettings.SelectedMetadataProperties)
            {
                logMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value);
                if (value != null) { values.Add(value); }
            }

            // Insert the metadata values into the log entry at the specified position.
            return logEntry.Insert(startIndex, " " + string.Join(" | ", values));
        }

        /// <summary>
        /// Removes text from the input string based on the specified criteria.
        /// </summary>
        /// <param name="inputText">The input string.</param>
        /// <param name="criteria">The criteria for removing text.</param>
        /// <param name="startPosition">The starting position on the input text after which the filter criteria should be applied.</param>
        /// <returns>The modified string with the specified text removed.</returns>
        public static string RemoveTextByCriteria(string inputText, int beforeIndex, int afterIndex)
        {

            if (string.IsNullOrEmpty(inputText) || beforeIndex == -1 || afterIndex == -1)
            {
                return inputText;
            }
            // Remove the portion of text from StartPosition to AfterPhrase.
            inputText = inputText.Remove(beforeIndex, afterIndex - beforeIndex);

            return inputText;
        }
    }
}
