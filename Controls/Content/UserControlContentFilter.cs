using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Utilities;
using LogScraper.Controls.Generic;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Content;
using LogScraper.Log;

namespace LogScraper.Controls.Content
{
    internal partial class UserControlLogContentFilter : UserControl
    {
        #region Private objects and initialization
        private const string DefaulSearchtText = "Filter...";

        private bool showTree = false;

        public UserControlLogContentFilter()
        {
            InitializeComponent();
            txtSearch.PlaceholderText = DefaulSearchtText;
        }

        private void UserControlLogContentFilter_Load(object sender, EventArgs e)
        {
            LogAppState.Instance.Layout.Changed += (s, e) => UpdateLogLayout();
            LogAppState.Instance.FilterResultWithRange.Changed += (s, e) => UpdateDisplayedLogEntries();
            LogAppState.Instance.ResetRequested += (s, e) => Reset();
        }

        #endregion

        #region Update log layout
        //TODO: REQUIRED remove
        private void UpdateLogLayout()
        {
            LogLayout logLayout = LogAppState.Instance.Layout.Value;
            LstLogContent.ItemHeight = LstLogContent.Font.Height;

            CboLogContentType.Items.Clear();
            if (logLayout == null || logLayout.LogContentProperties == null || logLayout.LogContentProperties.Count == 0) return;
            CboLogContentType.Items.AddRange([.. logLayout.LogContentProperties.Where(item => item.IsNavigationEnabled)]);

            if (CboLogContentType.Items.Count > 0) CboLogContentType.SelectedIndex = 0;
        }
        #endregion

        #region Update the log entries in the listbox

        private void UpdateDisplayedLogEntries()
        {
            List<LogEntry> logEntries = LogAppState.Instance.FilterResultWithRange.Value?.LogEntries ?? null;
            LogContentProperty logContentProperty = SelectedLogContentProperty;
            if (logEntries == null || logContentProperty == null)
            {
                LstLogContent.Items.Clear();
                return;
            }

            List<LogContentDisplayItem> logContentDisplayItems = CreateLogEntryDisplayObjects(logContentProperty, logEntries);

            UpdateDisplayedLogEntriesUsingNewLogEntries(logContentDisplayItems);
        }

        private List<LogContentDisplayItem> CreateLogEntryDisplayObjects(LogContentProperty logContentProperty, List<LogEntry> logEntries)
        {
            if (logContentProperty == null) return null;

            string filter = txtSearch.Text.Trim();
            if (filter == string.Empty) filter = null;

            bool showErrors = ConfigurationManager.GenericConfig.ShowErrorLinesInBeginAndEndFilters;
            bool filterEnabled = !string.IsNullOrEmpty(filter);

            List<LogContentDisplayItem> logContentDisplayItems = [];

            foreach (LogEntry logEntry in logEntries)
            {
                if (logEntry.LogContentProperties == null) continue;

                // Try to get the content for the selected log content property
                logEntry.LogContentProperties.TryGetValue(logContentProperty, out LogContentValue contentValue);

                if (contentValue == null)
                {
                    if (!showErrors || !logEntry.IsErrorLogEntry) continue;

                    //If the log entry has an error content property, get the content value and continue using this value
                    if (LogAppState.Instance.FilterResultWithRange.Value.ErrorMask[logEntry.Index])
                    {
                        contentValue = new("ERROR");
                    }
                }

                if (filterEnabled && contentValue != null)
                {
                    if (!contentValue.Value.Contains(filter, StringComparison.OrdinalIgnoreCase)) continue;
                }

                LogFlowTree logFlowTree = LogAppState.Instance.MetadataFilterResult.Value.LogFlowTrees[logContentProperty];
                LogFlowTreeNode flowtreeNode = null;
                logFlowTree?.LogEntryDictionary?.TryGetValue(logEntry, out flowtreeNode);

                logContentDisplayItems.Add(new LogContentDisplayItem(logEntry.TimeStamp.ToString("HH:mm:ss"), logEntry, contentValue, flowtreeNode));
            }
            return logContentDisplayItems;
        }

        private void UpdateDisplayedLogEntriesUsingNewLogEntries(List<LogContentDisplayItem> newLogEntries)
        {
            int currentCount = LstLogContent.Items.Count;
            int newCount = newLogEntries.Count;
            int compareCount = Math.Min(currentCount, newCount);
            bool startMatches = true;

            //Check if all items up to the count of the smaller list match
            for (int i = 0; i < compareCount; i++)
            {
                if (!newLogEntries[i].Equals((LogContentDisplayItem)LstLogContent.Items[i]))
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
                    if (item is not LogContentDisplayItem logContentDisplayItem) continue;
                    // Use the first item in the Linq expression as cases have been found where more than one log entry matches
                    logContentDisplayItem.FlowTreeNode = newLogEntries.Where(entry => entry.LogEntry.Equals(logContentDisplayItem.LogEntry)).First().FlowTreeNode;
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

        private void FullyRedrawList(List<LogContentDisplayItem> newLogEntries)
        {
            // Store the currently selected log entry
            LogContentDisplayItem selectedLogEntry = (LogContentDisplayItem)LstLogContent.SelectedItem;
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
            foreach (LogContentDisplayItem logContentDisplayItem in newLogEntries)
            {
                if (logContentDisplayItem.LogEntry == selectedLogEntry.LogEntry)
                {
                    try
                    {
                        ignoreSelectedItemChanged = true;
                        LstLogContent.SelectedItem = logContentDisplayItem;
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
            get => SelectedLogEntryDisplayObject?.LogEntry;
        }

        public LogContentValue SelectedContentValue
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogContentDisplayItem)LstLogContent.SelectedItem).ContentValue;
            }
        }


        private LogContentDisplayItem SelectedLogEntryDisplayObject
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return (LogContentDisplayItem)LstLogContent.SelectedItem;
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
            if (CboLogContentType.Items.Count > 0) CboLogContentType.SelectedIndex = 0;
            ClearSelectedLogEntry();
            ResetFilters();
        }
        #endregion

        #region Draw listbox items
        private void LstLogContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            // Fetch the item
            if (LstLogContent.Items[e.Index] is not LogContentDisplayItem item || item.ContentValue == null) return;

            Graphics g = e.Graphics;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            //Disable being out of scope for now
            //bool isOutOfScope = IslogContentDisplayItemOutOfScope(item);
            bool isOutOfScope = false;

            if (isOutOfScope && isSelected) g.FillRectangle(Brushes.LightGray, e.Bounds);
            else if (isSelected) g.FillRectangle(LogScraperBrushes.BlueSelectedLogline, e.Bounds);
            else g.FillRectangle(SystemBrushes.Control, e.Bounds);

            if (item.LogEntry.IsErrorLogEntry && !SelectedLogContentProperty.IsErrorProperty)
            {
                e.Graphics.DrawString(item.TimeStamp + " ERROR", LstLogContent.Font, DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope), e.Bounds);
                e.DrawFocusRectangle();
                return;
            }

            if (item.FlowTreeNode != null && showTree)
                DrawFlowTreeNode(e, item, g, isSelected, isOutOfScope);
            else
            {
                string value = item.ContentValue.Value?.Length > 128 ? item.ContentValue.Value[0..127] : item.ContentValue.Value ?? string.Empty;
                string truncatedValue = TruncateTextToFit(item.TimeStamp + " " + value, g, e.Bounds.Width);
                e.Graphics.DrawString(truncatedValue, LstLogContent.Font, DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope), e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        private static readonly Pen PenForDrawingLines = new(Color.Gray) { DashStyle = DashStyle.Dot };

        private int TimeDescriptionFixedWidth = -1;

        private void DrawFlowTreeNode(DrawItemEventArgs e, LogContentDisplayItem item, Graphics g, bool isSelected, bool isOutOfScope)
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
            g.DrawString(item.TimeStamp, LstLogContent.Font, textBrush, timeX, e.Bounds.Top);

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

        private static Brush DetermineTextColorBasedOnLogEntryPosition(LogContentDisplayItem logContentDisplayItem, bool isSelected, bool isOutOfScope)
        {
            if (isOutOfScope) return isSelected ? Brushes.Gray : LogScraperBrushes.GrayLogEntriesOutOfScope;
            return logContentDisplayItem.LogEntry.IsErrorLogEntry ? Brushes.DarkRed : SystemBrushes.ControlText;
        }
        #endregion

        #region Search
        private string lastSearch = string.Empty;

        private void PerformSearch()
        {
            string searchString = txtSearch.Text.Trim();
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

        private void CboLogContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LstLogContent.Items.Clear();
            UpdateDisplayedLogEntries();
            UpdateButtons();
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
        }
        #endregion

        private void BtnShowTree_Click(object sender, EventArgs e)
        {
            showTree = !showTree;
            UpdateButtons();
        }

    }
}