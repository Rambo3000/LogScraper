using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using System.Collections.Generic;
using System;

namespace LogScraper.Log
{
    internal class LogLayout : IEquatable<LogLayout>
    {
        public string Description { get; set; }
        public string DateTimeFormat { get; set; }
        public List<LogMetadataProperty> LogMetadataProperties { get; set; }
        public List<LogContentProperty> LogContentBeginEndFilters { get; set; }
        public FilterCriteria RemoveMetaDataCriteria { get; set; }

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
