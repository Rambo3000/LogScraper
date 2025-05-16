using System.Collections.Generic;
using LogScraper.Log.Content;

namespace LogScraper.Log.FlowTree
{
    /// <summary>
    /// Represents a node in a nested log flow tree.
    /// Each node tracks a begin log entry, an optional end log entry, a key for identification, and any nested child nodes.
    /// </summary>
    public class LogFlowTreeNode
    {
        /// <summary>
        /// The log entry that marks the beginning of this flow node.
        /// </summary>
        public LogEntry Begin { get; set; }

        /// <summary>
        /// The log entry that marks the end of this flow node, if present.
        /// </summary>
        public LogEntry End { get; set; }

        /// <summary>
        /// The key value that identifies this flow node (typically a unique identifier from the log content).
        /// </summary>
        public LogContentValue Key { get; set; }

        /// <summary>
        /// The list of child flow nodes nested within this node.
        /// </summary>
        public List<LogFlowTreeNode> Children { get; set; } = [];
    }
}
