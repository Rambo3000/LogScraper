using System;
using System.Collections.Generic;
using System.Threading;
using LogScraper.Sources.Adapters;

namespace LogScraper.Sources.Workers
{
    internal class SourceProcessingManager
    {
        private static SourceProcessingManager instance;
        private static readonly Lock lockObject = new();
        public static SourceProcessingManager Instance
        {
            get
            {
                // Check if an instance already exists
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance
                    lock (lockObject)
                    {
                        instance ??= new SourceProcessingManager();
                    }
                }
                return instance;
            }
        }

        private readonly Queue<(SourceProcessingWorker, ISourceAdapter, int, int, CancellationTokenSource)> workerQueue = new();
        private bool isProcessingQueue = false;
        public event Action QueueLengthUpdate;
        private CancellationTokenSource currentWorkercancellationTokenSource;

        public void AddWorker(SourceProcessingWorker logExporterWorker, ISourceAdapter sourceAdapter, int intervalInSeconds, int durationInSeconds)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            workerQueue.Enqueue((logExporterWorker, sourceAdapter, intervalInSeconds, durationInSeconds, cancellationTokenSource));

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
            var (worker, sourceAdapter, intervalInSeconds, durationInSeconds, cancellationTokenSource) = workerQueue.Dequeue();
            currentWorkercancellationTokenSource = cancellationTokenSource;
            await worker.DoWorkAsync(sourceAdapter, intervalInSeconds, durationInSeconds, cancellationTokenSource.Token);
            currentWorkercancellationTokenSource = null;

            // Start the next worker when the current one finishes
            StartNextWorker();
        }
        public void CancelAllWorkers()
        {
            foreach (var (_, _, _, _, cancellationTokenSource) in workerQueue)
            {
                cancellationTokenSource.Cancel();
            }
            currentWorkercancellationTokenSource?.Cancel();
        }
        protected virtual void OnQueueLengthUpdate()
        {
            // Raise the event on the UI thread
            QueueLengthUpdate?.Invoke();
        }
        public int QueueLength
        {
            get
            {
                return workerQueue.Count;
            }
        }
    }
}
