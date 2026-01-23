using System;
using System.Collections.Generic;
using System.Text;
using LogScraper.Export;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Metadata;
using LogScraper.LogPostProcessors;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log
{
    /// <summary>
    /// This class is responsible for exporting log data based on the filtered log metadata and export settings.
    /// </summary>
    internal class LogDataExporter
    {
        /// <summary>
        /// Creates a single string based on the filtered log metadata, export settings, and display options.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <returns>A single string containing the processed log data.</returns>
        public static string CreateExportedLog(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings, out int entryCount)
        {
            entryCount = 0;
            // Calculate the start and end indices based on the begin and end filters and extra nog entries to include.
            (int startIndex, int endIndex) = CalculateExportRange(filterResult, logExportSettings);

            // If the calculated indices are invalid, return an empty ExportedLogData object.
            if (startIndex <= -1 || endIndex <= 0 || startIndex > endIndex) return string.Empty;

            entryCount = endIndex - startIndex;

            return GetLogEntriesAsString(filterResult, startIndex, endIndex, logExportSettings);
        }

        /// <summary>
        /// Gets the log entries within a specified range.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <returns>A string containing the formatted log entries.</returns>
        public static List<LogEntry> GetLogEntriesActiveRange(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            // Calculate the start and end indices based on the begin and end filters and extra nog entries to include.
            (int startIndex, int endIndex) = CalculateExportRange(filterResult, logExportSettings);

            // If the calculated indices are invalid, return an empty ExportedLogData object.
            if (startIndex <= -1 || endIndex <= 0 || startIndex > endIndex) return [];

            return filterResult.LogEntries[startIndex..endIndex];
        }

        /// <summary>
        /// Determines the start and end indices for the log entries to be exported based on the export settings.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <returns>A tuple containing the start and end indices.</returns>
        private static (int startIndex, int endIndex) CalculateExportRange(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            int numberOfLogEntriesTotal = filterResult.LogEntries.Count;

            int startindex = 0;
            int endindex = numberOfLogEntriesTotal;

            // Adjust the start index based on the LogEntryBegin setting
            if (logExportSettings.LogEntryBegin != null)
            {
                for (int i = 0; i < numberOfLogEntriesTotal; i++)
                {
                    if (filterResult.LogEntries[i] == logExportSettings.LogEntryBegin)
                    {
                        startindex = i;
                        break;
                    }
                }
                if (startindex < 0) startindex = 0; // Ensure the start index is not negative.
            }

            // Adjust the end index based on the LogEntryEnd setting
            if (logExportSettings.LogEntryEnd != null)
            {
                for (int i = 0; i < numberOfLogEntriesTotal; i++)
                {
                    if (filterResult.LogEntries[i] == logExportSettings.LogEntryEnd)
                    {
                        endindex = i + 1;
                        break;
                    }
                }
                if (endindex > numberOfLogEntriesTotal) endindex = numberOfLogEntriesTotal; // Ensure the end index does not exceed the total log entries.
            }

            return (startindex, endindex);
        }

        /// <summary>
        /// Converts the specified range of log entries into a single string, optionally reducing the number of log entries for display.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="startIndex">The starting index of the log entries to include.</param>
        /// <param name="endIndex">The ending index of the log entries to include.</param>
        /// <param name="logExportSettings">Settings for exporting the log data.</param>
        /// <returns>A string containing the formatted log entries.</returns>
        private static string GetLogEntriesAsString(LogMetadataFilterResult filterResult, int startIndex, int endIndex, LogExportSettings logExportSettings)
        {
            StringBuilder stringBuilder = new();

            for (int i = startIndex; i < endIndex; i++)
            {
                LogFlowTreeNode logFlowTreeNode = null;
                AppendLogEntryToStringBuilder(stringBuilder, filterResult.LogEntries[i], logExportSettings, null, ref logFlowTreeNode, false);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts a collection of log entries into a single formatted string based on the specified export
        /// settings.
        /// </summary>
        /// <param name="logEntries">The list of log entries to be converted. Cannot be null.</param>
        /// <param name="logExportSettings">The settings that determine how each log entry is formatted. Cannot be null.</param>
        /// <returns>A string containing all log entries formatted according to the specified settings.</returns>
        public static string GetLogEntriesAsString(List<LogEntry> logEntries, LogPostProcessCollection logPostProcessCollection,  LogExportSettings logExportSettings, LogContentProperty logContentPropertyForFlowTree, List<LogFlowTreeNode> logFlowTreeNodes)
        {
            bool showTree = logFlowTreeNodes != null && logContentPropertyForFlowTree != null;
            StringBuilder stringBuilder = new();

            LogFlowTreeNode currentTreeNode = null;

            List<LogPostProcessStore> logPostProcessStores = logPostProcessCollection.GetStores();

            foreach (LogEntry logEntry in logEntries)
            {
                // Check if the flow tree content item is available
                if (showTree && logEntry.LogContentProperties != null && logEntry.LogContentProperties.ContainsKey(logContentPropertyForFlowTree))
                {
                    // If the log entry has such a content property, find the corresponding tree node
                    foreach (LogFlowTreeNode node in logFlowTreeNodes)
                    {
                        if (node.TryGetLogEntryNodeFromTree(logEntry, out LogFlowTreeNode matchedNode))
                        {
                            currentTreeNode = matchedNode;
                            break;
                        }
                    }
                }

                AppendLogEntryToStringBuilder(stringBuilder, logEntry, logExportSettings, logPostProcessStores, ref currentTreeNode, showTree);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Appends a formatted log entry and its associated metadata to the specified <see cref="StringBuilder"/>.
        /// </summary>
        /// <remarks>This method processes the log entry based on the provided export settings, including
        /// whether to include or modify metadata, and optionally adds tree structure information if <paramref
        /// name="showTree"/> is <see langword="true"/>. Additional log entries associated with the primary log entry
        /// are also appended, with formatting applied as needed.</remarks>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/> to which the log entry will be appended.</param>
        /// <param name="logEntry">The log entry to append, including its content and metadata.</param>
        /// <param name="logExportSettings">The settings that control how the log entry is formatted and exported.</param>
        /// <param name="treeNode">A reference to the current node in the log flow tree, used to determine hierarchical relationships between
        /// log entries. This parameter is updated if the log entry marks the end of a tree node.</param>
        /// <param name="showTree">A value indicating whether to include tree structure prefixes in the log entry output. If <see
        /// langword="true"/>, tree-related prefixes are added to the log entry.</param>
        private static void AppendLogEntryToStringBuilder(StringBuilder stringBuilder, LogEntry logEntry, LogExportSettings logExportSettings, List<LogPostProcessStore> logPostProcessStores, ref LogFlowTreeNode treeNode, bool showTree)
        {
            string text = logEntry.Entry;

            if (!logExportSettings.ShowOriginalMetadata)
            {
                if (logExportSettings.LogLayout.RemoveMetaDataCriteria != null)
                {
                    text = RemoveTextByCriteria(text, logExportSettings.LogLayout.StartIndexMetadata, logEntry.StartIndexContent);
                }

                text = InsertMetadataIntoLogEntry(text, logExportSettings.LogLayout.StartIndexMetadata, logEntry.LogMetadataPropertiesWithStringValue, logExportSettings);
            }

            string treePrefix = string.Empty;
            if (showTree)
            {
                bool isBeginNode = treeNode != null && treeNode.Begin == logEntry;
                bool isEndNode = treeNode != null && treeNode.End != null && treeNode.End == logEntry;
                treePrefix = GetTreePrefix(treeNode, isBeginNode || isEndNode);
                if (isEndNode) treeNode = treeNode.Parent;
            }

            text = text.Insert(logExportSettings.LogLayout.StartIndexMetadata, treePrefix);

            stringBuilder.AppendLine(text);

            if (logEntry.AdditionalLogEntries != null)
            {
                string additionLogEntryPrefix = string.Concat(logEntry.Entry.AsSpan(0, logExportSettings.LogLayout.StartIndexMetadata), " ", treePrefix);
                foreach (string extra in logEntry.AdditionalLogEntries)
                {
                    if (showTree)
                    {
                        stringBuilder.AppendLine(extra.Insert(0, additionLogEntryPrefix));
                    }
                    else
                    {
                        stringBuilder.AppendLine(extra);
                    }
                }
            }

            if ( logPostProcessStores == null) return;

            //TODO: optimize getting post process results
            foreach (LogPostProcessStore store in logPostProcessStores)
            {
                if (store.TryGet(logEntry.Index, out LogEntryPostProcessResult postProcessResult))
                {
                    stringBuilder.AppendLine($"--- LogScraper {store.Name} ---");
                    stringBuilder.AppendLine(postProcessResult.ProcessedText);
                    stringBuilder.AppendLine($"--- /LogScraper {store.Name} ---");
                }
            }
        }

        /// <summary>
        /// Generates a string prefix representing the depth of a node in a tree structure.
        /// </summary>
        /// <param name="node">The tree node for which the prefix is generated. Must not be <see langword="null"/>.</param>
        /// <param name="isBeginOrEndNode">A value indicating whether the node represents a "begin" or "end" entry.  If <see langword="true"/>, the
        /// prefix is calculated for the previous depth level.</param>
        /// <returns>A string consisting of tab characters (<c>'\t'</c>) representing the depth of the node in the tree.  Returns
        /// an empty string if the node is the root node or if the calculated depth is zero.</returns>
        private static string GetTreePrefix(LogFlowTreeNode node, bool isBeginOrEndNode)
        {
            if (node == null || (node.IsRootNode && isBeginOrEndNode))
                return string.Empty;

            // Calculate the actual depth, because the begin and end entry we want to show on the previous depth
            int depth = node.Depth + (isBeginOrEndNode ? 0 : 1);
            if (depth == 0) return "";
            return new string('\t', depth * 2);
        }



        /// <summary>
        /// Inserts metadata to a log entry at the specified position.
        /// </summary>
        /// <param name="logEntry">The original log entry.</param>
        /// <param name="startIndex">The position to insert the metadata.</param>
        /// <param name="logMetadataPropertiesWithStringValue">The metadata properties and their string values.</param>
        /// <param name="logExportSettingsMetadata">Metadata settings for the log export.</param>
        /// <returns>The log entry with added metadata.</returns>
        private static string InsertMetadataIntoLogEntry(string logEntry, int startIndex, IndexDictionary<LogMetadataProperty, string> logMetadataPropertiesWithStringValue, LogExportSettings logExportSettings)
        {
            if (startIndex <= 0 ||
                logExportSettings.SelectedMetadataProperties == null ||
                logExportSettings.SelectedMetadataProperties.Count == 0 ||
                logMetadataPropertiesWithStringValue == null ||
                logMetadataPropertiesWithStringValue.Count == 0)
            {
                return logEntry;
            }

            List<string> values = [];
            foreach (LogMetadataProperty logMetadataProperty in logExportSettings.SelectedMetadataProperties)
            {
                logMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value);
                if (value != null) { values.Add(value); }
            }

            // Insert the metadata values into the log entry at the specified position.
            return logEntry.Insert(startIndex, " " + string.Join(" | ", values));
        }

        /// <summary>
        /// Removes text from the input string based on the specified criteria.
        /// </summary>
        /// <param name="inputText">The input string.</param>
        /// <param name="criteria">The criteria for removing text.</param>
        /// <param name="startPosition">The starting position on the input text after which the filter criteria should be applied.</param>
        /// <returns>The modified string with the specified text removed.</returns>
        public static string RemoveTextByCriteria(string inputText, int beforeIndex, int afterIndex)
        {

            if (string.IsNullOrEmpty(inputText) || beforeIndex == -1 || afterIndex == -1)
            {
                return inputText;
            }
            // Remove the portion of text from StartPosition to AfterPhrase.
            inputText = inputText.Remove(beforeIndex, afterIndex - beforeIndex);

            return inputText;
        }
    }
}
