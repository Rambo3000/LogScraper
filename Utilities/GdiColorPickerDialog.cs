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

            int[] customColors = new int[16];
            GCHandle customColorsHandle = GCHandle.Alloc(customColors, GCHandleType.Pinned);

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
