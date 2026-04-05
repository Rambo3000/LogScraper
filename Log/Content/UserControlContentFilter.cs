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
using LogScraper.Log.Rendering;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;
using System.ComponentModel;

namespace LogScraper
{
    internal partial class UserControlLogContentFilter : UserControl
    {
        #region Private objects and initialization
        private const string DefaulSearchtTextFormat = "Filter {0}...";
        private string DefaulSearchtText = "Filter...";

        private List<LogContentProperty> LogContentPropertiesError = [];

        private LogMetadataFilterResult LogMetadataFilterResult;

        private LogRange logRange = new();

        private bool showTree = false;

        public UserControlLogContentFilter()
        {
            InitializeComponent();
            TxtSearch_Leave(null, null);
        }
        #endregion

        #region Update log layout
        public void UpdateLogLayout(LogLayout logLayout)
        {
            LstLogContent.ItemHeight = LstLogContent.Font.Height;

            CboLogContentType.Items.Clear();
            if (logLayout.LogContentProperties == null || logLayout.LogContentProperties.Count == 0) return;
            CboLogContentType.Items.AddRange([.. logLayout.LogContentProperties.Where(item => item.IsNavigationEnabled)]);
            LogContentPropertiesError = [];
            foreach (LogContentProperty logContentProperty in logLayout.LogContentProperties)
            {
                if (logContentProperty.IsErrorProperty)
                {
                    LogContentPropertiesError.Add(logContentProperty);
                    break;
                }
            }

            if (CboLogContentType.Items.Count > 0) CboLogContentType.SelectedIndex = 0;
        }
        #endregion

        #region Private class LogEntryDisplayObject
        private class LogEntryDisplayObject
        {
            public int Index { get; set; }
            public LogEntry OriginalLogEntry { get; set; }
            public LogContentValue ContentValue { get; set; }
            public LogFlowTreeNode FlowTreeNode { get; set; }
            public override string ToString()
            { return ContentValue != null ? ContentValue.Value : string.Empty; }
        }
        #endregion

        #region Update the log entries in the listbox

        public void UpdateLogEntries(LogMetadataFilterResult logMetadataFilterResult)
        {
            LogMetadataFilterResult = logMetadataFilterResult;
            UpdateDisplayedLogEntries();
        }

        private void UpdateDisplayedLogEntries()
        {
            if (LogMetadataFilterResult == null || LogMetadataFilterResult.LogEntries == null)
            {
                LstLogContent.Items.Clear();
                return;
            }

            LogContentProperty logContentProperty = SelectedLogContentProperty;
            if (logContentProperty == null) return;
            (int begin, int end) = LogRenderer.CalculateLogRenderRange(LogMetadataFilterResult.LogEntries, logRange);
            List<LogEntryDisplayObject> logEntryDisplayObjects = CreateLogEntryDisplayObjects(logContentProperty, LogMetadataFilterResult.LogEntries[begin..end]);

            UpdateDisplayedLogEntriesUsingNewLogEntries(logEntryDisplayObjects);
        }

        private List<LogEntryDisplayObject> CreateLogEntryDisplayObjects(LogContentProperty logContentProperty, List<LogEntry> logEntries)
        {
            if (logContentProperty == null) return null;

            string filter = txtSearch.Text.Trim();
            if (filter == DefaulSearchtText) filter = null;

            bool showErrors = ConfigurationManager.GenericConfig.ShowErrorLinesInBeginAndEndFilters;
            bool filterEnabled = !string.IsNullOrEmpty(filter);

            List<LogEntryDisplayObject> logEntryDisplayObjects = [];
            int index = 0;

            foreach (LogEntry logEntry in logEntries)
            {
                //Track the order to be able to show the correct disabled (grayed out) lines when setting the top and bottom values
                index++;

                if (logEntry.LogContentProperties == null) continue;


                // Try to get the content for the selected log content property
                logEntry.LogContentProperties.TryGetValue(logContentProperty, out LogContentValue contentValue);

                if (contentValue == null)
                {
                    if (!showErrors || !logEntry.IsErrorLogEntry) continue;

                    foreach (LogContentProperty logContentPropertyError in LogContentPropertiesError)
                    {
                        //If the log entry has an error content property, get the content value and continue using this value
                        //We are only interested in the formatted time description of the error content, so we can show the log entry in the correct order in the list
                        // but the actual value is not relevant as it will be shown as "ERROR"
                        if (logEntry.LogContentProperties.TryGetValue(logContentPropertyError, out contentValue)) break;
                    }
                }

                if (filterEnabled && contentValue != null)
                {
                    if (!contentValue.Value.Contains(filter, StringComparison.OrdinalIgnoreCase)) continue;
                }

                LogFlowTree logFlowTree = LogMetadataFilterResult.LogFlowTrees[logContentProperty];
                LogFlowTreeNode flowtreeNode = null;
                logFlowTree?.LogEntryDictionary?.TryGetValue(logEntry, out flowtreeNode);

                logEntryDisplayObjects.Add(new LogEntryDisplayObject
                {
                    Index = index,
                    OriginalLogEntry = logEntry,
                    ContentValue = contentValue,
                    FlowTreeNode = flowtreeNode
                });
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
            if (startMatches && currentCount == newCount) return;

            // Case 2: New list extends existing — add only new items
            if (startMatches && newCount > currentCount)
            {
                LstLogContent.SuspendDrawing();

                //Update the flow tree nodes for the existing items since they may have changed
                int index = 0;
                foreach (var item in LstLogContent.Items)
                {
                    if (item is not LogEntryDisplayObject logEntryDisplayObject) continue;
                    // Use the first item in the Linq expression as cases have been found where more than one log entry matches
                    logEntryDisplayObject.FlowTreeNode = newLogEntries.Where(entry => entry.OriginalLogEntry.Equals(logEntryDisplayObject.OriginalLogEntry)).First().FlowTreeNode;
                    logEntryDisplayObject.Index = index;
                    index++;
                }
                if (showTree) LstLogContent.Invalidate();

                LstLogContent.Items.AddRange([.. newLogEntries.Skip(currentCount)]);
                LstLogContent.ResumeDrawing();
                return;
            }

            // In all other cases: Mismatch — full redraw
            FullyRedrawList(newLogEntries);
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

            LstLogContent.ResumeDrawing();

            // If the selected item cannot be reselected raise the event that the selection changed
            if (!selectedEntryFound) SelectedItemChanged(this, EventArgs.Empty);
        }
        #endregion

        #region Public methods and properties

        private bool ClearSelectedLogEntryExternallyInProgress = false;

        public void ClearSelectedLogEntry()
        {
            ClearSelectedLogEntryExternallyInProgress = true;
            LstLogContent.SelectedIndex = -1;
            ClearSelectedLogEntryExternallyInProgress = false;
        }

        public LogEntry SelectedLogEntry
        {
            get => SelectedLogEntryDisplayObject?.OriginalLogEntry;
        }

        public LogContentValue SelectedContentValue
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogEntryDisplayObject)LstLogContent.SelectedItem).ContentValue;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogRange LogRange
        {
            get
            {
                return logRange;
            }
            set
            {
                logRange = value;
                UpdateDisplayedLogEntries();
            }
        }

        private LogEntryDisplayObject SelectedLogEntryDisplayObject
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return (LogEntryDisplayObject)LstLogContent.SelectedItem;
            }
        }

        private LogContentProperty SelectedLogContentProperty
        {
            get
            {
                if (CboLogContentType.SelectedItem == null) return null;
                return (LogContentProperty)CboLogContentType.SelectedItem;
            }
        }

        public void Reset()
        {
            ClearSelectedLogEntry();
            LogContentPropertiesError = [];
            LogMetadataFilterResult = null;
        }
        #endregion

        #region Draw listbox items
        private void LstLogContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            // Fetch the item
            if (LstLogContent.Items[e.Index] is not LogEntryDisplayObject item || item.ContentValue == null) return;

            Graphics g = e.Graphics;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            //Disable being out of scope for now
            //bool isOutOfScope = IslogEntryDisplayObjectOutOfScope(item);
            bool isOutOfScope = false;

            if (isOutOfScope && isSelected) g.FillRectangle(Brushes.LightGray, e.Bounds);
            else if (isSelected) g.FillRectangle(LogScraperBrushes.BlueSelectedLogline, e.Bounds);
            else g.FillRectangle(SystemBrushes.Window, e.Bounds);

            if (item.OriginalLogEntry.IsErrorLogEntry && !SelectedLogContentProperty.IsErrorProperty)
            {
                e.Graphics.DrawString(item.ContentValue.TimeDescription + " ERROR", LstLogContent.Font, DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope), e.Bounds);
                e.DrawFocusRectangle();
                return;
            }

            if (item.FlowTreeNode != null && showTree)
                DrawFlowTreeNode(e, item, g, isSelected, isOutOfScope);
            else
            {
                string value = item.ContentValue.Value?.Length > 128 ? item.ContentValue.Value[0..127] : item.ContentValue.Value ?? string.Empty;
                string truncatedValue = TruncateTextToFit(item.ContentValue.TimeDescription + " " + value, g, e.Bounds.Width);
                e.Graphics.DrawString(truncatedValue, LstLogContent.Font, DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope), e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        private static readonly Pen PenForDrawingLines = new(Color.Gray) { DashStyle = DashStyle.Dot };

        private int TimeDescriptionFixedWidth = -1;

        private void DrawFlowTreeNode(DrawItemEventArgs e, LogEntryDisplayObject item, Graphics g, bool isSelected, bool isOutOfScope)
        {
            // Indentation
            const int indentPerLevel = 10;
            int timeX = e.Bounds.Left;

            if (TimeDescriptionFixedWidth == -1)
            {
                // Calculate the fixed width for TimeDescription based on the widest possible time description
                // This is required when the application is scaled to non-100%
                TimeDescriptionFixedWidth = TextRenderer.MeasureText(g, "00:00: 00 ", LstLogContent.Font).Width;
            }

            int treeX = timeX + TimeDescriptionFixedWidth;
            int treeNodeDepth = item.FlowTreeNode == null ? 0 : item.FlowTreeNode.Depth;
            int indentPixels = treeNodeDepth * indentPerLevel;
            int textX = treeX + indentPixels - indentPerLevel / 2;

            LogFlowTreeNode currentNode = item.FlowTreeNode;
            for (int i = treeNodeDepth - 1; i >= 0; i--)
            {
                // Draw a vertical line for each parent node if it has older siblings
                if (currentNode.HasOlderSibling)
                {
                    int lineX = treeX + i * indentPerLevel;
                    g.DrawLine(PenForDrawingLines, lineX, e.Bounds.Top, lineX, e.Bounds.Bottom);
                }
                // Draw half a vertical line for the current value, if it does not have a next sibling
                else if (i == treeNodeDepth - 1 && currentNode.IsLastSibling)
                {
                    int lineX = treeX + i * indentPerLevel;
                    g.DrawLine(PenForDrawingLines, lineX, e.Bounds.Top, lineX, e.Bounds.Bottom - (e.Bounds.Bottom - e.Bounds.Top) / 2);
                }
                currentNode = currentNode.Parent;
            }

            // Draw the horizontal line just in front of the value text, indicating its connection in the tree
            if (!item.FlowTreeNode.IsRootNode)
            {
                int lineY = e.Bounds.Top + e.Bounds.Height / 2;
                int lineStartX = treeX + indentPixels - 10;
                g.DrawLine(PenForDrawingLines, lineStartX, lineY, textX, lineY);
            }

            // Prepare fonts and brushes
            Brush textBrush = DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope);

            // Draw TimeDescription
            g.DrawString(item.ContentValue.TimeDescription, LstLogContent.Font, textBrush, timeX, e.Bounds.Top);

            // Truncate description if it doesn’t fit

            string value = item.ContentValue.Value?.Length > 128 ? item.ContentValue.Value[0..127] : item.ContentValue.Value ?? string.Empty;
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
            if (isOutOfScope) return isSelected ? Brushes.Gray : LogScraperBrushes.GrayLogEntriesOutOfScope;
            return logEntryDisplayObject.OriginalLogEntry.IsErrorLogEntry ? Brushes.DarkRed : SystemBrushes.ControlText;
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

            return (LogRange.Begin != null && logEntryDisplayObject.Index < LogRange.Begin.Index) ||
                   (LogRange.End != null && logEntryDisplayObject.Index > LogRange.End.Index);
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
            if (ClearSelectedLogEntryExternallyInProgress) return;
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
            LstLogContent.Items.Clear();
            UpdateDisplayedLogEntries();
            UpdateButtons();

            if (SelectedLogContentProperty != null)
            {
                bool resetText = txtSearch.Text == DefaulSearchtText;
                DefaulSearchtText = string.Format(DefaulSearchtTextFormat, SelectedLogContentProperty.Description.ToLower());
                if (resetText) txtSearch.Text = DefaulSearchtText;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.SuspendDrawing();
            if (LstLogContent.Items.Count > 0) LstLogContent.SelectedIndex = -1;
            LstLogContent.Invalidate();
            this.ResumeDrawing();
        }

        private bool ignoreSelectedItemChanged = false;

        private void LstLogContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreSelectedItemChanged)
            {
                OnSelectedItemChanged(EventArgs.Empty);
            }
        }

        private void LstLogContent_DoubleClick(object sender, EventArgs e)
        {
            if (SelectedContentValue == null) return;
        }

        private bool updateShowTreeInProgress = false;

        private void UpdateButtons()
        {
            if (updateShowTreeInProgress) return;
            updateShowTreeInProgress = true;

            if (SelectedLogContentProperty == null || !SelectedLogContentProperty.IsBeginFlowTreeFilter)
            {
                BtnShowTree.Enabled = false;
                updateShowTreeInProgress = false;
                return;
            }

            BtnShowTree.ImageIndex = showTree ? 1 : 0;
            BtnShowTree.Enabled = true;
            LstLogContent.SuspendDrawing();
            LstLogContent.Invalidate();
            LstLogContent.ResumeDrawing();
            updateShowTreeInProgress = false;
        }

        public void ResetFilters()
        {
            txtSearch.Text = string.Empty;
            TxtSearch_Leave(this, EventArgs.Empty);
            if (ConfigurationManager.GenericConfig.AutoToggleHierarchy) showTree = true;
        }
        #endregion

        private void BtnShowTree_Click(object sender, EventArgs e)
        {
            showTree = !showTree;
            UpdateButtons();
        }
    }
}