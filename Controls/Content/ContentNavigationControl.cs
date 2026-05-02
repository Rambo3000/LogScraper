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
using LogScraper.Log.Rendering;

namespace LogScraper.Controls.Content
{
    //TODO: fix issue where last item in the list is showing up double because the background is not reset
    internal partial class ContentNavigationControl : UserControl
    {
        #region Private objects and initialization
        private const int ViewportAccentBarWidth = 2;
        private const int ViewportAccentBarPadding = -2;

        private const string DefaulSearchtText = "Filter...";

        private bool showTree = false;

        public ContentNavigationControl()
        {
            InitializeComponent();
            txtSearch.PlaceholderText = DefaulSearchtText;
            LogAppState.Instance.Layout.Changed += (s, e) => UpdateLogLayout();
            LogAppState.Instance.FilterResultWithRange.Changed += (s, e) => UpdateDisplayedLogEntries();
            LogAppState.Instance.ResetRequested += (s, e) => Reset();
            LogAppState.Instance.ViewportSelectedLogEntry.Changed += (s, e) => OnViewportSelectedLogEntryChanged();
            LogAppState.Instance.ViewportVisibleRange.Changed += (s, e) => OnViewportVisibleRangeChanged();
            _rangeDebounceTimer.Tick += RangeDebounceTimer_Tick;
        }

        #endregion

        #region Viewport state tracking

        private LogEntry _viewportSelectedLogEntry;
        // Indices into LstLogContent.Items for the viewport range highlight
        private HashSet<int> _rangeBarIndices = [];
        private int _startRangeBarIndex = -1;
        private int _endRangeBarIndex = -1;

        private readonly Timer _rangeDebounceTimer = new() { Interval = 10 };
        private bool _autoScrollEnabled = false;

        private void OnViewportSelectedLogEntryChanged()
        {
            LogEntry newEntry = LogAppState.Instance.ViewportSelectedLogEntry.Value;
            if (newEntry == _viewportSelectedLogEntry) return;

            LogEntry previous = _viewportSelectedLogEntry;
            _viewportSelectedLogEntry = newEntry;

            // Invalidate only the rows that changed underline state
            List<int> dirtyIndices = [];
            for (int i = 0; i < LstLogContent.Items.Count; i++)
            {
                if (LstLogContent.Items[i] is not LogContentDisplayItem item) continue;
                if (item.LogEntry == previous || item.LogEntry == newEntry)
                    dirtyIndices.Add(i);
            }
            LstLogContent.InvalidateItems(dirtyIndices);
        }

        private void OnViewportVisibleRangeChanged()
        {
            // Restart the debounce timer on every scroll event so the redraw
            // only happens once the user pauses, preventing flicker during fast scrolling.
            _rangeDebounceTimer.Stop();
            _rangeDebounceTimer.Start();
        }

        private void RangeDebounceTimer_Tick(object sender, EventArgs e)
        {
            _rangeDebounceTimer.Stop();

            HashSet<int> previousRangeIndices = _rangeBarIndices;
            int previousBefore = _startRangeBarIndex;
            int previousAfter = _endRangeBarIndex;

            ComputeViewportRangeIndices();

            ApplyAutoScroll();

            if (_rangeBarIndices.SetEquals(previousRangeIndices)
                && _startRangeBarIndex == previousBefore
                && _endRangeBarIndex == previousAfter) return;

            // Invalidate only rows that entered or left the range / boundary
            HashSet<int> dirtyIndices = [.. previousRangeIndices];
            dirtyIndices.SymmetricExceptWith(_rangeBarIndices);
            if (previousBefore >= 0) dirtyIndices.Add(previousBefore);
            if (previousAfter >= 0) dirtyIndices.Add(previousAfter);
            if (_startRangeBarIndex >= 0) dirtyIndices.Add(_startRangeBarIndex);
            if (_endRangeBarIndex >= 0) dirtyIndices.Add(_endRangeBarIndex);
            LstLogContent.InvalidateItems(dirtyIndices);
        }

        private void ApplyAutoScroll(bool scrollToCenter = false, bool force = false)
        {
            if (!_autoScrollEnabled && !force) return;
            if (LstLogContent.Items.Count == 0) return;

            //int firstVisible = LstLogContent.TopIndex;
            int visibleCount = (int)Math.Floor((double)LstLogContent.Height / LstLogContent.ItemHeight);
            //int lastVisible = Math.Min(firstVisible + visibleCount, LstLogContent.Items.Count - 1);

            // Use the range boundaries, but ensure they are within the list bounds to avoid issues when the range is outside of the currently loaded items
            //int startIndex = _startRangeBarIndex == -1 ? 0 : _startRangeBarIndex;
            int endIndex = _endRangeBarIndex == -1 ? LstLogContent.Items.Count - 1 : _endRangeBarIndex;

            LstLogContent.TopIndex = Math.Max(0, endIndex - (visibleCount / 2) - 1);
            return;

            // Logic below scrolls differently and let the view be more stable on small changes, keep it here as backup for now

            //if (scrollToCenter)
            //{
            //    LstLogContent.TopIndex = Math.Max(0, endIndex - (visibleCount / 2) - 1);
            //    return;
            //}

            //// Both boundaries are above the visible area — scrolled up, follow the before boundary
            //if (startIndex < firstVisible && endIndex < firstVisible)
            //{
            //    LstLogContent.TopIndex = startIndex;
            //}
            //// Both boundaries are below the visible area — scrolled down, follow the after boundary
            //else if (startIndex > lastVisible && endIndex > lastVisible)
            //{
            //    LstLogContent.TopIndex = endIndex - visibleCount + 1;
            //}
            //// Normal case: scroll to whichever boundary is out of view
            //else if (startIndex < firstVisible)
            //{
            //    LstLogContent.TopIndex = startIndex;
            //}
            //else if (endIndex > lastVisible)
            //{
            //    LstLogContent.TopIndex = endIndex - visibleCount + 1;
            //}
        }

        private void ComputeViewportRangeIndices()
        {
            _rangeBarIndices = [];
            _startRangeBarIndex = -1;
            _endRangeBarIndex = -1;

            LogRange range = LogAppState.Instance.ViewportVisibleRange.Value;
            if (range == null) return;

            int beginIndex = range.Begin?.Index ?? int.MinValue;
            int endIndex = range.End?.Index ?? int.MaxValue;

            int itemCount = LstLogContent.Items.Count;

            // Binary search for the first item with LogEntry.Index >= beginIndex
            int low = 0, high = itemCount - 1, startScanIdx = itemCount;
            while (low <= high)
            {
                int mid = (low + high) / 2;
                if (LstLogContent.Items[mid] is LogContentDisplayItem midItem && midItem.LogEntry.Index >= beginIndex)
                {
                    startScanIdx = mid;
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            // The last item strictly before the range is one position before the scan start
            int lastBeforeIdx = -1;
            for (int i = startScanIdx - 1; i >= 0; i--)
            {
                if (LstLogContent.Items[i] is LogContentDisplayItem prevItem && prevItem.LogEntry.Index < beginIndex)
                {
                    lastBeforeIdx = i;
                    break;
                }
            }

            int firstAfterIdx = -1;

            for (int i = startScanIdx; i < itemCount; i++)
            {
                if (LstLogContent.Items[i] is not LogContentDisplayItem item) continue;
                int idx = item.LogEntry.Index;

                if (idx <= endIndex)
                {
                    _rangeBarIndices.Add(i);
                }
                else
                {
                    firstAfterIdx = i;
                    break;
                }
            }

            _startRangeBarIndex = lastBeforeIdx;
            _endRangeBarIndex = firstAfterIdx;
        }

        #endregion

        #region Update log layout
        private void UpdateLogLayout()
        {
            LogLayout logLayout = LogAppState.Instance.Layout.Value;
            LstLogContent.ItemHeight = LstLogContent.Font.Height;

            CboLogContentType.Items.Clear();
            if (logLayout == null || logLayout.LogContentProperties == null || logLayout.LogContentProperties.Count == 0) return;
            CboLogContentType.Items.AddRange([.. logLayout.LogContentProperties.Where(item => item.IsNavigationEnabled)]);

            if (CboLogContentType.Items.Count > 0) CboLogContentType.SelectedIndex = 0;
            UpdateButtons();
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
                UpdateButtons();
                return;
            }

            List<LogContentDisplayItem> logContentDisplayItems = CreateLogEntryDisplayObjects(logContentProperty, logEntries);

            UpdateDisplayedLogEntriesUsingNewLogEntries(logContentDisplayItems);
            UpdateButtons();
        }

        private List<LogContentDisplayItem> CreateLogEntryDisplayObjects(LogContentProperty logContentProperty, List<LogEntry> logEntries)
        {
            if (logContentProperty == null) return null;

            string filter = txtSearch.Text.Trim();
            if (filter == string.Empty) filter = null;

            bool showErrors = ConfigAppState.Instance.GenericConfig.Value.ShowErrorLinesInBeginAndEndFilters;
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
                LstLogContent.ClearEmptyArea();
                ComputeViewportRangeIndices();
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
                ComputeViewportRangeIndices();
                LstLogContent.ResumeDrawing();
                LstLogContent.ClearEmptyArea();
                return;
            }

            // Iterate through the new log entries and select the previously selected log entry if it exists
            foreach (LogContentDisplayItem logContentDisplayItem in newLogEntries)
            {
                if (logContentDisplayItem.LogEntry == selectedLogEntry.LogEntry)
                {
                    try
                    {
                        ignoreSelectedItemChanged = true;
                        LstLogContent.SelectedItem = logContentDisplayItem;
                    }
                    finally
                    {
                        ignoreSelectedItemChanged = false;
                    }
                    break;
                }
            }
            ComputeViewportRangeIndices();

            LstLogContent.ResumeDrawing();
            LstLogContent.ClearEmptyArea();
        }
        #endregion

        #region Public methods

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
            LstLogContent.Items.Clear();
            ResetFilters();
        }
        #endregion

        #region Draw listbox items

        private void LstLogContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Fetch the item
            if (LstLogContent.Items[e.Index] is not LogContentDisplayItem item || item.ContentValue == null) return;

            Graphics g = e.Graphics;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            bool isViewportSelected = item.LogEntry == _viewportSelectedLogEntry;
            bool isInRange = _rangeBarIndices.Contains(e.Index);
            bool isBoundary = e.Index == _startRangeBarIndex || e.Index == _endRangeBarIndex;

            //Disable being out of scope for now
            //bool isOutOfScope = IslogContentDisplayItemOutOfScope(item);
            bool isOutOfScope = false;

            // Determine background
            if (isOutOfScope && isSelected)
                g.FillRectangle(Brushes.LightGray, e.Bounds);
            else if (isSelected)
                g.FillRectangle(LogScraperBrushes.BlueSelectedLogline, e.Bounds);
            else
                g.FillRectangle(SystemBrushes.Control, e.Bounds);

            // Draw left accent bar for viewport range
            if (isInRange || isBoundary)
            {
                Rectangle accentRect = new(e.Bounds.Left, e.Bounds.Top, ViewportAccentBarWidth, e.Bounds.Height);
                if (isInRange)
                {
                    g.FillRectangle(LogScraperBrushes.ViewportRangeAccent, accentRect);
                }
                else
                {
                    bool isBefore = e.Index == _startRangeBarIndex;
                    var (beforeBrush, afterBrush) = GetBoundaryBrushes(accentRect);
                    g.FillRectangle(isBefore ? beforeBrush : afterBrush, accentRect);
                }
            }
            if (item.LogEntry.IsErrorLogEntry && !SelectedLogContentProperty.IsErrorProperty)
            {
                DrawItemText(g, e, item, item.TimeStamp + " ERROR", isSelected, isOutOfScope, isViewportSelected);
                e.DrawFocusRectangle();
                return;
            }

            if (item.FlowTreeNode != null && showTree)
                DrawFlowTreeNode(e, item, g, isSelected, isOutOfScope, isViewportSelected);
            else
            {
                string value = item.ContentValue.Value?.Length > 128 ? item.ContentValue.Value[0..127] : item.ContentValue.Value ?? string.Empty;
                string truncatedValue = TruncateTextToFit(item.TimeStamp + " " + value, g, e.Bounds.Width);
                DrawItemText(g, e, item, truncatedValue, isSelected, isOutOfScope, isViewportSelected);
            }

            e.DrawFocusRectangle();
        }

        private void DrawItemText(Graphics g, DrawItemEventArgs e, LogContentDisplayItem item, string text, bool isSelected, bool isOutOfScope, bool isViewportSelected)
        {
            Color textColor = DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope);
            Font font = isViewportSelected ? UnderlineFont : LstLogContent.Font;
            int textLeft = e.Bounds.Left + ViewportAccentBarWidth + ViewportAccentBarPadding;
            Rectangle textBounds = new(textLeft, e.Bounds.Top, e.Bounds.Right - textLeft, e.Bounds.Height);
            TextRenderer.DrawText(g, text, font, textBounds, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        private Font _underlineFont;
        private Font UnderlineFont => _underlineFont ??= new Font(LstLogContent.Font, FontStyle.Underline);

        private LinearGradientBrush _boundaryBeforeBrush;
        private LinearGradientBrush _boundaryAfterBrush;
        private int _cachedBoundaryBrushItemHeight = -1;
        private int _cachedBoundaryBrushItemTop = -1;

        private (LinearGradientBrush before, LinearGradientBrush after) GetBoundaryBrushes(Rectangle accentRect)
        {
            if (_cachedBoundaryBrushItemHeight != accentRect.Height || _cachedBoundaryBrushItemTop != accentRect.Top)
            {
                _boundaryBeforeBrush?.Dispose();
                _boundaryAfterBrush?.Dispose();
                Color accentColor = ((SolidBrush)LogScraperBrushes.ViewportRangeAccent).Color;
                Color controlColor = SystemColors.Control;
                _boundaryBeforeBrush = new LinearGradientBrush(accentRect, controlColor, accentColor, LinearGradientMode.Vertical);
                _boundaryAfterBrush = new LinearGradientBrush(accentRect, accentColor, controlColor, LinearGradientMode.Vertical);
                _cachedBoundaryBrushItemHeight = accentRect.Height;
                _cachedBoundaryBrushItemTop = accentRect.Top;
            }
            return (_boundaryBeforeBrush, _boundaryAfterBrush);
        }

        private static readonly Pen PenForDrawingLines = new(Color.Gray) { DashStyle = DashStyle.Dot };

        private int TimeDescriptionFixedWidth = -1;

        private void DrawFlowTreeNode(DrawItemEventArgs e, LogContentDisplayItem item, Graphics g, bool isSelected, bool isOutOfScope, bool isViewportSelected)
        {
            // Indentation
            const int indentPerLevel = 10;
            int timeX = e.Bounds.Left + ViewportAccentBarWidth + ViewportAccentBarPadding;

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

            // Prepare color and font
            Color textColor = DetermineTextColorBasedOnLogEntryPosition(item, isSelected, isOutOfScope);
            Font font = isViewportSelected ? UnderlineFont : LstLogContent.Font;

            // Draw TimeDescription
            Rectangle timeBounds = new(timeX, e.Bounds.Top, TimeDescriptionFixedWidth, e.Bounds.Height);
            TextRenderer.DrawText(g, item.TimeStamp, font, timeBounds, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Truncate description if it doesn’t fit

            string value = item.ContentValue.Value?.Length > 128 ? item.ContentValue.Value[0..127] : item.ContentValue.Value ?? string.Empty;
            string truncatedValue = TruncateTextToFit(value, g, e.Bounds.Right - textX);

            // Draw text
            Rectangle valueBounds = new(textX, e.Bounds.Top, e.Bounds.Right - textX, e.Bounds.Height);
            TextRenderer.DrawText(g, truncatedValue, font, valueBounds, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
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

        private static Color DetermineTextColorBasedOnLogEntryPosition(LogContentDisplayItem logContentDisplayItem, bool isSelected, bool isOutOfScope)
        {
            if (isOutOfScope) return isSelected ? Color.Gray : Color.FromArgb(200, 200, 200);
            return logContentDisplayItem.LogEntry.IsErrorLogEntry ? Color.DarkRed : SystemColors.ControlText;
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
            ApplyAutoScroll(true, true);
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
                LogAppState.Instance.ViewportSelectedLogEntry.Set(SelectedLogEntry);
            }
        }

        private void UpdateButtons()
        {
            BtnAutoScroll.ImageIndex = _autoScrollEnabled ? 1 : 0;
            BtnJumpToLogPosition.Enabled = LstLogContent.Items.Count > 1;

            BtnShowTree.Enabled = SelectedLogContentProperty?.IsBeginFlowTreeFilter ?? false;

            BtnShowTree.ImageIndex = showTree ? 1 : 0;
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

            LstLogContent.SuspendDrawing();
            LstLogContent.Invalidate();
            LstLogContent.ResumeDrawing();
        }

        private void BtnAutoScroll_Click(object sender, EventArgs e)
        {
            _autoScrollEnabled = !_autoScrollEnabled;
            ApplyAutoScroll(true);
            UpdateButtons();
        }

        private void BtnJumpToLogPosition_Click(object sender, EventArgs e)
        {
            ApplyAutoScroll(true, true);
        }
    }
}