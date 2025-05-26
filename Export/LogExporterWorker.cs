using System;
using System.IO;
using System.Threading.Tasks;
using LogScraper.Log;
using LogScraper.Log.Metadata;

namespace LogScraper.Export
{
    /// <summary>
    /// Handles the export of log data to a file asynchronously.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LogExporterWorker"/> class.
    /// </remarks>
    /// <param name="logFilePath">The file path where the log will be exported.</param>
    internal class LogExporterWorker(string logFilePath)
    {
        // The file path where the log will be exported.
        private readonly string logFilePath = logFilePath;

        /// <summary>
        /// Event triggered to provide status updates during the export process.
        /// </summary>
        public event Action<string, bool> StatusUpdate;

        /// <summary>
        /// Performs the log export operation asynchronously.
        /// </summary>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">The settings to use for exporting the log.</param>
        public async Task DoWorkAsync(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            try
            {
                // Notify that the export process has started.
                OnStatusUpdate("Bezig met wegschrijven...", true);

                // Generate the log content based on the filter result and export settings.
                string exportedLog = LogDataExporter.CreateExportedLog(filterResult, logExportSettings, out int _);

                // Write the generated log content to the specified file asynchronously.
                await File.WriteAllTextAsync(logFilePath, exportedLog);

                // Notify that the export process completed successfully.
                OnStatusUpdate("Ok", true);
            }
            catch (Exception ex)
            {
                // Notify that an error occurred during the export process.
                OnStatusUpdate(ex.Message, false);
            }
        }

        /// <summary>
        /// Triggers the <see cref="StatusUpdate"/> event to notify listeners of the current status.
        /// </summary>
        /// <param name="message">The status message to send.</param>
        /// <param name="isSucces">Indicates whether the operation was successful.</param>
        protected virtual void OnStatusUpdate(string message, bool isSucces)
        {
            // Invoke the event, if there are any subscribers.
            StatusUpdate?.Invoke(message, isSucces);
        }
    }
}
