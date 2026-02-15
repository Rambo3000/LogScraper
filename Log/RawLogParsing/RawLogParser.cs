using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogScraper.Log.Layout;
using LogScraper.LogTransformers;

namespace LogScraper.Log.RawLogParsing
{
    /// <summary>
    /// Handles the reading and parsing of raw log entries into a structured format.
    /// </summary>
    public static class RawLogParser
    {
        /// <summary>
        /// Tries to parse the raw log entries and inserts them into the provided LogCollection based on the specified LogLayout.
        /// </summary>
        /// <param name="rawLogEntries">Array of raw log entries to process.</param>
        /// <param name="logCollection">The collection where processed log entries will be stored.</param>
        /// <param name="logLayout">The layout defining the structure and format of the log entries.</param>
        /// <returns>Returns true if the parsing was successful and log entries were added to the collection; returns false if no entries were added to the collection.</returns>
        public static bool TryParseAndAppendLogEntries(string[] rawLogEntries, LogCollection logCollection, LogLayout logLayout)
        {
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

            // Determine the last log entry and the starting index for extracting new log entries.
            int startIndex = GetLogEntriesStartIndex(rawLogEntries, logCollection);

            // Parse the timestamp in the log entries starting from the determined index.
            ParsedLogEntry[] parsedLogLines = new LogTimestampParser().Parse(rawLogEntries, startIndex, !logLayout.IsAutomaticTimeStampRecognitionEnabled ? logLayout.DateTimeFormat : null);

            if (parsedLogLines.Length == 0)
            {
                return false;
            }

            // Reverse the order of the log entries in place if necessary to ensure chronological order
            // Apply this to the parsed log lines which are used to create the log entries, so that the
            // log entries are created in the correct order and additional log entries are added to the correct log entry.
            if (ShouldReverseParsedLogEntries(parsedLogLines))
            {
                Array.Reverse(parsedLogLines);
            }

            // Combine additional log entries into the last log entry if applicable and add the log entries to the collection.
            BuildLogEntriesFromParsedLines(parsedLogLines, logCollection);

            // Handle the case where no valid log entries were added.
            if (rawLogEntries.Length > 0 && logCollection.LogEntries.Count == 0)
            {
                // Provide an explanation for the failure to parse the log entries
                string message =
                        "No timestamp detected at the start of the log entries. Log filtering and navigation is disabled" + Environment.NewLine +
                        "Select the correct log layout or configure a custom timestamp format." + Environment.NewLine;

                throw new Exception(message);
            }
            return true;
        }

        /// <summary>
        /// Determines the last log entry in the collection and calculates the index of the new logentries array which should be used to start reading new log entries.
        /// </summary>
        /// <param name="rawLogEntries">Array of log entries to process.</param>
        /// <param name="logCollection">The collection containing existing log entries.</param>
        private static int GetLogEntriesStartIndex(string[] rawLogEntries, LogCollection logCollection)
        {
            // Retrieve the last log entry from the collection, if it exists.
            if (logCollection.LogEntries.Count == 0) return 0;

            int startIndex = 0;

            // Find the index of the last log entry in the new log entries array.
            LogEntry lastLogEntry = logCollection.LogEntries.Last();
            // Iterate through the new log entries in reverse order which is generally faster
            for (int i = rawLogEntries.Length - 1; i >= 0; i--)
            {
                if (rawLogEntries[i] == string.Empty) continue;

                // Note that the == operator uses ordinal string comparison
                if (rawLogEntries[i] == lastLogEntry.Entry)
                {
                    startIndex = i + 1;
                    break;
                }
            }
            return startIndex;
        }

        /// <summary>
        /// Determines whether the order of log entries should be reversed based on the timestamps of the first and last entries.
        /// </summary>
        /// <remarks>This method checks the timestamps of the first and last log entries to determine if they are in descending order.
        /// If the first timestamp is greater than the last timestamp, it indicates that the log entries are in reverse chronological order and should be reversed to ensure correct ordering.</remarks>
        /// <param name="parsedLogEntries">An array of <see cref="LogEntry"/> objects to be evaluated for order. The method will check the timestamps of the first and last entries in this array.</param>
        /// <returns>Returns true if the log entries should be reversed to ensure chronological order; returns false if the entries are already in the correct order or if timestamps are not available.</returns>
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
        /// Processes an array of parsed log lines and populates a <see cref="LogCollection"/> with log entries.
        /// </summary>
        /// <remarks>This method iterates through the provided parsed log lines and creates new log
        /// entries for lines that contain a timestamp. Lines without a timestamp are treated as additional log data and
        /// appended to the most recent log entry, if one exists. If no log entry has been created yet, lines without a
        /// timestamp are ignored.</remarks>
        /// <param name="parsedLogEntries">An array of <see cref="ParsedLogEntry"/> objects representing the parsed log lines. Each element may contain
        /// a timestamp and raw log entry data.</param>
        /// <param name="logCollection">The <see cref="LogCollection"/> to which the processed log entries will be added. This collection will be
        /// updated with new log entries based on the parsed lines.</param>
        private static void BuildLogEntriesFromParsedLines(ParsedLogEntry[] parsedLogEntries, LogCollection logCollection)
        {
            bool logEntryIsAdded = false;
            LogEntry lastLogEntry = null;

            int indexOffset = logCollection.LogEntries.Count;
            for (int i = 0; i < parsedLogEntries.Length; i++)
            {
                ref ParsedLogEntry line = ref parsedLogEntries[i];
                if (line == null) continue;

                if (!line.HasTimestamp)
                {
                    // Only if te last log entry exists, add the additional log entry to it.
                    // Some entries without a timestamp may be ignored when they are the first entries.
                    if (!logEntryIsAdded || lastLogEntry == null) continue;

                    lastLogEntry.AdditionalLogEntries ??= [];
                    lastLogEntry.AdditionalLogEntries.Add(line.RawLogEntry);
                    continue;
                }

                LogEntry newLogEntry = new(line.RawLogEntry, line.Timestamp, i + indexOffset)
                {
                    StartIndexMetadata = line.StartIndexMetadata
                };
                logCollection.LogEntries.Add(newLogEntry);
                logEntryIsAdded = true;
                lastLogEntry = newLogEntry;
            }
        }

        /// <summary>
        /// Attempts to extract a DateTime object from a raw log entry based on the provided date-time format.
        /// </summary>
        /// <param name="rawLogEntry">The raw log entry to parse.</param>
        /// <param name="dateTimeFormat">The expected date-time format at the start of the log entry.</param>
        /// <param name="timestamp">The parsed DateTime object.</param>
        /// <returns></returns>
        private static bool TryGetDateTimeFromRawLogEntry(string rawLogEntry, string dateTimeFormat, out DateTime timestamp)
        {
            if (rawLogEntry.Length < dateTimeFormat.Length)
            {
                timestamp = default;
                return false;
            }

            string dateTimeStampString = rawLogEntry[..dateTimeFormat.Length];
            return DateTime.TryParseExact(dateTimeStampString, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp);
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
