using System;

namespace LogScraper.Log.RawLogParsing
{
    /// <summary>
    /// Represents a parsed log line, including its raw content and optional timestamp information.
    /// </summary>
    /// <remarks>This class encapsulates the details of a single log entry, including whether it
    /// contains a timestamp and the timestamp value if present. It is intended for internal use in log processing
    /// scenarios.</remarks>
    /// <param name="rawLogEntry"></param>
    /// <param name="hasTimestamp"></param>
    /// <param name="timestamp"></param>
    public class ParsedLogEntry(string rawLogEntry, bool hasTimestamp, DateTime timestamp, int startIndexMetadata)
    {
        /// <summary>
        /// The original raw log entry as a string. This is the unprocessed log line that was parsed to extract timestamp information.
        /// </summary>
        public string RawLogEntry { get; set; } = rawLogEntry;
        /// <summary>
        /// Indicates whether the log entry contains a valid timestamp. This flag is set to true if a timestamp was successfully parsed from the log entry, and false otherwise.
        /// </summary>
        public bool HasTimestamp { get; set; } = hasTimestamp;
        /// <summary>
        /// The timestamp extracted from the log entry, if available. This field holds the DateTime value representing the timestamp of the log entry. If HasTimestamp is false, this field may contain a default or invalid DateTime value.
        /// </summary>
        public DateTime Timestamp { get; set; } = timestamp;

        /// <summary>
        /// The starting position of the metadata of the log entry based on the raw log entry string. This index indicates where the metadata portion of the log entry begins, allowing for further parsing and processing of the log content. It is derived from the structure of the raw log entry and is used to separate metadata from the main content of the log entry.
        /// </summary>
        public int StartIndexMetadata { get; set; } = startIndexMetadata;
    }
}