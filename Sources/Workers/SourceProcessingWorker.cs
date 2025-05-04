using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LogScraper.Sources.Adapters;

namespace LogScraper.Sources.Workers
{
    /// <summary>
    /// Represents a worker responsible for processing log data from a source adapter.
    /// </summary>
    /// <remarks>
    /// This class handles retrieving log data at specified intervals and durations, raising events to notify
    /// listeners of progress, completion, and errors.
    /// </remarks>
    internal class SourceProcessingWorker
    {
        /// <summary>
        /// Event triggered to notify listeners of status updates, such as errors or success messages.
        /// </summary>
        public event Action<string, bool> StatusUpdate;

        /// <summary>
        /// Event triggered when log data has been successfully downloaded.
        /// </summary>
        public event Action<string[], DateTime?> DownloadCompleted;

        /// <summary>
        /// Event triggered to notify listeners of progress updates during the worker's execution.
        /// </summary>
        public event Action<int, int> ProgressUpdate;

        /// <summary>
        /// Executes the worker asynchronously, retrieving log data from the source adapter at specified intervals.
        /// </summary>
        /// <param name="sourceAdapter">The source adapter to retrieve log data from.</param>
        /// <param name="intervalInSeconds">The interval between log retrievals, in seconds.</param>
        /// <param name="durationInSeconds">The total duration for which the worker should run, in seconds. Use -1 for a single retrieval.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        public async Task DoWorkAsync(ISourceAdapter sourceAdapter, int intervalInSeconds, int durationInSeconds, CancellationToken cancellationToken)
        {
            try
            {
                if (durationInSeconds == -1)
                {
                    // Perform a single log retrieval if duration is -1.
                    OnProgressUpdate(0, durationInSeconds);
                    await GetLogFromSourceAdapter(sourceAdapter);
                }
                else
                {
                    // Perform log retrievals at regular intervals for the specified duration.
                    OnProgressUpdate(0, durationInSeconds);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (stopwatch.ElapsedMilliseconds / 1000 < durationInSeconds)
                    {
                        if (cancellationToken.IsCancellationRequested) return;

                        await GetLogFromSourceAdapter(sourceAdapter);

                        // Wait for the specified interval before the next retrieval.
                        await Task.Delay(intervalInSeconds * 1000, CancellationToken.None);

                        // Update progress based on elapsed time.
                        OnProgressUpdate(Convert.ToInt32(stopwatch.ElapsedMilliseconds / 1000), durationInSeconds);
                    }
                }
            }
            catch (Exception ex)
            {
                // Notify listeners of any exceptions that occur during execution.
                OnExceptionOccurred(ex.Message, false);
            }
        }

        /// <summary>
        /// Retrieves log data from the source adapter and raises the <see cref="DownloadCompleted"/> event.
        /// </summary>
        /// <param name="sourceAdapter">The source adapter to retrieve log data from.</param>
        private async Task GetLogFromSourceAdapter(ISourceAdapter sourceAdapter)
        {
            // Retrieve the raw log data as a string.
            string rawLog = await sourceAdapter.GetLogAsync();

            // Split the raw log data into an array of lines.
            string[] rawLogArray = rawLog.Split([Environment.NewLine, "\n", "\r"], StringSplitOptions.None);

            // Notify listeners that the log data has been downloaded.
            OnDownloadCompleted(rawLogArray, sourceAdapter.GetLastTrailTime());
        }

        /// <summary>
        /// Raises the <see cref="StatusUpdate"/> event to notify listeners of an exception or status message.
        /// </summary>
        /// <param name="message">The status or error message.</param>
        /// <param name="isSucces">Indicates whether the operation was successful.</param>
        protected virtual void OnExceptionOccurred(string message, bool isSucces)
        {
            StatusUpdate?.Invoke(message, isSucces);
        }

        /// <summary>
        /// Raises the <see cref="DownloadCompleted"/> event to notify listeners that log data has been downloaded.
        /// </summary>
        /// <param name="rawLog">The downloaded log data as an array of lines.</param>
        /// <param name="lastTrailTime">The timestamp of the last log trail, if applicable.</param>
        protected virtual void OnDownloadCompleted(string[] rawLog, DateTime? lastTrailTime)
        {
            DownloadCompleted?.Invoke(rawLog, lastTrailTime);
        }

        /// <summary>
        /// Raises the <see cref="ProgressUpdate"/> event to notify listeners of progress updates.
        /// </summary>
        /// <param name="secondsElapsed">The number of seconds elapsed since the worker started.</param>
        /// <param name="duration">The total duration for which the worker is running, in seconds.</param>
        protected virtual void OnProgressUpdate(int secondsElapsed, int duration)
        {
            ProgressUpdate?.Invoke(secondsElapsed, duration);
        }
    }
}
