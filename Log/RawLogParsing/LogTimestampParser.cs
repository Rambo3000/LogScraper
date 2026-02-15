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
        private readonly struct FormatInfo
        {
            public readonly string Format;
            public readonly int ActualLength;

            public FormatInfo(string format, int? explicitLength = null)
            {
                Format = format;
                ActualLength = explicitLength ?? format.Length;
            }
        }

        /// <summary>
        /// A thread-safe dictionary to keep track of recognized format indices and their actual timestamp lengths.
        /// Key: format index, Value: actual parsed timestamp length
        /// </summary>
        private readonly ConcurrentDictionary<int, int> recognizedFormatIndices = [];

        /// <summary>
        /// An array of common date-time formats that the parser will attempt to use when extracting timestamps from log entries.
        /// For formats with variable length (zzz, zz, Z), an explicit length is provided.
        /// </summary>
        private static readonly FormatInfo[] commonFormats =
        [
            // Rank the longest formats on top to prevent partial matches
            new("yyyy-MM-dd HH:mm:ss.fffzzz", 29),      // zzz expands to +00:00 (6 chars instead of 3)
            new("yyyy-MM-ddTHH:mm:ss.fffzzz", 29),
            new("yyyy-MM-dd HH:mm:ss.fffffff"),
            new("yyyy-MM-ddTHH:mm:ss.fffffff"),
            new("yyyy-MM-dd HH:mm:ss.ffffff"),
            new("yyyy-MM-ddTHH:mm:ss.ffffff"),
            new("yyyy-MM-dd HH:mm:ss.fffZ"),
            new("yyyy-MM-ddTHH:mm:ss.fffZ"),
            new("yyyy-MM-dd HH:mm:ss.fff"),
            new("yyyy-MM-ddTHH:mm:ss.fff"),
            new("yyyy-MM-dd HH:mm:ss,fff"),
            new("yyyy-MM-dd'T'HH:mm:ss,fff"),
            new("yyyy-MM-ddTHH:mm:sszzz", 25),          // zzz expands to +00:00
            new("yyyy-MM-dd HH:mm:sszzz", 25),
            new("yyyy-MM-dd HH:mm:ss"),
            new("yyyy-MM-ddTHH:mm:ss"),
            new("[yyyy-MM-dd HH:mm:ss]"),
            new("[yyyy-MM-ddTHH:mm:ss]"),
            new("[yyyy-MM-dd HH:mm:ss.fff]"),
            new("[yyyy-MM-ddTHH:mm:ss.fff]"),
            new("dd/MM/yyyy HH:mm:ss"),
            new("MM/dd/yyyy HH:mm:ss"),
            new("dd-MM-yyyy HH:mm:ss"),
            new("yyyyMMddHHmmss"),
            new("yyyyMMddHHmmssfff"),
            new("MMM dd HH:mm:ss"),
            new("MMM  d HH:mm:ss"),
            new("HH:mm:ss.fff"),
            new("HH:mm:ss"),
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

            // Calculate the expected length of the timestamp based on the forced format, accounting for variable-length timezone components
            int forceDateTimeFormatLength = forceDateTimeFormat == null ? -1 : (forceDateTimeFormat.Contains("zzz") ? forceDateTimeFormat.Length + 3 : forceDateTimeFormat.Length);

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
                    if (TryParseTimestamp(entry, forceDateTimeFormat, forceDateTimeFormatLength, out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, forceDateTimeFormatLength);
                    }
                    else
                    {
                        result[i] = new ParsedLogEntry(entry, false, default, -1);
                    }
                    return;
                }

                // Try recognized format indices first (hot path)
                foreach (var kvp in recognizedFormatIndices)
                {
                    int formatIndex = kvp.Key;
                    int actualLength = kvp.Value;

                    if (TryParseTimestamp(entry, commonFormats[formatIndex].Format, actualLength, out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, actualLength);
                        return;
                    }
                }

                // Try all formats to detect (cold path)
                for (int formatIndex = 0; formatIndex < commonFormats.Length; formatIndex++)
                {
                    if (recognizedFormatIndices.ContainsKey(formatIndex)) continue;

                    FormatInfo formatInfo = commonFormats[formatIndex];

                    if (TryParseTimestamp(entry, formatInfo.Format, formatInfo.ActualLength, out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, formatInfo.ActualLength);
                        recognizedFormatIndices.TryAdd(formatIndex, formatInfo.ActualLength);
                        return;
                    }
                }

                result[i] = new ParsedLogEntry(entry, false, default, -1);
            });

            return length == 1 && result[0] == null ? [] : result;
        }

        /// <summary>
        /// Attempts to parse a timestamp from a log entry using a specified date-time format.
        /// </summary>
        /// <param name="entry">The raw log entry from which to extract the timestamp.</param>
        /// <param name="format">The date-time format to use for parsing the timestamp.</param>
        /// <param name="length">The length of the timestamp to extract.</param>
        /// <param name="timestamp">An output parameter that will contain the parsed DateTime value if the parsing is successful.</param>
        /// <returns>True if the timestamp was successfully parsed using the specified format; otherwise, false.</returns>
        private static bool TryParseTimestamp(string entry, string format, int length, out DateTime timestamp)
        {
            timestamp = default;
            if (entry.Length < length) return false;

            return DateTime.TryParseExact(entry[..length], format, CultureInfo.InvariantCulture,
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