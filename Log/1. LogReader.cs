using LogScraper.Log.Collection;
using LogScraper.LogTransformers;
using System;
using System.Globalization;
using System.Linq;

namespace LogScraper.Log
{ 
    /// <summary>
    /// Handles the reading and parsing of log entries into a structured format.
    /// </summary>
    internal static class LogReader
    {
        /// <summary>
        /// Reads log entries into the provided LogCollection based on the specified LogLayout.
        /// </summary>
        /// <param name="logEntries">Array of log entries to process.</param>
        /// <param name="logCollection">The collection where processed log entries will be stored.</param>
        /// <param name="logLayout">The layout defining the structure and format of the log entries.</param>
        public static void ReadIntoLogCollection(string[] logEntries, LogCollection logCollection, LogLayout logLayout)
        {
            if (logEntries == null) return;
            if (logEntries.Length == 0) return;
            if (string.IsNullOrEmpty(logLayout.DateTimeFormat)) throw new Exception("The date time format is not provided for the given log layout");

            // Transform log entries using the provided transformers in the layout.
            if (logLayout.LogTransformers != null && logLayout.LogTransformers.Count > 0)
            {
                foreach (ILogTransformer logTransformer in logLayout.LogTransformers)
                {
                    logTransformer.Transform(logEntries);
                }
            }

            // Determine the last log entry and the starting index for extracting new log entries.
            GetLastLogEntryAndNewBeginningIndex(logEntries, logCollection, out LogEntry lastLogEntry, out int logEntriesStartIndex);

            bool logEntryIsAdded = false;
            for (int i = logEntriesStartIndex; i < logEntries.Length; i++)
            {
                if (logEntries[i] == string.Empty) continue;

                DateTime timestamp = GetDateTimeFromRawLogEntry(logEntries[i], logLayout.DateTimeFormat);

                // Handle additional log entries without a timestamp.
                if (timestamp.Year == 1)
                {
                    if (logEntryIsAdded == false) continue;

                    if (lastLogEntry == null) continue;

                    lastLogEntry.AdditionalLogEntries ??= [];
                    lastLogEntry.AdditionalLogEntries.Add(logEntries[i]);
                    continue;
                }

                // Create and add a new log entry.
                LogEntry newLine = new(logEntries[i], timestamp);
                logCollection.LogEntries.Add(newLine);
                logEntryIsAdded = true;
                lastLogEntry = newLine;
            }

            // Handle the case where no valid log entries were added.
            if (logEntries.Length > 0 && logCollection.LogEntries.Count == 0)
            {
                // Provide an explanation for the failure to parse the log entries, including the expected date-time format.
                int maxLength = logEntries[0].Length < logLayout.DateTimeFormat.Length ? logEntries[0].Length : logLayout.DateTimeFormat.Length;
                string message = "The log could not be interpreted. The timestamp required at the beginning of each line could not be parsed. Make sure you have selected the correct log layout and its date-time format matches the format of the log." + Environment.NewLine;
                message += $"Expected date time format {logLayout.DateTimeFormat} at the start of log entries but instead found (for example) " + logEntries[0][..maxLength];
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Determines the last log entry in the collection and calculates the index of the new logentries array which should be used to start reading new log entries.
        /// </summary>
        /// <param name="logEntries">Array of log entries to process.</param>
        /// <param name="logCollection">The collection containing existing log entries.</param>
        /// <param name="lastLogEntry">The last log entry in the collection, if any.</param>
        /// <param name="logEntriesStartIndex">The index in the logEntries array where new log entries start.</param>
        private static void GetLastLogEntryAndNewBeginningIndex(string[] logEntries, LogCollection logCollection, out LogEntry lastLogEntry, out int logEntriesStartIndex)
        {
            lastLogEntry = null;
            logEntriesStartIndex = 0;

            // Retrieve the last log entry from the collection, if it exists.
            if (logCollection.LogEntries.Count == 0) return;

            // Find the index of the last log entry in the new log entries array.
            lastLogEntry = logCollection.LogEntries.Last();
            // Iterate through the new log entries in reverse order which is generally faster
            for (int i = logEntries.Length - 1; i >= 0; i--)
            {
                if (logEntries[i] == string.Empty) continue;

                if (logEntries[i] == lastLogEntry.Line)
                {
                    logEntriesStartIndex = i + 1;
                    break;
                }
            }
        }

        /// <summary>
        /// Extracts a DateTime object from a raw log entry based on the provided date-time format.
        /// </summary>
        /// <param name="line">The raw log entry to parse.</param>
        /// <param name="dateTimeFormat">The expected date-time format at the start of the log entry.</param>
        /// <returns>A DateTime object representing the timestamp, or a default DateTime if parsing fails.</returns>
        public static DateTime GetDateTimeFromRawLogEntry(string line, string dateTimeFormat)
        {
            DateTime timeStampOut = new();
            int length = dateTimeFormat.Length;

            // Ensure the line is long enough to contain the date-time format.
            if (length > line.Length) return timeStampOut;

            string dateTimeStampString = line[..(dateTimeFormat.Length)];
            DateTime.TryParseExact(dateTimeStampString, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out timeStampOut);

            return timeStampOut;
        }
    }
}
