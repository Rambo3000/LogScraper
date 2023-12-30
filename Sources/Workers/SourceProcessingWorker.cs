using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LogScraper.Sources.Adapters;

namespace LogScraper.Sources.Workers
{
    internal class SourceProcessingWorker
    {
        public event Action<string, bool> StatusUpdate;
        public event Action<string[]> DownloadCompleted;
        public event Action<int, int> ProgressUpdate;

        public async Task DoWorkAsync(ISourceAdapter sourceAdapter, int intervalInSeconds, int durationInSeconds, CancellationToken cancellationToken)
        {
            try
            {

                if (durationInSeconds == -1)
                {
                    OnProgressUpdate(0, durationInSeconds);

                    await GetLogFromSourceAdapter(sourceAdapter);
                }
                else
                {
                    OnProgressUpdate(0, durationInSeconds);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (stopwatch.ElapsedMilliseconds / 1000 < durationInSeconds)
                    {
                        if (cancellationToken.IsCancellationRequested) return;

                        await GetLogFromSourceAdapter(sourceAdapter);

                        await Task.Delay(intervalInSeconds * 1000, CancellationToken.None);

                        OnProgressUpdate(Convert.ToInt32(stopwatch.ElapsedMilliseconds / 1000), durationInSeconds);
                    }
                }
            }
            catch (Exception ex)
            {
                // Raise the ExceptionOccurred event with the exception
                OnExceptionOccurred(ex.Message, false);
            }
        }

        private async Task GetLogFromSourceAdapter(ISourceAdapter sourceAdapter)
        {
            string rawLog = await sourceAdapter.GetLogAsync();
            string[] rawLogArray = rawLog.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);

            OnDownloadCompleted(rawLogArray);
        }
        protected virtual void OnExceptionOccurred(string message, bool isSucces)
        {
            // Raise the event on the UI thread
            StatusUpdate?.Invoke(message, isSucces);
        }
        protected virtual void OnDownloadCompleted(string[] rawLog)
        {
            // Raise the event on the UI thread
            DownloadCompleted?.Invoke(rawLog);
        }
        protected virtual void OnProgressUpdate(int secondsElapsed, int duration)
        {
            // Raise the event on the UI thread
            ProgressUpdate?.Invoke(secondsElapsed, duration);
        }
    }
}
