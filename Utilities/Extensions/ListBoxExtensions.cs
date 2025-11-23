using System;
using System.Windows.Forms;

namespace LogScraper.Utilities.Extensions
{
    public static class ListBoxExtensions
    {
        /// <summary>
        /// Moves the selection in the specified ListBox to the next or previous item, optionally wrapping around at the
        /// boundaries.
        /// </summary>
        /// <remarks>If no item is currently selected, the method selects the first or last item depending
        /// on the direction specified by selectNext. If wrapAround is false and the selection is at the boundary, the
        /// selection does not change and the method returns false.</remarks>
        /// <param name="listBox">The ListBox control whose selection will be moved. Must not be null and must have SelectionMode set to One.</param>
        /// <param name="selectNext">true to move the selection to the next item; false to move to the previous item.</param>
        /// <param name="wrapAround">true to wrap the selection to the start or end of the list when the boundary is reached; otherwise, false.</param>
        /// <returns>true if the selection was successfully changed; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if listBox is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the listbox is set to multiselection.</exception>
        public static bool MoveSelection(this ListBox listBox, bool selectNext, bool wrapAround)
        {
            if (listBox == null) throw new ArgumentNullException(nameof(listBox));
            if (listBox.SelectionMode != SelectionMode.One)
            {
                throw new InvalidOperationException("MoveSelection method requires SelectionMode.One (single-select).");
            }
            if (listBox.Items == null || listBox.Items.Count == 0) return false;

            int currentIndex = listBox.SelectedIndex;

            // If nothing is selected yet, pick start or end depending on direction
            if (currentIndex < 0)
            {
                int initialIndex = selectNext ? 0 : listBox.Items.Count - 1;
                listBox.SelectedIndex = initialIndex;
                EnsureIndexVisible(listBox, initialIndex);
                return true;
            }

            int candidateIndex = selectNext ? currentIndex + 1 : currentIndex - 1;

            // Out of range handling
            if (candidateIndex < 0 || candidateIndex >= listBox.Items.Count)
            {
                if (!wrapAround) return false;
                candidateIndex = (candidateIndex < 0) ? listBox.Items.Count - 1 : 0;
            }

            if (candidateIndex == currentIndex) return false;

            listBox.SelectedIndex = candidateIndex;
            EnsureIndexVisible(listBox, candidateIndex);
            return true;
        }

        /// <summary>
        /// Ensures that the item at the specified index in the provided ListBox is visible to the user by adjusting the
        /// ListBox's scroll position if necessary.
        /// </summary>
        /// <remarks>If the specified index is already visible, no action is taken. If the index is
        /// outside the valid range of items, the method does nothing.</remarks>
        /// <param name="listBox">The ListBox control whose scroll position will be adjusted to make the specified item visible. Cannot be
        /// null.</param>
        /// <param name="index">The zero-based index of the item to ensure is visible. Must be within the bounds of the ListBox's items.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="listBox"/> is null.</exception>
        private static void EnsureIndexVisible(ListBox listBox, int index)
        {
            ArgumentNullException.ThrowIfNull(listBox);

            if (index < 0 || index >= listBox.Items.Count) return;

            int visible = VisibleItemCount(listBox);

            // Already visible?
            if (index >= listBox.TopIndex && index < listBox.TopIndex + visible) return;

            // If above view -> show at top; if below -> align at bottom
            if (index < listBox.TopIndex)
            {
                listBox.TopIndex = index;
                return;
            }

            int newTop = index - visible + 1;
            if (newTop < 0) newTop = 0;
            listBox.TopIndex = newTop;
        }

        /// <summary>
        /// Calculates the maximum number of items that can be fully displayed in the specified ListBox without
        /// scrolling.
        /// </summary>
        /// <param name="listBox">The ListBox control for which to determine the visible item count. Cannot be null.</param>
        /// <returns>The number of items that can be fully displayed in the ListBox at once. The value is always at least 1.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="listBox"/> is null.</exception>
        private static int VisibleItemCount(ListBox listBox)
        {
            if (listBox == null) throw new ArgumentNullException(nameof(listBox));

            int itemHeight = listBox.ItemHeight;
            if (itemHeight <= 0) itemHeight = 1;
            int count = listBox.ClientSize.Height / itemHeight;
            if (count < 1) count = 1;
            return count;
        }
    }
}