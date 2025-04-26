using LogScraper.Log.Collection;
using LogScraper.LogTransformers;
using System;
using System.Globalization;
using System.Linq;

namespace LogScraper.Log
{ 
    /// <summary>
    /// Handles the reading and parsing of log lines into a structured format.
    /// </summary>
    internal static class LogReader
    {
        /// <summary>
        /// Reads log lines into the provided LogCollection based on the specified LogLayout.
        /// </summary>
        /// <param name="logLines">Array of log lines to process.</param>
        /// <param name="logCollection">The collection where processed log lines will be stored.</param>
        /// <param name="logLayout">The layout defining the structure and format of the log lines.</param>
        public static void ReadIntoLogCollection(string[] logLines, LogCollection logCollection, LogLayout logLayout)
        {
            if (logLines == null) return;
            if (logLines.Length == 0) return;
            if (string.IsNullOrEmpty(logLayout.DateTimeFormat)) throw new Exception("The date time format is not provided for the given log layout");

            // Transform log lines using the provided transformers in the layout.
            if (logLayout.LogTransformers != null && logLayout.LogTransformers.Count > 0)
            {
                foreach (ILogTransformer logTransformer in logLayout.LogTransformers)
                {
                    logTransformer.Transform(logLines);
                }
            }

            // Determine the last log line and the starting index for extracting new log lines.
            GetLastLogLineAndNewBeginningIndex(logLines, logCollection, out LogLine lastLogLine, out int logLinesStartIndex);

            bool logLineIsAdded = false;
            for (int i = logLinesStartIndex; i < logLines.Length; i++)
            {
                if (logLines[i] == string.Empty) continue;

                DateTime timestamp = GetDateTimeFromRawLogLine(logLines[i], logLayout.DateTimeFormat);

                // Handle additional log lines without a timestamp.
                if (timestamp.Year == 1)
                {
                    if (logLineIsAdded == false) continue;

                    if (lastLogLine == null) continue;

                    lastLogLine.AdditionalLogLines ??= [];
                    lastLogLine.AdditionalLogLines.Add(logLines[i]);
                    continue;
                }

                // Create and add a new log line.
                LogLine newLine = new(logLines[i], timestamp);
                logCollection.LogLines.Add(newLine);
                logLineIsAdded = true;
                lastLogLine = newLine;
            }

            // Handle the case where no valid log lines were added.
            if (logLines.Length > 0 && logCollection.LogLines.Count == 0)
            {
                // Provide an explanation for the failure to parse the log lines, including the expected date-time format.
                int maxLength = logLines[0].Length < logLayout.DateTimeFormat.Length ? logLines[0].Length : logLayout.DateTimeFormat.Length;
                string message = "The log could not be interpreted. The timestamp required at the beginning of each line could not be parsed. Make sure you have selected the correct log layout and its date-time format matches the format of the log." + Environment.NewLine;
                message += $"Expected date time format {logLayout.DateTimeFormat} at the start of log lines but instead found (for example) " + logLines[0][..maxLength];
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Determines the last log line in the collection and calculates the index of the new loglines array which should be used to start reading new log lines.
        /// </summary>
        /// <param name="logLines">Array of log lines to process.</param>
        /// <param name="logCollection">The collection containing existing log lines.</param>
        /// <param name="lastLogLine">The last log line in the collection, if any.</param>
        /// <param name="logLinesStartIndex">The index in the logLines array where new log lines start.</param>
        private static void GetLastLogLineAndNewBeginningIndex(string[] logLines, LogCollection logCollection, out LogLine lastLogLine, out int logLinesStartIndex)
        {
            lastLogLine = null;
            logLinesStartIndex = 0;

            // Retrieve the last log line from the collection, if it exists.
            if (logCollection.LogLines.Count == 0) return;

            // Find the index of the last log line in the new log lines array.
            lastLogLine = logCollection.LogLines.Last();
            // Iterate through the new log lines in reverse order which is generally faster
            for (int i = logLines.Length - 1; i >= 0; i--)
            {
                if (logLines[i] == string.Empty) continue;

                if (logLines[i] == lastLogLine.Line)
                {
                    logLinesStartIndex = i + 1;
                    break;
                }
            }
        }

        /// <summary>
        /// Extracts a DateTime object from a raw log line based on the provided date-time format.
        /// </summary>
        /// <param name="line">The raw log line to parse.</param>
        /// <param name="dateTimeFormat">The expected date-time format at the start of the log line.</param>
        /// <returns>A DateTime object representing the timestamp, or a default DateTime if parsing fails.</returns>
        public static DateTime GetDateTimeFromRawLogLine(string line, string dateTimeFormat)
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
