using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LogScraper.SourceAdapters;

namespace LogScraper.SourceProcessing
{
    internal class SourceProcessingWorker
    {
        public event Action<string, bool> StatusUpdate;
        public event Action<string[]> DownloadCompleted;
        public event Action<int, int> ProgressUpdate;

        public async Task DoWorkAsync(ISourceAdapter sourceAdapter, int intervalInSeconds, int durationInSeconds, string dateTimeFormat, CancellationToken cancellationToken)
        {
            try
            {

                if (durationInSeconds == -1)
                {
                    OnProgressUpdate(0, durationInSeconds);

                    await GetLogFromSourceAdapter(sourceAdapter, dateTimeFormat);
                }
                else
                {
                    OnProgressUpdate(0, durationInSeconds);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (stopwatch.ElapsedMilliseconds / 1000 < durationInSeconds)
                    {
                        if (cancellationToken.IsCancellationRequested) return;

                        await GetLogFromSourceAdapter(sourceAdapter, dateTimeFormat);

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

        private async Task GetLogFromSourceAdapter(ISourceAdapter sourceAdapter, string dateTimeFormat)
        {
            string rawLog = await sourceAdapter.GetLogAsync();
            string[] rawLogArray = rawLog.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);

            InvertRawLogWhenNeeded(rawLogArray, dateTimeFormat);

            OnDownloadCompleted(rawLogArray);
        }

        private static void InvertRawLogWhenNeeded(string[] rawLogArray, string dateTimeFormat)
        {
            if (string.IsNullOrEmpty(dateTimeFormat) || rawLogArray == null || rawLogArray.Length == 0) return;

            DateTime dateTimeFirstElement = LogReader.GetDateTimeFromRawLogLine(rawLogArray[0], dateTimeFormat);
            DateTime dateTimeLastElement = LogReader.GetDateTimeFromRawLogLine(rawLogArray[^1], dateTimeFormat);

            // For now do not invert when you cannot read the first or last element
            if (dateTimeFirstElement.Year < 1000 || dateTimeLastElement.Year < 1000) return;
            if (dateTimeFirstElement <= dateTimeLastElement) return;

            Array.Reverse(rawLogArray);
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
