using System;
using System.Collections.Generic;
using System.Text;
using LogScraper.Log.Layout;
using LogScraper.LogTransformers;

namespace LogScraper.Log.Processing.RawLogParsing
{
    /// <summary>
    /// Handles the reading and parsing of raw log entries into a structured format.
    /// </summary>
    public static class RawLogParser
    {
        /// <summary>
        /// Tries to parse the raw log entries and returns any new entries as a list, without modifying the collection.
        /// The caller is responsible for committing the result to the collection via <see cref="LogCollection.CommitParsedEntries"/>.
        /// </summary>
        /// <param name="rawLogEntries">Array of raw log entries to process.</param>
        /// <param name="logCollection">The collection used only to read anchor entries for deduplication.</param>
        /// <param name="logLayout">The layout defining the structure and format of the log entries.</param>
        /// <param name="newEntries">The newly parsed entries, ready to be committed. Empty if nothing new was found.</param>
        /// <returns>True if new entries were parsed; false if nothing new was found.</returns>
        public static bool TryParseNewLogEntries(string[] rawLogEntries, LogCollection logCollection, LogLayout logLayout, out List<LogEntry> newEntries)
        {
            newEntries = [];
            if (rawLogEntries == null) return false;
            if (rawLogEntries.Length == 0) return false;
            if (!logLayout.IsAutomaticTimeStampRecognitionEnabled && string.IsNullOrEmpty(logLayout.DateTimeFormat)) throw new Exception("The date time format is not provided for the given log layout");

            // Transform log entries using the provided transformers in the layout.
            if (logLayout.LogTransformers != null && logLayout.LogTransformers.Count > 0)
            {
                foreach (ILogTransformer logTransformer in logLayout.LogTransformers)
                {
                    logTransformer.Transform(rawLogEntries);
                }
            }

            // Snapshot the two anchor strings under a brief read lock, then release immediately.
            (string firstEntry, string lastEntry) = logCollection.GetAnchorsForParsing();
            int existingCount = logCollection.LogEntries.Count;

            // Determine the range of raw entries that have not yet been parsed.
            Range newEntriesRange = GetNewEntriesRange(rawLogEntries, firstEntry, lastEntry);

            // Parse the timestamp in the log entries starting from the determined index.
            ParsedLogEntry[] parsedLogLines = new LogTimestampParser().Parse(rawLogEntries, newEntriesRange, !logLayout.IsAutomaticTimeStampRecognitionEnabled ? logLayout.DateTimeFormat : null);

            if (parsedLogLines.Length == 0) return false;

            // Reverse the order of the log entries in place if necessary to ensure chronological order.
            if (ShouldReverseParsedLogEntries(parsedLogLines))
            {
                Array.Reverse(parsedLogLines);
            }

            // Build new entries into a standalone list
            newEntries = BuildLogEntriesFromParsedLines(parsedLogLines, existingCount);

            // Handle the case where no valid log entries were found.
            if (rawLogEntries.Length > 0 && existingCount == 0 && newEntries.Count == 0)
            {
                string message =
                    "No timestamp detected at the start of each log line. Log filtering and navigation are disabled." + Environment.NewLine +
                    "If the timestamp is not recognized, configure a timestamp format in the log layout settings." + Environment.NewLine;

                throw new Exception(message);
            }
            return newEntries.Count > 0;
        }

        /// <summary>
        /// Determines the range of raw entries that have not yet been parsed, using pre-snapshotted anchor strings.
        /// </summary>
        private static Range GetNewEntriesRange(string[] rawLogEntries, string firstEntry, string lastEntry)
        {
            //Note: this method compares complete log entry string. Since log entries contain a timestamp at the beginning,
            // since ordinal searching (with SIMD) is used this comparison is very fast.

            if (firstEntry == null) return 0..rawLogEntries.Length;

            // Ambiguous: cannot distinguish which anchor was matched.
            bool singleAnchor = string.Equals(firstEntry, lastEntry, StringComparison.Ordinal);

            // Backward scan: O(newCount) for normal logs.
            for (int i = rawLogEntries.Length - 1; i >= 0; i--)
            {
                string raw = rawLogEntries[i];
                if (string.IsNullOrEmpty(raw)) continue;

                if (string.Equals(raw, lastEntry, StringComparison.Ordinal))
                {
                    // Last() (newest) encountered first from the end → normal ordering.
                    // New entries are everything after this position.
                    return (i + 1)..rawLogEntries.Length;
                }

                if (!singleAnchor && string.Equals(raw, firstEntry, StringComparison.Ordinal))
                {
                    // First() (oldest) encountered before Last() from the end → inverse ordering.
                    // Switch to a forward scan to find Last() (newest) near the start.
                    return GetNewEntriesRangeForInverseLog(rawLogEntries, upToIndex: i, lastEntry);
                }
            }

            // Fallback: anchors missing (truncated/replaced log) or single-anchor ambiguity.
            return 0..rawLogEntries.Length;
        }

        /// <summary>
        /// Forward scan used when inverse ordering has been detected.
        /// Searches for <paramref name="lastEntry"/> within the new-entries region (indices 0 to
        /// <paramref name="upToIndex"/> exclusive), which is where the newest known entry sits after
        /// new lines have been prepended to the inverse log.
        /// Returns the range of new entries (0..j), or a full re-parse range if not found.
        /// </summary>
        private static Range GetNewEntriesRangeForInverseLog(string[] rawLogEntries, int upToIndex, string lastEntry)
        {
            for (int j = 0; j < upToIndex; j++)
            {
                if (!string.IsNullOrEmpty(rawLogEntries[j]) &&
                    string.Equals(rawLogEntries[j], lastEntry, StringComparison.Ordinal))
                {
                    // New entries are everything before Last() in the inverse array.
                    return 0..j;
                }
            }

            // Last() not found — fall back to full re-parse.
            return 0..rawLogEntries.Length;
        }

        /// <summary>
        /// Determines whether the order of log entries should be reversed based on the timestamps of the first and last entries.
        /// </summary>
        private static bool ShouldReverseParsedLogEntries(ParsedLogEntry[] parsedLogEntries)
        {
            // Find first entry with timestamp
            DateTime? firstTimestamp = null;
            for (int i = 0; i < parsedLogEntries.Length; i++)
            {
                if (parsedLogEntries[i] != null && parsedLogEntries[i].HasTimestamp)
                {
                    firstTimestamp = parsedLogEntries[i].Timestamp;
                    break;
                }
            }

            if (firstTimestamp == null) return false; // No timestamps found

            // Find last entry with timestamp
            DateTime? lastTimestamp = null;
            for (int i = parsedLogEntries.Length - 1; i >= 0; i--)
            {
                if (parsedLogEntries[i] != null && parsedLogEntries[i].HasTimestamp)
                {
                    lastTimestamp = parsedLogEntries[i].Timestamp;
                    break;
                }
            }

            if (lastTimestamp == null) return false; // Only one timestamp found

            // Reverse if descending order
            return firstTimestamp > lastTimestamp;
        }

        /// <summary>
        /// Builds a standalone list of new log entries from parsed lines.
        /// Does not touch the LogCollection — the caller commits via <see cref="LogCollection.CommitParsedEntries"/>.
        /// </summary>
        /// <param name="parsedLogEntries">Parsed log lines to convert.</param>
        /// <param name="existingCount">Current count in the collection, used to assign correct indices.</param>
        private static List<LogEntry> BuildLogEntriesFromParsedLines(ParsedLogEntry[] parsedLogEntries, int existingCount)
        {
            List<LogEntry> result = [];
            LogEntry lastLogEntry = null;
            int logEntriesListIndex = existingCount;

            for (int i = 0; i < parsedLogEntries.Length; i++)
            {
                ref ParsedLogEntry line = ref parsedLogEntries[i];
                if (line == null) continue;

                if (!line.HasTimestamp)
                {
                    // Only if the last log entry exists, add the additional log entry to it.
                    // Some entries without a timestamp may be ignored when they are the first entries.
                    if (lastLogEntry == null) continue;

                    lastLogEntry.AdditionalLogEntries ??= [];
                    lastLogEntry.AdditionalLogEntries.Add(line.RawLogEntry);
                    continue;
                }

                LogEntry newLogEntry = new(line.RawLogEntry, line.Timestamp, logEntriesListIndex)
                {
                    StartIndexMetadata = line.StartIndexMetadata
                };
                result.Add(newLogEntry);
                lastLogEntry = newLogEntry;
                logEntriesListIndex++;
            }
            return result;
        }

        /// <summary>
        /// Joins an array of raw log entries into a single string, separating each entry with a newline character.
        /// </summary>
        /// <param name="rawLogEntries">The raw log to parse.</param>
        /// <returns>A string of all strings in the provided array.</returns>
        public static string JoinRawLogIntoString(string[] rawLogEntries)
        {
            StringBuilder builder = new();
            for (int i = 0; i < rawLogEntries.Length; i++)
            {
                builder.Append(rawLogEntries[i]);
                if (i < rawLogEntries.Length - 1)
                {
                    builder.AppendLine();
                }
            }
            return builder.ToString();
        }
    }
}
