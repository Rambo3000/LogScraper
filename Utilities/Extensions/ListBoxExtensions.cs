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
            return true;
        }
    }
}