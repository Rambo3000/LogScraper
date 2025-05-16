using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogScraper.Log.Content
{
    public class LogContentValue : IEquatable<LogContentValue>
    {
        public string Value { get; set; }
        public string TimeDescription { get; set; }

        /// <summary>
        /// Cached hash code for the log content property, calculated from the description.
        /// This ensures consistent and efficient hash code generation.
        /// </summary>
        private int HashCodeCache { get; set; }

        /// <summary>
        /// Cached hash code for the log content property, calculated from the description.
        /// This ensures consistent and efficient hash code generation.
        /// </summary>
        private int ValueHashCodeCache { get; set; }

        /// <summary>
        /// Determines whether the current log content property is equal to another log content property.
        /// Equality is based on the hash code of the description.
        /// </summary>
        /// <param name="other">The other log content property to compare with.</param>
        /// <returns>True if the log content properties are equal; otherwise, false.</returns>
        public bool Equals(LogContentValue other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current log content property is equal to another log content property.
        /// Equality is based on the hash code of the description.
        /// </summary>
        /// <param name="other">The other log content property to compare with.</param>
        /// <returns>True if the log content properties are equal; otherwise, false.</returns>
        public bool EqualsValue(LogContentValue other)
        {
            return GetHashCodeValue() == other.GetHashCodeValue();
        }

        /// <summary>
        /// Determines whether the current log content property is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a LogContentProperty and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogContentValue);
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
                    if (HashCodeCache == 0) HashCodeCache = ToString().GetHashCode();
                }
            }
            return HashCodeCache;
        }

        /// <summary>
        /// Gets the hash code for the log content property.
        /// The hash code is cached for performance and is based on the description.
        /// </summary>
        /// <returns>The hash code for the log content property.</returns>
        public int GetHashCodeValue()
        {
            // Double-checked locking to ensure thread safety when initializing the hash code cache.
            if (ValueHashCodeCache == 0)
            {
                lock (this)
                {
                    if (ValueHashCodeCache == 0) ValueHashCodeCache = Value.ToString().GetHashCode();
                }
            }
            return ValueHashCodeCache;
        }

        public override string ToString()
        {
            return TimeDescription + " " + Value;
        }
    }
}
