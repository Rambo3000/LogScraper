using System;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// Represents a value of a metdata property in a log.
    /// This class includes the value itself, its occurrence count, and whether it is enabled for filtering.
    /// </summary>
    public class LogMetadataValue : IEquatable<LogMetadataValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogMetadataValue"/> class.
        /// </summary>
        /// <param name="value">The metadata value as a string.</param>
        /// <param name="count">The number of times this value appears in the log.</param>
        /// <param name="isFilterEnabled">Indicates whether this value is enabled for filtering.</param>
        public LogMetadataValue(string value, int count, bool isFilterEnabled)
        {
            Value = value;
            Count = count;
            IsFilterEnabled = isFilterEnabled;

            // Cache the hash code for performance, based on the value string.
            HashCodeCache = Value.GetHashCode();
        }

        /// <summary>
        /// The metadata value as a string.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The number of times this metadata value appears in the log.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Indicates whether this metadata value is enabled for filtering.
        /// </summary>
        public bool IsFilterEnabled { get; set; }

        /// <summary>
        /// Cached hash code for the metadata value, calculated from the value string.
        /// This ensures consistent and efficient hash code generation.
        /// </summary>
        private int HashCodeCache { get; set; }

        /// <summary>
        /// Returns the string representation of the metadata value.
        /// </summary>
        /// <returns>The metadata value as a string.</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Determines whether the current metadata value is equal to another metadata value.
        /// Equality is based on the hash code of the value string.
        /// </summary>
        /// <param name="other">The other metadata value to compare with.</param>
        /// <returns>True if the metadata values are equal; otherwise, false.</returns>
        public bool Equals(LogMetadataValue other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current metadata value is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a <see cref="LogMetadataValue"/> and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogMetadataValue);
        }

        /// <summary>
        /// Gets the hash code for the metadata value.
        /// The hash code is cached for performance and is based on the value string.
        /// </summary>
        /// <returns>The hash code for the metadata value.</returns>
        public override int GetHashCode()
        {
            return HashCodeCache;
        }
    }
}
