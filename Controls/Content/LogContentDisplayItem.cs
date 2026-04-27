using System;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;

namespace LogScraper.Controls.Content
{
    /// <summary>
    /// Represents a display item for log content
    /// </summary>
    /// <param name="timeStamp">The string formatted timestamp associated with the log entry, typically indicating when the log event occurred.</param>
    /// <param name="originalLogEntry">The original log entry object that this display item represents.</param>
    /// <param name="contentValue">The value of the log content to be displayed, including any formatting or extracted information.</param>
    /// <param name="flowTreeNode">The flow tree node associated with the log entry, representing its position in the log flow hierarchy.</param>
    public class LogContentDisplayItem(string timeStamp, LogEntry originalLogEntry, LogContentValue contentValue, LogFlowTreeNode flowTreeNode) : IEquatable<LogContentDisplayItem>
    {
        public string TimeStamp { get; set; } = timeStamp;
        public LogEntry LogEntry { get; set; } = originalLogEntry;
        public LogContentValue ContentValue { get; set; } = contentValue;
        public LogFlowTreeNode FlowTreeNode { get; set; } = flowTreeNode;

        private int HashCodeCache { get; set; } = HashCode.Combine(originalLogEntry, contentValue);

        public bool Equals(LogContentDisplayItem other)
        {
            return other != null && GetHashCode() == other.GetHashCode();
        }

        public override string ToString()
        {
            return ContentValue != null ? ContentValue.Value : string.Empty;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LogContentDisplayItem);
        }

        public override int GetHashCode()
        {
            return HashCodeCache;
        }
    }
}
