using System;

namespace LogScraper.Log.Content
{
    public class LogContentProperty : IEquatable<LogContentProperty>
    {
        public string Description { get; set; }
        public FilterCriteria Criteria { get; set; }
        private int HashCodeCache { get; set; }
        public bool Equals(LogContentProperty other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as LogContentProperty);
        }
        public override int GetHashCode()
        {
            if (HashCodeCache == 0)
            {
                lock (this)
                {
                    if (HashCodeCache == 0) HashCodeCache = Description.GetHashCode();
                }
            }
            return HashCodeCache;
        }
        public override string ToString()
        {
            return Description;
        }
    }

}
