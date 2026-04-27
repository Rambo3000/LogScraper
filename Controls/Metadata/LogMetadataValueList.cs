using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LogScraper.Controls.Generic;
using LogScraper.Log;
using LogScraper.Log.Filtering;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Metadata;

namespace LogScraper.Controls.Metadata
{
    /// <summary>
    /// Encapsulates the virtual ListView displaying metadata values with checkboxes.
    /// Manages checked state, filter mode, counts, owner draw, and mouse interaction.
    /// Sizing (height) is controlled externally by the parent UserControlLogMetadataFilter.
    /// </summary>
    public partial class LogMetadataValueList : UserControl
    {
        #region Events

        public event EventHandler FilterChanged;
        public event EventHandler CheckedItemsChanged;

        #endregion

        #region Properties

        public int SortedValueCount => sortedValues.Count;
        public int CheckedItemCount => checkedItems.Count;
        public FilterMode CurrentFilterMode => filterModeForAllCheckedItems;

        private bool IsScrollableViewEnabled => sortedValues.Count > MAX_NUMBER_OF_ITEMS_BEFORE_SCROLL;

        #endregion

        #region Private Fields

        internal const int MAX_NUMBER_OF_ITEMS_BEFORE_SCROLL = 50;
        internal const int SCROLL_VIEW_NUMBER_OF_ITEMS_SHOWN = 15;

        private LogMetadataProperty metadataProperty;
        private List<LogMetadataValue> sortedValues = [];
        private readonly HashSet<LogMetadataValue> checkedItems = [];
        private FilterMode filterModeForAllCheckedItems = FilterMode.Include;
        private readonly Dictionary<LogMetadataValue, int> valueCounts = [];

        private bool updateInProgress = false;
        private Point savedScrollPosition = Point.Empty;
        private LogEntry selectedLogEntry = null;
        private LogMetadataValue selectedEntryValue = null;

        #endregion

        #region Constructor

        public LogMetadataValueList()
        {
            InitializeComponent();
            ListViewItems.Columns.Add("Description", -2);
            ListViewItems.Columns.Add("Count", 50, HorizontalAlignment.Right);
        }
        private void LogMetadataValueList_Load(object sender, EventArgs e)
        {
            LogAppState.Instance.ViewportSelectedLogEntry.Changed += (s, e) => UpdateSelectedEntry();
        }

        #endregion

        #region Column Sizing

        /// <summary>
        /// Dynamically adjusts the count column width to fit the largest number.
        /// </summary>
        public void AdjustCountColumnWidth()
        {
            if (valueCounts.Count == 0) return;

            int maxCount = valueCounts.Values.DefaultIfEmpty(0).Max();
            int textWidth = TextRenderer.MeasureText(maxCount.ToString("N0"), ListViewItems.Font).Width;
            int newWidth = textWidth + ScaleByDpi(10);

            if (ListViewItems.Columns.Count > 1)
            {
                ListViewItems.Columns[1].Width = newWidth;
                ListViewItems.Columns[0].Width = ListViewItems.ClientSize.Width - newWidth - 1;
            }
        }

        private void ListView_Resize(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.Columns.Count < 2) return;
            if (listView.IsHandleCreated)
                BeginInvoke((MethodInvoker)(() =>
                {
                    if (listView.IsDisposed || !listView.IsHandleCreated) return;
                    listView.Columns[0].Width = listView.ClientSize.Width - listView.Columns[1].Width - 1;
                }));
            else
                listView.Columns[0].Width = listView.ClientSize.Width - listView.Columns[1].Width - 1;
        }

        #endregion

        #region Data Updates

        /// <summary>
        /// Updates the ListView with all known values for this property.
        /// Preserves existing checked states.
        /// Returns true when the value list changed (caller may need to resize).
        /// </summary>
        public bool UpdateValues(LogMetadataProperty property, List<LogMetadataValue> allValues)
        {
            bool isFirstLoad = metadataProperty == null;
            metadataProperty = property;

            bool valuesChanged =
                sortedValues.Count != allValues.Count ||
                !allValues.All(sortedValues.Contains);

            if (!valuesChanged)
            {
                UpdateCounts(null);
                return false;
            }

            updateInProgress = true;

            HashSet<LogMetadataValue> previousChecked = [.. checkedItems];
            checkedItems.Clear();

            sortedValues = [.. allValues.OrderBy(v => v.Value)];

            foreach (LogMetadataValue value in sortedValues)
            {
                if (previousChecked.Contains(value))
                    checkedItems.Add(value);
            }

            BuildValueCounts(null);

            ListViewItems.VirtualListSize = sortedValues.Count;
            AdjustCountColumnWidth();

            updateInProgress = false;
            return !isFirstLoad || !property.IsCollapsedByDefault;
        }

        /// <summary>
        /// Sets all counts to zero without rebuilding the value list.
        /// </summary>
        public void UpdateCountsToZero() => UpdateCounts(null);

        /// <summary>
        /// Lightweight update that only refreshes counts without rebuilding the value list.
        /// </summary>
        public void UpdateCounts(LogMetadataFilterStats stats)
        {
            int topItemIndex = ListViewItems.TopItem?.Index ?? 0;

            ListViewItems.SuspendDrawing();
            ListViewItems.BeginUpdate();

            BuildValueCounts(stats);
            AdjustCountColumnWidth();

            ListViewItems.EndUpdate();
            ListViewItems.ResumeDrawing();

            if (topItemIndex < ListViewItems.VirtualListSize && ListViewItems.VirtualListSize > 0)
                ListViewItems.EnsureVisible(topItemIndex);
        }

        private void UpdateSelectedEntry()
        {
            LogEntry newSelectedLogEntry = LogAppState.Instance.ViewportSelectedLogEntry.Value;
            if (selectedLogEntry != null && selectedLogEntry.Equals(newSelectedLogEntry)) return;
            selectedLogEntry = newSelectedLogEntry;

            LogMetadataValue newSelectedValue = GetSelectedEntryValueForProperty();
            if (newSelectedValue != selectedEntryValue)
            {
                selectedEntryValue = newSelectedValue;
                ListViewItems.Invalidate();
                if (IsScrollableViewEnabled) ScrollToSelectedValue();
            }
        }

        private void BuildValueCounts(LogMetadataFilterStats stats)
        {
            valueCounts.Clear();
            if (stats == null)
            {
                foreach (LogMetadataValue value in sortedValues)
                    valueCounts[value] = 0;
                return;
            }

            foreach (LogMetadataValueCount valueCount in stats.ValueCounts)
                valueCounts[valueCount.Value] = valueCount.Count;
        }

        #endregion

        #region Scroll / Visibility

        public void EnsureFirstItemVisible()
        {
            if (ListViewItems.IsHandleCreated && ListViewItems.VirtualListSize > 0)
                ListViewItems.EnsureVisible(0);
        }

        public void RedrawList(bool scrollToTop = false)
        {
            if (!IsHandleCreated) return;

            BeginInvoke((MethodInvoker)(() =>
            {
                if (IsDisposed || !ListViewItems.IsHandleCreated) return;

                if (scrollToTop && ListViewItems.VirtualListSize > 0)
                    ListViewItems.EnsureVisible(0);

                ListViewItems.Invalidate();
                ListViewItems.Update();
                Invalidate();
                Update();
            }));
        }

        private void ScrollToSelectedValue()
        {
            if (selectedEntryValue == null) return;
            int index = sortedValues.FindIndex(v => v.Equals(selectedEntryValue));
            if (index < 0) return;

            int visibleRows = ListViewItems.ClientSize.Height / (ListViewItems.Items.Count > 0 ? ListViewItems.GetItemRect(0).Height : 20);
            int centeredIndex = Math.Max(0, Math.Min(index - visibleRows / 2, ListViewItems.VirtualListSize - visibleRows));
            ListViewItems.TopItem = ListViewItems.Items[centeredIndex];
        }

        #endregion

        #region Virtual ListView Implementation

        private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= sortedValues.Count) return;

            LogMetadataValue value = sortedValues[e.ItemIndex];
            int count = valueCounts.TryGetValue(value, out int c) ? c : 0;

            ListViewItem item = new(value.Value)
            {
                ForeColor = count == 0 ? Color.Gray : Color.Black,
                Tag = value
            };

            item.SubItems.Add(count.ToString("N0"));
            e.Item = item;
        }

        #endregion

        #region Owner Draw Implementation

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) => e.DrawDefault = true;

        private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e) => e.DrawDefault = false;

        private void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.Item.Tag is not LogMetadataValue value) return;

            bool isChecked = checkedItems.Contains(value);
            bool isExclude = isChecked && filterModeForAllCheckedItems == FilterMode.Exclude;
            bool isSelectedLine = selectedEntryValue != null && selectedEntryValue.Equals(value);
            int count = valueCounts.TryGetValue(value, out int c) ? c : 0;
            Rectangle bounds = e.Bounds;

            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();

                int checkBoxPadding = ScaleByDpi(3);
                CheckBoxState state = isChecked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);

                if (isSelectedLine)
                {
                    int size = ScaleByDpi(4);
                    int cx = bounds.Left + size / 2;
                    int cy = bounds.Top + bounds.Height / 2;
                    Point[] triangle = [new(cx, cy - size), new(cx + size, cy), new(cx, cy + size)];
                    using SolidBrush brush = new(Color.FromArgb(150, 150, 150));
                    e.Graphics.FillPolygon(brush, triangle);
                }

                Point checkBoxLocation = new(
                    bounds.Left + 9,
                    bounds.Top + (bounds.Height - checkBoxSize.Height) / 2);

                CheckBoxRenderer.DrawCheckBox(e.Graphics, checkBoxLocation, state);

                Color textColor = e.Item.ForeColor;

                Rectangle textBounds = new(
                    checkBoxLocation.X + checkBoxSize.Width + checkBoxPadding,
                    bounds.Top,
                    bounds.Width - checkBoxLocation.X - checkBoxSize.Width - checkBoxPadding,
                    bounds.Height);

                TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter
                    | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;

                TextRenderer.DrawText(e.Graphics, value.Value, e.Item.Font, textBounds, textColor, flags);

                if (isExclude)
                {
                    Size textSize = TextRenderer.MeasureText(e.Graphics, value.Value, e.Item.Font, textBounds.Size, flags);
                    int strikeY = textBounds.Top + textBounds.Height / 2;
                    int strikeWidth = Math.Min(textSize.Width, textBounds.Width);
                    using Pen pen = new(textColor);
                    e.Graphics.DrawLine(pen, textBounds.Left, strikeY, textBounds.Left + strikeWidth, strikeY);
                }
            }
            else if (e.ColumnIndex == 1)
            {
                e.DrawBackground();

                Rectangle textBounds = new(bounds.Left, bounds.Top, bounds.Width - ScaleByDpi(5), bounds.Height);
                TextRenderer.DrawText(e.Graphics, count.ToString("N0"), e.Item.Font, textBounds, e.Item.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }
        }

        private LogMetadataValue GetSelectedEntryValueForProperty()
        {
            if (selectedLogEntry == null || metadataProperty == null) return null;
            return selectedLogEntry.Metadata.TryGetValue(metadataProperty, out LogMetadataValue value) ? value : null;
        }

        #endregion

        #region Mouse Interaction

        private void ListView_MouseDown(object sender, MouseEventArgs e)
        {
            ScrollableControl parentScroll = FindScrollableParent();
            if (parentScroll != null)
                savedScrollPosition = parentScroll.AutoScrollPosition;
        }

        private void ListView_MouseClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            ListViewItem item = listView.GetItemAt(e.X, e.Y);
            if (item == null) return;

            ListViewItem.ListViewSubItem subItem = item.GetSubItemAt(e.X, e.Y);
            if (subItem == null) return;

            if (e.Button == MouseButtons.Left && item.SubItems.IndexOf(subItem) == 0)
                ToggleCheckbox(item, listView);

            ScrollableControl parentScroll = FindScrollableParent();
            if (parentScroll != null && savedScrollPosition != Point.Empty)
            {
                parentScroll.AutoScrollPosition = new Point(
                    Math.Abs(savedScrollPosition.X),
                    Math.Abs(savedScrollPosition.Y));
                savedScrollPosition = Point.Empty;
            }
        }

        private void ToggleCheckbox(ListViewItem item, ListView listView)
        {
            if (item.Tag is not LogMetadataValue value) return;

            bool wasEmpty = checkedItems.Count == 0;

            if (!checkedItems.Remove(value))
                checkedItems.Add(value);

            bool isNowEmpty = checkedItems.Count == 0;

            if (wasEmpty != isNowEmpty) CheckedItemsChanged?.Invoke(this, EventArgs.Empty);

            listView.Invalidate(item.Bounds);
            OnFilterChanged(EventArgs.Empty);
        }

        private void ListViewItems_DoubleClick(object sender, EventArgs e)
        {
            Point cursorPosition = ListViewItems.PointToClient(Cursor.Position);
            ListViewItem item = ListViewItems.GetItemAt(cursorPosition.X, cursorPosition.Y);
            if (item == null) return;

            ListViewItem.ListViewSubItem subItem = item.GetSubItemAt(cursorPosition.X, cursorPosition.Y);
            if (subItem == null) return;

            if (item.SubItems.IndexOf(subItem) == 0)
                ToggleCheckbox(item, ListViewItems);
        }

        private void ListView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs handledArgs)
                handledArgs.Handled = true;

            Control parent = Parent;
            while (parent != null)
            {
                if (parent is ScrollableControl scrollable && scrollable.AutoScroll)
                {
                    int scrollAmount = SystemInformation.MouseWheelScrollLines * e.Delta / 120 * -20;
                    scrollable.AutoScrollPosition = new Point(
                        -scrollable.AutoScrollPosition.X,
                        -scrollable.AutoScrollPosition.Y + scrollAmount);
                    break;
                }
                parent = parent.Parent;
            }
        }

        #endregion

        #region Scroll Management

        protected override Point ScrollToControl(Control activeControl) => AutoScrollPosition;

        private ScrollableControl FindScrollableParent()
        {
            Control parent = Parent;
            while (parent != null)
            {
                if (parent is ScrollableControl scrollable && scrollable.AutoScroll) return scrollable;
                parent = parent.Parent;
            }
            return null;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Returns a MetadataFilter representing the current checked state.
        /// All checked values share filterModeForAllCheckedItems.
        /// </summary>
        public LogMetadataFilter GetCurrentFilter()
        {
            LogMetadataFilter filter = new(metadataProperty);
            foreach (LogMetadataValue value in checkedItems)
                filter.ActiveValues[value] = filterModeForAllCheckedItems;
            return filter;
        }

        public void ToggleFilterMode()
        {
            filterModeForAllCheckedItems = filterModeForAllCheckedItems == FilterMode.Include
                ? FilterMode.Exclude
                : FilterMode.Include;

            ListViewItems.Invalidate();
            OnFilterChanged(EventArgs.Empty);
        }

        public void InvalidateList() => ListViewItems.Invalidate();

        public void ResetFilters()
        {
            checkedItems.Clear();
            filterModeForAllCheckedItems = FilterMode.Include;
            ListViewItems.Invalidate();
            selectedEntryValue = null;
            selectedLogEntry = null;
            OnFilterChanged(EventArgs.Empty);
        }

        public void DeselectValue(LogMetadataValue value)
        {
            if (!checkedItems.Remove(value)) return;
            if (checkedItems.Count == 0) CheckedItemsChanged?.Invoke(this, EventArgs.Empty);
            ListViewItems.Invalidate();
            OnFilterChanged(EventArgs.Empty);
        }

        public void DeselectAllValues()
        {
            if (checkedItems.Count == 0) return;
            checkedItems.Clear();
            CheckedItemsChanged?.Invoke(this, EventArgs.Empty);
            ListViewItems.Invalidate();
            OnFilterChanged(EventArgs.Empty);
        }

        #endregion

        #region Event Helpers

        protected virtual void OnFilterChanged(EventArgs e)
        {
            if (!updateInProgress) FilterChanged?.Invoke(this, e);
        }

        #endregion

        #region Utilities

        private int ScaleByDpi(int logicalPixels)
        {
            return (int)Math.Ceiling(logicalPixels * DeviceDpi / 96f);
        }

        #endregion
    }
}

