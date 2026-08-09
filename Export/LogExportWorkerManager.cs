using System;
using System.Threading.Tasks;

namespace LogScraper.Export
{
    /// <summary>
    /// Coordinates on-demand export of the log to a user-chosen file, run on a background thread,
    /// and notifies listeners (e.g. a status bar progress indicator) while the write is in progress.
    /// </summary>
    internal static class LogExportWorkerManager
    {
        /// <summary>
        /// Event triggered whenever an on-demand export starts (<c>true</c>) or finishes (<c>false</c>),
        /// so the UI can show/hide a progress indicator.
        /// </summary>
        public static event Action<bool> IsWritingChanged;

        /// <summary>
        /// Writes the given log content to <paramref name="filePath"/> on a background thread.
        /// </summary>
        /// <param name="log">The rendered log content to write.</param>
        /// <param name="filePath">The destination file path, typically chosen by the user via a save dialog.</param>
        /// <returns><c>true</c> if the file was written successfully; otherwise <c>false</c>.</returns>
        public static async Task<bool> WriteToFileAsync(string log, string filePath)
        {
            OnIsWritingChanged(true);
            try
            {
                LogExporterWorker logExporter = new(filePath);
                bool success = false;
                logExporter.StatusUpdate += (_, isSuccess) => success = isSuccess;

                // Run the write on a background thread so the UI thread is never blocked.
                await Task.Run(() => logExporter.DoWorkAsync(log)).ConfigureAwait(false);

                return success;
            }
            finally
            {
                OnIsWritingChanged(false);
            }
        }

        private static void OnIsWritingChanged(bool isWriting)
        {
            IsWritingChanged?.Invoke(isWriting);
        }
    }
}
