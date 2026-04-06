using System;
using System.Drawing;
using System.Windows.Forms;

namespace LogScraper.Utilities.Extensions
{
    public class SplitContainerWithGrip : SplitContainer
    {
        public SplitContainerWithGrip()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SplitterWidth = 8;

            this.SplitterMoved += OnSplitterMoved;
            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            base.OnPaint(paintEventArgs);

            DrawSplitterGrip(paintEventArgs.Graphics);
        }

        protected override void OnMouseMove(MouseEventArgs mouseEventArgs)
        {
            base.OnMouseMove(mouseEventArgs);
        }

        private void OnSplitterMoved(object sender, SplitterEventArgs splitterEventArgs)
        {
            this.Invalidate();
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            this.Invalidate();
        }

        private void DrawSplitterGrip(Graphics graphics)
        {
            Rectangle splitterRectangle;

            if (this.Orientation == Orientation.Vertical)
            {
                splitterRectangle = new Rectangle(
                    this.SplitterDistance,
                    0,
                    this.SplitterWidth,
                    this.Height);
            }
            else
            {
                splitterRectangle = new Rectangle(
                    0,
                    this.SplitterDistance,
                    this.Width,
                    this.SplitterWidth);
            }

            int dotSize = 2;
            int spacing = 4;
            int dotCount = 5;

            int centerX = splitterRectangle.X + splitterRectangle.Width / 2;
            int centerY = splitterRectangle.Y + splitterRectangle.Height / 2;

            using (SolidBrush brush = new SolidBrush(Color.DarkGray))
            {
                for (int i = -dotCount / 2; i <= dotCount / 2; i++)
                {
                    Rectangle dot;

                    if (this.Orientation == Orientation.Vertical)
                    {
                        dot = new Rectangle(
                            centerX - dotSize / 2,
                            centerY + i * spacing,
                            dotSize,
                            dotSize);
                    }
                    else
                    {
                        dot = new Rectangle(
                            centerX + i * spacing,
                            centerY - dotSize / 2,
                            dotSize,
                            dotSize);
                    }

                    graphics.FillRectangle(brush, dot);
                }
            }
        }
    }
}
