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
        private readonly struct FormatInfo(string format, int? explicitLength = null)
        {
            public readonly string Format = format;
            public readonly int ActualLength = explicitLength ?? format.Length;
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
            new("yyyy-MM-dd HH:mm:ss.fffzzz", 29),      // ISO 8601 with space separator + timezone offset (common in APIs, ASP.NET, cloud logs)
            new("yyyy-MM-ddTHH:mm:ss.fffzzz", 29),      // RFC3339 / ISO 8601 with 'T' + offset (REST, JSON, Kubernetes, modern services)

            new("yyyy-MM-dd HH:mm:ss.fffffff"),         // .NET default 7-digit fractional precision (DateTime.ToString default custom logs)
            new("yyyy-MM-ddTHH:mm:ss.fffffff"),         // .NET ISO-like with full precision (Serilog, structured logging)

            new("yyyy-MM-dd HH:mm:ss.ffffff"),          // Microsecond precision (some Java frameworks, database logs)
            new("yyyy-MM-ddTHH:mm:ss.ffffff"),          // ISO-style microseconds (container/platform logs)

            new("yyyy-MM-dd HH:mm:ss.fffZ"),            // ISO UTC literal 'Z' (JSON serializers, APIs)
            new("yyyy-MM-ddTHH:mm:ss.fffZ"),            // RFC3339 UTC format (very common in REST responses)

            new("yyyy-MM-dd HH:mm:ss.fff"),             // Common enterprise log format (Java, .NET, middleware)
            new("yyyy-MM-ddTHH:mm:ss.fff"),             // ISO without timezone (application logs, structured logging)

            new("yyyy-MM-dd HH:mm:ss,fff"),             // Java / Log4j default (comma milliseconds)
            new("yyyy-MM-dd'T'HH:mm:ss,fff"),           // Java ISO-style with comma milliseconds

            new("yyyy-MM-ddTHH:mm:sszzz", 25),          // ISO 8601 with offset, no milliseconds (APIs, infrastructure logs)
            new("yyyy-MM-dd HH:mm:sszzz", 25),          // Space-separated variant with offset (less common but seen in app logs)
            
            new("ddd, dd MMM yyyy HH:mm:ss 'GMT'"),     // RFC1123 HTTP-date (IIS, Azure, reverse proxies, HTTP headers)
            new("dd-MMM-yyyy HH:mm:ss.fff"),            // Log4j / legacy enterprise logs (textual month)
            new("dd-MMM-yyyy HH:mm:ss"),                // Same without milliseconds (older systems)
            
            new("yyyy-MM-ddTHH:mm:ssZ"),                // ISO 8601 UTC without milliseconds (common in REST APIs)
            new("yyyy-MM-dd HH:mm:ss"),                 // Very common default log format (generic apps, SQL exports)
            new("yyyy-MM-ddTHH:mm:ss"),                 // ISO without milliseconds or offset (minimal structured logs)

            new("[yyyy-MM-dd HH:mm:ss]"),               // Bracketed classic log style (custom frameworks, older .NET apps)
            new("[yyyy-MM-ddTHH:mm:ss]"),               // Bracketed ISO style (structured logs with prefix metadata)

            new("[yyyy-MM-dd HH:mm:ss.fff]"),           // Bracketed with milliseconds (common in enterprise logging)
            new("[yyyy-MM-ddTHH:mm:ss.fff]"),           // Bracketed ISO with milliseconds (modern structured logs)

            new("dd/MM/yyyy HH:mm:ss"),                 // European regional logs (custom apps, Windows exports)
            new("MM/dd/yyyy HH:mm:ss"),                 // US regional logs (legacy Windows / IIS exports)
            new("dd-MM-yyyy HH:mm:ss"),                 // European dash-separated variant (custom business apps)
            new("dd/MMM/yyyy:HH:mm:ss zzz",27),            // Apache / Nginx combined log format

            new("yyyyMMddHHmmss"),                      // Compact sortable timestamp (batch jobs, file naming)
            new("yyyyMMddHHmmssfff"),                   // Compact with milliseconds (ETL, financial systems)

            new("MMM dd HH:mm:ss"),                     // Syslog format (Linux, Docker, Kubernetes node logs)
            new("MMM  d HH:mm:ss"),                     // Syslog single-digit day variant (double-space padding)

            new("HH:mm:ss.fff"),                        // Time-only logs (embedded systems, continuation entries)
            new("HH:mm:ss"),                            // Minimal time-only format (rare as standalone, often trace continuation)
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