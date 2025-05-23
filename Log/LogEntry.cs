using System;
using System.Collections.Generic;
using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log
{
    /// <summary>
    /// Represents a single log entry with its associated metadata and content properties.
    /// </summary>
    public class LogEntry: IEquatable<LogEntry>
    {
        /// <summary>
        /// Initializes a new instance of the LogEntry class with the specified entry content and timestamp.
        /// </summary>
        /// <param name="entry">The content of the log entry.</param>
        /// <param name="timestamp">The timestamp associated with the log entry.</param>
        public LogEntry(string entry, DateTime timestamp)
        {
            Entry =  entry.Trim();
            TimeStamp = timestamp;
            HashCodeCache = Entry.GetHashCode();
        }
        /// <summary>
        /// The content of the log entry.
        /// </summary>
        public string Entry { get; private set; }

        /// <summary>
        /// The timestamp associated with the log entry.
        /// </summary>
        public DateTime TimeStamp { get; set; } 

        /// <summary>
        /// Additional log entries that are related to this log entry. These are typically stack traces or other kind of log output which spans multiple log entries.
        /// </summary>
        public List<string> AdditionalLogEntries { get; set; }

        /// <summary>
        /// The starting position of the content of the log entry based. This excludes the timestamp and metadata (if applicable)
        /// This is derived from the position of the metadata-content seperator
        /// </summary>
        public int StartIndexContent { get; set; }

        /// <summary>
        /// Cached hash code for the log entry, calculated from the log entry content.
        /// This ensures consistent and efficient hash code generation.
        /// </summary>
        private int HashCodeCache { get; set; }

        /// <summary>
        /// Metadata properties associated with the log entry, represented as key-value pairs.
        /// </summary>
        public IndexDictionary<LogMetadataProperty, string> LogMetadataPropertiesWithStringValue { get; set; }

        /// <summary>
        /// Content properties associated with the log entry, represented as key-value pairs.
        /// </summary>
        public IndexDictionary<LogContentProperty, LogContentValue> LogContentProperties { get; set; }

        /// <summary>
        /// Determines whether the current log entry is equal to another log entry.
        /// Equality is based on the hash code of the log entry.
        /// </summary>
        /// <param name="other">The other log entry to compare with.</param>
        /// <returns>True if the log entries are equal; otherwise, false.</returns>
        public bool Equals(LogEntry other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current log entry is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a LogEntry and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogEntry);
        }

        /// <summary>
        /// Gets the hash code for the log entry.
        /// The hash code is cached for performance and is based on the log entry content.
        /// </summary>
        /// <returns>The hash code for the log entry.</returns>
        public override int GetHashCode()
        {
            return HashCodeCache;
        }

        /// <summary>
        /// Returns the string representation of the log entry, which is its content.
        /// </summary>
        /// <returns>The content of the log entry.</returns>
        public override string ToString()
        {
            return Entry;
        }
    }
}
