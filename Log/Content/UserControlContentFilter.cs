using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;

namespace LogScraper
{
    internal partial class UserControlLogContentFilter : UserControl
    {
        #region Private objects and initialization
        private const string DefaulSearchtTextFormat = "Filter {0}...";
        private string DefaulSearchtText = "Filter...";

        private List<LogContentProperty> LogContentPropertiesError = [];

        private List<LogMetadataProperty> LogMetadataPropertiesUserSession = [];

        private bool LogEntriesAreSingleSession = false;

        private bool IsSessionMetadataFilteringActive = false;

        private LogMetadataFilterResult LogMetadataFilterResult;

        private LogEntryDisplayObject selectedBeginEntryDisplayObject = null;

        private LogEntryDisplayObject selectedEndEntryDisplayObject = null;

        public UserControlLogContentFilter()
        {
            InitializeComponent();
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
            UpdateTopBottomControls();
        }
        #endregion

        #region Private class LogEntryDisplayObject
        private class LogEntryDisplayObject
        {
            public int Index { get; set; }
            public LogEntry OriginalLogEntry { get; set; }
            public LogContentValue ContentValue { get; set; }
            public bool IsError { get; set; }
            public LogFlowTreeNode FlowTreeNode { get; set; }
            public override string ToString()
            { return ContentValue != null ? ContentValue.Value : string.Empty; }
        }
        #endregion

        #region Update the log entries in the listbox
        public void UpdateLogEntries(LogMetadataFilterResult logMetadataFilterResult)
        {
            // Determine if the session metadata filtering is active, to control the session filtering button
            IsSessionMetadataFilteringActive = false;
            foreach (LogMetadataPropertyAndValues logMetadataPropertyAndValues in logMetadataFilterResult.LogMetadataPropertyAndValues)
            {
                if (logMetadataPropertyAndValues.LogMetadataProperty.IsSessionData)
                {
                    int numberOfEnabledFilters = logMetadataPropertyAndValues.LogMetadataValues.Keys.Where(value => value.IsFilterEnabled).ToList().Count;
                    if (numberOfEnabledFilters > 0)
                    {
                        IsSessionMetadataFilteringActive = true;
                        break;
                    }
                }
            }

            LogMetadataFilterResult = logMetadataFilterResult;
            UpdateDisplayedLogEntries();
        }
        private void UpdateDisplayedLogEntries()
        {
            // If there are no log entries, clear the list and return
            if (LogMetadataFilterResult == null || LogMetadataFilterResult.LogEntries == null)
            {
                LstLogContent.Items.Clear();
                BtnResetMetadataFilter_Click(null, null);
                return;
            }

            // Get the selected log content property
            LogContentProperty logContentProperty = SelectedLogContentProperty;

            // If no log content property is selected, return
            if (logContentProperty == null) return;

            // Create display objects for the log entries containing the log entry, content value, treenode, and error status
            List<LogEntryDisplayObject> logEntryDisplayObjects = CreateLogEntryDisplayObjects(logContentProperty, LogMetadataFilterResult.LogEntries);

            ValidateBeginEndContentFiltersOnNewLogEnties(LogMetadataFilterResult.LogEntries);

            UpdateDisplayedLogEntriesUsingNewLogEntries(logEntryDisplayObjects);

            UpdateBeginEndFilterDisplayObjectsIndex();
        }

        /// <summary>
        /// Validates the currently selected begin and end content filters against a list of new log entries.
        /// </summary>
        /// <remarks>This method ensures that the selected begin and end content filters remain valid by
        /// checking if their corresponding log entries exist in the provided list. If a filter's associated log entry
        /// is not found, the filter is reset to null.</remarks>
        /// <param name="logEntries">A list of <see cref="LogEntry"/> objects representing the new log entries to check against.</param>
        private void ValidateBeginEndContentFiltersOnNewLogEnties(List<LogEntry> logEntries)
        {
            if (selectedBeginEntryDisplayObject != null)
            {
                bool beginFilterFound = false;
                foreach (LogEntry logEntry in logEntries)
                {
                    if (logEntry == selectedBeginEntryDisplayObject.OriginalLogEntry)
                    {
                        beginFilterFound = true;
                        break;
                    }
                }
                if (!beginFilterFound)
                {
                    // If the begin filter is not found, reset the begin filter
                    selectedBeginEntryDisplayObject = null;
                }
            }


            if (selectedEndEntryDisplayObject == null) return;

            bool endFilterFound = false;
            foreach (LogEntry logEntry in logEntries)
            {
                if (logEntry == selectedEndEntryDisplayObject.OriginalLogEntry)
                {
                    endFilterFound = true;
                    break;
                }
            }
            if (!endFilterFound)
            {
                // If the begin filter is not found, reset the begin filter
                selectedBeginEntryDisplayObject = null;
            }
        }

        private List<LogEntryDisplayObject> CreateLogEntryDisplayObjects(LogContentProperty logContentProperty, List<LogEntry> logEntries)
        {
            if (logContentProperty == null) return null;

            // Get the search filter text
            string filter = txtSearch.Text.Trim();
            if (filter == DefaulSearchtText) filter = null;

            List<LogEntryDisplayObject> logEntryDisplayObjects = [];
            int index = 0;
            // Iterate through the latest version of log entries
            foreach (LogEntry logEntry in logEntries)
            {
                //Track the in order to be able to show the correct disabled (grayed out) lines when setting the top and bottom values
                index++;

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

                // Get the log tree flow beforehand, so we can use it to find the corresponding node for each log entry.
                List<LogFlowTreeNode> treeNodes = LogMetadataFilterResult.LogFlowTrees[logContentProperty];

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
                    Index = index,
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

            LogEntriesAreSingleSession = HasOnlyOneSession(LogMetadataFilterResult.LogEntries);
            UpdateFilterOnMetadataControls();

            // Case 2: New list extends existing — add only new items
            if (startMatches && newCount > currentCount)
            {
                LstLogContent.SuspendDrawing();

                //Update the flow tree nodes for the existing items since they may have changed
                int index = 0;
                foreach (var item in LstLogContent.Items)
                {
                    if (item is not LogEntryDisplayObject logEntryDisplayObject) continue;
                    logEntryDisplayObject.FlowTreeNode = newLogEntries.Where(entry => entry.OriginalLogEntry.Equals(logEntryDisplayObject.OriginalLogEntry)).Single().FlowTreeNode;
                    logEntryDisplayObject.Index = index;
                    index++;
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

            bool selectedEntryFound = false;
            // Iterate through the new log entries and select the previously selected log entry if it exists
            foreach (LogEntryDisplayObject logEntryDisplayObject in newLogEntries)
            {
                if (logEntryDisplayObject.OriginalLogEntry == selectedLogEntry.OriginalLogEntry)
                {
                    try
                    {
                        ignoreSelectedItemChanged = true;
                        LstLogContent.SelectedItem = logEntryDisplayObject;
                        selectedEntryFound = true;
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

            // If the selected item cannot be reselected raise the event that the selection changed
            if (!selectedEntryFound) SelectedItemChanged(this, EventArgs.Empty);
        }

        private void UpdateBeginEndFilterDisplayObjectsIndex()
        {
            if (LogMetadataFilterResult == null || LogMetadataFilterResult.LogEntries == null)
            {
                selectedBeginEntryDisplayObject = null;
                selectedEndEntryDisplayObject = null;
                return;
            }
            if (selectedBeginEntryDisplayObject != null)
            {
                foreach (LogEntryDisplayObject logEntryDisplayObject in LstLogContent.Items)
                {
                    if (logEntryDisplayObject.OriginalLogEntry == selectedBeginEntryDisplayObject.OriginalLogEntry)
                    {
                        selectedBeginEntryDisplayObject.Index = logEntryDisplayObject.Index;
                        break;
                    }
                }
            }
            if (selectedEndEntryDisplayObject != null)
            {
                foreach (LogEntryDisplayObject logEntryDisplayObject in LstLogContent.Items)
                {
                    if (logEntryDisplayObject.OriginalLogEntry == selectedEndEntryDisplayObject.OriginalLogEntry)
                    {
                        selectedEndEntryDisplayObject.Index = logEntryDisplayObject.Index;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Public properties
        public LogEntry SelectedLogEntry
        {
            get
            {
                if (SelectedLogEntryDisplayObject == null) return null;
                return SelectedLogEntryDisplayObject.OriginalLogEntry;
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

        public LogEntry SelectedBeginLogEntry
        {
            get
            {
                return selectedBeginEntryDisplayObject?.OriginalLogEntry;
            }
        }

        public LogEntry SelectedEndLogEntry
        {
            get
            {
                return selectedEndEntryDisplayObject?.OriginalLogEntry;
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
        #endregion

        #region Draw listbox items
        private readonly SolidBrush LighterBrush = new(Color.FromArgb(240, 240, 240));
        private void LstLogContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            // Fetch the item
            if (LstLogContent.Items[e.Index] is not LogEntryDisplayObject item || item.ContentValue == null) return;

            Graphics g = e.Graphics;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            bool isOutOfScope = IslogEntryDisplayObjectOutOfScope(item);

            // Draw background
            if (selectedBeginEntryDisplayObject != null && selectedBeginEntryDisplayObject.OriginalLogEntry == item.OriginalLogEntry)
            {
                if (isSelected) g.FillRectangle(LogScraperBrushes.BlueSelectedLogline, e.Bounds);
                else g.FillRectangle(LogScraperBrushes.GraySelectedBeginOrEnd, e.Bounds);
            }
            else if (selectedEndEntryDisplayObject != null && selectedEndEntryDisplayObject.OriginalLogEntry == item.OriginalLogEntry)
            {
                if (isSelected) g.FillRectangle(LogScraperBrushes.BlueSelectedLogline, e.Bounds);
                else g.FillRectangle(LogScraperBrushes.GraySelectedBeginOrEnd, e.Bounds);
            }
            else
            {
                if (isOutOfScope && isSelected) g.FillRectangle(Brushes.LightGray, e.Bounds);
                else if (isSelected) g.FillRectangle(LogScraperBrushes.BlueSelectedLogline, e.Bounds);
                else g.FillRectangle(SystemBrushes.Window, e.Bounds);
            }

            if (item.IsError)
            {
                e.Graphics.DrawString(item.ContentValue.TimeDescription + " ERROR", LstLogContent.Font, DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope), e.Bounds);

                e.DrawFocusRectangle();
                return;
            }

            // Draw the flow tree node if it exists and the checkbox is checked
            if (item.FlowTreeNode != null && ChkShowFlowTree.Checked)
            {
                DrawFlowTreeNode(e, item, g, isSelected, isOutOfScope);
            }
            else
            {
                //Default drawing, without tree
                string truncatedValue = TruncateTextToFit(item.ContentValue.TimeDescription + " " + item.ContentValue.Value, g, e.Bounds.Width);
                e.Graphics.DrawString(truncatedValue, LstLogContent.Font, DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope), e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        private void DrawFlowTreeNode(DrawItemEventArgs e, LogEntryDisplayObject item, Graphics g, bool isSelected, bool isOutOfScope)
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
            Brush textBrush = DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope);

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

        private static Brush DetermineTextColorBasedOnLogEntryPosition(LogEntryDisplayObject logEntryDisplayObject, bool isSelected, bool isOutOfScope)
        {
            if (isOutOfScope)
            {
                return isSelected ? Brushes.Gray : LogScraperBrushes.GrayLogEntriesOutOfScope;
            }
            return logEntryDisplayObject.IsError ? Brushes.DarkRed : SystemBrushes.ControlText; // Default text color
        }

        /// <summary>
        /// Determines whether the specified <see cref="LogEntryDisplayObject"/> is outside the currently selected
        /// range.
        /// </summary>
        /// <remarks>A <see cref="LogEntryDisplayObject"/> is considered out of scope if its <c>Index</c>
        /// is less than the  <c>Index</c> of the selected begin entry display object, or greater than the <c>Index</c>
        /// of the selected  end entry display object. If either the selected begin or end entry display object is
        /// <c>null</c>, the  corresponding boundary is considered unbounded.</remarks>
        /// <param name="logEntryDisplayObject">The <see cref="LogEntryDisplayObject"/> to evaluate. Must not be <c>null</c>.</param>
        /// <returns><see langword="true"/> if the <paramref name="logEntryDisplayObject"/> is outside the range defined by  the
        /// selected begin and end entry display objects; otherwise, <see langword="false"/>.</returns>
        private bool IslogEntryDisplayObjectOutOfScope(LogEntryDisplayObject logEntryDisplayObject)
        {
            if (logEntryDisplayObject == null) return true;

            return (selectedBeginEntryDisplayObject != null && logEntryDisplayObject.Index < selectedBeginEntryDisplayObject.Index) ||
                (selectedEndEntryDisplayObject != null && logEntryDisplayObject.Index > selectedEndEntryDisplayObject.Index);
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
        public event EventHandler EndEntryChanged;
        protected virtual void OnEndEntryChanged(EventArgs e)
        {
            EndEntryChanged?.Invoke(this, e);
        }

        /// Event to filter log entries based on the selected log content property
        public event EventHandler BeginEntryChanged;
        protected virtual void OnBeginEntryChanged(EventArgs e)
        {
            BeginEntryChanged?.Invoke(this, e);
        }

        /// Event to filter log entries based on the selected log content property
        public event EventHandler SelectedItemChanged;
        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            SelectedItemChanged?.Invoke(this, e);
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
            UpdateShowTreeControls(false);

            if (SelectedLogContentProperty != null)
            {
                bool resetText = (txtSearch.Text == DefaulSearchtText);
                DefaulSearchtText = string.Format(DefaulSearchtTextFormat, SelectedLogContentProperty.Description.ToLower());
                if (resetText) txtSearch.Text = DefaulSearchtText;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.SuspendDrawing();
            if (LstLogContent.Items.Count > 0) LstLogContent.SelectedIndex = -1;

            //Reset the top end end filters
            selectedBeginEntryDisplayObject = null;
            selectedEndEntryDisplayObject = null;
            OnBeginEntryChanged(e);
            OnEndEntryChanged(e);
            LstLogContent.Invalidate();
            UpdateTopBottomControls();

            this.ResumeDrawing();
        }
        private bool ignoreSelectedItemChanged = false;

        private void LstLogContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreSelectedItemChanged == false)
            {
                OnSelectedItemChanged(EventArgs.Empty);

                UpdateFilterOnMetadataControls();
                UpdateTopBottomControls();
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
            if (ConfigurationManager.GenericConfig.AutoToggleHierarchy) ChkShowNoTree.Checked = true;

            if (LogMetadataFilterResult == null || LogMetadataFilterResult.LogEntries == null || LogMetadataFilterResult.LogEntries.Count == 0) return;

            Dictionary<LogMetadataProperty, string> FilterOnMetadataPropertiesAndValues = [];
            foreach (LogMetadataProperty logMetadataProperty in LogMetadataPropertiesUserSession)
            {
                if (LogMetadataFilterResult.LogEntries[0].LogMetadataPropertiesWithStringValue.TryGetValue(logMetadataProperty, out string value))
                {
                    FilterOnMetadataPropertiesAndValues.Add(logMetadataProperty, value);
                }
            }
            if (FilterOnMetadataPropertiesAndValues.Count > 0) FilterOnMetadata?.Invoke(FilterOnMetadataPropertiesAndValues, false);
        }

        private void ChkShowFlowTree_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShowTreeControls(true);
        }
        private void ChkShowNoTree_CheckedChanged(object sender, EventArgs e)
        {
            UpdateShowTreeControls(false);
        }

        private bool updateShowTreeInProgress = false;
        private void UpdateShowTreeControls(bool showTree)
        {
            if (updateShowTreeInProgress) return;

            updateShowTreeInProgress = true;
            if (SelectedLogContentProperty == null || !SelectedLogContentProperty.IsBeginFlowTreeFilter)
            {
                ChkShowNoTree.Checked = false;
                ChkShowFlowTree.Checked = false;
                ChkShowNoTree.Enabled = false;
                ChkShowFlowTree.Enabled = false;

                updateShowTreeInProgress = false;
                return;
            }

            //Also show no tree if previously no tree was available
            ChkShowFlowTree.Checked = showTree;
            ChkShowFlowTree.Enabled = !showTree;
            ChkShowNoTree.Checked = !showTree;
            ChkShowNoTree.Enabled = showTree;
            LstLogContent.SuspendDrawing();
            LstLogContent.Invalidate();
            LstLogContent.ResumeDrawing();
            updateShowTreeInProgress = false;
        }

        private void BtnSelectBegin_Click(object sender, EventArgs e)
        {
            SelectLogEntryBegin();
        }
        public void SelectLogEntryBegin()
        {
            if (SelectedLogEntryDisplayObject == null) return;

            selectedBeginEntryDisplayObject = SelectedLogEntryDisplayObject;

            if (selectedEndEntryDisplayObject != null && selectedEndEntryDisplayObject.Index < selectedBeginEntryDisplayObject.Index)
            {
                selectedEndEntryDisplayObject = null;
            }
            OnBeginEntryChanged(null);
            LstLogContent.Invalidate();
            UpdateTopBottomControls();
        }

        private void BtnSelectEnd_Click(object sender, EventArgs e)
        {
            SelectLogEntryEnd();
        }

        public void SelectLogEntryEnd()
        {
            if (SelectedLogEntryDisplayObject == null) return;

            selectedEndEntryDisplayObject = SelectedLogEntryDisplayObject;

            if (selectedBeginEntryDisplayObject != null && selectedBeginEntryDisplayObject.Index > selectedEndEntryDisplayObject.Index)
            {
                selectedBeginEntryDisplayObject = null;
            }

            OnEndEntryChanged(null);
            LstLogContent.Invalidate();
            UpdateTopBottomControls();
        }
        private void UpdateTopBottomControls()
        {
            BtnSelectTop.Enabled = LstLogContent.SelectedIndex > 0;
            BtnSelectEnd.Enabled = LstLogContent.SelectedIndex != -1 && LstLogContent.SelectedIndex != LstLogContent.Items.Count - 1;
            BtnReset.Enabled = LstLogContent.SelectedIndex != -1 || selectedBeginEntryDisplayObject != null || selectedEndEntryDisplayObject != null;
        }
        private void UpdateFilterOnMetadataControls()
        {
            // In case there is no session filtering or no log entries, disable the filter button
            if (LogMetadataPropertiesUserSession.Count == 0 || LogMetadataFilterResult == null || LogMetadataFilterResult.LogEntries == null || LogMetadataFilterResult.LogEntries.Count == 0)
            {
                BtnFilterOnSameMetadata.Enabled = false;
                BtnFilterOnSameMetadata.Visible = true;
                BtnResetMetadataFilter.Visible = false;
                return;
            }
            //Enable the filter button if there is a selected log entry and the log entries are not from a single session
            BtnFilterOnSameMetadata.Enabled = SelectedContentValue != null && !LogEntriesAreSingleSession;
            // Show the filter button if the log entries are not from a single session or the session metadata filtering is not active
            // In case the metadata filtering is not active, the button will be disabled as there is no action to perform
            BtnFilterOnSameMetadata.Visible = !LogEntriesAreSingleSession || !IsSessionMetadataFilteringActive;
            BtnResetMetadataFilter.Visible = !BtnFilterOnSameMetadata.Visible;
        }
        #endregion
    }
}
