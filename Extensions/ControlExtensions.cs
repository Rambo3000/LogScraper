using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LogScraper.Extensions
{
    public static class ControlExtensions
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        public static void SuspendDrawing(this Control control)
        {
            LockWindowUpdate(control.Handle);
        }

        public static void ResumeDrawing(this Control control)
        {
            LockWindowUpdate(IntPtr.Zero);
        }
    }
}
