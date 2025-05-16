using System.Collections.Generic;
using LogScraper.Log.Content;

namespace LogScraper.Log.Flow
{
    internal class LogFlowTreeBuilder
    {
        /// <summary>
        /// Builds a nested tree structure from a flat list of log entries using begin/end properties.
        /// </summary>
        /// <param name="entries">The log entries to process.</param>
        /// <param name="beginProperty">The property indicating the start of a flow.</param>
        /// <param name="endProperty">The property indicating the end of a flow.</param>
        /// <returns>A list of root nodes representing the nested flows.</returns>
        public static List<LogFlowNode> BuildLogFlowTree(List<LogEntry> entries, LogContentProperty beginProperty, LogContentProperty endProperty)
        {
            List<LogFlowNode> roots = [];
            // Stack to track open (unclosed) begin nodes
            Stack<(LogContentValue Key, LogFlowNode Node)> stack = new();

            foreach (LogEntry entry in entries)
            {
                // Check if entry marks the beginning of a flow
                if (entry.LogContentProperties.TryGetValue(beginProperty, out LogContentValue beginValue))
                {
                    LogFlowNode node = new()
                    {
                        Begin = entry,
                        Key = beginValue
                    };

                    if (stack.Count > 0)
                    {
                        // Nest under the current open node
                        stack.Peek().Node.Children.Add(node);
                    }
                    else
                    {
                        // No open node, this is a root
                        roots.Add(node);
                    }

                    stack.Push((beginValue, node));
                    continue;
                }

                // Check if entry marks the end of a flow
                if (entry.LogContentProperties.TryGetValue(endProperty, out LogContentValue endValue))
                {
                    // Find matching begin node from top of stack down
                    Stack<(LogContentValue, LogFlowNode)> temp = new();
                    bool matched = false;

                    while (stack.Count > 0)
                    {
                        var top = stack.Pop();
                        if (top.Key.EqualsValue(endValue))
                        {
                            // Found matching begin, set end
                            top.Node.End = entry;
                            matched = true;
                            break;
                        }
                        else
                        {
                            // Temporarily hold unmatched nodes
                            temp.Push(top);
                        }
                    }

                    // If there is a match, throw away
                    //if (matched) continue;

                    // Restore unmatched nodes to stack
                    while (temp.Count > 0)
                    {
                        stack.Push(temp.Pop());
                    }

                    // Note: Orphan nodes (unmatched ends) are not added to the tree to keep the tree clean
                    //if (!matched)
                    //{
                    //    LogFlowNode orphan = new()
                    //    {
                    //        Begin = null,
                    //        End = entry,
                    //        Key = endValue
                    //    };
                    //    roots.Add(orphan);
                    //}
                }
            }

            // Note: The stack may still contain unmatched begin nodes, these are not added to the tree to keep the tree clean

            return roots;
        }
    }
}
