using System;

namespace LogScraper.Log.Content
{
    /// <summary>
    /// Represents a property of log content, including its description and filtering criteria.
    /// This is used to identify and filter log entries based on specific content properties.
    /// </summary>
    public class LogContentProperty : IEquatable<LogContentProperty>
    {
        /// <summary>
        /// A description of the log content property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Criteria used to filter or identify this log content property.
        /// </summary>
        public FilterCriteria Criteria { get; set; }

        /// <summary>
        /// Cached hash code for the log content property, calculated from the description.
        /// This ensures consistent and efficient hash code generation.
        /// </summary>
        private int HashCodeCache { get; set; }

        /// <summary>
        /// Determines whether the current log content property is equal to another log content property.
        /// Equality is based on the hash code of the description.
        /// </summary>
        /// <param name="other">The other log content property to compare with.</param>
        /// <returns>True if the log content properties are equal; otherwise, false.</returns>
        public bool Equals(LogContentProperty other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current log content property is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a LogContentProperty and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogContentProperty);
        }

        /// <summary>
        /// Gets the hash code for the log content property.
        /// The hash code is cached for performance and is based on the description.
        /// </summary>
        /// <returns>The hash code for the log content property.</returns>
        public override int GetHashCode()
        {
            // Double-checked locking to ensure thread safety when initializing the hash code cache.
            if (HashCodeCache == 0)
            {
                lock (this)
                {
                    if (HashCodeCache == 0) HashCodeCache = Description.GetHashCode();
                }
            }
            return HashCodeCache;
        }

        /// <summary>
        /// Returns the string representation of the log content property, which is its description.
        /// </summary>
        /// <returns>The description of the log content property.</returns>
        public override string ToString()
        {
            return Description;
        }
    }

}
