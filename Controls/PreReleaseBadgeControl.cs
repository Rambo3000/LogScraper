using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LogScraper.Utilities;

namespace LogScraper.Controls
{
    /// <summary>
    /// Displays a small badge when the running version is a pre-release (alpha/beta).
    /// The control hides itself automatically when the current version is a stable release.
    /// Renders with a rounded rectangle background and small-caps style uppercase text.
    /// </summary>
    public partial class PreReleaseBadgeControl : UserControl
    {
        private string _badgeText = "";
        private Color _badgeColor = Color.Gray;

        public PreReleaseBadgeControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            ResizeRedraw = true;
            ApplyPreReleaseInfo();
        }

        private void ApplyPreReleaseInfo()
        {
            (string label, int number, string fullVersion) = GitHubUpdateChecker.GetCurrentPreReleaseInfo();

            if (string.IsNullOrEmpty(label))
            {
                Visible = false;
                return;
            }

            string labelLower = "beta"; // label.ToLowerInvariant();
            _badgeText = number > 0 ? $"{label.ToUpperInvariant()} v{number}" : label.ToUpperInvariant();
            _badgeColor = labelLower switch
            {
                "alpha" => Color.FromArgb(195, 80, 90),
                "beta"  => Color.FromArgb(195, 140, 55),
                _       => Color.FromArgb(120, 90, 170)
            };

            ToolTip.SetToolTip(this, $"Pre-release versie: {label} versie {number} ({fullVersion})");

            Visible = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle rect = new(1, 3, Width - 3, Height - 7);
            int radius = 5;

            using GraphicsPath path = RoundedRect(rect, radius);
            using SolidBrush brush = new(_badgeColor);
            g.FillPath(brush, path);

            using Font font = new("Segoe UI", 7F, FontStyle.Bold);
            using SolidBrush textBrush = new(Color.FromArgb(240, 240, 240));
            StringFormat sf = new() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(_badgeText, font, textBrush, rect, sf);
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
