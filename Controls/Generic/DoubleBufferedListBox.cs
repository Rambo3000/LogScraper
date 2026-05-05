using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LogScraper.Controls.Generic
{
    /// <summary>
    /// A ListBox with double buffering enabled to prevent flicker during owner-draw operations.
    /// </summary>
    internal class DoubleBufferedListBox : ListBox
    {
        public DoubleBufferedListBox()
        {
            DoubleBuffered = true;
        }

        protected override void WndProc(ref Message m)
        {
            // Suppress WM_ERASEBKGND to eliminate the background-erase flash before each owner-draw repaint.
            if (m.Msg == 0x0014)
            {
                m.Result = (nint)1;
                return;
            }
            base.WndProc(ref m);
            // After each WM_PAINT, clear any empty area below the last item.
            if (m.Msg == 0x000F)
                ClearEmptyArea();
        }

        /// <summary>
        /// Clears the empty area below the last item, if any. Call after the item list shrinks.
        /// </summary>
        public void ClearEmptyArea()
        {
            int lastItemBottom = Items.Count > 0 ? GetItemRectangle(Items.Count - 1).Bottom : 0;
            // lastItemBottom <= 0 means the last item is scrolled above the visible area,
            // so all visible rows are real items and there is nothing to clear.
            if (lastItemBottom > 0 && lastItemBottom < ClientSize.Height)
            {
                using Graphics g = CreateGraphics();
                g.FillRectangle(SystemBrushes.Control,
                    0, lastItemBottom, ClientSize.Width, ClientSize.Height - lastItemBottom);
            }
        }

        /// <summary>
        /// Invalidates only the list items at the specified indices, avoiding a full control repaint.
        /// </summary>
        public void InvalidateItems(IEnumerable<int> indices)
        {
            foreach (int i in indices)
            {
                if (i >= 0 && i < Items.Count)
                    Invalidate(GetItemRectangle(i));
            }
        }
    }
}
