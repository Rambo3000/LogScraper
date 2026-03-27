using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LogScraper.Log;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.Extensions;

namespace LogScraper
{
    /// <summary>
    /// A user control that displays a filterable list of metadata values with checkboxes.
    /// Uses virtual ListView to handle thousands of items efficiently without creating excessive window handles.
    /// All checked values share a single filter mode (Include or Exclude), toggled via the header.
    /// </summary>
    public partial class UserControlLogMetadataFilter : UserControl
    {
        #region Properties and Events

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Collapsed { get; set; }

        public event EventHandler FilterChanged;
        public event EventHandler CollapseChanged;

        /// <summary>
        /// The currently selected log entry. Set to null to deselect.
        /// Updates the indicator in the list to show which value matches the selected line.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogEntry SelectedLogEntry
        {
            get => selectedLogEntry;
            set
            {
                if (selectedLogEntry == value) return;
                selectedLogEntry = value;
                LogMetadataValue newSelectedValue = GetSelectedEntryValueForProperty();
                if (newSelectedValue != selectedEntryValue)
                {
                    selectedEntryValue = newSelectedValue;
                    ListViewItems.Invalidate();
                    if (IsScrollableViewEnabled) ScrollToSelectedValue();
                }
            }
        }

        private void ScrollToSelectedValue()
        {
            if (selectedEntryValue == null) return;
            int index = sortedValues.FindIndex(v => v == selectedEntryValue);
            if (index < 0) return;

            int visibleRows = ListViewItems.ClientSize.Height / (ListViewItems.Items.Count > 0 ? ListViewItems.GetItemRect(0).Height : 20);
            int centeredIndex = Math.Max(0, Math.Min(index - visibleRows / 2, ListViewItems.VirtualListSize - visibleRows));
            ListViewItems.TopItem = ListViewItems.Items[centeredIndex];
        }

        private bool IsScrollableViewEnabled => sortedValues.Count > MAX_NUMBER_OF_ITEMS_BEFORE_SCOLL;

        #endregion

        #region Private Fields

        private const int MAX_NUMBER_OF_ITEMS_BEFORE_SCOLL = 50;
        private const int SCROLL_VIEW_NUMBER_OF_ITEMS_SHOWN = 15;

        /// <summary>
        /// The metadata property this control represents.
        /// </summary>
        private LogMetadataProperty metadataProperty;

        /// <summary>
        /// Sorted list of all known values for this property, used for virtual ListView rendering.
        /// </summary>
        private List<LogMetadataValue> sortedValues = [];

        /// <summary>
        /// Checked values. All share the single filterModeForAllCheckedItems.
        /// </summary>
        private readonly HashSet<LogMetadataValue> checkedItems = [];

        /// <summary>
        /// The filter mode applied to all checked values. Toggled via the header toggle.
        /// Only relevant when at least one item is checked.
        /// </summary>
        private FilterMode filterModeForAllCheckedItems = FilterMode.Include;

        /// <summary>
        /// Lookup from value string to count, updated after each filter pass.
        /// </summary>
        private readonly Dictionary<LogMetadataValue, int> valueCounts = [];

        private bool updateInProgress = false;
        private Point savedScrollPosition = Point.Empty;
        private LogEntry selectedLogEntry = null;
        private LogMetadataValue selectedEntryValue = null;

        /// <summary>
        /// Cached bounds of the IN|EX toggle, updated during label paint.
        /// Used for hit-testing on mouse click.
        /// </summary>
        private Rectangle toggleBounds = Rectangle.Empty;

        #endregion

        #region Constructor

        public UserControlLogMetadataFilter(string description)
        {
            InitializeComponent();
            Collapsed = false;
            LblLogFilterDescription.Text = description;
        }

        #endregion

        #region Layout and Sizing

        protected override void OnLayout(LayoutEventArgs layoutEventArgs)
        {
            base.OnLayout(layoutEventArgs);
            UpdateHeightFromFont();
        }

        /// <summary>
        /// Positions the ListView below the description label based on font size.
        /// </summary>
        private void UpdateHeightFromFont()
        {
            int textHeight = TextRenderer.MeasureText(LblLogFilterDescription.Text, LblLogFilterDescription.Font).Height;
            int desiredTopPosition = textHeight + 1;
            if (ListViewItems.Top != desiredTopPosition)
                ListViewItems.Top = desiredTopPosition;
        }

        /// <summary>
        /// Adjusts the control's height based on the number of items.
        /// </summary>
        private void ResizeVertically()
        {
            if (sortedValues.Count == 0)
            {
                ListViewItems.Height = 0;
                Height = ListViewItems.Top + Padding.Bottom;
                return;
            }

            int itemHeight = TextRenderer.MeasureText("Test", ListViewItems.Font).Height + ScaleByDpi(4);
            int totalHeight = sortedValues.Count * itemHeight;
            int maxHeight = MAX_NUMBER_OF_ITEMS_BEFORE_SCOLL * itemHeight;
            int actualHeight = totalHeight > maxHeight ? SCROLL_VIEW_NUMBER_OF_ITEMS_SHOWN * itemHeight : totalHeight;
            int newHeight = ListViewItems.Top + actualHeight + Padding.Bottom;

            if (Height != newHeight) Height = newHeight;
            if (ListViewItems.Height != actualHeight) ListViewItems.Height = actualHeight;
        }

        /// <summary>
        /// Dynamically adjusts the count column width to fit the largest number.
        /// </summary>
        private void AdjustCountColumnWidth()
        {
            if (valueCounts.Count == 0) return;

            int maxCount = valueCounts.Values.DefaultIfEmpty(0).Max();
            int textWidth = TextRenderer.MeasureText(maxCount.ToString("N0"), ListViewItems.Font).Width;
            int newWidth = textWidth + ScaleByDpi(10);

            if (ListViewItems.Columns.Count > 0 && ListViewItems.Columns[1].Width != newWidth)
            {
                ListViewItems.Columns[1].Width = newWidth;
                ListViewItems.Columns[0].Width = ListViewItems.ClientSize.Width - newWidth - 4;
            }
        }

        private void ListView_Resize(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.Columns.Count > 0)
                listView.Columns[0].Width = listView.ClientSize.Width - listView.Columns[1].Width - 4;
        }

        #endregion

        #region Header Toggle

        /// <summary>
        /// Draws the IN | EX toggle right-aligned.
        /// Only visible when at least one value is checked.
        /// The active segment is drawn in normal text color; the inactive one is grayed.
        /// </summary>
        private void LblIncludeExclude_Paint(object sender, PaintEventArgs e)
        {
            if (checkedItems.Count == 0)
            {
                toggleBounds = Rectangle.Empty;
                return;
            }

            Font font = LblIncludeExclude.Font;
            Graphics graphics = e.Graphics;

            string textIn = "in";
            string textSeparator = "|";
            string textEx = "ex";

            Size sizeIn = TextRenderer.MeasureText(graphics, textIn, font, Size.Empty, TextFormatFlags.NoPadding);
            Size sizeSeparator = TextRenderer.MeasureText(graphics, textSeparator, font, Size.Empty, TextFormatFlags.NoPadding);
            Size sizeEx = TextRenderer.MeasureText(graphics, textEx, font, Size.Empty, TextFormatFlags.NoPadding);

            int totalWidth = sizeIn.Width + sizeSeparator.Width + sizeEx.Width;
            int labelHeight = LblIncludeExclude.ClientSize.Height;
            int x = LblIncludeExclude.ClientSize.Width - totalWidth - ScaleByDpi(2);
            int y = (labelHeight - sizeIn.Height) / 2;

            toggleBounds = new Rectangle(x, 0, totalWidth, labelHeight);

            Color activeColor = LblIncludeExclude.ForeColor;
            Color inactiveColor = Color.Silver;

            Color colorIn = filterModeForAllCheckedItems == FilterMode.Include ? activeColor : inactiveColor;
            Color colorSeparator = inactiveColor;
            Color colorEx = filterModeForAllCheckedItems == FilterMode.Exclude ? activeColor : inactiveColor;

            TextRenderer.DrawText(graphics, textIn, font, new Point(x, y), colorIn, TextFormatFlags.NoPadding);
            x += sizeIn.Width;
            TextRenderer.DrawText(graphics, textSeparator, font, new Point(x, y), colorSeparator, TextFormatFlags.NoPadding);
            x += sizeSeparator.Width;
            TextRenderer.DrawText(graphics, textEx, font, new Point(x, y), colorEx, TextFormatFlags.NoPadding);
        }

        /// <summary>
        /// Handles clicks on the description label. Toggles the filter mode when the IN|EX toggle is hit.
        /// </summary>
        private void LblIncludeExclude_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkedItems.Count == 0) return;
            if (!toggleBounds.Contains(e.Location)) return;

            filterModeForAllCheckedItems = filterModeForAllCheckedItems == FilterMode.Include
                ? FilterMode.Exclude
                : FilterMode.Include;

            LblIncludeExclude.Invalidate();
            ListViewItems.Invalidate();
            OnFilterChanged(EventArgs.Empty);
        }

        #endregion

        #region Data Updates

        /// <summary>
        /// Updates the ListView with all known values for this property.
        /// Preserves existing checked states. If the set of values has not changed, only updates counts.
        /// </summary>
        /// <param name="allValues">All known values for this property from LogCollection.MetadataValues.</param>
        /// <param name="stats">Current counts per value after filtering. Pass null to show full counts.</param>
        public void UpdateListView(LogMetadataProperty property, List<LogMetadataValue> allValues, LogMetadataFilterStats stats)
        {
            metadataProperty = property;

            bool valuesChanged =
                sortedValues.Count != allValues.Count ||
                !allValues.All(sortedValues.Contains);

            if (!valuesChanged)
            {
                UpdateCounts(stats);
                return;
            }

            updateInProgress = true;

            // Preserve checked states for values that still exist.
            HashSet<LogMetadataValue> previousChecked = [.. checkedItems];
            checkedItems.Clear();

            sortedValues = [.. allValues.OrderBy(v => v.Value)];

            foreach (LogMetadataValue value in sortedValues)
            {
                if (previousChecked.Contains(value))
                    checkedItems.Add(value);
            }

            BuildValueCounts(stats);

            ListViewItems.VirtualListSize = sortedValues.Count;
            AdjustCountColumnWidth();
            ResizeVertically();

            updateInProgress = false;
        }

        /// <summary>
        /// Lightweight update that only refreshes counts without rebuilding the value list.
        /// </summary>
        /// <param name="stats">Updated counts per value. Pass null when no filters are active.</param>
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

        /// <summary>
        /// Rebuilds the valueCounts lookup from the given stats, or zeros out if stats is null.
        /// </summary>
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

        #region Virtual ListView Implementation

        /// <summary>
        /// Virtual mode handler: creates ListViewItems on-demand as they scroll into view.
        /// </summary>
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

        /// <summary>
        /// Custom drawing for list items. Renders checkbox, optional selection indicator, and text.
        /// The ▶ indicator marks the value matching the currently selected log entry.
        /// Checked items use CheckedNormal; excluded items additionally show a strikethrough.
        /// </summary>
        private void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.Item.Tag is not LogMetadataValue value) return;

            bool isChecked = checkedItems.Contains(value);
            bool isExclude = isChecked && filterModeForAllCheckedItems == FilterMode.Exclude;
            bool isSelectedLine = selectedEntryValue != null && selectedEntryValue == value;
            int count = valueCounts.TryGetValue(value, out int c) ? c : 0;
            Rectangle bounds = e.Bounds;

            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();

                int checkBoxPadding = ScaleByDpi(3);
                CheckBoxState state = isChecked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);

                // Draw ▶ indicator if this row matches the selected log entry.
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

                // Draw strikethrough over the text only when in exclude mode.
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

        /// <summary>
        /// Returns the interned LogMetadataValue for the selected log entry's value for this property.
        /// </summary>
        private LogMetadataValue GetSelectedEntryValueForProperty()
        {
            if (selectedLogEntry == null || metadataProperty == null) return null;
            return selectedLogEntry.Metadata.TryGetValue(metadataProperty, out LogMetadataValue value) ? value : null;
        }

        #endregion

        #region Mouse Interaction

        /// <summary>
        /// Captures parent scroll position before click to prevent unwanted scrolling.
        /// </summary>
        private void ListView_MouseDown(object sender, MouseEventArgs e)
        {
            ScrollableControl parentScroll = FindScrollableParent();
            if (parentScroll != null)
                savedScrollPosition = parentScroll.AutoScrollPosition;
        }

        /// <summary>
        /// Toggles the checked state of a value on left click.
        /// </summary>
        private void ListView_MouseClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            ListViewItem item = listView.GetItemAt(e.X, e.Y);
            if (item == null) return;

            ListViewItem.ListViewSubItem subItem = item.GetSubItemAt(e.X, e.Y);
            if (subItem == null) return;

            if (e.Button == MouseButtons.Left && item.SubItems.IndexOf(subItem) == 0)
                ToggleCheckbox(item, listView);

            // Restore parent scroll position to prevent jumping.
            ScrollableControl parentScroll = FindScrollableParent();
            if (parentScroll != null && savedScrollPosition != Point.Empty)
            {
                parentScroll.AutoScrollPosition = new Point(
                    Math.Abs(savedScrollPosition.X),
                    Math.Abs(savedScrollPosition.Y));
                savedScrollPosition = Point.Empty;
            }
        }

        /// <summary>
        /// Toggles the checked state of a value: unchecked → checked → unchecked.
        /// Updates the header toggle visibility when the first item is checked or last is unchecked.
        /// </summary>
        private void ToggleCheckbox(ListViewItem item, ListView listView)
        {
            if (item.Tag is not LogMetadataValue value) return;

            bool wasEmpty = checkedItems.Count == 0;

            if (!checkedItems.Remove(value))
                checkedItems.Add(value);

            bool isNowEmpty = checkedItems.Count == 0;

            // Refresh the include/exclude label when toggle visibility changes.
            if (wasEmpty != isNowEmpty) LblIncludeExclude.Invalidate();

            listView.Invalidate(item.Bounds);
            OnFilterChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Treats double-click as a second single click so fast clicking cycles state correctly.
        /// </summary>
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

        /// <summary>
        /// Forwards mouse wheel events to the parent scrollable container.
        /// </summary>
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
        /// Returns a MetadataFilter representing the current checked state of this control.
        /// All checked values share filterModeForAllCheckedItems.
        /// </summary>
        public LogMetadataFilter GetCurrentFilter()
        {
            LogMetadataFilter filter = new(metadataProperty);
            foreach (LogMetadataValue value in checkedItems)
                filter.ActiveValues[value] = filterModeForAllCheckedItems;
            return filter;
        }

        internal void ResetFilters()
        {
            checkedItems.Clear();
            filterModeForAllCheckedItems = FilterMode.Include;
            LblIncludeExclude.Invalidate();
            ListViewItems.Invalidate();
            OnFilterChanged(EventArgs.Empty);
        }

        #endregion

        #region Event Handlers

        protected virtual void OnFilterChanged(EventArgs e)
        {
            if (!updateInProgress) FilterChanged?.Invoke(this, e);
        }

        protected virtual void OnCollapseChanged(EventArgs e)
        {
            CollapseChanged?.Invoke(this, e);
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