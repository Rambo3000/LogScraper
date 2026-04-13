using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LogScraper.Log.Metadata;
using LogScraper.Log.Rendering;

namespace LogScraper.Controls.FilterOverview
{
    // -------------------------------------------------------------------------
    // Chip variant — determines color scheme and interaction behaviour
    // -------------------------------------------------------------------------

    public enum ChipVariant
    {
        /// <summary>Metadata filter chip — blue, has remove (×) button.</summary>
        Metadata,

        /// <summary>Log range filter chip — amber, has remove (×) button.</summary>
        Range,

        /// <summary>Error count chip — red, entire chip is clickable, chevron (›) on the right.</summary>
        Error
    }

    // -------------------------------------------------------------------------
    // Color scheme per variant
    // -------------------------------------------------------------------------

    internal readonly struct ChipColors
    {
        public readonly Color Background;
        public readonly Color Border;
        public readonly Color Text;
        public readonly Color ButtonHover;

        public ChipColors(Color background, Color border, Color text, Color buttonHover)
        {
            Background = background;
            Border = border;
            Text = text;
            ButtonHover = buttonHover;
        }

        internal static readonly ChipColors Metadata = new ChipColors(
            background: Color.FromArgb(219, 234, 254), // light blue
            border: Color.FromArgb(59, 130, 246), // steel blue
            text: Color.FromArgb(29, 78, 216), // dark blue
            buttonHover: Color.FromArgb(60, 29, 78, 216));

        internal static readonly ChipColors Range = new ChipColors(
            background: Color.FromArgb(254, 243, 199), // light amber
            border: Color.FromArgb(217, 119, 6), // amber
            text: Color.FromArgb(146, 64, 14), // dark amber
            buttonHover: Color.FromArgb(60, 146, 64, 14));

        internal static readonly ChipColors Error = new ChipColors(
            background: Color.FromArgb(254, 226, 226), // light red
            border: Color.FromArgb(220, 38, 38), // red
            text: Color.FromArgb(153, 27, 27), // dark red
            buttonHover: Color.FromArgb(60, 153, 27, 27));

        internal static ChipColors ForVariant(ChipVariant variant) => variant switch
        {
            ChipVariant.Metadata => Metadata,
            ChipVariant.Range => Range,
            ChipVariant.Error => Error,
            _ => throw new ArgumentOutOfRangeException(nameof(variant))
        };
    }

    // -------------------------------------------------------------------------
    // FilterChipControl
    // -------------------------------------------------------------------------

    /// <summary>
    /// A rounded chip control representing an active filter or error count.
    /// Fully owner-drawn — no designer file needed.
    ///
    /// Variants:
    ///   Metadata — blue,  remove (×) button on the right
    ///   Range    — amber, remove (×) button on the right
    ///   Error    — red,   chevron (›) on the right, entire chip is clickable
    ///
    /// Use the factory methods to create instances.
    /// Subscribe to <see cref="ChipClicked"/> for both remove and navigation actions.
    /// </summary>
    public class FilterChipControl : UserControl
    {
        // -------------------------------------------------------------------------
        // Constants
        // -------------------------------------------------------------------------

        private const int CornerRadius = 4;
        private const int ChipHeight = 20;
        private const int ButtonSize = 14;
        private const int HorizontalPad = 4;
        private const int InnerGap = 2;

        // -------------------------------------------------------------------------
        // Events
        // -------------------------------------------------------------------------

        /// <summary>
        /// Raised when:
        ///   Metadata / Range — the remove (×) button is clicked.
        ///   Error            — anywhere on the chip is clicked.
        /// </summary>
        public event EventHandler ChipClicked;

        // -------------------------------------------------------------------------
        // Fields
        // -------------------------------------------------------------------------

        private readonly ChipVariant _variant;
        private readonly ChipColors _colors;
        private LogMetadataFilter _metadataFilter;
        private LogRange _logRange;
        private int _errorCount;
        private string _labelText = string.Empty;
        private bool _buttonHover = false;
        private bool _chipHover = false;
        private Rectangle _buttonBounds;
        private Image _removeImage;

        // -------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------

        public ChipVariant Variant => _variant;
        public LogMetadataFilter MetadataFilter => _metadataFilter;
        public LogRange LogRange => _logRange;
        public int ErrorCount => _errorCount;

        /// <summary>
        /// Optional icon for the remove button (Metadata / Range chips).
        /// When null a fallback × character is drawn.
        /// </summary>
        public Image RemoveImage
        {
            get => _removeImage;
            set { _removeImage = value; Invalidate(); }
        }

        // -------------------------------------------------------------------------
        // Factory methods
        // -------------------------------------------------------------------------

        /// <summary>Creates a blue chip for a metadata filter.</summary>
        public static FilterChipControl FromMetadataFilter(LogMetadataFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var chip = new FilterChipControl(ChipVariant.Metadata);
            chip._metadataFilter = filter;
            chip.UpdateLabel();
            return chip;
        }

        /// <summary>Creates an amber chip for a log range filter.</summary>
        public static FilterChipControl FromLogRange(LogRange range)
        {
            if (range == null) throw new ArgumentNullException(nameof(range));

            var chip = new FilterChipControl(ChipVariant.Range);
            chip._logRange = range;
            chip.UpdateLabel();
            return chip;
        }

        /// <summary>
        /// Creates a red error count chip.
        /// The entire chip is clickable — subscribe to <see cref="ChipClicked"/> to handle navigation.
        /// </summary>
        public static FilterChipControl FromErrorCount(int errorCount)
        {
            var chip = new FilterChipControl(ChipVariant.Error);
            chip.SetErrorCount(errorCount);
            return chip;
        }

        // -------------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------------

        private FilterChipControl(ChipVariant variant)
        {
            _variant = variant;
            _colors = ChipColors.ForVariant(variant);

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point);
            BackColor = Color.Transparent;
            Margin = new Padding(0, 0, 6, 0);
            Height = ChipHeight;

            if (variant == ChipVariant.Error)
                Cursor = Cursors.Hand;
        }

        // -------------------------------------------------------------------------
        // Public update methods
        // -------------------------------------------------------------------------

        /// <summary>Rebuilds display text and recalculates chip width. Call after filter values change.</summary>
        public void UpdateLabel()
        {
            _labelText = _variant switch
            {
                ChipVariant.Metadata => BuildMetadataLabel(_metadataFilter),
                ChipVariant.Range => BuildRangeLabel(_logRange),
                ChipVariant.Error => BuildErrorLabel(_errorCount),
                _ => string.Empty
            };

            RecalculateWidth();
            Invalidate();
        }

        /// <summary>Updates the error count and redraws. Only meaningful for Error variant.</summary>
        public void SetErrorCount(int count)
        {
            _errorCount = count;
            UpdateLabel();
        }

        // -------------------------------------------------------------------------
        // Label building
        // -------------------------------------------------------------------------

        private void RecalculateWidth()
        {
            using var graphics = CreateGraphics();
            float textWidth = graphics.MeasureString(_labelText, Font).Width;

            // HorizontalPad | text | InnerGap | ButtonSize | HorizontalPad
            Width = (int)Math.Ceiling(HorizontalPad + textWidth + InnerGap + ButtonSize + HorizontalPad);
        }

        private static string BuildMetadataLabel(LogMetadataFilter filter)
        {
            int count = filter.ActiveValues.Count;

            if (count == 0)
                return filter.Property.Description;

            if (count == 1)
                foreach (var key in filter.ActiveValues.Keys)
                    return $"{filter.Property.Description}: {key.Value}";

            return $"{filter.Property.Description} ({count})";
        }

        private static string BuildRangeLabel(LogRange range)
        {
            if (range == null) return "Range";

            string begin = range.Begin?.TimeStamp.ToString("HH:mm:ss") ?? "?";
            string end = range.End?.TimeStamp.ToString("HH:mm:ss") ?? "?";

            return $"Range: {begin} – {end}";
        }

        private static string BuildErrorLabel(int count) =>
            count == 1 ? "1 error" : $"{count} errors";

        // -------------------------------------------------------------------------
        // Painting
        // -------------------------------------------------------------------------

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);

            // Slightly lighten background on hover for the error chip
            Color background = (_variant == ChipVariant.Error && _chipHover)
                ? LightenColor(_colors.Background, 0.05f)
                : _colors.Background;

            // Background
            using (var path = RoundedRectangle(bounds, CornerRadius))
            using (var brush = new SolidBrush(background))
                g.FillPath(brush, path);

            // Border
            using (var path = RoundedRectangle(bounds, CornerRadius))
            using (var pen = new Pen(_colors.Border, 1f))
                g.DrawPath(pen, path);

            // Icon bounds — right-aligned, vertically centred
            int iconX = Width - HorizontalPad - ButtonSize;
            int iconY = (Height - ButtonSize) / 2;
            _buttonBounds = new Rectangle(iconX, iconY, ButtonSize, ButtonSize);

            // Label text
            var textArea = new RectangleF(
                HorizontalPad,
                0,
                Width - HorizontalPad - InnerGap - ButtonSize - HorizontalPad,
                Height);

            using (var brush = new SolidBrush(_colors.Text))
                g.DrawString(_labelText, Font, brush, textArea, new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                });

            // Button hover highlight (Metadata / Range only — Error highlights the whole chip)
            if (_buttonHover && _variant != ChipVariant.Error)
            {
                using var brush = new SolidBrush(_colors.ButtonHover);
                using var path = RoundedRectangle(_buttonBounds, _buttonBounds.Height / 2);
                g.FillPath(brush, path);
            }

            DrawRightIcon(g);
        }

        private void DrawRightIcon(Graphics g)
        {
            string icon = _variant == ChipVariant.Error ? "›" : "×";
            float fontSize = _variant == ChipVariant.Error ? 10f : 9f;

            if (_variant != ChipVariant.Error && _removeImage != null)
            {
                g.DrawImage(_removeImage, _buttonBounds);
                return;
            }

            using var brush = new SolidBrush(_colors.Text);
            g.DrawString(icon, new Font("Segoe UI", fontSize, FontStyle.Bold), brush, _buttonBounds,
                new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
        }

        // -------------------------------------------------------------------------
        // Mouse handling
        // -------------------------------------------------------------------------

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_variant == ChipVariant.Error) return; // whole chip is the target

            bool overButton = _buttonBounds.Contains(e.Location);
            if (overButton == _buttonHover) return;

            _buttonHover = overButton;
            Cursor = overButton ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (_variant != ChipVariant.Error) return;
            _chipHover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (_variant == ChipVariant.Error)
            {
                _chipHover = false;
                Invalidate();
                return;
            }

            if (!_buttonHover) return;
            _buttonHover = false;
            Cursor = Cursors.Default;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Left) return;

            bool shouldFire = _variant == ChipVariant.Error
                || _buttonBounds.Contains(e.Location);

            if (shouldFire)
                ChipClicked?.Invoke(this, EventArgs.Empty);
        }

        // -------------------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------------------

        private static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private static Color LightenColor(Color color, float amount)
        {
            float factor = 1f + Math.Clamp(amount, 0f, 1f);
            return Color.FromArgb(
                color.A,
                Math.Min(255, (int)(color.R * factor)),
                Math.Min(255, (int)(color.G * factor)),
                Math.Min(255, (int)(color.B * factor)));
        }
    }
}