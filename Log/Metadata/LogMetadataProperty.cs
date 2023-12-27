using LogScraper.Log.Collection;
using System;

namespace LogScraper.Log.Metadata
{
    public class LogMetadataProperty : IEquatable<LogMetadataProperty>
    {
        public string Description { get; set; }
        public FilterCriteria Criteria { get; set; }
        private int HashCodeCache { get; set; }

        public override string ToString()
        {
            return Description;
        }
        public bool Equals(LogMetadataProperty other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as LogMetadataProperty);
        }
        public override int GetHashCode()
        {
            if (HashCodeCache == 0)
            {
                lock(this) 
                {
                    if (HashCodeCache == 0) HashCodeCache = Description.GetHashCode(); 
                }    
            }
            return HashCodeCache;
        }
    }
}
