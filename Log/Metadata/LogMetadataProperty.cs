using System;
using LogScraper.Utilities.IndexDictionary;
using Newtonsoft.Json;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// Represents the definition of a metadata property of a log, including its description and filtering criteria.
    /// </summary>
    public class LogMetadataProperty : IEquatable<LogMetadataProperty>, IHasIndex
    {
        /// <summary>
        /// Index of the metadata property in the list of metadata properties. Used for using the <see cref="IndexDictionary{TKey,TValue}"/> class.
        /// </summary>
        [JsonIgnore]
        public int Index { get; set; } = -1;

        /// <summary>
        /// A description of the metadata property.
        /// This provides a human-readable explanation of the property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Criteria used to filter or identify this metadata property.
        /// This defines the conditions under which the property is relevant.
        /// </summary>
        public FilterCriteria Criteria { get; set; }

        /// <summary>
        /// Indicates whether the metadata property is related to session data.
        /// This is used to quickly filter on session-related properties.
        /// </summary>
        public bool IsSessionData { get; set; } = false;

        /// <summary>
        /// Cached hash code for the metadata property, calculated from the description.
        /// This ensures consistent and efficient hash code generation.
        /// </summary>
        private int HashCodeCache { get; set; }

        /// <summary>
        /// Returns the string representation of the metadata property, which is its description.
        /// </summary>
        /// <returns>The description of the metadata property.</returns>
        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// Determines whether the current metadata property is equal to another metadata property.
        /// Equality is based on the hash code of the description.
        /// </summary>
        /// <param name="other">The other metadata property to compare with.</param>
        /// <returns>True if the metadata properties are equal; otherwise, false.</returns>
        public bool Equals(LogMetadataProperty other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current metadata property is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a LogMetadataProperty and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogMetadataProperty);
        }

        /// <summary>
        /// Gets the hash code for the metadata property.
        /// The hash code is cached for performance and is based on the description.
        /// </summary>
        /// <returns>The hash code for the metadata property.</returns>
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
    }
}
