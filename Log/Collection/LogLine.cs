using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using System;
using System.Collections.Generic;

namespace LogScraper.Log.Collection
{
    /// <summary>
    /// Represents a single log line with its associated metadata and content properties.
    /// </summary>
    public class LogLine(string line, DateTime timestamp) : IEquatable<LogLine>
    {
        /// <summary>
        /// The content of the log line. The input is trimmed to remove leading and trailing whitespace.
        /// </summary>
        public string Line { get; set; } = line.Trim();

        /// <summary>
        /// The timestamp associated with the log line.
        /// </summary>
        public DateTime TimeStamp { get; set; } = timestamp;

        /// <summary>
        /// Additional log lines that are related to this log line. These are typically stack traces or other kind of log output which spans multiple lines.
        /// </summary>
        public List<string> AdditionalLogLines { get; set; }

        /// <summary>
        /// Cached hash code for the log line, calculated from the line content.
        /// This ensures consistent and efficient hash code generation.
        /// </summary>
        private int HashCodeCache { get; set; } = line.GetHashCode();

        /// <summary>
        /// Metadata properties associated with the log line, represented as key-value pairs.
        /// </summary>
        public Dictionary<LogMetadataProperty, string> LogMetadataPropertiesWithStringValue { get; set; }

        /// <summary>
        /// Content properties associated with the log line, represented as key-value pairs.
        /// </summary>
        public Dictionary<LogContentProperty, string> LogContentProperties { get; set; }

        /// <summary>
        /// Determines whether the current log line is equal to another log line.
        /// Equality is based on the hash code of the log line.
        /// </summary>
        /// <param name="other">The other log line to compare with.</param>
        /// <returns>True if the log lines are equal; otherwise, false.</returns>
        public bool Equals(LogLine other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current log line is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a LogLine and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogLine);
        }

        /// <summary>
        /// Gets the hash code for the log line.
        /// The hash code is cached for performance and is based on the line content.
        /// </summary>
        /// <returns>The hash code for the log line.</returns>
        public override int GetHashCode()
        {
            return HashCodeCache;
        }

        /// <summary>
        /// Returns the string representation of the log line, which is its content.
        /// </summary>
        /// <returns>The content of the log line.</returns>
        public override string ToString()
        {
            return Line;
        }
    }
}
