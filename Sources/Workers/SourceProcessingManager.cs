using System;
using System.Collections.Generic;
using System.Threading;
using LogScraper.Sources.Adapters;

namespace LogScraper.Sources.Workers
{
    /// <summary>
    /// Manages the processing of source workers in a queue, ensuring that only one worker is processed at a time.
    /// </summary>
    /// <remarks>
    /// This class implements a singleton pattern to ensure a single instance manages all source processing workers.
    /// It supports adding workers to a queue, processing them sequentially, and canceling all workers if needed.
    /// </remarks>
    internal class SourceProcessingManager
    {
        // Singleton instance of the SourceProcessingManager.
        private static SourceProcessingManager instance;

        // Lock object to ensure thread-safe initialization of the singleton instance.
        private static readonly Lock lockObject = new();

        /// <summary>
        /// Gets the singleton instance of the <see cref="SourceProcessingManager"/>.
        /// </summary>
        public static SourceProcessingManager Instance
        {
            get
            {
                // Check if an instance already exists.
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance.
                    lock (lockObject)
                    {
                        instance ??= new SourceProcessingManager();
                    }
                }
                return instance;
            }
        }

        // Queue to hold workers and their associated data.
        private readonly Queue<(SourceProcessingWorker, ISourceAdapter, int, int, CancellationTokenSource)> workerQueue = new();

        // Indicates whether the queue is currently being processed.
        private bool isProcessingQueue = false;

        /// <summary>
        /// Event triggered to notify listeners of the current queue length.
        /// </summary>
        public event Action QueueLengthUpdate;

        // Cancellation token source for the currently active worker.
        private CancellationTokenSource currentWorkercancellationTokenSource;

        /// <summary>
        /// Adds a new worker to the queue and starts processing if not already in progress.
        /// </summary>
        /// <param name="logExporterWorker">The worker responsible for processing the source.</param>
        /// <param name="sourceAdapter">The source adapter to retrieve logs from.</param>
        /// <param name="intervalInSeconds">The interval between log retrievals, in seconds.</param>
        /// <param name="durationInSeconds">The total duration for which the worker should run, in seconds.</param>
        public void AddWorker(SourceProcessingWorker logExporterWorker, ISourceAdapter sourceAdapter, int intervalInSeconds, int durationInSeconds)
        {
            // Create a cancellation token source for the worker.
            var cancellationTokenSource = new CancellationTokenSource();

            // Enqueue the worker and its associated data.
            workerQueue.Enqueue((logExporterWorker, sourceAdapter, intervalInSeconds, durationInSeconds, cancellationTokenSource));

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

            // Dequeue the next worker and process it.
            var (worker, sourceAdapter, intervalInSeconds, durationInSeconds, cancellationTokenSource) = workerQueue.Dequeue();
            currentWorkercancellationTokenSource = cancellationTokenSource;

            // Execute the worker's task asynchronously.
            await worker.DoWorkAsync(sourceAdapter, intervalInSeconds, durationInSeconds, cancellationTokenSource.Token);

            // Clear the current worker's cancellation token source after completion.
            currentWorkercancellationTokenSource = null;

            // Start the next worker when the current one finishes.
            StartNextWorker();
        }

        /// <summary>
        /// Cancels all workers in the queue and the currently active worker.
        /// </summary>
        public void CancelAllWorkers()
        {
            // Cancel all workers in the queue.
            foreach (var (_, _, _, _, cancellationTokenSource) in workerQueue)
            {
                cancellationTokenSource.Cancel();
            }

            // Cancel the currently active worker, if any.
            currentWorkercancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Triggers the <see cref="QueueLengthUpdate"/> event to notify listeners of the current queue length.
        /// </summary>
        protected virtual void OnQueueLengthUpdate()
        {
            // Raise the event on the UI thread.
            QueueLengthUpdate?.Invoke();
        }

        /// <summary>
        /// Gets a value indicating whether the worker queue is currently active.
        /// </summary>
        public bool IsWorkerActive
        {
            get
            {
                return workerQueue.Count > 0;
            }
        }
    }
}
