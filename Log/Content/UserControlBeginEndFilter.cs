using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Extensions;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;

namespace LogScraper
{
    internal partial class UserControlBeginEndFilter : UserControl
    {
        private const string DefaulSearchtText = "Filteren...";

        public event EventHandler FilterChanged;
        /// <summary>
        ///     Used for showing the error log entries for non-error LogContentProperties
        /// </summary>
        private LogContentProperty LogContentPropertyError;

        private List<LogMetadataProperty> LogMetadataPropertiesUserSession;

        public event Action<Dictionary<LogMetadataProperty, string>, bool> FilterOnMetadata;

        public UserControlBeginEndFilter()
        {
            InitializeComponent();
            SelectedItemBackColor = Brushes.Orange;
            //Preset the default search text
            TxtSearch_Leave(null, null);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush SelectedItemBackColor { get; set; }

        private List<LogEntry> LogEntriesLatestVersion;

        private LogLayout ActiveLogLayout;

        public void UpdateLogLayout(LogLayout logLayout)
        {
            ActiveLogLayout = logLayout;
            CboLogContentType.Items.Clear();
            if (logLayout.LogContentProperties == null || logLayout.LogContentProperties.Count == 0) return;
            CboLogContentType.Items.AddRange([.. logLayout.LogContentProperties]);
            foreach (LogContentProperty logContentProperty in logLayout.LogContentProperties)
            {
                if (logContentProperty.Description == "Errors")
                {
                    LogContentPropertyError = logContentProperty;
                    break;
                }
            }
            LogMetadataPropertiesUserSession = [];
            foreach (LogMetadataProperty logMetadataProperty in logLayout.LogMetadataProperties)
            {
                if (logMetadataProperty.IsSessionData)
                {
                    LogMetadataPropertiesUserSession.Add(logMetadataProperty);
                }
            }
            if (CboLogContentType.Items.Count > 0) CboLogContentType.SelectedIndex = 0;
            FilterOnMetadataProperties(false);
        }

        public void UpdateLogEntries(List<LogEntry> logEntries)
        {
            LogEntriesLatestVersion = logEntries;
            UpdateLogContentList();
        }
        private void UpdateLogContentList()
        {
            // If there are no log entries, clear the list and return
            if (LogEntriesLatestVersion == null)
            {
                LstLogContent.Items.Clear();
                return;
            }

            // Get the selected log content property
            LogContentProperty logContentProperty = (LogContentProperty)CboLogContentType.SelectedItem;

            // If no log content property is selected, return
            if (logContentProperty == null) return;

            // Create a list to store the log entries with overridden ToString method
            List<LogEntryDisplayModel> logEntryWithToStringOverrides = CreateLogEntryDisplayModels(logContentProperty);

            UpdateOrRedrawList(logEntryWithToStringOverrides);
        }

        private void UpdateOrRedrawList(List<LogEntryDisplayModel> newLogEntries)
        {
            int currentCount = LstLogContent.Items.Count;
            int newCount = newLogEntries.Count;

            int compareCount = Math.Min(currentCount, newCount);
            bool startMatches = true;

            for (int i = 0; i < compareCount; i++)
            {
                if (!newLogEntries[i].ContentValue.Equals(((LogEntryDisplayModel)LstLogContent.Items[i]).ContentValue))
                {
                    startMatches = false;
                    break;
                }
            }

            // Case 1: Exactly same items — skip update
            if (startMatches && currentCount == newCount)
            {
                return;
            }

            // Case 2: New list extends existing — add only new items
            if (startMatches && newCount > currentCount)
            {
                LstLogContent.SuspendDrawing();
                for (int i = currentCount; i < newCount; i++)
                {
                    LstLogContent.Items.Add(newLogEntries[i]);
                }
                LstLogContent.ResumeDrawing();
                return;
            }

            // In all other cases: Mismatch — full redraw
            FullyRedrawList(newLogEntries);
        }

        private List<LogEntryDisplayModel> CreateLogEntryDisplayModels(LogContentProperty logContentProperty)
        {
            if (logContentProperty == null) return null;

            List<LogFlowTreeNode> treeNodes = [];
            if (logContentProperty.IsBeginFlowTreeFilter && logContentProperty.EndFlowTreeContentProperty != null)
            {
                treeNodes = LogFlowTreeBuilder.BuildLogFlowTree(LogEntriesLatestVersion, logContentProperty, logContentProperty.EndFlowTreeContentProperty);
            }

            List<LogEntryDisplayModel> logEntryDisplayModels = [];

            // Get the search filter text
            string filter = txtSearch.Text.Trim();
            if (filter == DefaulSearchtText) filter = null;

            // Iterate through the latest version of log entries
            foreach (LogEntry logEntry in LogEntriesLatestVersion)
            {
                // If the log entry has no content properties, continue to the next log entry
                if (logEntry.LogContentProperties == null) continue;

                // Try to get the content for the selected log content property
                logEntry.LogContentProperties.TryGetValue(logContentProperty, out LogContentValue contentValue);
                bool isError = false;

                // If the content is null, check for errors if the "Show Errors" checkbox is checked
                if (contentValue == null)
                {
                    if (ConfigurationManager.GenericConfig.ShowErrorLinesInBeginAndEndFilters) logEntry.LogContentProperties.TryGetValue(LogContentPropertyError, out contentValue);
                    if (contentValue == null) continue;
                    isError = true;
                }

                LogFlowTreeNode flowtreeNode = null;
                if (contentValue != null)
                {
                    foreach (LogFlowTreeNode node in treeNodes)
                    {
                        if (TryFindDepthRecursive(node, contentValue, out flowtreeNode))
                        {
                            break;
                        }
                    }
                }

                // If the content is not an error and a filter is applied, check if the content contains the filter text
                if (!isError && !string.IsNullOrEmpty(filter))
                {
                    if (!contentValue.Value.Contains(filter, StringComparison.InvariantCultureIgnoreCase)) continue;
                }

                // Create a new LogEntryWithToStringOverride object and add it to the list
                LogEntryDisplayModel logEntryWithToStringOverride = new()
                {
                    OriginalLogEntry = logEntry,
                    ContentValue = contentValue,
                    IsError = isError,
                    FlowTreeNode = flowtreeNode

                };
                logEntryDisplayModels.Add(logEntryWithToStringOverride);
            }
            return logEntryDisplayModels;
        }

        private static bool TryFindDepthRecursive(LogFlowTreeNode root, LogContentValue contentValue, out LogFlowTreeNode foundNode)
        {
            if (root.Key.Equals(contentValue))
            {
                foundNode = root;
                return true;
            }

            foreach (LogFlowTreeNode child in root.Children)
            {
                if (TryFindDepthRecursive(child, contentValue, out foundNode))
                {
                    return true;
                }
            }

            foundNode = null;
            return false;
        }

        private void FullyRedrawList(List<LogEntryDisplayModel> newLogEntries)
        {
            // Store the currently selected log entry
            LogEntryDisplayModel selectedLogEntry = (LogEntryDisplayModel)LstLogContent.SelectedItem;
            // Store the current top index of the list
            int topIndex = LstLogContent.TopIndex;

            // Suspend drawing of the list to improve performance
            LstLogContent.SuspendDrawing();
            // Clear the list and add the new log entries
            LstLogContent.Items.Clear();
            LstLogContent.Items.AddRange([.. newLogEntries]);
            // Restore the top index of the list
            LstLogContent.TopIndex = topIndex > LstLogContent.Items.Count - 1 ? LstLogContent.Items.Count - 1 : topIndex;

            // If no log entry was previously selected, resume drawing and return
            if (selectedLogEntry == null)
            {
                LstLogContent.ResumeDrawing();
                return;
            }

            // Iterate through the new log entries and select the previously selected log entry if it exists
            foreach (LogEntryDisplayModel logEntriestringOverride in newLogEntries)
            {
                if (logEntriestringOverride.OriginalLogEntry == selectedLogEntry.OriginalLogEntry)
                {
                    try
                    {
                        ignoreSelectedItemChanged = true;
                        LstLogContent.SelectedItem = logEntriestringOverride;
                    }
                    finally
                    {
                        ignoreSelectedItemChanged = false;
                    }
                    break;
                }
            }
            // Resume drawing of the list
            LstLogContent.ResumeDrawing();
        }

        private void CboLogContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLogContentList();
            OnFilterChanged(EventArgs.Empty);
            ChkShowFlowTree.Enabled = SelectedLogContentProperty != null && SelectedLogContentProperty.Description == "Begin flow";
        }

        private class LogEntryDisplayModel
        {
            public LogEntry OriginalLogEntry { get; set; }
            public LogContentValue ContentValue { get; set; }
            public bool IsError { get; set; }
            public LogFlowTreeNode FlowTreeNode { get; set; }
            public override string ToString()
            { return ContentValue != null ? ContentValue.Value : string.Empty; }
        }

        public LogEntry SelectedLogEntry
        {
            get
            {
                if (SelectedLogEntryDisplayModel == null) return null;
                return SelectedLogEntryDisplayModel.OriginalLogEntry;
            }
        }

        private LogEntryDisplayModel SelectedLogEntryDisplayModel
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogEntryDisplayModel)LstLogContent.SelectedItem);
            }
        }
        private LogContentProperty SelectedLogContentProperty
        {
            get
            {
                if (CboLogContentType.SelectedItem == null) return null;
                return ((LogContentProperty)CboLogContentType.SelectedItem);
            }
        }

        public LogContentValue SelectedContentValue
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogEntryDisplayModel)LstLogContent.SelectedItem).ContentValue;
            }
        }
        public int ExtraLogEntryCount
        {
            get
            {
                if (!ChkShowExtraLogEntries.Checked) return 0;

                if (int.TryParse(TxtExtraLogEntries.Text, out int extraLogEntryCount)) return extraLogEntryCount;

                return 0;
            }
        }

        public bool FilterIsEnabled { get { return LstLogContent.SelectedIndex != -1; } }

        // Method to raise the custom FilterChanged event.
        protected virtual void OnFilterChanged(EventArgs e)
        {
            FilterChanged?.Invoke(this, e);
        }

        private bool ignoreSelectedItemChanged = false;

        private void LstLogContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreSelectedItemChanged == false)
            {
                OnFilterChanged(EventArgs.Empty);
                BtnFilterOnSameMetadata.Enabled = LstLogContent.SelectedIndex != -1 && LogMetadataPropertiesUserSession.Count > 0;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (LstLogContent.Items.Count > 0) LstLogContent.SelectedIndex = -1;
        }

        private void ChkShowExtraLogEntries_CheckedChanged(object sender, EventArgs e)
        {
            TxtExtraLogEntries.Visible = ChkShowExtraLogEntries.Checked;
            OnFilterChanged(EventArgs.Empty);
        }

        private void TxtExtraLogEntries_TextChanged(object sender, EventArgs e)
        {
            OnFilterChanged(EventArgs.Empty);
        }

        private void LstLogContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Fetch the item
            if (LstLogContent.Items[e.Index] is not LogEntryDisplayModel item || item.ContentValue == null) return;

            Graphics g = e.Graphics;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Draw background
            if (isSelected)
            {
                g.FillRectangle(SelectedItemBackColor, e.Bounds);
            }
            else
            {
                g.FillRectangle(SystemBrushes.Window, e.Bounds);
            }

            if (item.IsError)
            {
                e.Graphics.DrawString(item.ContentValue.TimeDescription + " ERROR", LstLogContent.Font, isSelected ? Brushes.Black : Brushes.DarkRed, e.Bounds);
                return;
            }

            // Draw the flow tree node if it exists and the checkbox is checked
            if (item.FlowTreeNode != null && ChkShowFlowTree.Checked)
            {
                DrawFlowTreeNode(e, item, g);
            }
            else
            {
                //Default drawing, without tree
                string truncatedValue = TruncateTextToFit(item.ContentValue.TimeDescription + " " + item.ContentValue.Value, g, e.Bounds.Width);
                e.Graphics.DrawString(truncatedValue, LstLogContent.Font, Brushes.Black, e.Bounds);
            }
        }

        private void DrawFlowTreeNode(DrawItemEventArgs e, LogEntryDisplayModel item, Graphics g)
        {
            // Indentation
            const int indentPerLevel = 10;
            int timeX = e.Bounds.Left;
            int treeX = timeX + 56; // fixed width for TimeDescription
            int treeNodeDepth = item.FlowTreeNode == null ? 0 : item.FlowTreeNode.Depth;
            int indentPixels = treeNodeDepth * indentPerLevel; // tweak as needed
            int textX = treeX + indentPixels - indentPerLevel / 2;

            // Draw tree line(s)
            using (Pen pen = new(Color.Gray) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
            {
                LogFlowTreeNode currentNode = item.FlowTreeNode;
                for (int i = treeNodeDepth - 1; i >= 0; i--)
                {
                    // Draw a vertical line for each parent node if it has older siblings
                    if (currentNode.HasOlderSibling)
                    {
                        int lineX = treeX + i * indentPerLevel;
                        g.DrawLine(pen, lineX, e.Bounds.Top, lineX, e.Bounds.Bottom);
                    }
                    // Draw half a vertical line for the current value, if it does not have a next sibling
                    else if (i == treeNodeDepth - 1 && currentNode.IsLastSibling)
                    {
                        int lineX = treeX + i * indentPerLevel;
                        g.DrawLine(pen, lineX, e.Bounds.Top, lineX, e.Bounds.Bottom - (e.Bounds.Bottom - e.Bounds.Top) / 2);
                    }
                    currentNode = currentNode.Parent;
                }
            }
            // Draw the horizontal line just in front of the value text, indicating its connection in the tree
            if (!item.FlowTreeNode.IsRootNode)
            {
                int lineY = e.Bounds.Top + e.Bounds.Height / 2;
                int lineStartX = treeX + indentPixels - 10;
                int lineEndX = textX; // small gap before text

                using Pen pen = new(Color.Gray) { DashStyle = DashStyle.Dot };
                g.DrawLine(pen, lineStartX, lineY, lineEndX, lineY);
            }

            // Prepare fonts and brushes
            Brush textBrush = item.IsError ? Brushes.DarkRed : SystemBrushes.ControlText;

            // Draw TimeDescription
            g.DrawString(item.ContentValue.TimeDescription, LstLogContent.Font, textBrush, timeX, e.Bounds.Top);

            // Truncate description if it doesn’t fit
            string value = item.ContentValue.Value ?? string.Empty;
            string truncatedValue = TruncateTextToFit(value, g, e.Bounds.Right - textX);

            // Draw text
            g.DrawString(truncatedValue, LstLogContent.Font, textBrush, textX, e.Bounds.Top);
        }

        private string TruncateTextToFit(string text, Graphics graphics, int maxWidth)
        {
            string truncatedText = text;
            int ellipsisWidth = TextRenderer.MeasureText(graphics, ".....", LstLogContent.Font).Width;

            while (TextRenderer.MeasureText(graphics, truncatedText, LstLogContent.Font).Width + ellipsisWidth > maxWidth)
            {
                if (truncatedText.Length == 0) break;
                truncatedText = truncatedText[..^1];
            }

            if (truncatedText.Length < text.Length)
            {
                truncatedText += "...";
            }

            return truncatedText;
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == DefaulSearchtText)
            {
                txtSearch.Text = string.Empty;
                txtSearch.ForeColor = SystemColors.ControlText;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = DefaulSearchtText;
                txtSearch.ForeColor = Color.DarkGray;
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private string lastSearch = string.Empty;
        private void PerformSearch()
        {
            string searchString = txtSearch.Text.Trim();
            if (searchString == DefaulSearchtText) return;
            if (searchString != lastSearch)
            {
                UpdateLogContentList();
                lastSearch = searchString;
            }
        }
        private Dictionary<LogMetadataProperty, string> FilterOnMetadataPropertiesAndValues = null;
        private void BtnFilterOnSameMetadata_Click(object sender, EventArgs e)
        {
            FilterOnMetadataProperties(true);
        }
        private void FilterOnMetadataProperties(bool enableFilter)
        {
            BtnFilterOnSameMetadata.Visible = !enableFilter;
            BtnResetMetadataFilter.Visible = enableFilter;
            ChkShowFlowTree.Checked = enableFilter;
            if (enableFilter)
            {
                if (LstLogContent.SelectedIndex == -1) return;
                FilterOnMetadataPropertiesAndValues = [];
                foreach (var item in SelectedLogEntry.LogMetadataPropertiesWithStringValue)
                {
                    foreach (var logMetadataProperty in LogMetadataPropertiesUserSession)
                    {
                        if (item.Key == logMetadataProperty)
                        {
                            FilterOnMetadataPropertiesAndValues.Add(item.Key, item.Value);
                            break;
                        }
                    }
                }
                FilterOnMetadata?.Invoke(FilterOnMetadataPropertiesAndValues, true);
            }
            else
            {
                if (FilterOnMetadataPropertiesAndValues == null) return;
                FilterOnMetadata?.Invoke(FilterOnMetadataPropertiesAndValues, false);
                FilterOnMetadataPropertiesAndValues = null;
            }
        }

        private void BtnResetMetadataFilter_Click(object sender, EventArgs e)
        {
            FilterOnMetadataProperties(false);
        }

        private void ChkShowFlowTree_CheckedChanged(object sender, EventArgs e)
        {
            LstLogContent.SuspendDrawing();
            LstLogContent.Invalidate();
            LstLogContent.ResumeDrawing();
        }
    }
}
