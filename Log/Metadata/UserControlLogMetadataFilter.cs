using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.Extensions;

namespace LogScraper
{
    /// <summary>
    /// A user control that displays a filterable list of metadata values with checkboxes.
    /// Uses virtual ListView to handle thousands of items efficiently without creating excessive window handles.
    /// </summary>
    public partial class UserControlLogMetadataFilter : UserControl
    {
        #region Properties and Events

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Collapsed { get; set; }

        public event EventHandler FilterChanged;
        public event EventHandler CollapseChanged;

        #endregion

        #region Private Fields

        private LogMetadataPropertyAndValues LogMetadataPropertyAndValues;
        private List<LogMetadataValue> sortedValues = [];
        private readonly HashSet<string> checkedItems = [];
        private bool updateInProgress = false;
        private Point savedScrollPosition = Point.Empty;

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
        /// Ensures proper DPI scaling.
        /// </summary>
        private void UpdateHeightFromFont()
        {
            int textHeight = TextRenderer.MeasureText(LblLogFilterDescription.Text, LblLogFilterDescription.Font).Height;
            int desiredTopPosition = textHeight + 1;

            if (ListViewItems.Top != desiredTopPosition)
            {
                ListViewItems.Top = desiredTopPosition;
            }
        }

        /// <summary>
        /// Adjusts the control's height based on the number of items.
        /// Limits maximum height to prevent the control from dominating the screen.
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

            int maxHeight = 50 * itemHeight;
            int actualHeight = totalHeight;
            if (actualHeight > maxHeight) actualHeight = 15 * itemHeight;

            int newHeight = ListViewItems.Top + actualHeight + Padding.Bottom;

            if (Height != newHeight) Height = newHeight;
            if (ListViewItems.Height != actualHeight) ListViewItems.Height = actualHeight;
        }

        /// <summary>
        /// Dynamically adjusts the count column width to fit the largest number.
        /// Prevents wasted space and ensures counts are always fully visible.
        /// </summary>
        private void AdjustCountColumnWidth()
        {
            if (sortedValues.Count == 0) return;

            int maxCount = sortedValues.Max(v => v.Count);
            string maxCountText = maxCount.ToString("N0");
            int textWidth = TextRenderer.MeasureText(maxCountText, ListViewItems.Font).Width;
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
            {
                listView.Columns[0].Width = listView.ClientSize.Width - listView.Columns[1].Width - 4;
            }
        }

        #endregion

        #region Data Updates

        /// <summary>
        /// Updates the ListView with new metadata values.
        /// If only counts changed, performs lightweight update. Otherwise rebuilds the list.
        /// </summary>
        /// <summary>
        /// Updates the ListView with new metadata values.
        /// If only counts changed, performs lightweight update. Otherwise rebuilds the list.
        /// </summary>
        public void UpdateListView(LogMetadataPropertyAndValues logMetadataPropertyAndValuesNew)
        {
            // Optimize: if keys haven't changed, only update counts
            if (LogMetadataPropertyAndValues != null)
            {
                bool keysAreEqual =
                    logMetadataPropertyAndValuesNew.LogMetadataValues.Count == LogMetadataPropertyAndValues.LogMetadataValues.Count &&
                    logMetadataPropertyAndValuesNew.LogMetadataValues.Keys.All(LogMetadataPropertyAndValues.LogMetadataValues.ContainsKey);

                if (keysAreEqual)
                {
                    UpdateCountInListView(logMetadataPropertyAndValuesNew);
                    return;
                }
            }

            updateInProgress = true;

            // Preserve checkbox states across updates
            HashSet<string> previousCheckedItems = [.. checkedItems];

            // Sort values alphabetically
            sortedValues = [.. logMetadataPropertyAndValuesNew.LogMetadataValues.Keys.OrderBy(lmv => lmv.Value)];

            // Restore checkbox states for items that still exist
            checkedItems.Clear();
            foreach (LogMetadataValue value in sortedValues)
            {
                // If item was previously checked, keep it checked
                if (previousCheckedItems.Contains(value.Value))
                {
                    checkedItems.Add(value.Value);
                    value.IsFilterEnabled = true;
                }
                // Otherwise use the value from the new data
                else if (value.IsFilterEnabled)
                {
                    checkedItems.Add(value.Value);
                }
            }

            LogMetadataPropertyAndValues = logMetadataPropertyAndValuesNew;

            // Update ListView
            ListViewItems.VirtualListSize = sortedValues.Count;
            AdjustCountColumnWidth();

            ResizeVertically();

            updateInProgress = false;
        }

        /// <summary>
        /// Lightweight update that only refreshes item counts without rebuilding the list.
        /// Preserves scroll position to avoid jarring user experience.
        /// </summary>
        public void UpdateCountInListView(LogMetadataPropertyAndValues logMetadataPropertyAndValues)
        {
            // Save scroll position to restore after update
            int topItemIndex = ListViewItems.TopItem?.Index ?? 0;

            ListViewItems.SuspendDrawing();
            ListViewItems.BeginUpdate();

            // Update counts for existing items
            for (int i = 0; i < sortedValues.Count; i++)
            {
                LogMetadataValue existing = sortedValues[i];
                foreach (var kvp in logMetadataPropertyAndValues.LogMetadataValues)
                {
                    if (kvp.Key == existing)
                    {
                        existing.Count = kvp.Key.Count;
                        break;
                    }
                }
            }

            AdjustCountColumnWidth();

            ListViewItems.EndUpdate();
            ListViewItems.ResumeDrawing();

            // Restore scroll position
            if (topItemIndex < ListViewItems.VirtualListSize && ListViewItems.VirtualListSize > 0)
            {
                ListViewItems.EnsureVisible(topItemIndex);
            }
        }

        #endregion

        #region Virtual ListView Implementation

        /// <summary>
        /// Virtual mode handler: creates ListViewItems on-demand as they become visible.
        /// This is key to handling thousands of items without creating thousands of controls.
        /// </summary>
        private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= sortedValues.Count) return;

            LogMetadataValue value = sortedValues[e.ItemIndex];

            ListViewItem item = new(value.Value)
            {
                ForeColor = value.Count == 0 ? Color.Gray : Color.Black,
                Tag = value
            };

            item.SubItems.Add(value.Count.ToString("N0"));

            e.Item = item;
        }

        #endregion

        #region Owner Draw Implementation

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;
        }

        /// <summary>
        /// Custom drawing for list items to render checkboxes and text.
        /// Mimics the appearance of the original UserControlLogMetadataFilterItem.
        /// </summary>
        private void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.Item.Tag == null) return;

            LogMetadataValue value = e.Item.Tag as LogMetadataValue;
            bool isChecked = checkedItems.Contains(value.Value);

            Rectangle bounds = e.Bounds;

            if (e.ColumnIndex == 0)
            {
                // Draw description column with checkbox
                e.DrawBackground();

                int checkBoxPadding = ScaleByDpi(3);

                // Get the actual checkbox size from the system theme
                CheckBoxState state = isChecked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);

                // Center checkbox vertically in the row
                Point checkBoxLocation = new(bounds.Left + checkBoxPadding, bounds.Top + (bounds.Height - checkBoxSize.Height) / 2);

                CheckBoxRenderer.DrawCheckBox(e.Graphics, checkBoxLocation, state);

                Rectangle textBounds = new(
                    checkBoxLocation.X + checkBoxSize.Width + checkBoxPadding,
                    bounds.Top,
                    bounds.Width - checkBoxLocation.X - checkBoxSize.Width - checkBoxPadding,
                    bounds.Height
                );

                TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                                       TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;

                TextRenderer.DrawText(e.Graphics, value.Value, e.Item.Font, textBounds, e.Item.ForeColor, flags);
            }
            else if (e.ColumnIndex == 1)
            {
                // Draw count column (right-aligned)
                e.DrawBackground();

                TextFormatFlags flags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix;

                Rectangle textBounds = new(
                    bounds.Left,
                    bounds.Top,
                    bounds.Width - ScaleByDpi(5),
                    bounds.Height
                );

                TextRenderer.DrawText(e.Graphics, value.Count.ToString("N0"), e.Item.Font, textBounds, e.Item.ForeColor, flags);
            }
        }

        #endregion

        #region Mouse Interaction

        /// <summary>
        /// Captures parent scroll position before click to prevent unwanted scrolling.
        /// The act of clicking can cause focus changes that trigger auto-scroll.
        /// </summary>
        private void ListView_MouseDown(object sender, MouseEventArgs e)
        {
            ScrollableControl parentScroll = FindScrollableParent();
            if (parentScroll != null)
            {
                savedScrollPosition = parentScroll.AutoScrollPosition;
            }
        }

        /// <summary>
        /// Handles checkbox toggling when user clicks on the description column.
        /// Restores parent scroll position to prevent jumping.
        /// </summary>
        private void ListView_MouseClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            ListViewItem item = listView.GetItemAt(e.X, e.Y);

            if (item == null) return;

            ListViewItem.ListViewSubItem subItem = item.GetSubItemAt(e.X, e.Y);
            if (subItem == null) return;

            int subItemIndex = item.SubItems.IndexOf(subItem);

            // Only toggle checkbox if clicking on description column (entire column is clickable)
            if (subItemIndex == 0)
            {
                ToggleCheckbox(item, listView);
            }

            // Restore parent scroll position to prevent jumping
            ScrollableControl parentScroll = FindScrollableParent();
            if (parentScroll != null && savedScrollPosition != Point.Empty)
            {
                parentScroll.AutoScrollPosition = new Point(
                    Math.Abs(savedScrollPosition.X),
                    Math.Abs(savedScrollPosition.Y)
                );
                savedScrollPosition = Point.Empty;
            }
        }

        /// <summary>
        /// Toggles the checkbox state for a metadata value.
        /// </summary>
        private void ToggleCheckbox(ListViewItem item, ListView listView)
        {
            if (item.Tag is not LogMetadataValue value) return;

            bool wasChecked = checkedItems.Contains(value.Value);

            if (wasChecked)
            {
                checkedItems.Remove(value.Value);
            }
            else
            {
                checkedItems.Add(value.Value);
            }

            listView.Invalidate(item.Bounds);

            OnFilterChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Forwards mouse wheel events to the parent scrollable container.
        /// Prevents the ListView from scrolling internally when it's short enough to fit.
        /// </summary>
        private void ListView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs handledArgs)
            {
                handledArgs.Handled = true;
            }

            Control parent = Parent;
            while (parent != null)
            {
                if (parent is ScrollableControl scrollable && scrollable.AutoScroll)
                {
                    int scrollAmount = SystemInformation.MouseWheelScrollLines * e.Delta / 120 * -20;
                    scrollable.AutoScrollPosition = new Point(
                        -scrollable.AutoScrollPosition.X,
                        -scrollable.AutoScrollPosition.Y + scrollAmount
                    );
                    break;
                }
                parent = parent.Parent;
            }
        }

        #endregion

        #region Scroll Management

        /// <summary>
        /// Prevents auto-scrolling when child controls gain focus.
        /// Without this, clicking items can cause the parent container to jump.
        /// </summary>
        protected override Point ScrollToControl(Control activeControl)
        {
            return AutoScrollPosition;
        }

        private ScrollableControl FindScrollableParent()
        {
            Control parent = Parent;
            while (parent != null)
            {
                if (parent is ScrollableControl scrollable && scrollable.AutoScroll)
                {
                    return scrollable;
                }
                parent = parent.Parent;
            }
            return null;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Returns the current metadata property with updated filter states.
        /// </summary>
        public LogMetadataPropertyAndValues GetCurrentLogMetadataPropertyAndValues()
        {
            foreach (LogMetadataValue value in sortedValues)
            {
                value.IsFilterEnabled = checkedItems.Contains(value.Value);
            }

            return LogMetadataPropertyAndValues;
        }

        /// <summary>
        /// Enables or disables filtering on a specific metadata value.
        /// When enabling, clears all other selections (exclusive mode).
        /// </summary>
        internal void EnableDisableFilterOnSpecificMetdataValue(string value, bool isEnabled)
        {
            if (isEnabled)
            {
                checkedItems.Clear();
                checkedItems.Add(value);
            }
            else
            {
                checkedItems.Remove(value);
            }

            ListViewItems.Invalidate();

            OnFilterChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Clears all filter selections.
        /// </summary>
        internal void ResetFilters()
        {
            checkedItems.Clear();

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