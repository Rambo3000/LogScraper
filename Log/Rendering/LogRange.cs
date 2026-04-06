using System;
using System.Collections.Generic;

namespace LogScraper.Log.Rendering
{
    /// <summary>
    /// Represents a range of log entries defined by a beginning and an end log entry.
    /// This can be used to specify a subset of log entries for rendering.
    /// </summary>
    public class LogRange : IEquatable<LogRange>
    {
        /// <summary>
        /// The log entry that marks the beginning of the log range.
        /// If null, the range starts from the beginning of the log collection.
        /// </summary>
        public LogEntry Begin { get; set; }

        /// <summary>
        /// The log entry that marks the end of the log range.
        /// If null, the range ends at the beginning of the log collection.
        /// </summary>
        public LogEntry End { get; set; }

        /// <summary>
        /// Indicates whether the log range is constrained by either a beginning or an end log entry.
        /// </summary>
        public bool IsConstrained
        {
            get
            {
                return Begin != null || End != null;
            }
        }

        /// <summary>
        /// Determines whether the specified LogRange is equal to the current LogRange.
        /// </summary>
        /// <param name="other">The LogRange to compare with the current LogRange.</param>
        /// <returns>true if the specified LogRange is equal to the current LogRange; otherwise, false.</returns>
        public bool Equals(LogRange other)
        {
            return other != null &&
                   EqualityComparer<LogEntry>.Default.Equals(Begin, other.Begin) &&
                   EqualityComparer<LogEntry>.Default.Equals(End, other.End);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Begin, End);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LogRange);
        }
    }
}
