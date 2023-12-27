using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using System;
using System.Collections.Generic;

namespace LogScraper.Log.Collection
{
    public class LogLine(string line, DateTime timestamp) : IEquatable<LogLine>
    {
        public string Line { get; private set; } = line.Trim();
        public DateTime TimeStamp { get; set; } = timestamp;
        public List<string> AdditionalLogLines { get; set; }
        private int HashCodeCache { get; set; } = line.GetHashCode();

        public Dictionary<LogMetadataProperty, string> LogMetadataPropertiesWithStringValue { get; set; }
        public Dictionary<LogContentProperty, string> LogContentProperties { get; set; }

        public bool Equals(LogLine other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as LogLine);
        }
        public override int GetHashCode()
        {
            return HashCodeCache;
        }
        public override string ToString()
        {
            return Line;
        }
    }
}
