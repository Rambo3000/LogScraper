using LogScraper.Configuration;
using LogScraper.Log.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LogScraper.Export.Workers
{
    internal class LogExportWorkerManager
    {
        private static LogExportWorkerManager instance;
        private static readonly Lock lockObject = new();
        public static LogExportWorkerManager Instance
        {
            get
            {
                // Check if an instance already exists
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance
                    lock (lockObject)
                    {
                        instance ??= new LogExportWorkerManager();
                    }
                }
                return instance;
            }
        }

        private readonly Queue<(LogExporterWorker, LogMetadataFilterResult, LogExportSettings)> workerQueue = new();
        private bool isProcessingQueue = false;
        public event Action<int> QueueLengthUpdate;

        public void AddWorker(LogExporterWorker worker, LogMetadataFilterResult filterResult, LogExportSettings logExportSettings)
        {
            workerQueue.Enqueue((worker, filterResult, logExportSettings));

            if (!isProcessingQueue)
            {
                StartNextWorker();
            }
        }
        private async void StartNextWorker()
        {
            OnQueueLengthUpdate();
            if (workerQueue.Count == 0)
            {
                isProcessingQueue = false;
                return;
            }

            isProcessingQueue = true;

            // Dequeue and discard all workers except the last one
            while (workerQueue.Count > 1)
            {
                workerQueue.TryDequeue(out _);
            }

            var (worker, filterResult, logExportSettings) = workerQueue.Dequeue();
            await worker.DoWorkAsync(filterResult, logExportSettings);

            // Start the next worker when the current one finishes
            StartNextWorker();
        }
        protected virtual void OnQueueLengthUpdate()
        {
            QueueLengthUpdate?.Invoke(workerQueue.Count);
        }
        internal static void OpenFileInExternalEditor()
        {
            string fileName = Debugger.IsAttached ? AppContext.BaseDirectory + "Log.log" : ConfigurationManager.GenericConfig.ExportFileName;
            if (!File.Exists(fileName))
            {
                string path = Path.GetDirectoryName(fileName);

                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    // Path bestaat
                    MessageBox.Show($"Bestand is niet gevonden: {fileName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Path bestaat niet of was leeg
                    MessageBox.Show($"Er is een ongeldig locatie opgegeven voor het exporteren van het log: {fileName}, pas deze aan via de instellingen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            try
            {
                Process.Start(ConfigurationManager.GenericConfig.EditorFileName, "\"" + fileName + "\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void WriteToFile(LogMetadataFilterResult logMetadataFilterResult, LogExportSettings logExportSettings)
        {
            if (ConfigurationManager.GenericConfig.ExportToFile)
            {
                string fileName = Debugger.IsAttached ? AppContext.BaseDirectory + "Log.log" : ConfigurationManager.GenericConfig.ExportFileName;
                LogExporterWorker logExporter = new(fileName);
                LogExportWorkerManager.Instance.AddWorker(logExporter, logMetadataFilterResult, logExportSettings);
            }
        }
    }
}
