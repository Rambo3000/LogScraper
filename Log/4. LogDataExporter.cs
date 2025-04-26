using LogScraper.Export;
using LogScraper.Log.Collection;
using LogScraper.Log.Filter;
using LogScraper.Log.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogScraper.Log
{
    internal class LogDataExporter
    {
        /// <summary>
        /// Creates a LogExportData object based on the filtered log metadata, export settings, and display options.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <param name="reduceNumberOfLinesForDisplaying">Whether to reduce the number of lines for display purposes.</param>
        /// <returns>A LogExportData object containing the processed log data.</returns>
        public static LogExportData GenerateExportedLogData(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings, bool reduceNumberOfLinesForDisplaying)
        {
            // Calculate the start and end indices based on the begin and end filters and extra lines to include.
            (int startIndex, int endIndex) = CalculateExportRange(filterResult, logExportSettings);

            // If the calculated indices are invalid, return an empty LogExportData object.
            if (startIndex <= -1 || endIndex <= 0 || startIndex > endIndex) return new() { ExportRaw = string.Empty };

            return new()
            {
                DateTimeFirstLine = filterResult.LogLines[startIndex].TimeStamp,
                DateTimeLastLine = filterResult.LogLines[endIndex - 1].TimeStamp,
                LineCount = endIndex - startIndex,
                ExportRaw = GetLogLinesAsString(filterResult, reduceNumberOfLinesForDisplaying, startIndex, endIndex, logExportSettings)
            };
        }

        /// <summary>
        /// Determines the start and end indices for the log lines to be exported based on the export settings.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <returns>A tuple containing the start and end indices.</returns>
        private static (int startIndex, int endIndex) CalculateExportRange(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            int numberOfLinesTotal = filterResult.LogLines.Count;

            int startindex = 0;
            int endindex = numberOfLinesTotal;

            // Adjust the start index based on the LoglineBegin setting and extra lines to include.
            if (logExportSettings.LoglineBegin != null)
            {
                for (int i = 0; i < numberOfLinesTotal; i++)
                {
                    if (filterResult.LogLines[i] == logExportSettings.LoglineBegin)
                    {
                        startindex = i;
                        break;
                    }
                }
                startindex -= logExportSettings.ExtraLinesBegin;
                if (startindex < 0) startindex = 0; // Ensure the start index is not negative.
            }

            // Adjust the end index based on the LogLineEnd setting and extra lines to include.
            if (logExportSettings.LogLineEnd != null)
            {
                for (int i = 0; i < numberOfLinesTotal; i++)
                {
                    if (filterResult.LogLines[i] == logExportSettings.LogLineEnd)
                    {
                        endindex = i + 1;
                        break;
                    }
                }
                endindex += logExportSettings.ExtraLinesEnd;
                if (endindex > numberOfLinesTotal) endindex = numberOfLinesTotal; // Ensure the end index does not exceed the total lines.
            }

            return (startindex, endindex);
        }

        /// <summary>
        /// Converts the specified range of log lines into a single string, optionally reducing the number of lines for display.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="reduceNumberOfLinesForDisplaying">Whether to reduce the number of lines for display purposes.</param>
        /// <param name="startIndex">The starting index of the log lines to include.</param>
        /// <param name="endIndex">The ending index of the log lines to include.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <returns>A string containing the formatted log lines.</returns>
        private static string GetLogLinesAsString(LogMetadataFilterResult filterResult, bool reduceNumberOfLinesForDisplaying, int startIndex, int endIndex, LogExportSettings logExportSettings)
        {
            StringBuilder stringBuilder = new();
            const int maxNrOfRecordsShown = 1000;
            bool dottedLinesAdded = false;

            for (int i = startIndex; i < endIndex; i++)
            {
                // Skip lines in the middle if reducing the number of lines for display.
                if (reduceNumberOfLinesForDisplaying && i - startIndex > maxNrOfRecordsShown && endIndex - i > maxNrOfRecordsShown)
                {
                    if (!dottedLinesAdded)
                    {
                        // Add placeholder lines to indicate skipped content.
                        for (int j = 0; j < 10; j++)
                        {
                            stringBuilder.AppendLine("... <log is ingekort>");
                        }
                        dottedLinesAdded = true;
                    }
                    continue;
                }

                AppendLogLineToBuilder(stringBuilder, filterResult.LogLines[i], logExportSettings.LogExportSettingsMetadata);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Appends a log line and its metadata to the StringBuilder.
        /// </summary>
        /// <param name="stringbuilder">The StringBuilder to append to.</param>
        /// <param name="logLine">The log line to append.</param>
        /// <param name="logExportSettingsMetadata">Metadata settings for the log export.</param>
        private static void AppendLogLineToBuilder(StringBuilder stringbuilder, LogLine logLine, LogExportSettingsMetadata logExportSettingsMetadata)
        {
            string logLineMetadataFormatted = logLine.Line;

            // Modify the log line metadata if the original metadata is not to be shown.
            if (!logExportSettingsMetadata.ShowOriginalMetadata)
            {
                if (logExportSettingsMetadata.RemoveMetaDataCriteria != null)
                {
                    logLineMetadataFormatted = RemoveTextByCriteria(logLine.Line, logExportSettingsMetadata.RemoveMetaDataCriteria, logExportSettingsMetadata.MetadataStartPosition);
                }
                // Insert metadata at the original metadata position.
                logLineMetadataFormatted = InsertMetadataIntoLogLine(logLineMetadataFormatted, logExportSettingsMetadata.MetadataStartPosition, logLine.LogMetadataPropertiesWithStringValue, logExportSettingsMetadata);
            }

            stringbuilder.AppendLine(logLineMetadataFormatted);

            // Append any additional log lines associated with the current log line.
            if (logLine.AdditionalLogLines != null)
            {
                for (int j = 0; j < logLine.AdditionalLogLines.Count; j++)
                {
                    stringbuilder.AppendLine(logLine.AdditionalLogLines[j]);
                }
            }
        }

        /// <summary>
        /// Inserts metadata to a log line at the specified position.
        /// </summary>
        /// <param name="logLine">The original log line.</param>
        /// <param name="startIndex">The position to insert the metadata.</param>
        /// <param name="logMetadataPropertiesWithStringValue">The metadata properties and their string values.</param>
        /// <param name="logExportSettingsMetadata">Metadata settings for the log export.</param>
        /// <returns>The log line with added metadata.</returns>
        private static string InsertMetadataIntoLogLine(string logLine, int startIndex, Dictionary<LogMetadataProperty, string> logMetadataPropertiesWithStringValue, LogExportSettingsMetadata logExportSettingsMetadata)
        {
            if (startIndex <= 0 ||
                logExportSettingsMetadata.SelectedMetadataProperties == null ||
                logExportSettingsMetadata.SelectedMetadataProperties.Count == 0 ||
                logMetadataPropertiesWithStringValue == null ||
                logMetadataPropertiesWithStringValue.Count == 0)
            {
                return logLine;
            }

            List<string> values = [];
            foreach (LogMetadataProperty logMetadataProperty in logExportSettingsMetadata.SelectedMetadataProperties)
            {
                logMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value);
                if (value != null) { values.Add(value); }
            }

            // Insert the metadata values into the log line at the specified position.
            return logLine.Insert(startIndex, " " + string.Join(" | ", values));
        }

        /// <summary>
        /// Removes text from the input string based on the specified criteria.
        /// </summary>
        /// <param name="inputText">The input string.</param>
        /// <param name="criteria">The criteria for removing text.</param>
        /// <param name="startPosition">The starting position on the input text after which the filter criteria should be applied.</param>
        /// <returns>The modified string with the specified text removed.</returns>
        public static string RemoveTextByCriteria(string inputText, FilterCriteria criteria, int startPosition)
        {
            int startIndex = startPosition;

            if (string.IsNullOrEmpty(inputText) || criteria == null)
            {
                return inputText;
            }

            if (!string.IsNullOrEmpty(criteria.BeforePhrase))
            {
                // Find the index of the BeforePhrase.
                int beforeIndex = inputText.IndexOf(criteria.BeforePhrase, startIndex, StringComparison.Ordinal);

                if (beforeIndex != -1)
                {
                    // Find the index of the AfterPhrase.
                    int afterIndex = inputText.IndexOf(criteria.AfterPhrase, beforeIndex + 1, StringComparison.Ordinal);

                    if (afterIndex != -1)
                    {
                        // Remove the portion of text from BeforePhrase to AfterPhrase.
                        inputText = inputText.Remove(beforeIndex, afterIndex - beforeIndex + criteria.AfterPhrase.Length);
                    }
                }
            }
            else
            {
                // BeforePhrase is null, use the StartPosition and always use AfterPhrase.
                int afterIndex = inputText.IndexOf(criteria.AfterPhrase, startIndex, StringComparison.Ordinal);

                if (afterIndex != -1)
                {
                    // Remove the portion of text from StartPosition to AfterPhrase.
                    inputText = inputText.Remove(startIndex, afterIndex - startIndex + criteria.AfterPhrase.Length);
                }
            }

            return inputText;
        }
    }
}
