using System;
using System.Collections.Generic;

namespace LogScraper.Log.Metadata
{
    public class LogMetadataPropertyAndValues : IEquatable<LogMetadataPropertyAndValues>
    {
        public Dictionary<LogMetadataValue, string> LogMetadataValues { get; set; }
        public LogMetadataProperty LogMetadataProperty { get; set; }
        public bool IsFilterEnabled
        {
            get
            {
                if (LogMetadataValues == null || LogMetadataValues.Count == 0) return false;

                foreach (KeyValuePair<LogMetadataValue, string> kvp in LogMetadataValues)
                {
                    if (kvp.Key.IsFilterEnabled == true) return true;
                }
                return false;

            }
        }
        public bool Equals(LogMetadataPropertyAndValues other)
        {
            return null != other && LogMetadataProperty.Description == other.LogMetadataProperty.Description;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as LogMetadataProperty);
        }
        public override int GetHashCode()
        {
            return LogMetadataProperty.GetHashCode();
        }
    }
}
