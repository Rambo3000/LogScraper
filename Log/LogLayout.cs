using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using System.Collections.Generic;
using System;
using LogScraper.LogTransformers;
using Newtonsoft.Json;
using LogScraper.Log.Filter;

namespace LogScraper.Log
{
    internal class LogLayout : IEquatable<LogLayout>
    {
        public string Description { get; set; }
        [JsonIgnore]
        public string DateTimeFormatCache;
        public string DateTimeFormat
        {
            get { return DateTimeFormatCache; }
            set
            {
                DateTimeFormatCache = value;
                if (null != value)
                {
                    StartPositionCache = value.Length;
                }
                else
                {
                    StartPositionCache = 0;
                }
            }
        }
        [JsonIgnore]
        private int StartPositionCache { get; set; }
        public int StartPosition { get { return StartPositionCache; } }
        public List<LogMetadataProperty> LogMetadataProperties { get; set; }
        public List<LogContentProperty> LogContentBeginEndFilters { get; set; }
        public FilterCriteriaWithStartPosition RemoveMetaDataCriteria { get; set; }
        public List<ILogTransformer> LogTransformers { get; set; }
        [JsonProperty("transformers")]
        public List<LogTransformerConfig> LogTransformersConfig { get; set; }

        public bool Equals(LogLayout other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as LogLayout);
        }
        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }
        public override string ToString()
        {
            return Description.ToString();
        }
    }
}
