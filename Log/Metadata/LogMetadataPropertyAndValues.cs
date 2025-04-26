using System;
using System.Collections.Generic;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// Represents a metadata property and its associated values which are found throughout the log.
    /// This class also provides functionality to determine if filtering is enabled for the metadata.
    /// </summary>
    public class LogMetadataPropertyAndValues : IEquatable<LogMetadataPropertyAndValues>
    {
        /// <summary>
        /// A dictionary containing metadata values associated with the property.
        /// The key is a <see cref="LogMetadataValue"/> object, and the value is its string representation.
        /// </summary>
        public Dictionary<LogMetadataValue, string> LogMetadataValues { get; set; }

        /// <summary>
        /// The metadata property for which the values are associated.
        /// This defines the type or category of the metadata.
        /// </summary>
        public LogMetadataProperty LogMetadataProperty { get; set; }

        /// <summary>
        /// Indicates whether at least one of the values in has filtering enabled.
        /// Returns true if at least one metadata value has filtering enabled; otherwise, false.
        /// </summary>
        public bool IsFilterEnabled
        {
            get
            {
                // If there are no metadata values, filtering is not enabled.
                if (LogMetadataValues == null || LogMetadataValues.Count == 0) return false;

                // Check if any metadata value has filtering enabled.
                foreach (KeyValuePair<LogMetadataValue, string> kvp in LogMetadataValues)
                {
                    if (kvp.Key.IsFilterEnabled == true) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Determines whether the current object is equal to another <see cref="LogMetadataPropertyAndValues"/> object.
        /// Equality is based on the description of the associated metadata property.
        /// </summary>
        /// <param name="other">The other <see cref="LogMetadataPropertyAndValues"/> object to compare with.</param>
        /// <returns>True if the objects are equal; otherwise, false.</returns>
        public bool Equals(LogMetadataPropertyAndValues other)
        {
            return null != other && LogMetadataProperty.Description == other.LogMetadataProperty.Description;
        }

        /// <summary>
        /// Determines whether the current object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a <see cref="LogMetadataProperty"/> and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogMetadataProperty);
        }

        /// <summary>
        /// Gets the hash code for the current object.
        /// The hash code is based on the associated metadata property's hash code.
        /// </summary>
        /// <returns>The hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return LogMetadataProperty.GetHashCode();
        }
    }
}
