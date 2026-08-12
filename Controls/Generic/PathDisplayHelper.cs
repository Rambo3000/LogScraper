using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LogScraper.Controls.Generic
{
    /// <summary>
    /// Helper for producing a shortened, display-friendly version of a full file path,
    /// similar to how Windows Explorer/common dialogs compact long paths (e.g. for menu
    /// items and tooltips).
    /// </summary>
    internal static class PathDisplayHelper
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern bool PathCompactPathEx(StringBuilder pszOut, string pszSrc, int cchMax, int dwFlags);

        /// <summary>
        /// Shortens the given path to fit within approximately <paramref name="maxLength"/>
        /// characters, using ellipsis in the middle of the path (e.g. "C:\Users\...\log.txt").
        /// Falls back to a simple manual truncation if the native API call fails.
        /// </summary>
        /// <param name="path">The full path to shorten.</param>
        /// <param name="maxLength">The maximum desired display length, including the ellipsis.</param>
        public static string ShortenPath(string path, int maxLength = 60)
        {
            if (string.IsNullOrEmpty(path)) return path;
            if (path.Length <= maxLength) return path;

            try
            {
                StringBuilder buffer = new(maxLength + 1);
                if (PathCompactPathEx(buffer, path, maxLength + 1, 0))
                {
                    return buffer.ToString();
                }
            }
            catch
            {
                // Fall through to the manual fallback below if the native call is unavailable.
            }

            return ManualShorten(path, maxLength);
        }

        /// <summary>
        /// Simple fallback that keeps the start and end of the path, replacing the middle with an ellipsis.
        /// </summary>
        private static string ManualShorten(string path, int maxLength)
        {
            if (path.Length <= maxLength) return path;

            const string ellipsis = "...";
            int keep = maxLength - ellipsis.Length;
            if (keep <= 0) return path[..maxLength];

            int startLength = keep / 2;
            int endLength = keep - startLength;

            return path[..startLength] + ellipsis + path[^endLength..];
        }
    }
}
