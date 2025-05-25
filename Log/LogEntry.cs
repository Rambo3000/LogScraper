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
    public class LogEntry: IEquatable<LogEntry>, IComparable<LogEntry>
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

        /// <summary>
        /// Compares the current <see cref="LogEntry"/> instance to another <see cref="LogEntry"/> based on their
        /// timestamps.
        /// </summary>
        /// <remarks>This method compares <see cref="LogEntry"/> instances based solely on their <see
        /// cref="TimeStamp"/> property.</remarks>
        /// <param name="other">The <see cref="LogEntry"/> to compare to the current instance. Can be <see langword="null"/>.</param>
        /// <returns>A value indicating the relative order of the <see cref="LogEntry"/> instances: <list type="bullet">
        public int CompareTo(LogEntry other)
        {
            if (other == null) return 1;
            return this.TimeStamp.CompareTo(other.TimeStamp);
        }

        /// <summary>
        /// Determines whether one <see cref="LogEntry"/> instance is greater than another based on their
        /// timestamps.
        /// </summary>
        /// <remarks>This operator uses the <see cref="LogEntry.CompareTo(LogEntry)"/> method to perform
        /// the comparison.</remarks>
        /// <param name="left">The first <see cref="LogEntry"/> instance to compare.</param>
        /// <param name="right">The second <see cref="LogEntry"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(LogEntry left, LogEntry right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether one <see cref="LogEntry"/> instance is less than another based on their
        /// timestamps.
        /// </summary>
        /// <remarks>This operator uses the <see cref="LogEntry.CompareTo(LogEntry)"/> method to perform
        /// the comparison.</remarks>
        /// <param name="left">The first <see cref="LogEntry"/> instance to compare.</param>
        /// <param name="right">The second <see cref="LogEntry"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(LogEntry left, LogEntry right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether the first <see cref="LogEntry"/> is greater than or equal to the second <see
        /// cref="LogEntry"/> based on their timestamps.
        /// </summary>
        /// <remarks>This operator uses the <see cref="LogEntry.CompareTo(LogEntry)"/> method to perform
        /// the comparison.</remarks>
        /// <param name="left">The first <see cref="LogEntry"/> instance to compare.</param>
        /// <param name="right">The second <see cref="LogEntry"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; 
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(LogEntry left, LogEntry right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Determines whether one <see cref="LogEntry"/> instance is less than or equal to another based on their
        /// timestamps.
        /// </summary>
        /// <remarks>This operator uses the <see cref="LogEntry.CompareTo(LogEntry)"/> method to perform
        /// the comparison.</remarks>
        /// <param name="left">The first <see cref="LogEntry"/> instance to compare.</param>
        /// <param name="right">The second <see cref="LogEntry"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; 
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(LogEntry left, LogEntry right)
        {
            return left.CompareTo(right) <= 0;
        }
    }
}
