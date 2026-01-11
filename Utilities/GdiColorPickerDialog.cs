using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LogScraper.Utilities
{
    /// <summary>
    /// Provides a dialog for selecting a color using the standard Windows GDI color picker interface. The colors of the dialog match the scintilla GDI colors.
    /// </summary>
    /// <remarks>This class is intended for internal use and leverages the native Windows color picker dialog.</remarks>
    internal class GdiColorPickerDialog
    {

        [DllImport("comdlg32.dll", SetLastError = true)]
        static extern bool ChooseColor(ref CHOOSECOLOR chooseColor);

        private static readonly int[] CustomColors =
        {
            // Dark colors for text
            ToColorRef(Color.FromArgb(0, 97, 0)),      // dark green
            ToColorRef(Color.FromArgb(191, 144, 0)),   // dark yellow
            ToColorRef(Color.FromArgb(31, 78, 121)),   // dark blue
            ToColorRef(Color.FromArgb(156, 0, 6)),     // dark red
            ToColorRef(Color.FromArgb(64, 64, 64)),    // dark gray
            ToColorRef(Color.FromArgb(191, 97, 0)),    // dark orange
            ToColorRef(Color.FromArgb(112, 48, 160)),  // dark purple
            ToColorRef(Color.FromArgb(0, 64, 0)),      // darker green

            // Matching Light colors for background
            ToColorRef(Color.FromArgb(198, 239, 206)), // light green
            ToColorRef(Color.FromArgb(255, 235, 156)), // light yellow
            ToColorRef(Color.FromArgb(221, 235, 247)), // light blue
            ToColorRef(Color.FromArgb(244, 204, 204)), // light red
            ToColorRef(Color.FromArgb(217, 217, 217)), // light gray
            ToColorRef(Color.FromArgb(255, 242, 204)), // light orange
            ToColorRef(Color.FromArgb(234, 209, 220)), // light purple
            ToColorRef(Color.FromArgb(232, 245, 233)) // very light green
        };
        private static int ToColorRef(Color color)
        {
            return color.R | (color.G << 8) | (color.B << 16);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CHOOSECOLOR
        {
            public int Size;
            public IntPtr Owner;
            public IntPtr Instance;
            public int ResultRgb;
            public IntPtr CustomColors;
            public int Flags;
            public IntPtr CustomData;
            public IntPtr Hook;
            public IntPtr TemplateName;
        }
        /// <summary>
        /// Displays a color selection dialog box and returns the user's selection result.
        /// </summary>
        /// <remarks>If the user cancels the dialog, <paramref name="selectedColor"/> is set to <see
        /// langword="null"/>. This method blocks until the user closes the dialog box.</remarks>
        /// <param name="parentControl">The parent control that owns the dialog box. The dialog will be centered over this control. Cannot be null.</param>
        /// <param name="initialColor">The color initially selected when the dialog box is displayed.</param>
        /// <param name="selectedColor">When this method returns, contains the color selected by the user if the dialog result is <see
        /// cref="DialogResult.OK"/>; otherwise, <see langword="null"/>.</param>
        /// <returns>A <see cref="DialogResult"/> value indicating whether the user selected a color (<see
        /// cref="DialogResult.OK"/>) or canceled the dialog (<see cref="DialogResult.Cancel"/>).</returns>
        public static DialogResult ShowDialog(Control parentControl, Color initialColor, out Color? selectedColor)
        {
            IntPtr ownerHandle = parentControl.Handle;

            GCHandle customColorsHandle = GCHandle.Alloc(CustomColors, GCHandleType.Pinned);

            try
            {
                CHOOSECOLOR chooseColor = new()
                {
                    Size = Marshal.SizeOf<CHOOSECOLOR>(),
                    Owner = ownerHandle,
                    ResultRgb =
                        initialColor.R |
                        (initialColor.G << 8) |
                        (initialColor.B << 16),
                    CustomColors = customColorsHandle.AddrOfPinnedObject(),
                    Flags = 0x00000001 | 0x00000002 // CC_RGBINIT | CC_FULLOPEN
                };

                if (!ChooseColor(ref chooseColor))
                {
                    selectedColor = null;
                    return DialogResult.Cancel;
                }

                int rgb = chooseColor.ResultRgb;

                selectedColor = Color.FromArgb(rgb & 0xFF, (rgb >> 8) & 0xFF, (rgb >> 16) & 0xFF);

                return DialogResult.OK;
            }
            finally
            {
                customColorsHandle.Free();
            }
        }
    }
}
