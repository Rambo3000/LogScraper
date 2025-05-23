﻿using System;
using System.Collections.Generic;
using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using LogScraper.LogTransformers;
using LogScraper.LogTransformers.Implementations;
using Newtonsoft.Json;

namespace LogScraper.Log.Layout
{
    /// <summary>
    /// Represents the layout of a log file, including its description, date and time format,
    ///  metadata properties, content filters, and transformers.
    /// </summary>
    public class LogLayout : IEquatable<LogLayout>
    {
        /// <summary>
        /// A description of the log layout.
        /// </summary>
        public string Description { get; set; }

        [JsonIgnore]
        public string DateTimeFormatCache;

        /// <summary>
        /// The format used for parsing date and time in the log.
        /// Setting this property also updates the StartPositionCache based on the length of the format.
        /// </summary>
        public string DateTimeFormat
        {
            get { return DateTimeFormatCache; }
            set
            {
                DateTimeFormatCache = value;

                // Update the StartPositionCache based on the length of the DateTimeFormat.
                if (null != value)
                {
                    StartPositionCache = value.Length;
                }
                else
                {
                    StartPositionCache = 0; // Reset the cache if the format is null.
                }
            }
        }

        [JsonIgnore]
        private int StartPositionCache { get; set; }

        /// <summary>
        /// The starting position of the log entry based on the DateTimeFormat.
        /// This is derived from the DateTimeFormat length.
        /// </summary>
        [JsonIgnore]
        public int StartIndexMetadata { get { return StartPositionCache; } }

        /// <summary>
        /// A list of metadata properties associated with the log layout.
        /// These properties define the metadata that can be extracted from the log.
        /// </summary>
        public List<LogMetadataProperty> LogMetadataProperties { get; set; }

        /// <summary>
        /// A list of content filters that define specific events within the log.
        /// These filters are used to identify specific sections of the log.
        /// </summary>
        [JsonProperty("LogContentBeginEndFilters")]
        public List<LogContentProperty> LogContentProperties { get; set; }

        /// <summary>
        /// Criteria for removing metadata from the log.
        /// This is used to clean up or simplify log entries by removing unnecessary metadata.
        /// </summary>
        public FilterCriteria RemoveMetaDataCriteria { get; set; }

        [JsonIgnore]
        /// <summary>
        /// A list of transformers that can be applied to the log entries.
        /// These transformers modify the log entries based on specific rules or configurations.
        /// </summary>
        public List<ILogTransformer> LogTransformers { get; set; }

        [JsonProperty("transformers")]
        /// <summary>
        /// Configuration for log transformers, used for JSON serialization and deserialization.
        /// </summary>
        public List<LogTransformerConfig> LogTransformersConfig { get; set; }

        /// <summary>
        /// Determines whether the current LogLayout is equal to another LogLayout based on their hash codes.
        /// </summary>
        /// <param name="other">The other LogLayout to compare with.</param>
        /// <returns>True if the two LogLayouts are equal; otherwise, false.</returns>
        public bool Equals(LogLayout other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current LogLayout is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if the object is a LogLayout and is equal to the current instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LogLayout);
        }

        /// <summary>
        /// Gets the hash code for the LogLayout, based on its Description.
        /// </summary>
        /// <returns>The hash code for the LogLayout.</returns>
        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation of the LogLayout, which is its Description.
        /// </summary>
        /// <returns>The Description of the LogLayout.</returns>
        public override string ToString()
        {
            return Description.ToString();
        }

        /// <summary>
        /// Creates a deep copy of the current LogLayout instance.
        /// </summary>
        /// <returns>A new LogLayout instance that is a copy of the current instance.</returns>
        public LogLayout Copy()
        {
            LogLayout layoutCopy = new()
            {
                Description = Description,
                DateTimeFormat = DateTimeFormat,
                RemoveMetaDataCriteria = new()
                {
                    AfterPhrase = RemoveMetaDataCriteria.AfterPhrase,
                    BeforePhrase = RemoveMetaDataCriteria.BeforePhrase,
                },
                LogMetadataProperties = [],
                LogContentProperties = [],
                LogTransformers = [],
            };

            foreach (LogMetadataProperty property in LogMetadataProperties)
            {
                LogMetadataProperty newProperty = new()
                {
                    Description = property.Description,
                    IsSessionData = property.IsSessionData,
                    IsDefaultVisibleInLog = property.IsDefaultVisibleInLog,
                    Criteria = new FilterCriteria()
                    {
                        BeforePhrase = property.Criteria.BeforePhrase,
                        AfterPhrase = property.Criteria.AfterPhrase
                    }
                };
                layoutCopy.LogMetadataProperties.Add(newProperty);
            }
            foreach (LogContentProperty property in LogContentProperties)
            {
                LogContentProperty newProperty = new()
                {
                    Description = property.Description,
                    IsErrorProperty = property.IsErrorProperty,
                    IsBeginFlowTreeFilter = property.IsBeginFlowTreeFilter,
                    EndFlowTreeContentProperty = property.EndFlowTreeContentProperty,
                    EndFlowTreeContentPropertyDescription = property.EndFlowTreeContentPropertyDescription
                };
                foreach (FilterCriteria criteria in property.Criterias)
                {
                    newProperty.Criterias.Add(new FilterCriteria()
                    {
                        BeforePhrase = criteria.BeforePhrase,
                        AfterPhrase = criteria.AfterPhrase
                    });
                }
                layoutCopy.LogContentProperties.Add(newProperty);
            }
            if (LogTransformers != null)
            {
                foreach (ILogTransformer transformer in LogTransformers)
                {
                    if (transformer is OrderReversalTransformer)
                    {
                        ILogTransformer newTransformer = new OrderReversalTransformer();
                        layoutCopy.LogTransformers.Add(newTransformer);
                    }
                    if (transformer is JsonPathExtractionTranformer jsonPathExtractionTranformer)
                    {
                        ILogTransformer newTransformer = new JsonPathExtractionTranformer(jsonPathExtractionTranformer.JsonPath);
                        layoutCopy.LogTransformers.Add(newTransformer);
                    }
                }
            }
            return layoutCopy;
        }
    }
}
