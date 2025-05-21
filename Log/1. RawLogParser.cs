﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogScraper.Log.Layout;
using LogScraper.LogTransformers;

namespace LogScraper.Log
{
    /// <summary>
    /// Handles the reading and parsing of raw log entries into a structured format.
    /// </summary>
    public static class RawLogParser
    {
        /// <summary>
        /// Represents a parsed log line, including its raw content and optional timestamp information.
        /// </summary>
        /// <remarks>This class encapsulates the details of a single log entry, including whether it
        /// contains a timestamp and the timestamp value if present. It is intended for internal use in log processing
        /// scenarios.</remarks>
        /// <param name="rawEntry"></param>
        /// <param name="hasTimestamp"></param>
        /// <param name="timestamp"></param>
        private class ParsedLogLine(string rawEntry, bool hasTimestamp, DateTime timestamp)
        {
            public string RawEntry = rawEntry;
            public bool HasTimestamp = hasTimestamp;
            public DateTime Timestamp = timestamp;
        }

        /// <summary>
        /// Parses the raw log entries and inserts them into the provided LogCollection based on the specified LogLayout.
        /// </summary>
        /// <param name="rawLogEntries">Array of raw log entries to process.</param>
        /// <param name="logCollection">The collection where processed log entries will be stored.</param>
        /// <param name="logLayout">The layout defining the structure and format of the log entries.</param>
        public static void ParseLogEntriesIntoCollection(string[] rawLogEntries, LogCollection logCollection, LogLayout logLayout)
        {
            if (rawLogEntries == null) return;
            if (rawLogEntries.Length == 0) return;
            if (string.IsNullOrEmpty(logLayout.DateTimeFormat)) throw new Exception("The date time format is not provided for the given log layout");

            // Transform log entries using the provided transformers in the layout.
            if (logLayout.LogTransformers != null && logLayout.LogTransformers.Count > 0)
            {
                foreach (ILogTransformer logTransformer in logLayout.LogTransformers)
                {
                    logTransformer.Transform(rawLogEntries);
                }
            }

            // Determine the last log entry and the starting index for extracting new log entries.
            GetLastLogEntryAndNewBeginningIndex(rawLogEntries, logCollection, out int logEntriesStartIndex);

            // Parse the timestamp in the log entries starting from the determined index.
            ParsedLogLine[] parsedLogLines = PreParseLogTimestamps(rawLogEntries, logEntriesStartIndex, logLayout.DateTimeFormat);

            // Combine additional log entries into the last log entry if applicable and add the log entries to the collection.
            BuildLogEntriesFromParsedLines(parsedLogLines, logCollection);

            // Handle the case where no valid log entries were added.
            if (rawLogEntries.Length > 0 && logCollection.LogEntries.Count == 0)
            {
                // Provide an explanation for the failure to parse the log entries, including the expected date-time format.
                int maxLength = rawLogEntries[0].Length < logLayout.DateTimeFormat.Length ? rawLogEntries[0].Length : logLayout.DateTimeFormat.Length;
                string message = "The log could not be interpreted. The timestamp required at the beginning of each entry could not be parsed. Make sure you have selected the correct log layout and its date-time format matches the format of the log." + Environment.NewLine;
                message += $"Expected date time format {logLayout.DateTimeFormat} at the start of log entries but instead found (for example) " + rawLogEntries[0][..maxLength];
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Determines the last log entry in the collection and calculates the index of the new logentries array which should be used to start reading new log entries.
        /// </summary>
        /// <param name="rawLogEntries">Array of log entries to process.</param>
        /// <param name="logCollection">The collection containing existing log entries.</param>
        /// <param name="logEntriesStartIndex">The index in the rawLogEntries array where new log entries start.</param>
        private static void GetLastLogEntryAndNewBeginningIndex(string[] rawLogEntries, LogCollection logCollection, out int logEntriesStartIndex)
        {
            logEntriesStartIndex = 0;

            // Retrieve the last log entry from the collection, if it exists.
            if (logCollection.LogEntries.Count == 0) return;

            // Find the index of the last log entry in the new log entries array.
            LogEntry lastLogEntry = logCollection.LogEntries.Last();
            // Iterate through the new log entries in reverse order which is generally faster
            for (int i = rawLogEntries.Length - 1; i >= 0; i--)
            {
                if (rawLogEntries[i] == string.Empty) continue;

                if (rawLogEntries[i] == lastLogEntry.Entry)
                {
                    logEntriesStartIndex = i + 1;
                    break;
                }
            }
        }
        /// <summary>
        /// Parses a subset of raw log entries to extract timestamps based on the specified date-time format.
        /// </summary>
        /// <remarks>This method processes the log entries in parallel for improved performance. If a log
        /// entry is empty or its timestamp cannot be parsed, the corresponding element in the returned array will
        /// indicate a failure to parse the timestamp.</remarks>
        /// <param name="rawLogEntries">An array of raw log entry strings to be parsed.</param>
        /// <param name="startIndex">The zero-based index in <paramref name="rawLogEntries"/> from which parsing should begin.</param>
        /// <param name="dateTimeFormat">The expected date-time format used to parse timestamps from the log entries.</param>
        /// <returns>An array of <see cref="ParsedLogLine"/> objects representing the parsed log entries. Each element contains
        /// the original log entry, a flag indicating whether the timestamp was successfully parsed, and the parsed
        /// timestamp if available.</returns>
        private static ParsedLogLine[] PreParseLogTimestamps(string[] rawLogEntries, int startIndex, string dateTimeFormat)
        {
            int length = rawLogEntries.Length - startIndex;
            ParsedLogLine[] parsedLines = new ParsedLogLine[length];

            // Process the log entries in parallel to improve performance.
            Parallel.For(0, length, i =>
            {
                string rawEntry = rawLogEntries[startIndex + i];

                // Skip empty log entries
                if (rawEntry == "")
                {
                    parsedLines[i] = null;
                    return;
                }

                if (TryGetDateTimeFromRawLogEntry(rawEntry, dateTimeFormat, out DateTime timestamp))
                {
                    parsedLines[i] = new ParsedLogLine(rawEntry, true, timestamp);
                }
                else
                {
                    parsedLines[i] = new ParsedLogLine(rawEntry, false, default);
                }
            });

            return parsedLines;
        }

        /// <summary>
        /// Processes an array of parsed log lines and populates a <see cref="LogCollection"/> with log entries.
        /// </summary>
        /// <remarks>This method iterates through the provided parsed log lines and creates new log
        /// entries for lines that contain a timestamp. Lines without a timestamp are treated as additional log data and
        /// appended to the most recent log entry, if one exists. If no log entry has been created yet, lines without a
        /// timestamp are ignored.</remarks>
        /// <param name="parsedLines">An array of <see cref="ParsedLogLine"/> objects representing the parsed log lines. Each element may contain
        /// a timestamp and raw log entry data.</param>
        /// <param name="logCollection">The <see cref="LogCollection"/> to which the processed log entries will be added. This collection will be
        /// updated with new log entries based on the parsed lines.</param>
        private static void BuildLogEntriesFromParsedLines(ParsedLogLine[] parsedLines, LogCollection logCollection)
        {
            bool logEntryIsAdded = false;
            LogEntry lastLogEntry = null;

            for (int i = 0; i < parsedLines.Length; i++)
            {
                ref ParsedLogLine line = ref parsedLines[i];
                if (line == null) continue;

                if (!line.HasTimestamp)
                {
                    // Only if te last log entry exists, add the additional log entry to it.
                    // Some entries without a timestamp may be ignored when they are the first entries.
                    if (!logEntryIsAdded || lastLogEntry == null) continue;

                    lastLogEntry.AdditionalLogEntries ??= [];
                    lastLogEntry.AdditionalLogEntries.Add(line.RawEntry);
                    continue;
                }

                LogEntry newLogEntry = new(line.RawEntry, line.Timestamp);
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
        public static bool TryGetDateTimeFromRawLogEntry(string rawLogEntry, string dateTimeFormat, out DateTime timestamp)
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
