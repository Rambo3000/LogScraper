using LogScraper.Log.Collection;
using System;
using System.Globalization;
using System.Linq;

namespace LogScraper
{
    internal static class LogReader
    {
        public static void ReadIntoLogCollection(string[] logLines, LogCollection logCollection, string dateTimeFormat)
        {
            if (logLines == null) return;
            if (string.IsNullOrEmpty(dateTimeFormat)) throw new ArgumentNullException(nameof(dateTimeFormat));

            LogLine lastLogLine = null;
            if (LogCollection.Instance.LogLines.Count > 0)
            {
                lastLogLine = logCollection.LogLines.Last();
            }

            int logLinesStartIndex = 0;
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

            bool logLineIsAdded = false;
            for (int i = logLinesStartIndex; i < logLines.Length; i++)
            {
                //Empty lines are skipped always
                if (logLines[i] == string.Empty) continue;

                DateTime timestamp = GetDateTimeFromRawLogLine(logLines[i], dateTimeFormat);

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
