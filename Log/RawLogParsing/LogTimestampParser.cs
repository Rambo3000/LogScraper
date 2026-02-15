using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.Tasks;

namespace LogScraper.Log.RawLogParsing
{
    /// <summary>
    /// A class responsible for parsing timestamps from raw log entries. 
    /// It attempts to detect and extract timestamps based on a set of common date-time formats, 
    /// while also optimizing for performance by caching recognized formats. The parser can handle 
    /// various timestamp formats commonly found in logs, and it provides a mechanism to force a 
    /// specific format if needed.
    /// </summary>
    public class LogTimestampParser
    {
        /// <summary>
        /// A thread-safe dictionary to keep track of recognized format indices. 
        /// This allows the parser to quickly identify which formats have been successfully parsed in previous log entries.
        /// </summary>
        private readonly ConcurrentDictionary<int, byte> recognizedFormatIndices = [];

        /// <summary>
        /// An array of common date-time formats that the parser will attempt to use when extracting timestamps from log entries.
        /// </summary>
        private static readonly string[] commonFormats =
        [
            // Rank the longest formats on top to prevent partial matches
            // Note: do NOT use zzz as this will not parse correctly as the length of the format will not match the entry length
            "yyyy-MM-dd HH:mm:ss.fff+00:00",
            "yyyy-MM-ddTHH:mm:ss.fff+00:00",
            "yyyy-MM-dd HH:mm:ss.fffffff",
            "yyyy-MM-ddTHH:mm:ss.fffffff",
            "yyyy-MM-dd HH:mm:ss.ffffff",
            "yyyy-MM-ddTHH:mm:ss.ffffff",
            "yyyy-MM-dd HH:mm:ss.fffZ",
            "yyyy-MM-ddTHH:mm:ss.fffZ",
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyy-MM-dd HH:mm:ss,fff",
            "yyyy-MM-dd'T'HH:mm:ss,fff",
            "yyyy-MM-ddTHH:mm:ss+00:00",
            "yyyy-MM-dd HH:mm:ss+00:00",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss",
            "[yyyy-MM-dd HH:mm:ss]",
            "[yyyy-MM-ddTHH:mm:ss]",
            "[yyyy-MM-dd HH:mm:ss.fff]",
            "[yyyy-MM-ddTHH:mm:ss.fff]",
            "dd/MM/yyyy HH:mm:ss",
            "MM/dd/yyyy HH:mm:ss",
            "dd-MM-yyyy HH:mm:ss",
            "yyyyMMddHHmmss",
            "yyyyMMddHHmmssfff",
            "MMM dd HH:mm:ss",
            "MMM  d HH:mm:ss",
            "HH:mm:ss.fff",
            "HH:mm:ss",
        ];

        /// <summary>
        /// Parses an array of raw log entries starting from a specified index, attempting to extract timestamps based on common formats.
        /// </summary>
        /// <param name="rawLogEntries">The array of raw log entries to parse.</param>
        /// <param name="startIndex">The index from which to start parsing the log entries.</param>
        /// <param name="forceDateTimeFormat">An optional parameter to force a specific date-time format for parsing. If provided, the parser will attempt to use this format first before trying the common formats.</param>
        /// <returns>An array of ParsedLogEntry objects containing the raw log entry, whether a timestamp was found, the extracted timestamp (if any), and the length of the timestamp format used.</returns>
        public ParsedLogEntry[] Parse(string[] rawLogEntries, int startIndex, string forceDateTimeFormat = null)
        {
            int length = rawLogEntries.Length - startIndex;
            if (length == 0) return [];

            ParsedLogEntry[] result = new ParsedLogEntry[length];

            Parallel.For(0, length, i =>
            {
                string entry = rawLogEntries[startIndex + i];

                if (string.IsNullOrEmpty(entry))
                {
                    result[i] = null;
                    return;
                }

                if (!CouldHaveTimestamp(entry))
                {
                    result[i] = new ParsedLogEntry(entry, false, default, -1);
                    return;
                }

                // If a specific format is forced, try it first
                if (forceDateTimeFormat != null)
                {
                    if (TryParseTimestamp(entry, forceDateTimeFormat, out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, forceDateTimeFormat.Length);
                    }
                    else
                    {
                        result[i] = new ParsedLogEntry(entry, false, default, -1);
                    }
                    return;
                }

                // Try recognized format indices first
                foreach (int formatIndex in recognizedFormatIndices.Keys)
                {
                    if (TryParseTimestamp(entry, commonFormats[formatIndex], out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, commonFormats[formatIndex].Length);
                        return;
                    }
                }

                // Try all formats to detect
                for (int formatIndex = 0; formatIndex < commonFormats.Length; formatIndex++)
                {
                    if (recognizedFormatIndices.ContainsKey(formatIndex)) continue;

                    if (TryParseTimestamp(entry, commonFormats[formatIndex], out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, commonFormats[formatIndex].Length);
                        recognizedFormatIndices.TryAdd(formatIndex, 0);
                        return;
                    }
                }

                result[i] = new ParsedLogEntry(entry, false, default, -1);
            });

            return length == 1 && result[0] == null ? [] : result;
        }
        /// <summary>
        /// Attempts to parse a timestamp from a log entry using a specified date-time format. 
        /// The method checks if the log entry is long enough to contain the format and then tries 
        /// to parse the timestamp using the provided format. 
        /// </summary>
        /// <param name="entry">The raw log entry from which to extract the timestamp.</param>
        /// <param name="format">The date-time format to use for parsing the timestamp.</param>
        /// <param name="timestamp">An output parameter that will contain the parsed DateTime value if the parsing is successful.</param>
        /// <returns>True if the timestamp was successfully parsed using the specified format; otherwise, false.</returns>
        private static bool TryParseTimestamp(string entry, string format, out DateTime timestamp)
        {
            if (entry.Length < format.Length)
            {
                timestamp = default;
                return false;
            }
            return DateTime.TryParseExact(entry[..format.Length], format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out timestamp);
        }

        /// <summary>
        /// Determines whether a log entry could potentially contain a timestamp based on its content.
        /// </summary>
        /// <param name="entry">The raw log entry to evaluate.</param>
        /// <returns>True if the log entry could contain a timestamp; otherwise, false.</returns>
        private static bool CouldHaveTimestamp(string entry)
        {
            if (entry.Length < 8) return false;

            char first = entry[0];
            if (char.IsDigit(first) || first == '[') return true;

            int digitCount = 0;
            int checkLength = Math.Min(8, entry.Length);
            for (int i = 0; i < checkLength; i++)
            {
                if (char.IsDigit(entry[i])) digitCount++;
                if (digitCount >= 6) return true;
            }

            return false;
        }
    }
}