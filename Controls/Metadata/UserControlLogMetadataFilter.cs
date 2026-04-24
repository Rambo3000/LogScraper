using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Filtering;
using LogScraper.Log.Metadata;

namespace LogScraper.Controls.Metadata
{
    /// <summary>
    /// A user control that displays a filterable list of metadata values with checkboxes.
    /// The header (description label, chevron, in/ex toggle) is managed here.
    /// All list logic is encapsulated in the embedded LogMetadataValueList control.
    /// Resizing of LogMetadataValueList is also handled here based on item count.
    /// </summary>
    public partial class UserControlLogMetadataFilter : UserControl
    {
        #region Properties and Events

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Collapsed { get; set; }

        public event EventHandler FilterChanged;
        public event EventHandler CollapseChanged;

        /// <summary>
        /// The currently selected log entry. Forwarded to the embedded value list.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogEntry SelectedLogEntry
        {
            get => ValueList.SelectedLogEntry;
            set => ValueList.SelectedLogEntry = value;
        }

        #endregion

        #region Constructor

        private string baseDescription;
        private bool headerHovered = false;
        private bool toggleHovered = false;
        private ToolTip toolTip;

        public UserControlLogMetadataFilter(string description)
        {
            InitializeComponent();
            Collapsed = false;
            baseDescription = description;

            PnlHeader.Resize += (s, e) => PnlHeader.Invalidate();
            ValueList.FilterChanged += (s, e) =>
            {
                PnlHeader.Invalidate(); OnFilterChanged(e);
                LblIncludeExclude.Visible = ValueList.CheckedItemCount > 0;
            };
            ValueList.CheckedItemsChanged += (s, e) => { PnlHeader.Invalidate(); LblIncludeExclude.Invalidate(); };
            LblIncludeExclude.BackColor = Color.FromArgb(10, 0, 0, 0);

            toolTip = new ToolTip();
            toolTip.SetToolTip(LblIncludeExclude, "Schakel tussen opnemen (in) en uitsluiten (ex) van geselecteerde waarden");

            LblIncludeExclude.MouseEnter += LblIncludeExclude_MouseEnter;
            LblIncludeExclude.MouseLeave += LblIncludeExclude_MouseLeave;

            PnlHeader.Invalidate();
        }

        #endregion

        #region Layout and Sizing

        protected override void OnLayout(LayoutEventArgs layoutEventArgs)
        {
            base.OnLayout(layoutEventArgs);
            UpdateHeightFromFont();
        }

        /// <summary>
        /// Positions the ValueList below the description label based on font size.
        /// </summary>
        private void UpdateHeightFromFont()
        {
            int headerHeight = PnlHeader.Height + 3;
            if (ValueList.Top != headerHeight)
                ValueList.Top = headerHeight;
        }

        /// <summary>
        /// Adjusts the control's height based on the number of items.
        /// Called after data updates and collapse/expand transitions.
        /// </summary>
        private void ResizeVertically()
        {
            if (Collapsed || ValueList.SortedValueCount == 0)
            {
                ValueList.Height = 0;
                Height = ValueList.Top + Padding.Bottom;
                return;
            }

            int itemHeight = TextRenderer.MeasureText("Test", ValueList.Font).Height + ScaleByDpi(4);
            int totalHeight = ValueList.SortedValueCount * itemHeight;
            int maxHeight = LogMetadataValueList.MAX_NUMBER_OF_ITEMS_BEFORE_SCROLL * itemHeight;
            int actualHeight = totalHeight > maxHeight ? LogMetadataValueList.SCROLL_VIEW_NUMBER_OF_ITEMS_SHOWN * itemHeight : totalHeight;
            int newHeight = ValueList.Top + actualHeight + Padding.Bottom;

            if (Height != newHeight) Height = newHeight;
            if (ValueList.Height != actualHeight) ValueList.Height = actualHeight;
        }

        #endregion

        #region Header Toggle

        private void Header_Click(object sender, MouseEventArgs e)
        {
            SetCollapsed(!Collapsed);
        }

        private void Header_MouseEnter(object sender, EventArgs e)
        {
            headerHovered = true;
            PnlHeader.Invalidate();
        }

        private void Header_MouseLeave(object sender, EventArgs e)
        {
            Point cursor = PointToClient(Cursor.Position);
            bool stillInHeader = PnlHeader.Bounds.Contains(cursor);
            if (headerHovered == stillInHeader) return;
            headerHovered = stillInHeader;
            PnlHeader.Invalidate();
        }

        private void ValueList_MouseEnter(object sender, EventArgs e)
        {
            if (!headerHovered) return;
            headerHovered = false;
            PnlHeader.Invalidate();
        }

        private void LblIncludeExclude_MouseEnter(object sender, EventArgs e)
        {
            toggleHovered = true;
            PnlHeader.Invalidate();
        }

        private void LblIncludeExclude_MouseLeave(object sender, EventArgs e)
        {
            toggleHovered = false;
            PnlHeader.Invalidate();
        }

        public void SetCollapsed(bool collapsed)
        {
            if (Collapsed == collapsed) return;

            Collapsed = collapsed;

            SuspendLayout();
            ResizeVertically();
            ResumeLayout(true);

            PnlHeader.Invalidate();
            LblIncludeExclude.Invalidate();

            if (!collapsed)
                ValueList.RedrawList(scrollToTop: true);

            OnCollapseChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Owner-draws the full header row:
        /// Tip 1: 3px left accent bar — colored when filter active, light gray otherwise.
        /// Tip 2: Hover background tint signals clickability; label flush left at x=8.
        /// Tip 3: Text alpha 60% inactive / 100% active — no font size change.
        /// Tip 4: Rounded count badge always visible after the title text.
        /// Tip 5: 1px separator line at the bottom of the header.
        /// +/− icon appears on hover only, drawn over the accent bar area.
        /// </summary>
        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            bool hasFilter = ValueList.CheckedItemCount > 0;
            int w = PnlHeader.Width;
            int h = PnlHeader.Height;

            // subtle background
            using SolidBrush hoverBrush = new(Color.FromArgb(10, 0, 0, 0));
            g.FillRectangle(hoverBrush, 0, 0, w, h);

            // Separator line at bottom
            using Pen sepPen = new(Color.FromArgb(35, 0, 0, 0), 1f);
            g.DrawLine(sepPen, 0, h - 1, w, h - 1);

            // 3px left accent bar for collapsed filters which have active filters
            if (Collapsed && hasFilter)
            {
                Color accentColor = hasFilter ? Color.FromArgb(0, 120, 215) : Color.FromArgb(180, 180, 180);
                using SolidBrush accentBrush = new(accentColor);
                g.FillRectangle(accentBrush, 0, 2, 3, h - 4);
            }


            // text alpha — same font, only opacity changes
            Font font = new(PnlHeader.Font, FontStyle.Bold);
            Color textColor = SystemColors.ControlText;

            string title = baseDescription;
            int textX = 7;
            int textY = (h - TextRenderer.MeasureText(title, font).Height) / 2;


            // draw text using GDI+ for alpha support
            using SolidBrush textBrush = new(textColor);
            g.DrawString(title, font, textBrush, textX, textY);

            // +/− icon on hover (drawn over the accent bar area), suppressed when hovering the IN/EX toggle
            if ((headerHovered && !toggleHovered) || Collapsed)
            {
                int cx = TextRenderer.MeasureText(title, font).Width + 8, cy = h / 2;
                int arm = 3;
                Color iconColor = Color.FromArgb(90, 90, 90);
                using Pen iconPen = new(iconColor, 1.5f);
                g.DrawLine(iconPen, cx - arm, cy, cx + arm, cy);
                if (Collapsed)
                    g.DrawLine(iconPen, cx, cy - arm, cx, cy + arm);
            }

        }

        /// <summary>
        /// Draws the IN | EX toggle right-aligned.
        /// Only visible when at least one value is checked.
        /// </summary>
        private void LblIncludeExclude_Paint(object sender, PaintEventArgs e)
        {
            if (ValueList.CheckedItemCount == 0)
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

            Color colorIn = ValueList.CurrentFilterMode == FilterMode.Include ? activeColor : inactiveColor;
            Color colorSeparator = inactiveColor;
            Color colorEx = ValueList.CurrentFilterMode == FilterMode.Exclude ? activeColor : inactiveColor;

            TextRenderer.DrawText(graphics, textIn, font, new Point(x, y), colorIn, TextFormatFlags.NoPadding);
            x += sizeIn.Width;
            TextRenderer.DrawText(graphics, textSeparator, font, new Point(x, y), colorSeparator, TextFormatFlags.NoPadding);
            x += sizeSeparator.Width;
            TextRenderer.DrawText(graphics, textEx, font, new Point(x, y), colorEx, TextFormatFlags.NoPadding);
        }

        /// <summary>
        /// Handles clicks on the include/exclude label. Toggles the filter mode when the IN|EX toggle is hit.
        /// </summary>
        private void LblIncludeExclude_MouseClick(object sender, MouseEventArgs e)
        {
            if (ValueList.CheckedItemCount == 0) return;
            if (!toggleBounds.Contains(e.Location)) return;

            ValueList.ToggleFilterMode();
            LblIncludeExclude.Invalidate();
        }

        private Rectangle toggleBounds = Rectangle.Empty;

        #endregion

        #region Data Updates

        /// <summary>
        /// Updates the embedded list with all known values for this property.
        /// </summary>
        public void UpdateListView(LogMetadataProperty property, List<LogMetadataValue> allValues)
        {
            bool needsResize = ValueList.UpdateValues(property, allValues);

            if (property.IsCollapsedByDefault && ValueList.SortedValueCount > 0 && !Collapsed)
                SetCollapsed(true);
            else if (needsResize)
                ResizeVertically();

            PnlHeader.Invalidate();
        }

        /// <summary>
        /// Sets all counts to zero without rebuilding the value list.
        /// </summary>
        public void UpdateCountsToZero() => ValueList.UpdateCountsToZero();

        /// <summary>
        /// Lightweight update that only refreshes counts without rebuilding the value list.
        /// </summary>
        public void UpdateCounts(LogMetadataFilterStats stats)
        {
            ValueList.UpdateCounts(stats);
        }

        #endregion

        #region Public API

        public LogMetadataFilter GetCurrentFilter() => ValueList.GetCurrentFilter();

        internal void ResetFilters()
        {
            ValueList.ResetFilters();
            PnlHeader.Invalidate();
            LblIncludeExclude.Invalidate();
        }

        public void DeselectValue(LogMetadataValue value)
        {
            ValueList.DeselectValue(value);
            LblIncludeExclude.Invalidate();
        }

        public void DeselectAllValues()
        {
            ValueList.DeselectAllValues();
            PnlHeader.Invalidate();
            LblIncludeExclude.Invalidate();
        }

        #endregion

        #region Event Handlers

        protected virtual void OnFilterChanged(EventArgs e)
        {
            FilterChanged?.Invoke(this, e);
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

        private sealed class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint, true);
                UpdateStyles();
            }
        }
    }
}