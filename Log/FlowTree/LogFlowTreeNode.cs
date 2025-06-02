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

        /// <summary>
        /// The parent node of this flow node, if it exists.
        /// </summary>
        public LogFlowTreeNode Parent { get; set; }

        /// <summary>
        /// The root node of the tree, which is the topmost parent node in the hierarchy.
        /// </summary>
        public LogFlowTreeNode RootNode
        {
            get
            {
                LogFlowTreeNode root = this;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }
                return root;
            }
        }

        /// <summary>
        /// Indicates whether this node is the root node of the tree.
        /// </summary>
        public bool IsRootNode
        {
            get
            {
                return Parent == null;
            }
        }

        /// <summary>
        /// Indicates whether this node has one or more siblings.
        /// A sibling is defined as another node that shares the same parent.
        /// </summary>
        public bool HasSiblings
        {
            get
            {
                if (Parent == null || Parent.Children == null) return false;

                return Parent.Children.Count > 1;
            }
        }

        /// <summary>
        /// Indicates whether this node has one or more siblings which are older than this node.
        /// An older sibling has a higher index in the parent node's children list.
        /// </summary>
        public bool HasOlderSibling
        {
            get
            {
                if (Parent == null || Parent.Children == null) return false;

                for (int i = 0; i < Parent.Children.Count; i++)
                {
                    if (Parent.Children[i] == this)
                    {
                        if (i < Parent.Children.Count - 1)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Indicates whether this node is the last sibling in its parent's children list.
        /// </summary>
        public bool IsLastSibling
        {
            get
            {
                if (Parent == null || Parent.Children == null) return false;

                for (int i = 0; i < Parent.Children.Count; i++)
                {
                    if (Parent.Children[i] == this)
                    {
                        return i == Parent.Children.Count - 1;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// The depth of this node in the tree, calculated as the number of parent nodes.
        /// </summary>
        public int Depth
        {
            get
            {
                int depth = 0;
                LogFlowTreeNode currentNode = this;
                while (currentNode.Parent != null)
                {
                    depth++;
                    currentNode = currentNode.Parent;
                }
                return depth;
            }
        }

        /// <summary>
        /// Attempts to find a child node with the specified content value in the tree.
        /// </summary>
        /// <param name="contentValue">The content value to search for.</param>
        /// <param name="foundNode">The found node, if any.</param>
        /// <returns>True if the node was found; otherwise, false.</returns>
        public bool TryGetContentValueNodeFromTree(LogContentValue contentValue, out LogFlowTreeNode foundNode)
        {
            return TryFindContentValueNodeInTreeRecursive(this, contentValue, out foundNode);
        }

        /// <summary>
        /// Attempts to find a child node with the specified root node and content value in the tree.
        /// </summary>
        /// <param name="root">The root node to start the search from.</param>
        /// <param name="contentValue">The content value to search for.</param>
        /// <param name="foundNode">The found node, if any.</param>
        /// <returns>True if the node was found; otherwise, false.</returns>
        private static bool TryFindContentValueNodeInTreeRecursive(LogFlowTreeNode root, LogContentValue contentValue, out LogFlowTreeNode foundNode)
        {
            if (root.Key.Equals(contentValue))
            {
                foundNode = root;
                return true;
            }

            foreach (LogFlowTreeNode child in root.Children)
            {
                if (TryFindContentValueNodeInTreeRecursive(child, contentValue, out foundNode))
                {
                    return true;
                }
            }

            foundNode = null;
            return false;
        }

        /// <summary>
        /// Attempts to find a child node with the specified log entry in the tree.
        /// </summary>
        /// <param name="logEntry">The log entry to search for.</param>
        /// <param name="foundNode">The found node, if any.</param>
        /// <returns>True if the node was found; otherwise, false.</returns>
        public bool TryGetLogEntryNodeFromTree(LogEntry logEntry, out LogFlowTreeNode foundNode)
        {
            return TryGetLogEntryNodeFromTree(this, logEntry, out foundNode);
        }

        /// <summary>
        /// Attempts to find a child node with the specified root node and content value in the tree.
        /// </summary>
        /// <param name="root">The root node to start the search from.</param>
        /// <param name="logEntry">The log entry to search for.</param>
        /// <param name="foundNode">The found node, if any.</param>
        /// <returns>True if the node was found; otherwise, false.</returns>
        private static bool TryGetLogEntryNodeFromTree(LogFlowTreeNode root, LogEntry logEntry, out LogFlowTreeNode foundNode)
        {
            if ((root.Begin.Equals(logEntry)) || (root.End != null && root.End.Equals(logEntry)))
            {
                foundNode = root;
                return true;
            }

            foreach (LogFlowTreeNode child in root.Children)
            {
                if (TryGetLogEntryNodeFromTree(child, logEntry, out foundNode))
                {
                    return true;
                }
            }

            foundNode = null;
            return false;
        }
    }
}
