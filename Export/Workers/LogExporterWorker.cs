using LogScraper.Log;
using LogScraper.Log.Metadata;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LogScraper.Export.Workers
{
    internal class LogExporterWorker(string logFilePath)
    {
        private readonly string logFilePath = logFilePath;
        public event Action<string, bool> StatusUpdate;

        public async Task DoWorkAsync(LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            try
            {
                OnStatusUpdate("Bezig met wegschrijven...", true);
                // Concatenate log entries into a single string
                LogExportData logExportData = LogDataExporter.GenerateExportedLogData(filterResult, logExportSettings, false);

                // Write the entire log content to the file asynchronously
                await File.WriteAllTextAsync(logFilePath, logExportData.ExportRaw);
                OnStatusUpdate("Ok", true);
            }
            catch (Exception ex)
            {
                // Raise the ExceptionOccurred event with the exception
                OnStatusUpdate(ex.Message, false);
            }
        }

        protected virtual void OnStatusUpdate(string message, bool isSucces)
        {
            // Raise the event on the UI thread
            StatusUpdate?.Invoke(message, isSucces);
        }
    }
}
