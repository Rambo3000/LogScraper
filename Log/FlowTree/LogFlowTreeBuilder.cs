using System.Collections.Generic;
using System.Text.RegularExpressions;
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
                    LogFlowTreeNode node = new()
                    {
                        Begin = entry,
                        Key = beginValue
                    };

                    if (stack.Count > 0)
                    {
                        // If there is an open parent node, nest this node as its child.
                        stack.Peek().Node.Children.Add(node);
                    }
                    else
                    {
                        // No open parent node, so this is a root node.
                        roots.Add(node);
                    }

                    // Push the new node onto the stack for future matching with end entries.
                    stack.Push((beginValue, node));

                    continue;
                }

                // If this entry marks the end of a flow, try to find and close the most recent matching begin node.
                if (entry.LogContentProperties.TryGetValue(endProperty, out LogContentValue endValue))
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
                    // This follows the principle that if a begin node is closed, all nodes above it in the stack (child nodes) are also closed.
                    if (matched) continue;

                    // Restore any unmatched nodes back to the main stack in case no match can be made
                    while (temp.Count > 0)
                    {
                        stack.Push(temp.Pop());
                    }

                    // Orphan end nodes (without a matching begin) are ignored for tree cleanliness.
                }
            }

            // Any unmatched begin nodes left in the stack are ignored (not added to the tree).
            // This ensures only well-formed flows are represented.

            return roots;
        }
    }
}
