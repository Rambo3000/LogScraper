using LogScraper.Log.Collection;
using LogScraper.LogTransformers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace LogScraper.Log
{
    internal static class LogReader
    {
        public static void ReadIntoLogCollection(string[] logLines, LogCollection logCollection, LogLayout logLayout)
        {
            if (logLines == null) return;
            if (logLines.Length == 0) return;
            if (string.IsNullOrEmpty(logLayout.DateTimeFormat)) throw new Exception("The date time format is not provided for the given log layout");

            // Always transform everything first. This because the index of the new beginning will be matched based on the transformed line.
            //   It is quite complex to get the original hashcode (of the non transformed line) of the non-stack-trace-last-line of the collection.
            //   This would be more optimal since only lines which are new will be transformed. Although log inversion should always happen before determining the new start index.
            if (logLayout.LogTransformers != null && logLayout.LogTransformers.Count > 0)
            {
                //always first invert the log when needed so the startindex of the raw log can be determined
                foreach (ILogTransformer logTransformer in logLayout.LogTransformers)
                {
                    logTransformer.Transform(logLines);
                }
            }

            GetLastLogLineAndNewBeginningIndex(logLines, logCollection, out LogLine lastLogLine, out int logLinesStartIndex);

            bool logLineIsAdded = false;
            for (int i = logLinesStartIndex; i < logLines.Length; i++)
            {
                //Empty lines are skipped always
                if (logLines[i] == string.Empty) continue;

                DateTime timestamp = GetDateTimeFromRawLogLine(logLines[i], logLayout.DateTimeFormat);

                // In case we have no timestamp we have additional information
                if (timestamp.Year == 1)
                {
                    //In this case we have an additional log line without an added log line. We just skip these
                    if (logLineIsAdded == false) continue;

                    if (lastLogLine == null) continue;

                    lastLogLine.AdditionalLogLines ??= [];
                    lastLogLine.AdditionalLogLines.Add(logLines[i]);
                    continue;
                }

                // Create and add logline
                LogLine newLine = new(logLines[i], timestamp);
                logCollection.LogLines.Add(newLine);
                logLineIsAdded = true;
                lastLogLine = newLine;
            }
            if (logLines.Length > 0 && logCollection.LogLines.Count == 0)
            {
                int maxLength = logLines[0].Length < logLayout.DateTimeFormat.Length ? logLines[0].Length : logLayout.DateTimeFormat.Length;
                string message = "The log could not be interpreted. The timestamp required at the beginning of each line could not be parsed. Make sure you have selected the correct log layout and its date-time format matches the format of the log." + Environment.NewLine;
                message += $"Expected date time format {logLayout.DateTimeFormat} at the start of log lines but instead found (for example) " + logLines[0][..maxLength];
                throw new Exception(message);
            }
        }

        private static void GetLastLogLineAndNewBeginningIndex(string[] logLines, LogCollection logCollection, out LogLine lastLogLine, out int logLinesStartIndex)
        {
            lastLogLine = null;
            if (logCollection.LogLines.Count > 0)
            {
                lastLogLine = logCollection.LogLines.Last();
            }

            logLinesStartIndex = 0;
            if (lastLogLine != null)
            {
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
        }

        public static DateTime GetDateTimeFromRawLogLine(string line, string dateTimeFormat)
        {
            DateTime timeStampOut = new();
            int length = dateTimeFormat.Length;

            if (length > line.Length) return timeStampOut;

            string dateTimeStampString = line[..(dateTimeFormat.Length)];
            DateTime.TryParseExact(dateTimeStampString, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out timeStampOut);

            return timeStampOut;
        }
    }
}
