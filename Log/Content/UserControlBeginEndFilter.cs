using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Utilities.Extensions;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;

namespace LogScraper
{
    internal partial class UserControlBeginEndFilter : UserControl
    {
        #region Private objects and initialization
        private const string DefaulSearchtText = "Filteren...";

        private List<LogContentProperty> LogContentPropertiesError = [];

        private List<LogMetadataProperty> LogMetadataPropertiesUserSession = [];

        private bool LogEntriesAreSingleSession = false;

        private List<LogEntry> LogEntriesLatestVersion;

        public UserControlBeginEndFilter()
        {
            InitializeComponent();
            SelectedItemBackColor = Brushes.Orange;
            //Preset the default search text
            TxtSearch_Leave(null, null);
        }
        #endregion

        #region Update log layout
        public void UpdateLogLayout(LogLayout logLayout)
        {
            CboLogContentType.Items.Clear();
            if (logLayout.LogContentProperties == null || logLayout.LogContentProperties.Count == 0) return;
            CboLogContentType.Items.AddRange([.. logLayout.LogContentProperties]);
            LogContentPropertiesError = [];
            foreach (LogContentProperty logContentProperty in logLayout.LogContentProperties)
            {
                if (logContentProperty.IsErrorProperty)
                {
                    LogContentPropertiesError.Add(logContentProperty);
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
            BtnResetMetadataFilter_Click(null, null);
            UpdateFilterOnMetadataControls();
        }
        #endregion

        #region Update the log entries in the listbox
        public void UpdateLogEntries(List<LogEntry> logEntries)
        {
            LogEntriesLatestVersion = logEntries;
            UpdateDisplayedLogEntries();
        }
        private void UpdateDisplayedLogEntries()
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

            // Create display objects for the log entries containing the log entry, content value, treenode, and error status
            List<LogEntryDisplayObject> LogEntryDisplayObjects = CreateLogEntryDisplayObjects(logContentProperty, LogEntriesLatestVersion);

            UpdateDisplayedLogEntriesUsingNewLogEntries(LogEntryDisplayObjects);
        }
        private List<LogEntryDisplayObject> CreateLogEntryDisplayObjects(LogContentProperty logContentProperty, List<LogEntry> logEntries)
        {
            if (logContentProperty == null) return null;

            // Get the search filter text
            string filter = txtSearch.Text.Trim();
            if (filter == DefaulSearchtText) filter = null;

            // Build the log tree flow beforehand, so we can use it to find the corresponding node for each log entry.
            List<LogFlowTreeNode> treeNodes = null;
            if (logContentProperty.IsBeginFlowTreeFilter && logContentProperty.EndFlowTreeContentProperty != null)
            {
                treeNodes = LogFlowTreeBuilder.BuildLogFlowTree(logEntries, logContentProperty, logContentProperty.EndFlowTreeContentProperty);
            }

            List<LogEntryDisplayObject> logEntryDisplayObjects = [];

            // Iterate through the latest version of log entries
            foreach (LogEntry logEntry in logEntries)
            {
                // If the log entry has no content properties, continue to the next log entry
                if (logEntry.LogContentProperties == null) continue;

                // Try to get the content for the selected log content property
                logEntry.LogContentProperties.TryGetValue(logContentProperty, out LogContentValue contentValue);
                bool isError = false;

                // If no normal content is found, check for error content if the "Show Errors" checkbox is checked
                if (contentValue == null)
                {
                    if (ConfigurationManager.GenericConfig.ShowErrorLinesInBeginAndEndFilters)
                    {
                        foreach (LogContentProperty logContentPropertyError in LogContentPropertiesError)
                        {
                            //If the log entry has an error content property, get the content value and continue using this value
                            if (logEntry.LogContentProperties.TryGetValue(logContentPropertyError, out contentValue)) break;
                        }
                        //If no error content is found, continue to the next log entry
                        if (contentValue == null) continue;
                        isError = true;
                    }
                }

                // Skip the log entry if it is not an error and the content value does not match the filter
                if (!isError && !string.IsNullOrEmpty(filter))
                {
                    if (!contentValue.Value.Contains(filter, StringComparison.InvariantCultureIgnoreCase)) continue;
                }

                // In order to show the flow tree, we need to find the corresponding node in the tree
                LogFlowTreeNode flowtreeNode = null;
                if (contentValue != null && treeNodes != null)
                {
                    foreach (LogFlowTreeNode node in treeNodes)
                    {
                        if (node.TryGetContentValueNodeFromTree(contentValue, out flowtreeNode))
                        {
                            break;
                        }
                    }
                }

                LogEntryDisplayObject logEntryDisplayObject = new()
                {
                    OriginalLogEntry = logEntry,
                    ContentValue = contentValue,
                    FlowTreeNode = flowtreeNode,
                    IsError = isError

                };
                logEntryDisplayObjects.Add(logEntryDisplayObject);
            }
            return logEntryDisplayObjects;
        }
        private void UpdateDisplayedLogEntriesUsingNewLogEntries(List<LogEntryDisplayObject> newLogEntries)
        {
            int currentCount = LstLogContent.Items.Count;
            int newCount = newLogEntries.Count;

            int compareCount = Math.Min(currentCount, newCount);
            bool startMatches = true;

            for (int i = 0; i < compareCount; i++)
            {
                if (!newLogEntries[i].ContentValue.Equals(((LogEntryDisplayObject)LstLogContent.Items[i]).ContentValue))
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

            LogEntriesAreSingleSession = HasOnlyOneSession(LogEntriesLatestVersion);
            UpdateFilterOnMetadataControls();

            // Case 2: New list extends existing — add only new items
            if (startMatches && newCount > currentCount)
            {
                LstLogContent.SuspendDrawing();

                //Update the flow tree nodes for the existing items since they may have changed
                foreach (var item in LstLogContent.Items)
                {
                    if (item is not LogEntryDisplayObject logEntryDisplayObject) continue;
                    logEntryDisplayObject.FlowTreeNode = newLogEntries.Where(entry => entry.OriginalLogEntry.Equals(logEntryDisplayObject.OriginalLogEntry)).Single().FlowTreeNode;
                }
                if (ChkShowFlowTree.Checked) LstLogContent.Invalidate();

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
        private bool HasOnlyOneSession(List<LogEntry> logEntries)
        {
            foreach (LogMetadataProperty metadataProperty in LogMetadataPropertiesUserSession)
            {
                HashSet<string> values = [];

                foreach (LogEntry logEntry in logEntries)
                {
                    if (logEntry.LogMetadataPropertiesWithStringValue.TryGetValue(metadataProperty, out string value))
                    {
                        values.Add(value);

                        if (values.Count > 1)
                        {
                            return false; // meerdere sessies gevonden
                        }
                    }
                }
            }

            return true; // alle properties hebben maximaal 1 waarde
        }

        private void FullyRedrawList(List<LogEntryDisplayObject> newLogEntries)
        {
            // Store the currently selected log entry
            LogEntryDisplayObject selectedLogEntry = (LogEntryDisplayObject)LstLogContent.SelectedItem;
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
            foreach (LogEntryDisplayObject logEntryDisplayObject in newLogEntries)
            {
                if (logEntryDisplayObject.OriginalLogEntry == selectedLogEntry.OriginalLogEntry)
                {
                    try
                    {
                        ignoreSelectedItemChanged = true;
                        LstLogContent.SelectedItem = logEntryDisplayObject;
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
        #endregion

        #region Private class LogEntryDisplayObject
        private class LogEntryDisplayObject
        {
            public LogEntry OriginalLogEntry { get; set; }
            public LogContentValue ContentValue { get; set; }
            public bool IsError { get; set; }
            public LogFlowTreeNode FlowTreeNode { get; set; }
            public override string ToString()
            { return ContentValue != null ? ContentValue.Value : string.Empty; }
        }
        #endregion

        #region Public properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush SelectedItemBackColor { get; set; }
        public LogEntry SelectedLogEntry
        {
            get
            {
                if (SelectedLogEntryDisplayObject == null) return null;
                return SelectedLogEntryDisplayObject.OriginalLogEntry;
            }
        }

        private LogEntryDisplayObject SelectedLogEntryDisplayObject
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogEntryDisplayObject)LstLogContent.SelectedItem);
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
                return ((LogEntryDisplayObject)LstLogContent.SelectedItem).ContentValue;
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
        public bool FilterIsEnabled
        {
            get { return LstLogContent.SelectedIndex != -1; }
        }
        #endregion

        #region Draw listbox items
        private void LstLogContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Fetch the item
            if (LstLogContent.Items[e.Index] is not LogEntryDisplayObject item || item.ContentValue == null) return;

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

        private void DrawFlowTreeNode(DrawItemEventArgs e, LogEntryDisplayObject item, Graphics g)
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
        #endregion

        #region Search
        private string lastSearch = string.Empty;
        private void PerformSearch()
        {
            string searchString = txtSearch.Text.Trim();
            if (searchString == DefaulSearchtText) return;
            if (searchString != lastSearch)
            {
                UpdateDisplayedLogEntries();
                lastSearch = searchString;
            }
        }
        #endregion

        #region Events
        /// Event to filter log entries based on metadata properties
        public event Action<Dictionary<LogMetadataProperty, string>, bool> FilterOnMetadata;

        /// Event to filter log entries based on the selected log content property
        public event EventHandler FilterChanged;
        protected virtual void OnFilterChanged(EventArgs e)
        {
            FilterChanged?.Invoke(this, e);
        }
        #endregion

        #region Control events
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
            }
        }
        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == DefaulSearchtText)
            {
                txtSearch.Text = string.Empty;
                txtSearch.ForeColor = SystemColors.ControlText;
            }
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (LstLogContent.Items.Count > 0) LstLogContent.SelectedIndex = -1;
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = DefaulSearchtText;
                txtSearch.ForeColor = Color.DarkGray;
            }
        }

        private void CboLogContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDisplayedLogEntries();
            OnFilterChanged(EventArgs.Empty);
            ChkShowFlowTree.Enabled = SelectedLogContentProperty != null && SelectedLogContentProperty.IsBeginFlowTreeFilter;
        }
        private bool ignoreSelectedItemChanged = false;

        private void LstLogContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreSelectedItemChanged == false)
            {
                OnFilterChanged(EventArgs.Empty);
                UpdateFilterOnMetadataControls();
            }
        }
        private void LstLogContent_DoubleClick(object sender, EventArgs e)
        {
            if (SelectedContentValue == null) return;

            if (!LogEntriesAreSingleSession) BtnFilterOnSameMetadata_Click(null, null);
        }

        private void BtnFilterOnSameMetadata_Click(object sender, EventArgs e)
        {
            if (LstLogContent.SelectedIndex == -1) return;

            if (ConfigurationManager.GenericConfig.AutoToggleHierarchy) ChkShowFlowTree.Checked = SelectedLogContentProperty.IsBeginFlowTreeFilter;

            Dictionary<LogMetadataProperty, string> FilterOnMetadataPropertiesAndValues = [];
            foreach (var logMetadataProperty in LogMetadataPropertiesUserSession)
            {
                if (SelectedLogEntry.LogMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value))
                {
                    FilterOnMetadataPropertiesAndValues.Add(logMetadataProperty, value);
                    break;
                }
            }
            FilterOnMetadata?.Invoke(FilterOnMetadataPropertiesAndValues, true);
        }

        private void BtnResetMetadataFilter_Click(object sender, EventArgs e)
        {
            if (ConfigurationManager.GenericConfig.AutoToggleHierarchy) ChkShowFlowTree.Checked = false;

            if (LogEntriesLatestVersion == null || LogEntriesLatestVersion.Count == 0) return;

            Dictionary<LogMetadataProperty, string> FilterOnMetadataPropertiesAndValues = [];
            foreach (LogMetadataProperty logMetadataProperty in LogMetadataPropertiesUserSession)
            {
                if (LogEntriesLatestVersion[0].LogMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value))
                {
                    FilterOnMetadataPropertiesAndValues.Add(logMetadataProperty, value);
                }
            }
            if (FilterOnMetadataPropertiesAndValues.Count > 0) FilterOnMetadata?.Invoke(FilterOnMetadataPropertiesAndValues, false);
        }
        private void ChkShowFlowTree_CheckedChanged(object sender, EventArgs e)
        {
            LstLogContent.SuspendDrawing();
            LstLogContent.Invalidate();
            LstLogContent.ResumeDrawing();
        }
        private void UpdateFilterOnMetadataControls()
        {
            BtnFilterOnSameMetadata.Enabled = LogMetadataPropertiesUserSession.Count > 0 && SelectedContentValue != null;
            // In case there is no session filtering or no log entries, still show the filter button
            BtnFilterOnSameMetadata.Visible = !LogEntriesAreSingleSession || LogMetadataPropertiesUserSession.Count == 0 || LogEntriesLatestVersion.Count == 0;
            BtnResetMetadataFilter.Visible = !BtnFilterOnSameMetadata.Visible;
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
        #endregion

    }
}
