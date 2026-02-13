using System.Collections.Generic;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log.FlowTree
{
    /// <summary>
    /// Represents a tree structure of log entries, where each node corresponds to a log entry and its children represent related log entries. The tree is built based on the content properties of the log entries, allowing for efficient organization and retrieval of log data.
    /// </summary>
    /// <param name="roots">The list of root nodes in the log flow tree. Each root node represents a top-level log entry that does not have a parent log entry.</param>
    /// <param name="logEntryDictionary">A dictionary that maps each log entry to its corresponding tree node in the log flow tree. This allows for quick lookup of tree nodes based on log entries.</param>
    public class LogFlowTree ( List<LogFlowTreeNode> roots, IndexDictionary<LogEntry, LogFlowTreeNode> logEntryDictionary)
    {
        /// <summary>
        /// Gets the list of root nodes in the log flow tree. Each root node represents a top-level log entry that does not have a parent log entry.
        /// </summary>
        public List<LogFlowTreeNode> Roots { get; } = roots;
        /// <summary>
        /// Gets the dictionary that maps each log entry to its corresponding tree node in the log flow tree. This allows for quick lookup of tree nodes based on log entries. 
        /// </summary>
        public IndexDictionary<LogEntry, LogFlowTreeNode> LogEntryDictionary { get; } = logEntryDictionary;
    }
}
