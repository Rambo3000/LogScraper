using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LogScraper.Controls.Generic
{
    public enum CollapsePanel { None, Panel1, Panel2 }

    public class SplitContainerWithGrip : SplitContainer
    {
        #region Constants

        private const int NormalSplitterWidth = 6;
        private const int DefaultCollapsedSplitterWidth = 20;

        #endregion

        #region Fields

        private CollapsePanel _collapseablePanel = CollapsePanel.None;
        private Rectangle _triangleHitRect = Rectangle.Empty;
        private bool _isCollapsed = false;
        private double _savedSplitterRatio = -1;   // fraction of total size, resize-safe
        private int _savedPanel1MinSize;
        private int _savedPanel2MinSize;
        private bool _isHoveringCollapsedBar = false;
        private bool _isPressedCollapsedBar = false;
        private string _textSplitter = string.Empty;
        private int _collapsedSplitterWidth = DefaultCollapsedSplitterWidth;
        private readonly ToolTip _toolTip = new();

        #endregion

        #region Events

        /// <summary>Raised after the panel is collapsed or expanded. The bool argument is <c>true</c> when collapsed.</summary>
        public event EventHandler<bool> CollapseChanged;

        #endregion

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public CollapsePanel CollapseablePanel
        {
            get => _collapseablePanel;
            set { _collapseablePanel = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TextSplitter
        {
            get => _textSplitter;
            set { _textSplitter = value ?? string.Empty; Invalidate(); }
        }

        /// <summary>Width (or height) of the bar shown when the panel is collapsed.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(DefaultCollapsedSplitterWidth)]
        public int CollapsedSplitterWidth
        {
            get => _collapsedSplitterWidth;
            set
            {
                if (value < 8) value = 8;
                _collapsedSplitterWidth = value;
                if (_isCollapsed)
                {
                    SplitterWidth = value;
                    Invalidate();
                }
            }
        }

        /// <summary>Returns <c>true</c> when the collapseable panel is currently collapsed.</summary>
        [Browsable(false)]
        public bool IsCollapsed => _isCollapsed;

        #endregion

        #region Constructor

        public SplitContainerWithGrip()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UseTextForAccessibility, false);
            SplitterWidth = NormalSplitterWidth;
            SplitterMoved += OnSplitterMoved;
            SizeChanged += OnSizeChanged;

            // Give both panels an explicit cursor so they never inherit
            // whatever cursor we set on the splitter bar itself.
            Panel1.Cursor = Cursors.Default;
            Panel2.Cursor = Cursors.Default;

            _toolTip.InitialDelay = 400;
            _toolTip.ReshowDelay = 200;
        }

        #endregion

        #region Overrides

        // Suppress the focus rectangle that WinForms draws on the splitter bar.
        protected override void WndProc(ref Message m)
        {
            const int WM_SETFOCUS = 0x0007;
            if (m.Msg == WM_SETFOCUS)
                return;
            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_isCollapsed)
            {
                DrawCollapsedBar(e.Graphics);
            }
            else
            {
                DrawSplitterGrip(e.Graphics);
                if (_collapseablePanel != CollapsePanel.None)
                    DrawCollapseTriangle(e.Graphics);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isCollapsed)
            {
                bool wasHovering = _isHoveringCollapsedBar;
                _isHoveringCollapsedBar = GetSplitterRectangle().Contains(e.Location);
                if (_isHoveringCollapsedBar != wasHovering)
                {
                    UpdateToolTip(_isHoveringCollapsedBar);
                    Invalidate();
                }
                Cursor = _isHoveringCollapsedBar ? Cursors.Hand : Cursors.Default;
            }
            else
            {
                _isHoveringCollapsedBar = false;
                Cursor = (_collapseablePanel != CollapsePanel.None && _triangleHitRect.Contains(e.Location))
                    ? Cursors.Hand
                    : Cursors.Default;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Cursor = Cursors.Default;
            if (_isHoveringCollapsedBar || _isPressedCollapsedBar)
            {
                _isHoveringCollapsedBar = false;
                _isPressedCollapsedBar = false;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isCollapsed && GetSplitterRectangle().Contains(e.Location))
                {
                    _isPressedCollapsedBar = true;
                    Invalidate();
                    return;
                }

                if (!_isCollapsed && _collapseablePanel != CollapsePanel.None && _triangleHitRect.Contains(e.Location))
                {
                    Collapse();
                    return;
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _isPressedCollapsedBar)
            {
                _isPressedCollapsedBar = false;
                if (GetSplitterRectangle().Contains(e.Location))
                    Expand();
                else
                    Invalidate();
                return;
            }

            base.OnMouseUp(e);
        }

        #endregion

        #region Collapse / Expand

        private void Collapse()
        {
            int totalSize = Orientation == Orientation.Vertical ? ClientSize.Width : ClientSize.Height;
            _savedSplitterRatio = totalSize > 0 ? (double)SplitterDistance / totalSize : 0.5;
            _savedPanel1MinSize = Panel1MinSize;
            _savedPanel2MinSize = Panel2MinSize;
            _isCollapsed = true;

            SplitterWidth = _collapsedSplitterWidth;
            IsSplitterFixed = true;

            if (_collapseablePanel == CollapsePanel.Panel1)
            {
                Panel1MinSize = 0;
                SplitterDistance = 0;
            }
            else
            {
                Panel2MinSize = 0;
                SplitterDistance = totalSize - _collapsedSplitterWidth;
            }

            CollapseChanged?.Invoke(this, true);
            Invalidate();
        }

        private void Expand()
        {
            _isCollapsed = false;

            // Restore min sizes before adjusting the distance.
            Panel1MinSize = _savedPanel1MinSize;
            Panel2MinSize = _savedPanel2MinSize;
            SplitterWidth = NormalSplitterWidth;
            IsSplitterFixed = false;

            if (_savedSplitterRatio >= 0)
            {
                // Restore proportionally so the splitter lands in the right place
                // even if the control was resized while collapsed.
                int totalSize = Orientation == Orientation.Vertical ? ClientSize.Width : ClientSize.Height;
                SplitterDistance = Math.Max(Panel1MinSize,
                                   Math.Min((int)Math.Round(_savedSplitterRatio * totalSize),
                                            totalSize - Panel2MinSize - NormalSplitterWidth));
            }

            _isHoveringCollapsedBar = false;
            _isPressedCollapsedBar = false;
            CollapseChanged?.Invoke(this, false);
            Invalidate();
        }

        #endregion

        #region Event Handlers

        private void OnSplitterMoved(object sender, SplitterEventArgs e) => Invalidate();

        private void OnSizeChanged(object sender, EventArgs e)
        {
            // Re-pin the splitter to the correct edge when collapsed so the
            // collapsed panel never leaks into view after a resize.
            if (_isCollapsed && _collapseablePanel == CollapsePanel.Panel2)
            {
                int totalSize    = Orientation == Orientation.Vertical ? ClientSize.Width : ClientSize.Height;
                SplitterDistance = totalSize - _collapsedSplitterWidth;
            }

            Invalidate();
        }

        #endregion

        #region Drawing

        private void DrawCollapsedBar(Graphics graphics)
        {
            Rectangle barRect = GetSplitterRectangle();

            if (_isPressedCollapsedBar)
            {
                // Pressed: the accent colour itself mixed slightly toward white, like a depressed button.
                using SolidBrush pressBrush = new(Blend(SystemColors.Highlight, Color.White, 0.30f));
                graphics.FillRectangle(pressBrush, barRect);

                using Pen pressBorder = new(Blend(SystemColors.Highlight, Color.Black, 0.15f), 1);
                DrawBarBorders(graphics, barRect, pressBorder);
            }
            else if (_isHoveringCollapsedBar)
            {
                // Hover: accent colour washed out toward white (same feel as a hovered button).
                Color hoverNear = Blend(SystemColors.Highlight, Color.White, 0.82f);
                Color hoverFar = Blend(SystemColors.Highlight, Color.White, 0.68f);
                LinearGradientMode gradMode = Orientation == Orientation.Vertical
                    ? LinearGradientMode.Horizontal
                    : LinearGradientMode.Vertical;
                using LinearGradientBrush gradBrush = new(barRect, hoverNear, hoverFar, gradMode);
                graphics.FillRectangle(gradBrush, barRect);

                using Pen hoverBorder = new(SystemColors.Highlight, 1);
                DrawBarBorders(graphics, barRect, hoverBorder);
            }
            else
            {
                // Normal: flat, blends with the rest of the UI.
                using SolidBrush bgBrush = new(SystemColors.Control);
                graphics.FillRectangle(bgBrush, barRect);

                using Pen subtleBorder = new(Color.FromArgb(60, SystemColors.ControlDark));
                DrawBarBorders(graphics, barRect, subtleBorder);
            }

            DrawCollapseTriangle(graphics);

            if (!string.IsNullOrEmpty(_textSplitter))
                DrawCollapsedBarText(graphics, barRect);
        }

        private void DrawBarBorders(Graphics graphics, Rectangle barRect, Pen pen)
        {
            if (Orientation == Orientation.Vertical)
            {
                graphics.DrawLine(pen, barRect.Left, barRect.Top, barRect.Left, barRect.Bottom - 1);
                graphics.DrawLine(pen, barRect.Right - 1, barRect.Top, barRect.Right - 1, barRect.Bottom - 1);
            }
            else
            {
                graphics.DrawLine(pen, barRect.Left, barRect.Top, barRect.Right - 1, barRect.Top);
                graphics.DrawLine(pen, barRect.Left, barRect.Bottom - 1, barRect.Right - 1, barRect.Bottom - 1);
            }
        }

        private void DrawCollapsedBarText(Graphics graphics, Rectangle barRect)
        {
            using Font font = new(Font.FontFamily, 8f, FontStyle.Regular);

            Color textColor = _isPressedCollapsedBar
                ? Color.White
                : SystemColors.ControlDarkDark;
            using SolidBrush textBrush = new(textColor);

            if (Orientation == Orientation.Vertical)
            {
                int textStartY = _triangleHitRect.Bottom + 6;
                float cx = barRect.X + barRect.Width / 2f;

                graphics.TranslateTransform(cx, textStartY);
                graphics.RotateTransform(90f);
                graphics.DrawString(_textSplitter, font, textBrush, 0f, -font.Height / 2f);
                graphics.ResetTransform();
            }
            else
            {
                int textStartX = _triangleHitRect.Right + 6;
                float cy = barRect.Y + barRect.Height / 2f;
                graphics.DrawString(_textSplitter, font, textBrush, textStartX, cy - font.Height / 2f);
            }
        }

        private void DrawSplitterGrip(Graphics graphics)
        {
            Rectangle splitterRect = GetSplitterRectangle();

            const int dotSize = 2;
            const int spacing = 4;
            const int dotCount = 5;

            int centerX = splitterRect.X + splitterRect.Width / 2;
            int centerY = splitterRect.Y + splitterRect.Height / 2;

            using SolidBrush brush = new(SystemColors.ControlDark);
            for (int i = -dotCount / 2; i <= dotCount / 2; i++)
            {
                Rectangle dot = Orientation == Orientation.Vertical
                    ? new Rectangle(centerX - dotSize / 2, centerY + i * spacing, dotSize, dotSize)
                    : new Rectangle(centerX + i * spacing, centerY - dotSize / 2, dotSize, dotSize);
                graphics.FillRectangle(brush, dot);
            }
        }

        private void DrawCollapseTriangle(Graphics graphics)
        {
            Rectangle splitterRect = GetSplitterRectangle();
            int cx, cy;

            if (Orientation == Orientation.Vertical)
            {
                cx = splitterRect.X + splitterRect.Width / 2;
                cy = _isCollapsed
                    ? splitterRect.Y + 14
                    : splitterRect.Y + splitterRect.Height / 2 - 18;
            }
            else
            {
                cx = _isCollapsed
                    ? splitterRect.X + 14
                    : splitterRect.X + splitterRect.Width / 2 - 18;
                cy = splitterRect.Y + splitterRect.Height / 2;
            }

            // Collapsed bar: slightly larger triangle; normal: smaller and subtle.
            int hw = _isCollapsed
                ? (Orientation == Orientation.Vertical ? 5 : 4)
                : (Orientation == Orientation.Vertical ? 4 : 3);
            int hh = _isCollapsed
                ? (Orientation == Orientation.Vertical ? 4 : 5)
                : (Orientation == Orientation.Vertical ? 3 : 4);

            Point[] triangle = BuildTrianglePoints(cx, cy, hw, hh);

            int padding = 5;
            _triangleHitRect = new Rectangle(cx - hw - padding, cy - hh - padding, (hw + padding) * 2, (hh + padding) * 2);

            // Triangle colour responds to interaction state.
            Color triangleColor;
            if (_isPressedCollapsedBar)
                triangleColor = Blend(SystemColors.Highlight, Color.Black, 0.40f);
            else if (_isHoveringCollapsedBar)
                triangleColor = Blend(SystemColors.Highlight, Color.Black, 0.10f);
            else if (_isCollapsed)
                triangleColor = SystemColors.ControlDarkDark;
            else
                triangleColor = SystemColors.ControlDark;

            SmoothingMode prevSmoothing = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (SolidBrush brush = new(triangleColor))
                graphics.FillPolygon(brush, triangle);
            graphics.SmoothingMode = prevSmoothing;
        }

        private Point[] BuildTrianglePoints(int cx, int cy, int hw, int hh)
        {
            if (Orientation == Orientation.Vertical)
            {
                bool pointLeft = (_collapseablePanel == CollapsePanel.Panel1 && !_isCollapsed)
                              || (_collapseablePanel == CollapsePanel.Panel2 && _isCollapsed);
                return pointLeft
                    ? [new Point(cx - hh, cy), new Point(cx + hh, cy - hw), new Point(cx + hh, cy + hw)]
                    : [new Point(cx + hh, cy), new Point(cx - hh, cy - hw), new Point(cx - hh, cy + hw)];
            }
            else
            {
                bool pointUp = (_collapseablePanel == CollapsePanel.Panel1 && !_isCollapsed)
                            || (_collapseablePanel == CollapsePanel.Panel2 && _isCollapsed);
                return pointUp
                    ? [new Point(cx, cy - hh), new Point(cx - hw, cy + hh), new Point(cx + hw, cy + hh)]
                    : [new Point(cx, cy + hh), new Point(cx - hw, cy - hh), new Point(cx + hw, cy - hh)];
            }
        }

        private Rectangle GetSplitterRectangle()
        {
            return Orientation == Orientation.Vertical
                ? new Rectangle(SplitterDistance, 0, SplitterWidth, Height)
                : new Rectangle(0, SplitterDistance, Width, SplitterWidth);
        }

        private static Color Blend(Color a, Color b, float t) =>
            Color.FromArgb(
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));

        private void UpdateToolTip(bool show)
        {
            if (show)
            {
                string tip = !string.IsNullOrEmpty(_textSplitter)
                    ? $"Klik om te vergroten \"{_textSplitter}\""
                    : "Klik om te vergroten";
                _toolTip.SetToolTip(this, tip);
            }
            else
            {
                _toolTip.SetToolTip(this, string.Empty);
            }
        }

        #endregion
    }
}
