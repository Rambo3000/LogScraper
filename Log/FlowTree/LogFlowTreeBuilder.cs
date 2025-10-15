using System.Collections.Generic;
using LogScraper.Log.Content;

namespace LogScraper.Log.FlowTree
{
    /// <summary>
    /// Provides functionality to build a nested log flow tree from a flat list of log entries.
    /// </summary>
    internal class LogFlowTreeBuilder
    {
        /// <summary>
        /// Builds a nested tree structure from a flat list of log entries using specified begin and end properties. The nested tree structure represents the hyrarchical flow of log entries.
        /// </summary>
        /// <param name="entries">The log entries to process, in chronological order.</param>
        /// <param name="beginProperty">The property indicating the start of a flow (e.g., method entry).</param>
        /// <param name="endProperty">The property indicating the end of a flow (e.g., method exit).</param>
        /// <returns>
        /// A list of root <see cref="LogFlowTreeNode"/> objects representing the top-level flows.
        /// Nested flows are represented as children of their parent nodes.
        /// Unmatched end entries are ignored to keep the tree clean.
        /// </returns>
        public static List<LogFlowTreeNode> BuildLogFlowTree(List<LogEntry> entries, LogContentProperty beginProperty, LogContentProperty endProperty)
        {
            List<LogFlowTreeNode> roots = [];
            // Stack tracks open (unclosed) begin nodes and their keys.
            Stack<(LogContentValue Key, LogFlowTreeNode Node)> stack = new();

            foreach (LogEntry entry in entries)
            {
                // If this entry marks the beginning of a flow, create a new node and push it onto the stack.
                if (entry.LogContentProperties.TryGetValue(beginProperty, out LogContentValue beginValue))
                {
                    AddBeginNode(roots, stack, entry, beginValue);
                    continue;
                }

                // If this entry marks the end of a flow, try to find and close the most recent matching begin node.
                if (entry.LogContentProperties.TryGetValue(endProperty, out LogContentValue endValue))
                {
                    TryMatchEndNode(stack, entry, endValue);
                }
            }

            return roots;
        }
        
        /// <summary>
        /// Adds a new "begin" node to the log flow tree structure, either as a root node or as a child of the current
        /// parent node.
        /// </summary>
        /// <remarks>This method creates a new <see cref="LogFlowTreeNode"/> instance, initializes it with
        /// the provided log entry and key, and determines its position in the tree based on the current state of the
        /// <paramref name="stack"/>. If the stack is empty, the node is added as a root node. Otherwise, it is added as
        /// a child of the node at the top of the stack.</remarks>
        /// <param name="roots">The list of root nodes in the log flow tree. If the new node has no parent, it will be added to this list.</param>
        /// <param name="stack">A stack used to track open nodes in the log flow tree. The new node will be pushed onto the stack for future
        /// matching with corresponding "end" entries.</param>
        /// <param name="entry">The log entry associated with the "begin" node being added.</param>
        /// <param name="beginValue">The key value that uniquely identifies the "begin" node.</param>
        private static void AddBeginNode(List<LogFlowTreeNode> roots, Stack<(LogContentValue Key, LogFlowTreeNode Node)> stack, LogEntry entry, LogContentValue beginValue)
        {
            LogFlowTreeNode node = new()
            {
                Begin = entry,
                Key = beginValue
            };

            if (stack.Count > 0)
            {
                // If there is an open parent node, nest this node as its child.
                stack.Peek().Node.Children.Add(node);
                node.Parent = stack.Peek().Node;
            }
            else
            {
                // No open parent node, so this is a root node.
                roots.Add(node);
                node.Parent = null;
            }

            // Push the new node onto the stack for future matching with end entries.
            stack.Push((beginValue, node));
        }
        /// <summary>
        /// Attempts to match an end node in the stack with the specified end value.
        /// </summary>
        /// <remarks>This method processes the stack to find a node whose key matches the specified
        /// <paramref name="endValue"/>.  If a match is found, the corresponding node is closed by setting its end to
        /// the provided <paramref name="entry"/>.  Any unmatched nodes above the matching node in the stack are
        /// discarded, as they are considered child nodes of the closed node. If no match is found, the stack is
        /// restored to its original state, and the method returns <see langword="true"/> to indicate that the end node
        /// is orphaned.</remarks>
        /// <param name="stack">A stack of tuples, where each tuple contains a <see cref="LogContentValue"/> key and a corresponding <see
        /// cref="LogFlowTreeNode"/>. The stack represents the current hierarchy of nodes being processed.</param>
        /// <param name="entry">The log entry that represents the end node. If a match is found, this entry is assigned as the end  of the
        /// corresponding node.</param>
        /// <param name="endValue">The value used to identify the end node. This is compared against the keys in the stack to find a match.</param>
        /// <returns><see langword="false"/> if no matching begin node is found for the specified end value, indicating that  the
        /// end node is orphaned and ignored; otherwise, <see langword="true"/> if a matching begin node is found and
        /// successfully closed.</returns>
        private static bool TryMatchEndNode(Stack<(LogContentValue Key, LogFlowTreeNode Node)> stack, LogEntry entry, LogContentValue endValue)
        {
            // Use a temporary stack to hold nodes that do not match the current end value.
            Stack<(LogContentValue, LogFlowTreeNode)> temp = new();
            bool matched = false;

            while (stack.Count > 0)
            {
                var top = stack.Pop();

                // Use EqualsValue to compare keys for matching begin/end.
                if (top.Key.EqualsValue(endValue))
                {
                    // Found the matching begin node; set its end and stop searching.
                    top.Node.End = entry;
                    matched = true;
                    break;
                }
                else
                {
                    // Not a match; temporarily hold this node.
                    temp.Push(top);
                }
            }

            // If a matching begin node was found, discard the unmatched nodes in the temp stack.
            // This follows the principle that if a begin node is closed, all nodes above it in the stack (child nodes) are also closed, although no matching ends were presented for the child nodes previously.
            if (matched) return true;

            // Orphan end nodes (without a matching begin) are ignored for tree cleanliness.
            // Restore any unmatched nodes back to the main stack in case no match can be made
            while (temp.Count > 0)
            {
                stack.Push(temp.Pop());
            }

            return false;
        }
    }
}
