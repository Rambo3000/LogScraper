using LogScraper.Export;
using LogScraper.Log.Collection;
using LogScraper.Log.Filter;
using LogScraper.Log.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogScraper.Log
{
    internal class LogExportDataCreator
    {
        public static LogExportData CreateLogExportData(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings, bool reduceNumberOfLinesForDisplaying)
        {
            (int startIndex, int endIndex) = GetStartAndEndIndex(filterResult, logExportSettings);

            if (startIndex <= -1 || endIndex <= 0 || startIndex > endIndex) return new() { ExportRaw = string.Empty };

            return new()
            {
                DateTimeFirstLine = filterResult.LogLines[startIndex].TimeStamp,
                DateTimeLastLine = filterResult.LogLines[endIndex - 1].TimeStamp,
                LineCount = endIndex - startIndex,
                ExportRaw = GetLogLinesAsString(filterResult, reduceNumberOfLinesForDisplaying, startIndex, endIndex, logExportSettings)
            };

        }
        private static (int startIndex, int endIndex) GetStartAndEndIndex(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            int numberOfLinesTotal = filterResult.LogLines.Count;

            int startindex = 0;
            int endindex = numberOfLinesTotal;
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
                if (startindex < 0) startindex = 0;
            }
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
                if (endindex > numberOfLinesTotal) endindex = numberOfLinesTotal;
            }

            return (startindex, endindex);
        }
        private static string GetLogLinesAsString(LogMetadataFilterResult filterResult, bool reduceNumberOfLinesForDisplaying, int startIndex, int endIndex, LogExportSettings logExportSettings)
        {
            StringBuilder stringBuilder = new();
            const int maxNrOfRecordsShown = 1000;
            bool dottedLinesAdded = false;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (reduceNumberOfLinesForDisplaying && i - startIndex > maxNrOfRecordsShown && endIndex - i > maxNrOfRecordsShown)
                {
                    if (dottedLinesAdded == false)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            stringBuilder.AppendLine("... <log is ingekort>");
                        }
                        dottedLinesAdded = true;
                    }
                    continue;
                }

                StringBuilderAppendLogLine(stringBuilder, filterResult.LogLines[i], logExportSettings.LogExportSettingsMetadata);
            }
            return stringBuilder.ToString();
        }
        private static void StringBuilderAppendLogLine(StringBuilder stringbuilder, LogLine logLine, LogExportSettingsMetadata logExportSettingsMetadata)
        {
            string logLineMetadataFormatted = logLine.Line;
            if (!logExportSettingsMetadata.ShowOriginalMetadata)
            {
                if (logExportSettingsMetadata.RemoveMetaDataCriteria != null)
                {
                    logLineMetadataFormatted = RemoveTextBasedOnCriteria(logLine.Line, logExportSettingsMetadata.RemoveMetaDataCriteria, logExportSettingsMetadata.MetadataStartPosition);
                }
                logLineMetadataFormatted = AddMetadata(logLineMetadataFormatted, logExportSettingsMetadata.MetadataStartPosition, logLine.LogMetadataPropertiesWithStringValue, logExportSettingsMetadata);
            }

            stringbuilder.AppendLine(logLineMetadataFormatted);

            if (logLine.AdditionalLogLines != null)
            {
                for (int j = 0; j < logLine.AdditionalLogLines.Count; j++)
                {
                    stringbuilder.AppendLine(logLine.AdditionalLogLines[j]);
                }
            }
        }

        private static string AddMetadata(string logLine, int startIndex, Dictionary<LogMetadataProperty, string> logMetadataPropertiesWithStringValue, LogExportSettingsMetadata logExportSettingsMetadata)
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

            return logLine.Insert(startIndex, " " + string.Join(" | ", values));
        }

        public static string RemoveTextBasedOnCriteria(string inputText, FilterCriteria criteria, int startPosition)
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
