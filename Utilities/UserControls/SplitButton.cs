using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LogScraper.Utilities.UserControls
{
    /// <summary>
    /// A split button control with a main action area and a dropdown menu area.
    /// Left side triggers ButtonClick event, right side shows DropDownMenu.
    /// </summary>
    public partial class SplitButton : UserControl
    {
        private ContextMenuStrip _contextMenu;
        private Image _icon;
        private ImageList _imageList;
        private int _imageIndex = -1;
        private ButtonState _leftState = ButtonState.Normal;
        private ButtonState _rightState = ButtonState.Normal;
        private int _dropDownWidth = 20;
        private bool _clickHandled = false;

        /// <summary>Internal button state enumeration</summary>
        private enum ButtonState { Normal, Hot, Pressed }

        #region Public Properties

        /// <summary>Gets or sets the icon displayed on the left side of the button</summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Icon
        {
            get => _icon;
            set { _icon = value; Invalidate(); }
        }

        /// <summary>Gets or sets the ImageList containing button icons</summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ImageList ImageList
        {
            get => _imageList;
            set
            {
                if (_imageList != value)
                {
                    _imageList = value;
                    Invalidate();
                }
            }
        }

        /// <summary>Gets or sets the index of the icon in the ImageList to display</summary>
        [Category("Appearance")]
        [DefaultValue(-1)]
        [TypeConverter(typeof(ImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ImageIndex
        {
            get => _imageIndex;
            set
            {
                if (_imageIndex != value)
                {
                    _imageIndex = value;
                    Invalidate();
                }
            }
        }

        /// <summary>Gets or sets the context menu shown when the dropdown area is clicked</summary>
        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ContextMenuStrip DropDownMenu
        {
            get => _contextMenu;
            set => _contextMenu = value;
        }

        /// <summary>Gets or sets the width of the dropdown area in pixels (minimum 15)</summary>
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DropDownWidth
        {
            get => _dropDownWidth;
            set { _dropDownWidth = Math.Max(15, value); Invalidate(); }
        }

        #endregion

        #region Events

        /// <summary>Occurs when the main button area (left side) is clicked</summary>
        public event EventHandler ButtonClick;

        #endregion

        #region Constructor

        public SplitButton()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable, true);
            Size = new Size(100, 23);
            TabStop = true;
        }

        #endregion

        #region Drawing

        /// <summary>Gets the rectangle for the left (main button) area</summary>
        private Rectangle LeftRect => new(0, 0, Width - _dropDownWidth, Height);

        /// <summary>Gets the rectangle for the right (dropdown) area</summary>
        private Rectangle RightRect => new(Width - _dropDownWidth, 0, _dropDownWidth, Height);

        /// <summary>
        /// Retrieves the currently active icon based on priority:
        /// 1. ImageList with valid ImageIndex
        /// 2. Icon property
        /// </summary>
        /// <returns>The icon to display, or null if none available</returns>
        private Image GetCurrentIcon()
        {
            if (_imageList != null && _imageIndex >= 0 && _imageIndex < _imageList.Images.Count)
                return _imageList.Images[_imageIndex];
            return _icon;
        }

        /// <summary>
        /// Paints the control
        /// </summary>
        /// <param name="e">Paint event arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            if (Application.RenderWithVisualStyles)
                DrawThemedControl(g);
            else
                DrawClassicControl(g);

            DrawSeparator(g);

            var currentIcon = GetCurrentIcon();
            if (currentIcon != null)
            {
                int iconX = (LeftRect.Width - currentIcon.Width) / 2 + 1;
                int iconY = (Height - currentIcon.Height) / 2;
                g.DrawImage(currentIcon, iconX, iconY, currentIcon.Width, currentIcon.Height);
            }

            DrawArrow(g, RightRect);
        }

        /// <summary>
        /// Draws the control using Windows visual styles (themed rendering)
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        private void DrawThemedControl(Graphics g)
        {
            IntPtr hTheme = OpenThemeData(Handle, "Button");
            if (hTheme == IntPtr.Zero)
            {
                DrawClassicControl(g);
                return;
            }

            try
            {
                var overallState = GetOverallState();
                int stateId = overallState == ButtonState.Normal ? PBS_NORMAL :
                             overallState == ButtonState.Hot ? PBS_HOT : PBS_PRESSED;

                IntPtr hdc = g.GetHdc();
                try
                {
                    // Draw outer border with appropriate state
                    RECT rc = new() { Left = 0, Top = 0, Right = Width, Bottom = Height };
                    int result = DrawThemeBackground(hTheme, hdc, BP_PUSHBUTTON, stateId, ref rc, IntPtr.Zero);
                    if (result != 0) // If themed drawing failed, fallback to classic
                    {
                        DrawClassicControl(g);
                        return;
                    }
                    // Overdraw interior with normal state to show only border highlight
                    if (overallState == ButtonState.Hot)
                    {
                        RECT innerRc = new() { Left = 1, Top = 1, Right = Width - 1, Bottom = Height - 1 };
                        result = DrawThemeBackground(hTheme, hdc, BP_PUSHBUTTON, PBS_NORMAL, ref innerRc, IntPtr.Zero);
                        if (result != 0) // If themed drawing failed, fallback to classic
                        {
                            DrawClassicControl(g);
                            return;
                        }
                    }
                }
                finally
                {
                    g.ReleaseHdc(hdc);
                }

                DrawSectionHighlight(g);
            }
            finally
            {
                int result = CloseThemeData(hTheme);
                if (result != 0)
                {
                    // Log or handle theme closing error if necessary
                }
            }
        }

        /// <summary>
        /// Draws highlight overlays for hovered or pressed sections
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        private void DrawSectionHighlight(Graphics g)
        {
            // Left section highlight
            if (_leftState == ButtonState.Hot)
            {
                using var brush = new SolidBrush(Color.FromArgb(30, SystemColors.Highlight));
                g.FillRectangle(brush, 2, 2, Width - _dropDownWidth - 2, Height - 4);
            }
            else if (_leftState == ButtonState.Pressed)
            {
                using var brush = new SolidBrush(Color.FromArgb(50, SystemColors.ControlDark));
                g.FillRectangle(brush, 2, 2, Width - _dropDownWidth - 2, Height - 4);
            }

            // Right section highlight
            if (_rightState == ButtonState.Hot)
            {
                using var brush = new SolidBrush(Color.FromArgb(30, SystemColors.Highlight));
                g.FillRectangle(brush, Width - _dropDownWidth + 1, 2, _dropDownWidth - 4, Height - 4);
            }
            else if (_rightState == ButtonState.Pressed)
            {
                using var brush = new SolidBrush(Color.FromArgb(50, SystemColors.ControlDark));
                g.FillRectangle(brush, Width - _dropDownWidth + 1, 2, _dropDownWidth - 4, Height - 4);
            }
        }

        /// <summary>
        /// Determines the overall state of the button based on individual section states
        /// </summary>
        /// <returns>The highest priority state (Pressed > Hot > Normal)</returns>
        private ButtonState GetOverallState()
        {
            if (_leftState == ButtonState.Pressed || _rightState == ButtonState.Pressed) return ButtonState.Pressed;
            if (_leftState == ButtonState.Hot || _rightState == ButtonState.Hot) return ButtonState.Hot;
            return ButtonState.Normal;
        }

        /// <summary>
        /// Draws the vertical separator line between left and right sections
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        private void DrawSeparator(Graphics g)
        {
            int x = Width - _dropDownWidth;
            using var pen = new Pen(Color.FromArgb(100, SystemColors.ControlDark));
            g.DrawLine(pen, x, 4, x, Height - 5);
        }

        /// <summary>
        /// Draws the control using classic Windows style (non-themed rendering)
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        private void DrawClassicControl(Graphics g)
        {
            g.Clear(SystemColors.Control);

            var overallState = GetOverallState();
            var bgColor = overallState == ButtonState.Hot ? SystemColors.ControlLight :
                          overallState == ButtonState.Pressed ? SystemColors.ControlDark : SystemColors.Control;

            using (var brush = new SolidBrush(bgColor))
                g.FillRectangle(brush, ClientRectangle);

            var border3D = overallState == ButtonState.Pressed ? Border3DStyle.Sunken : Border3DStyle.Raised;
            ControlPaint.DrawBorder3D(g, ClientRectangle, border3D);

            // Left section highlight
            if (_leftState == ButtonState.Hot)
            {
                using var brush = new SolidBrush(SystemColors.ControlLight);
                g.FillRectangle(brush, 2, 2, Width - _dropDownWidth - 3, Height - 4);
            }
            else if (_leftState == ButtonState.Pressed)
            {
                using var brush = new SolidBrush(SystemColors.ControlDark);
                g.FillRectangle(brush, 2, 2, Width - _dropDownWidth - 3, Height - 4);
            }

            // Right section highlight
            if (_rightState == ButtonState.Hot)
            {
                using var brush = new SolidBrush(SystemColors.ControlLight);
                g.FillRectangle(brush, Width - _dropDownWidth + 1, 2, _dropDownWidth - 3, Height - 4);
            }
            else if (_rightState == ButtonState.Pressed)
            {
                using var brush = new SolidBrush(SystemColors.ControlDark);
                g.FillRectangle(brush, Width - _dropDownWidth + 1, 2, _dropDownWidth - 3, Height - 4);
            }
        }

        /// <summary>
        /// Draws the dropdown arrow in the right section
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="rect">Rectangle defining the dropdown area</param>
        private static void DrawArrow(Graphics g, Rectangle rect)
        {
            int arrowWidth = 7, arrowHeight = 4;
            int arrowX = rect.X + (rect.Width - arrowWidth) / 2;
            int arrowY = rect.Y + (rect.Height - arrowHeight) / 2;

            var points = new Point[] {
                new (arrowX, arrowY),
                new (arrowX + arrowWidth, arrowY),
                new (arrowX + arrowWidth / 2, arrowY + arrowHeight)
            };

            using var brush = new SolidBrush(SystemColors.ControlText);
            g.FillPolygon(brush, points);
        }

        #endregion

        #region Mouse Events

        /// <summary>
        /// Handles mouse movement to update hover states
        /// </summary>
        /// <param name="e">Mouse event arguments</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            UpdateHoverState(e.Location);
        }

        /// <summary>
        /// Resets hover states when mouse leaves the control
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _leftState = _rightState = ButtonState.Normal;
            Invalidate();
        }

        /// <summary>
        /// Handles mouse down to set pressed states
        /// </summary>
        /// <param name="e">Mouse event arguments</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _clickHandled = false;

            if (LeftRect.Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                _leftState = ButtonState.Pressed;
                Invalidate();
            }
            else if (RightRect.Contains(e.Location) && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right))
            {
                _rightState = ButtonState.Pressed;
                Invalidate();
            }
        }

        /// <summary>
        /// Handles click events to trigger appropriate actions
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (_clickHandled) return;
            _clickHandled = true;

            var mousePos = PointToClient(Cursor.Position);

            if (LeftRect.Contains(mousePos))
            {
                ButtonClick?.Invoke(this, EventArgs.Empty);
            }
            else if (RightRect.Contains(mousePos))
            {
                _contextMenu?.Show(this, new Point(0, Height));
            }

            _leftState = ButtonState.Normal;
            _rightState = ButtonState.Normal;
            Invalidate();
        }

        /// <summary>
        /// Handles mouse up to reset visual states
        /// </summary>
        /// <param name="e">Mouse event arguments</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            // OnClick will handle the action, only reset state if OnClick didn't fire
            if (!_clickHandled)
            {
                _leftState = ButtonState.Normal;
                _rightState = ButtonState.Normal;
                Invalidate();
            }
        }

        /// <summary>
        /// Updates the hover state for left and right sections based on mouse position
        /// </summary>
        /// <param name="location">Current mouse position relative to control</param>
        private void UpdateHoverState(Point location)
        {
            var newLeftState = LeftRect.Contains(location) ? ButtonState.Hot : ButtonState.Normal;
            var newRightState = RightRect.Contains(location) ? ButtonState.Hot : ButtonState.Normal;

            if (newLeftState != _leftState || newRightState != _rightState)
            {
                _leftState = newLeftState;
                _rightState = newRightState;
                Invalidate();
            }
        }

        #endregion

        #region P/Invoke for Windows Theming

        /// <summary>Opens theme data for the specified window and class</summary>
        /// <param name="hwnd">Window handle</param>
        /// <param name="pszClassList">Theme class name</param>
        /// <returns>Theme handle</returns>
        [LibraryImport("uxtheme.dll", StringMarshalling = StringMarshalling.Utf16)]
        private static partial IntPtr OpenThemeData(IntPtr hwnd, string pszClassList);

        /// <summary>Closes an open theme data handle</summary>
        /// <param name="hTheme">Theme handle to close</param>
        /// <returns>Status code</returns>
        [LibraryImport("uxtheme.dll")]
        private static partial int CloseThemeData(IntPtr hTheme);

        /// <summary>Draws themed background</summary>
        /// <param name="hTheme">Theme handle</param>
        /// <param name="hdc">Device context handle</param>
        /// <param name="iPartId">Part identifier</param>
        /// <param name="iStateId">State identifier</param>
        /// <param name="pRect">Drawing rectangle</param>
        /// <param name="pClipRect">Clipping rectangle (use IntPtr.Zero for none)</param>
        /// <returns>Status code</returns>
        [LibraryImport("uxtheme.dll")]
        private static partial int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr pClipRect);

        /// <summary>Rectangle structure for theme drawing</summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        // Button theme constants
        private const int BP_PUSHBUTTON = 1;  // Push button part
        private const int PBS_NORMAL = 1;     // Normal state
        private const int PBS_HOT = 2;        // Hot (hover) state
        private const int PBS_PRESSED = 3;    // Pressed state

        #endregion
    }
}