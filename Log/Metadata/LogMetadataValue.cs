using LogScraper.Log.Collection;
using System;

namespace LogScraper.Log.Metadata
{
    public class LogMetadataValue : IEquatable<LogMetadataValue>
    {
        public LogMetadataValue(string value, int count, bool isFilterEnabled)
        {
            Value = value;
            Count = count;
            IsFilterEnabled = isFilterEnabled;
            HashCodeCache = Value.GetHashCode();
        }
    
        public string Value { get; set; }
        public int Count { get; set; }
        public bool IsFilterEnabled { get; set; }
        private int HashCodeCache { get; set; }

        public override string ToString()
        {
            return Value;
        }
        public bool Equals(LogMetadataValue other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as LogMetadataValue);
        }
        public override int GetHashCode()
        {
            return HashCodeCache;
        }
    }
}
