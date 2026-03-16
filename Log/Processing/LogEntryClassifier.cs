using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LogScraper.Log.Content;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log.Processing
{
    /// <summary>
    /// Classifies log entries by extracting and assigning metadata and content properties
    /// based on the provided log layout.
    /// </summary>
    internal static class LogEntryClassifier
    {
        /// <summary>
        /// Classifies metadata and content properties for all log entries in the collection.
        /// Skips entries that have already been classified, allowing safe incremental calls.
        /// </summary>
        /// <remarks>
        /// Uses a two-phase approach: parallel extraction of raw strings, followed by
        /// sequential interning of shared LogMetadataValue instances via the LogCollection pool.
        /// </remarks>
        /// <param name="logLayout">The layout defining extraction rules for metadata and content properties.</param>
        /// <param name="logCollection">The collection of log entries to classify.</param>
        public static void Classify(LogLayout logLayout, LogCollection logCollection)
        {
            if (logCollection == null) return;

            // Just-in-time compile the regex parser if needed.
            if (logLayout.RemoveMetaDataCriteria != null && logLayout.RemoveMetaDataCriteria.IsRegex && logLayout.RemoveMetaDataCriteria.RegexCompiled == null)
                logLayout.RemoveMetaDataCriteria.RegexCompiled = new Regex(logLayout.RemoveMetaDataCriteria.AfterPhrase, RegexOptions.Compiled);

            int logMetadataPropertyCount = logLayout.LogMetadataProperties?.Count ?? 0;

            // Create a 2D array to hold raw metadata values for each property and log entry during parallel processing.
            string[][] rawMetadata = new string[logCollection.LogEntries.Count][];

            // Phase 1 — parallel: extract raw string values and classify content properties.
            Parallel.For(0, logCollection.LogEntries.Count, i =>
            {
                LogEntry logEntry = logCollection.LogEntries[i];

                // Skip entries already classified, which happens on incremental loads.
                if (logEntry.Metadata != null) return;

                SetContentStartPosition(logEntry, logLayout);
                ExtractRawMetadataValues(logEntry, logLayout, logMetadataPropertyCount, rawMetadata);

                logEntry.Metadata = new IndexDictionary<LogMetadataProperty, LogMetadataValue>(logMetadataPropertyCount);

                if (TryClassifyContentProperties(logEntry, logLayout))
                {
                    if (logEntry.IsErrorLogEntry) Interlocked.Increment(ref logCollection.ErrorCount);
                }
            });

            // Phase 2 — sequential: intern raw strings into shared LogMetadataValue instances.
            InternRawMetadata(logCollection, rawMetadata, logLayout, logMetadataPropertyCount);
        }

        /// <summary>
        /// Interns raw extracted metadata strings into shared LogMetadataValue instances
        /// and assigns them to each log entry's Metadata dictionary.
        /// </summary>
        private static void InternRawMetadata(LogCollection logCollection, string[][] rawMetadata, LogLayout logLayout, int logMetadataPropertyCount)
        {
            for (int i = 0; i < logCollection.LogEntries.Count; i++)
            {
                if (rawMetadata[i] == null) continue;

                for (int j = 0; j < logMetadataPropertyCount; j++)
                {
                    if (rawMetadata[i][j] == null) continue;

                    LogMetadataProperty logMetadataProperty = logLayout.LogMetadataProperties[j];
                    logCollection.LogEntries[i].Metadata[logMetadataProperty] = logCollection.GetSharedLogMetadataValueObject(logMetadataProperty, rawMetadata[i][j]);
                }
            }
        }

        /// <summary>
        /// Sets the content start position on a log entry by locating the metadata/content separator.
        /// Falls back to StartIndexMetadata if the separator is not found.
        /// </summary>
        private static void SetContentStartPosition(LogEntry logEntry, LogLayout logLayout)
        {
            if (logEntry.StartIndexContent != 0) return; // Content start index already set, skip processing. 

            int index = -1;

            if (logLayout.RemoveMetaDataCriteria.IsRegex)
            {
                Match match = logLayout.RemoveMetaDataCriteria.RegexCompiled.Match(logEntry.Entry, logEntry.StartIndexMetadata);
                if (match.Success)
                    index = match.Index + match.Length;
            }
            else
            {
                index = logEntry.Entry.IndexOf(logLayout.RemoveMetaDataCriteria.AfterPhrase, logEntry.StartIndexMetadata, StringComparison.Ordinal);
                if (index != -1)
                    index += logLayout.RemoveMetaDataCriteria.AfterPhrase.Length;
            }

            // Fallback: no match found, set to startIndexMetadata.
            if (index == -1) index = logEntry.StartIndexMetadata;
            logEntry.StartIndexContent = index;
        }

        /// <summary>
        /// Extracts raw metadata string values from a log entry and stores them in the rawMetadata array.
        /// The row for this entry is lazily allocated only when at least one value is found.
        /// </summary>
        private static void ExtractRawMetadataValues(LogEntry logEntry, LogLayout logLayout, int logMetadataPropertyCount, string[][] rawMetadata)
        {
            if (logLayout.LogMetadataProperties == null) return;

            foreach (LogMetadataProperty logMetadataProperty in logLayout.LogMetadataProperties)
            {
                if (!TryExtractValue(logEntry.Entry, logMetadataProperty.Criteria, true, logEntry.StartIndexMetadata, out string propertyValue)) continue;

                if (rawMetadata[logEntry.Index] == null) rawMetadata[logEntry.Index] = new string[logMetadataPropertyCount];

                rawMetadata[logEntry.Index][logMetadataProperty.Index] = propertyValue;
            }
        }

        /// <summary>
        /// Classifies content properties for a log entry and assigns them.
        /// Returns false if the entry was already classified.
        /// </summary>
        private static bool TryClassifyContentProperties(LogEntry logEntry, LogLayout logLayout)
        {
            if (logEntry.LogContentProperties != null) return false;

            logEntry.LogContentProperties = new IndexDictionary<LogContentProperty, LogContentValue>(logLayout.LogContentProperties.Count);

            foreach (LogContentProperty logContentProperty in logLayout.LogContentProperties)
            {
                foreach (FilterCriteria filterCriteria in logContentProperty.Criterias)
                {
                    int startPosition = logContentProperty.IsErrorProperty ? logEntry.StartIndexMetadata : logEntry.StartIndexContent;
                    if (!TryExtractValue(logEntry.Entry, filterCriteria, false, startPosition, out string value))                        continue;

                    logEntry.LogContentProperties[logContentProperty] = new LogContentValue(value.Trim(), logEntry.TimeStamp.ToString("HH:mm:ss"));
                    if (logContentProperty.IsErrorProperty) logEntry.IsErrorLogEntry = true;
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Extracts a substring from a log entry string based on before/after phrase criteria.
        /// </summary>
        /// <param name="logEntry">The raw log entry string to search.</param>
        /// <param name="criteria">The criteria defining BeforePhrase and AfterPhrase boundaries.</param>
        /// <param name="afterPhraseMandatory">Whether the AfterPhrase must be present for extraction to succeed.</param>
        /// <param name="startPosition">The position in the string to start searching from.</param>
        /// <param name="value">The extracted value, or null if extraction failed.</param>
        /// <returns>True if extraction succeeded; otherwise false.</returns>
        private static bool TryExtractValue(string logEntry, FilterCriteria criteria, bool afterPhraseMandatory, int startPosition, out string value)
        {
            value = null;

            if (criteria == null || string.IsNullOrEmpty(criteria.BeforePhrase)) return false;

            bool isAfterPhraseNullOrEmpty = string.IsNullOrEmpty(criteria.AfterPhrase);
            if (afterPhraseMandatory && isAfterPhraseNullOrEmpty) return false;

            int startIndex = logEntry.IndexOf(criteria.BeforePhrase, startPosition, StringComparison.Ordinal);
            if (startIndex == -1) return false;

            startIndex += criteria.BeforePhrase.Length;

            int endIndex = isAfterPhraseNullOrEmpty ? -1 : logEntry.IndexOf(criteria.AfterPhrase, startIndex, StringComparison.Ordinal);
            if ((afterPhraseMandatory || !isAfterPhraseNullOrEmpty) && endIndex == -1) return false;

            if (endIndex == -1) endIndex = logEntry.Length;

            value = endIndex == startIndex ? string.Empty : logEntry[startIndex..endIndex];
            return true;
        }
    }
}