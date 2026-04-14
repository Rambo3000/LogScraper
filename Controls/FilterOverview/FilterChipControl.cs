using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LogScraper.Log.Metadata;
using LogScraper.Log.Rendering;
using System.ComponentModel;

namespace LogScraper.Controls.FilterOverview
{
    #region Chip variant — determines color scheme and interaction behaviour

    public enum ChipVariant
    {
        /// <summary>Metadata filter chip — blue, has remove (×) button.</summary>
        Metadata,

        /// <summary>Log range filter chip — amber, has remove (×) button.</summary>
        Range,

        /// <summary>Error count chip — red, entire chip is clickable, chevron (›) on the right.</summary>
        Error
    }
    public enum LogRangeChipVariant
    {
        Begin,
        End
    }
    #endregion

    #region Color scheme per variant 
    internal readonly struct ChipColors(Color background, Color border, Color text, Color buttonHover)
    {
        public readonly Color Background = background;
        public readonly Color Border = border;
        public readonly Color Text = text;
        public readonly Color ButtonHover = buttonHover;

        internal static readonly ChipColors Metadata = new(
            background: Color.FromArgb(235, 242, 255), // very light blue-grey
            border: Color.FromArgb(180, 200, 230), // muted steel blue
            text: Color.FromArgb(80, 110, 160), // soft slate blue
            buttonHover: Color.FromArgb(40, 80, 110, 160));

        internal static readonly ChipColors Range = new(
            background: Color.FromArgb(255, 248, 235), // very light warm cream
            border: Color.FromArgb(210, 175, 110), // muted sand/amber
            text: Color.FromArgb(150, 110, 50), // soft warm brown
            buttonHover: Color.FromArgb(40, 150, 110, 50));

        internal static readonly ChipColors Error = new(
            background: Color.FromArgb(255, 238, 238), // very light blush
            border: Color.FromArgb(210, 160, 160), // muted dusty rose
            text: Color.FromArgb(160, 80, 80), // soft muted red
            buttonHover: Color.FromArgb(40, 160, 80, 80));

        internal static ChipColors ForVariant(ChipVariant variant) => variant switch
        {
            ChipVariant.Metadata => Metadata,
            ChipVariant.Range => Range,
            ChipVariant.Error => Error,
            _ => throw new ArgumentOutOfRangeException(nameof(variant))
        };
    }
    #endregion

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
        #region Fields and constructor
        private const int CornerRadius = 4;
        private const int ChipHeight = 18;
        private const int ButtonSize = 14;
        private const int HorizontalPad = 4;
        private const int InnerGap = 2;

        /// <summary>
        /// Raised when:
        ///   Metadata / Range — the remove (×) button is clicked.
        ///   Error            — anywhere on the chip is clicked.
        /// </summary>
        public event EventHandler ChipClicked;

        private readonly ChipVariant _variant;
        private readonly ChipColors _colors;
        private LogMetadataFilter _metadataFilter;
        private LogMetadataValue _specificValue;
        private LogRange _logRange;
        private LogRangeChipVariant _logRangeChipVariant;
        private int _errorCount;
        private string _labelText = string.Empty;
        private bool _buttonHover = false;
        private bool _chipHover = false;
        private Rectangle _buttonBounds;
        private Image _removeImage;

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
            Margin = new Padding(0, 0, 6, 3);
            Height = ChipHeight;

            if (variant == ChipVariant.Error)
                Cursor = Cursors.Hand;
        }
        #endregion

        #region Static constructor factory methods
        /// <summary>
        /// Optional icon for the remove button (Metadata / Range chips).
        /// When null a fallback × character is drawn.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image RemoveImage
        {
            get => _removeImage;
            set { _removeImage = value; Invalidate(); }
        }

        // -------------------------------------------------------------------------
        // Factory methods
        // -------------------------------------------------------------------------

        /// <summary>Creates a blue chip for a metadata filter (one chip represents all active values).</summary>
        public static FilterChipControl FromMetadataFilter(LogMetadataFilter filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            FilterChipControl chip = new(ChipVariant.Metadata)
            {
                _metadataFilter = filter
            };
            chip.UpdateLabel();
            return chip;
        }

        /// <summary>Creates a blue chip representing a single active value within a metadata filter.</summary>
        public static FilterChipControl FromMetadataFilterValue(LogMetadataFilter filter, LogMetadataValue value)
        {
            ArgumentNullException.ThrowIfNull(filter);
            ArgumentNullException.ThrowIfNull(value);

            FilterChipControl chip = new(ChipVariant.Metadata)
            {
                _metadataFilter = filter,
                _specificValue = value
            };
            chip.UpdateLabel();
            return chip;
        }

        /// <summary>Creates an amber chip for a log range filter.</summary>
        public static FilterChipControl FromLogRange(LogRange range, LogRangeChipVariant variant)
        {
            ArgumentNullException.ThrowIfNull(range);

            FilterChipControl chip = new(ChipVariant.Range)
            {
                _logRange = range,
                _logRangeChipVariant = variant
            };
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
        #endregion

        #region Public properties and methods
        public ChipVariant Variant => _variant;
        public LogMetadataFilter MetadataFilter => _metadataFilter;
        public LogMetadataValue SpecificValue => _specificValue;
        public LogRange LogRange => _logRange;
        public int ErrorCount => _errorCount;

        /// <summary>Rebuilds display text and recalculates chip width. Call after filter values change.</summary>
        public void UpdateLabel()
        {
            _labelText = _variant switch
            {
                ChipVariant.Metadata => BuildMetadataLabel(_metadataFilter, _specificValue),
                ChipVariant.Range => BuildRangeLabel(_logRange, _logRangeChipVariant),
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
        #endregion

        #region Label building
        private void RecalculateWidth()
        {
            int textWidth = TextRenderer.MeasureText(_labelText, Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width + 3;
            Width = HorizontalPad + textWidth + InnerGap + ButtonSize + HorizontalPad;
        }

        private static string BuildMetadataLabel(LogMetadataFilter filter, LogMetadataValue specificValue)
        {
            if (specificValue != null)
                return $"{filter.Property.Description}: {specificValue.Value}";

            int count = filter.ActiveValues.Count;
            if (count == 0)
                return filter.Property.Description;

            return $"{filter.Property.Description} ({count})";
        }

        private static string BuildRangeLabel(LogRange range, LogRangeChipVariant variant)
        {
            if (variant == LogRangeChipVariant.Begin)
            {
                string begin = range.Begin?.TimeStamp.ToString("HH:mm:ss");
                return $"Vanaf: {begin}";
            }
            else if (variant == LogRangeChipVariant.End)
            {
                string end = range.End?.TimeStamp.ToString("HH:mm:ss");
                return $"Tot: {end}";
            }
            return string.Empty ;
        }

        private static string BuildErrorLabel(int count) =>
            count == 1 ? "1 error" : $"{count} errors";
        #endregion

        #region Painting
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
            int iconY = (Height - ButtonSize) / 2 ;
            if (_variant == ChipVariant.Error) iconY -= 1;
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
                    Trimming = StringTrimming.None,
                    FormatFlags = StringFormatFlags.NoWrap
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
        #endregion

        #region Mouse interaction
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
        #endregion

        #region Helpers
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
        #endregion
    }
}