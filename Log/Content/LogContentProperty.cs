using System;
using System.Collections.Generic;
using System.Drawing;
using LogScraper.Utilities.IndexDictionary;
using Newtonsoft.Json;

namespace LogScraper.Log.Content
{
    /// <summary>
    /// Represents a property of log content, including its description and filtering criteria.
    /// This is used to identify and filter log entries based on specific content properties.
    /// </summary>
    public class LogContentProperty() : IEquatable<LogContentProperty>, IHasIndex
    {
        /// <summary>
        /// A description of the log content property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Criteria used to filter or identify this log content property.
        /// </summary>
        /// <remarks>Do nu use this single criteria as it is absolete, use the <see cref="Criterias"/> instead.</remarks>
        [Obsolete("Use Criterias instead.")]
        public FilterCriteria Criteria { get; set; }

        /// <summary>
        /// List of criteria used to filter or identify this log content property. 
        /// If one of the criteria is met, the log content property is considered valid.
        /// </summary>
        public List<FilterCriteria> Criterias { get; set; } = [];

        /// <summary>
        /// Indicates whether the log content property is a filter for the beginning of a flow tree.
        /// </summary>
        public bool IsBeginFlowTreeFilter { get; set; }

        /// <summary>
        /// Indicates whether an error is logged in the log entry if this log content property is present.
        /// </summary>
        public bool IsErrorProperty { get; set; } = false;

        /// <summary>
        /// Indicates whether a custom color is active for this log content property.
        /// </summary>
        public bool IsCustomStyleEnabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the color used to display log text.
        /// </summary>
        public Color CustomTextColor { get; set; } = Color.Empty;

        /// <summary>
        /// Gets or sets the background color used for log display.
        /// </summary>
        public Color CustomBackColor { get; set; } = Color.Empty;

        /// <summary>
        /// Indicates whether the custom background color should be applied to the entire width of the line, beyond the text width.
        /// </summary>
        public bool IsCustomBackColorFillLine { get; set; } = false;

        /// <summary>
        /// Description of the content property which is a filter for the end of a flow tree.
        /// Used for JSON serilization and only when <see cref="IsBeginFlowTreeFilter"/> is true.
        /// </summary>
        public string EndFlowTreeContentPropertyDescription { get; set; }

        /// <summary>
        /// Content property which is a filter for the end of a flow tree.
        /// Only when <see cref="IsBeginFlowTreeFilter"/> is true, this property is used.
        /// </summary>
        [JsonIgnore]
        public LogContentProperty EndFlowTreeContentProperty { get; set; }

        /// <summary>
        /// Index of the log content property in the list of log content properties. Used for using the <see cref="IndexDictionary{TKey,TValue}"/> class.
        /// </summary>
        [JsonIgnore]
        public int Index { get; set; } = -1;

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
        /// <summary>
        /// Creates a deep copy of the current LogContentProperty instance, including its filter criteria and related
        /// property values.
        /// </summary>
        /// <remarks>The returned copy is independent of the original instance. Changes to the copied
        /// LogContentProperty or its filter criteria do not affect the original object.</remarks>
        /// <returns>A new LogContentProperty instance that is a deep copy of the current instance. All filter criteria and
        /// property values are duplicated.</returns>
        internal LogContentProperty Copy()
        {
            LogContentProperty newProperty = new()
            {
                Description = Description,
                IsErrorProperty = IsErrorProperty,
                IsBeginFlowTreeFilter = IsBeginFlowTreeFilter,
                IsCustomStyleEnabled = IsCustomStyleEnabled,
                IsCustomBackColorFillLine = IsCustomBackColorFillLine,
                CustomTextColor = CustomTextColor,
                CustomBackColor = CustomBackColor,
                EndFlowTreeContentProperty = EndFlowTreeContentProperty,
                EndFlowTreeContentPropertyDescription = EndFlowTreeContentPropertyDescription
            };
            foreach (FilterCriteria criteria in Criterias)
            {
                newProperty.Criterias.Add(new FilterCriteria()
                {
                    BeforePhrase = criteria.BeforePhrase,
                    AfterPhrase = criteria.AfterPhrase
                });
            }
            return newProperty;
        }
    }

}
