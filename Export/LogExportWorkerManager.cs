using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.Metadata;

namespace LogScraper.Export
{
    /// <summary>
    /// Manages the execution of log export workers, ensuring that only one worker is processed at a time.
    /// </summary>
    internal class LogExportWorkerManager
    {
        // Singleton instance of the LogExportWorkerManager.
        private static LogExportWorkerManager instance;

        // Lock object to ensure thread-safe initialization of the singleton instance.
        private static readonly Lock lockObject = new();

        /// <summary>
        /// Gets the singleton instance of the <see cref="LogExportWorkerManager"/>.
        /// </summary>
        public static LogExportWorkerManager Instance
        {
            get
            {
                // Check if an instance already exists.
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance.
                    lock (lockObject)
                    {
                        instance ??= new LogExportWorkerManager();
                    }
                }
                return instance;
            }
        }

        // Queue to hold workers and their associated data.
        private readonly Queue<(LogExporterWorker, LogMetadataFilterResult, LogExportSettings)> workerQueue = new();

        // Indicates whether the queue is currently being processed.
        private bool isProcessingQueue = false;

        /// <summary>
        /// Event triggered to notify listeners of the current queue length.
        /// </summary>
        public event Action<int> QueueLengthUpdate;

        /// <summary>
        /// Adds a new worker to the queue and starts processing if not already in progress.
        /// </summary>
        /// <param name="worker">The log exporter worker to add.</param>
        /// <param name="filterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">The settings to use for exporting the log.</param>
        public void AddWorker(LogExporterWorker worker, LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            // Enqueue the worker and its associated data.
            workerQueue.Enqueue((worker, filterResult, logExportSettings));

            // Start processing the queue if not already in progress.
            if (!isProcessingQueue)
            {
                StartNextWorker();
            }
        }

        /// <summary>
        /// Starts the next worker in the queue asynchronously.
        /// </summary>
        private async void StartNextWorker()
        {
            // Notify listeners of the updated queue length.
            OnQueueLengthUpdate();

            // If the queue is empty, stop processing.
            if (workerQueue.Count == 0)
            {
                isProcessingQueue = false;
                return;
            }

            isProcessingQueue = true;

            // Dequeue and discard all workers except the last one.
            while (workerQueue.Count > 1)
            {
                workerQueue.TryDequeue(out _);
            }

            // Dequeue the last worker and process it.
            var (worker, filterResult, logExportSettings) = workerQueue.Dequeue();
            await worker.DoWorkAsync(filterResult, logExportSettings);

            // Start the next worker when the current one finishes.
            StartNextWorker();
        }

        /// <summary>
        /// Triggers the <see cref="QueueLengthUpdate"/> event to notify listeners of the current queue length.
        /// </summary>
        protected virtual void OnQueueLengthUpdate()
        {
            QueueLengthUpdate?.Invoke(workerQueue.Count);
        }

        /// <summary>
        /// Opens the exported log file in an external editor.
        /// </summary>
        internal static void OpenFileInExternalEditor()
        {
            string fileName = GetExportFileName();

            // Check if the file exists.
            if (!File.Exists(fileName))
            {
                string path = Path.GetDirectoryName(fileName);

                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    // Path exists but the file is missing.
                    MessageBox.Show($"Bestand is niet gevonden: {fileName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Path is invalid or empty.
                    MessageBox.Show($"Er is een ongeldig locatie opgegeven voor het exporteren van het log: {fileName}, pas deze aan via de instellingen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            try
            {
                // Open the file in the configured external editor.
                Process.Start(ConfigurationManager.GenericConfig.EditorFileName, "\"" + fileName + "\"");
            }
            catch (Exception ex)
            {
                // Show an error message if the file cannot be opened.
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Writes the filtered log data to a file using the specified export settings.
        /// </summary>
        /// <param name="logMetadataFilterResult">The result of filtering log metadata.</param>
        /// <param name="logExportSettings">The settings to use for exporting the log.</param>
        internal static void WriteToFile(LogMetadataFilterResult logMetadataFilterResult, LogExportSettings logExportSettings)
        {
            if (ConfigurationManager.GenericConfig.ExportToFile)
            {
                // Create a new log exporter worker and add it to the manager.
                LogExporterWorker logExporter = new(GetExportFileName());
                LogExportWorkerManager.Instance.AddWorker(logExporter, logMetadataFilterResult, logExportSettings);
            }
        }

        /// <summary>
        /// Gets the export file name based on the current configuration and whether the debugger is attached.
        /// </summary>
        /// <returns></returns>
        private static string GetExportFileName()
        {
            // Determine the file name based on whether the debugger is attached.
            return Debugger.IsAttached ? AppContext.BaseDirectory + "Log.log" : ConfigurationManager.GenericConfig.ExportFileName;
        }
    }
}
