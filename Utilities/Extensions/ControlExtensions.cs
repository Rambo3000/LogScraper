using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LogScraper.Utilities.Extensions
{
    /// <summary>
    /// Provides extension methods for Windows Forms controls to manage drawing operations.
    /// </summary>
    public static partial class ControlExtensions
    {
        /// <summary>
        /// Temporarily suspends drawing updates for the specified control.
        /// This can improve performance when making multiple updates to the control.
        /// </summary>
        /// <param name="control">The control to suspend drawing for.</param>
        public static void SuspendDrawing(this Control control)
        {
            // Lock the window to prevent drawing updates.
            LockWindowUpdate(control.Handle);
        }

        /// <summary>
        /// Resumes drawing updates for the specified control.
        /// </summary>
        /// <param name="control">The control to resume drawing for.</param>
#pragma warning disable IDE0060 // Remove unused parameter
        public static void ResumeDrawing(this Control control)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            // Unlock the window to allow drawing updates.
            LockWindowUpdate(nint.Zero);
        }

        /// <summary>
        /// Locks or unlocks the window for drawing updates.
        /// </summary>
        /// <param name="hWndLock">The handle of the window to lock or unlock. Pass <see cref="nint.Zero"/> to unlock.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool LockWindowUpdate(nint hWndLock);
    }
}
