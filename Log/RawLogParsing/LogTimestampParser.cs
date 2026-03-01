using System;
using System.Collections.Immutable;
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
            public FormatInfo(string format)
            { 
                if (format == null || string.IsNullOrWhiteSpace(format)) return;
                Format = format;
                ActualLength = format.Length;

                // 2009-06-15T13:45:30-07:00 -> -07:00 is 6 chars but zzz is 3 chars, so we need to adjust the length accordingly
                if (format.Contains("zzz")) ActualLength += 3;
                // 2009-06-15T13:45:30-07 -> -07 is 3 chars but zz is 2 chars, so we need to adjust the length accordingly
                else if (format.Contains("zz")) ActualLength += 1;
                // 2009-06-15T13:45:30-7 -> -7 is 2 chars but z is 1 char, so we need to adjust the length accordingly
                else if (format.Contains('z')) ActualLength += 1;
            }
        }

        /// <summary>
        /// An immutable sorted set to keep track of recognized format indices in order.
        /// Formats are always tried in the order they appear in commonFormats array.
        /// </summary>
        private ImmutableSortedSet<int> recognizedFormatIndices = ImmutableSortedSet<int>.Empty;

        /// <summary>
        /// An array of common date-time formats that the parser will attempt to use when extracting timestamps from log entries.
        /// For formats with variable length (zzz, zz, Z), an explicit length is provided.
        /// </summary>
        private static readonly FormatInfo[] commonFormats =
        [
            // Rank the longest formats on top to prevent partial matches
            new("yyyy-MM-dd HH:mm:ss.fffzzz"),      // ISO 8601 with space separator + timezone offset (common in APIs, ASP.NET, cloud logs)
            new("yyyy-MM-ddTHH:mm:ss.fffzzz"),      // RFC3339 / ISO 8601 with 'T' + offset (REST, JSON, Kubernetes, modern services)

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

            new("yyyy-MM-ddTHH:mm:sszzz"),          // ISO 8601 with offset, no milliseconds (APIs, infrastructure logs)
            new("yyyy-MM-dd HH:mm:sszzz"),          // Space-separated variant with offset (less common but seen in app logs)
            
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
            new("dd/MMM/yyyy:HH:mm:ss zzz"),            // Apache / Nginx combined log format

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
            FormatInfo forceFormat = new(forceDateTimeFormat);

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

                if (forceDateTimeFormat != null)
                {
                    if (TryParseTimestamp(entry, forceFormat, out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, forceFormat.ActualLength);
                    }
                    else
                    {
                        result[i] = new ParsedLogEntry(entry, false, default, -1);
                    }
                    return;
                }

                // Try recognized format indices first (hot path) - automatically sorted
                foreach (int formatIndex in recognizedFormatIndices)
                {
                    FormatInfo formatInfo = commonFormats[formatIndex];

                    if (TryParseTimestamp(entry, formatInfo, out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, formatInfo.ActualLength);
                        return;
                    }
                }

                // Try all formats to detect (cold path)
                for (int formatIndex = 0; formatIndex < commonFormats.Length; formatIndex++)
                {
                    if (recognizedFormatIndices.Contains(formatIndex)) continue;

                    FormatInfo formatInfo = commonFormats[formatIndex];

                    if (TryParseTimestamp(entry, formatInfo, out DateTime timestamp))
                    {
                        result[i] = new ParsedLogEntry(entry, true, timestamp, formatInfo.ActualLength);
                        ImmutableInterlocked.Update(ref recognizedFormatIndices, set => set.Add(formatIndex));
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
        /// <param name="timestamp">An output parameter that will contain the parsed DateTime value if the parsing is successful.</param>
        /// <returns>True if the timestamp was successfully parsed using the specified format; otherwise, false.</returns>
        private static bool TryParseTimestamp(string entry, FormatInfo formatInfo, out DateTime timestamp)
        {
            timestamp = default;
            if (entry.Length < formatInfo.ActualLength) return false;

            return DateTime.TryParseExact(entry[..formatInfo.ActualLength], formatInfo.Format, CultureInfo.InvariantCulture,
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