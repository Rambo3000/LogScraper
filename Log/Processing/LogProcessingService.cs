using System;
using System.Collections.Generic;
using LogScraper.Configuration;
using LogScraper.Log.Layout;
using LogScraper.Log.Processing.RawLogParsing;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using LogScraper.Utilities.Extensions;
using AppState = LogScraper.Log.LogAppState.LogAppState;

namespace LogScraper.Log.Processing
{
    /// <summary>
    /// Singleton service that owns the full log retrieval and processing pipeline.
    /// Controls interact with this service to start/stop fetching; they never touch
    /// the parser, classifier, or collection directly.
    /// </summary>
    internal class LogProcessingService
    {

        #region Public pipeline control
        public static bool CanStartFetching()
        {
            return !AppState.Instance.ProcessingState.Value.IsActive
                && AppState.Instance.IsSourceValid.Value
                && AppState.Instance.Layout.Value != null;
        }

        public static bool CanStopFetching()
        {
            return AppState.Instance.ProcessingState.Value.IsActive;
        }

        public static void StartSingleFetch()
        {
            if (!CanStartFetching()) return;
            AppState.Instance.ProcessingState.Set(LogProcessingState.Active(false));
            StartFetching();
        }

        public static void StartTimedFetch()
        {
            if (!CanStartFetching()) return;
            AppState.Instance.ProcessingState.Set(LogProcessingState.Active(true));
            StartFetching(1, ConfigAppState.Instance.GenericConfig.Value.AutomaticReadTimeMinutes * 60);
        }

        public static void StopFetching()
        {
            if (!CanStopFetching()) return;
            Stop();
        }

        /// <summary>
        /// Creates a source worker, wires it to <see cref="HandleDownloadCompleted"/>,
        /// and hands it to <see cref="SourceProcessingManager"/>.
        /// </summary>
        /// <param name="intervalInSeconds">Polling interval; -1 for a single fetch.</param>
        /// <param name="durationInSeconds">Total run duration; -1 for a single fetch.</param>
        public static void StartFetching(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            if (AppState.Instance.Layout.Value == null) return;
            try
            {
                if (AppState.Instance.SourceAdapterProvider == null) throw new InvalidOperationException("SourceAdapterProvider is not set in AppState.");

                ISourceAdapter sourceAdapter = AppState.Instance.SourceAdapterProvider(AppState.Instance.LastTrailTime.Value);

                SourceProcessingWorker worker = new();
                worker.DownloadCompleted += HandleDownloadCompleted;
                SourceProcessingManager.Instance.AddWorker(worker, sourceAdapter, intervalInSeconds, durationInSeconds);
            }
            catch (Exception ex)
            {
                ex.LogStackTraceToFile();
                AppState.Instance.StatusMessage.Set((ex.Message, false));
                AppState.Instance.ProcessingState.Set(LogProcessingState.Inactive);
            }
        }

        /// <summary>
        /// Cancels all active and queued source workers.
        /// </summary>
        public static void Stop()
        {
            AppState.Instance.ProcessingState.Set(LogProcessingState.Inactive);
            SourceProcessingManager.Instance.CancelAllWorkers();
        }
        #endregion

        #region Private pipeline control
        /// <summary>
        /// Parses, classifies, commits, and updates application state for a batch of raw log lines.
        /// </summary>
        /// <remarks>The source processing worker thread invokes this method, so it is executed using this thread. </remarks>
        /// <param name="rawLog">The raw log lines to process.</param>
        /// <param name="updatedLastTrailTime">The new last trail time to set after processing, if applicable.</param>
        /// <param name="isContinuous">Whether this download is part of a continuous fetching process (as opposed to a single fetch), which determines whether to set the status to Waiting after processing.</param>
        private static void HandleDownloadCompleted(string[] rawLog, DateTime? updatedLastTrailTime, bool isContinuous)
        {
            try
            {
                LogLayout logLayout = AppState.Instance.Layout.Value;
                if (logLayout == null) return;

                LogCollection logCollection = AppState.Instance.LogCollection.Value ?? new();

                bool newLogEntriesReceived;
                try
                {
                    AppState.Instance.ProcessingState.Set(AppState.Instance.ProcessingState.Value.WithStatus(LogProcessingStatus.Processing));

                    newLogEntriesReceived = RawLogParser.TryParseNewLogEntries(rawLog, logCollection, logLayout, out List<LogEntry> newEntries);

                    if (newLogEntriesReceived)
                    {
                        // Classify the new entries
                        LogEntryClassifier.Classify(logLayout, logCollection, newEntries, out var contentPropertyMask, out var errorMask);

                        // Atomic commit: AddRange + mask assignment under a brief write lock
                        logCollection.CommitParsedEntries(newEntries, contentPropertyMask, errorMask);
                    }
                }
                catch (Exception ex)
                {
                    ex.LogStackTraceToFile("Fout tijdens parsen van raw log.");
                    AppState.Instance.RawLogFallback.Set(RawLogParser.JoinRawLogIntoString(rawLog));
                    throw;
                }

                if (newLogEntriesReceived)
                {
                    AppState.Instance.RawLogFallback.Set(null);
                    AppState.Instance.LogCollection.ForceSet(logCollection);
                }

                AppState.Instance.StatusMessage.Set((string.Empty, true));
                AppState.Instance.LastTrailTime.Set(updatedLastTrailTime);
                if (isContinuous) AppState.Instance.ProcessingState.Set(AppState.Instance.ProcessingState.Value.WithStatus(LogProcessingStatus.Waiting));
            }
            catch (Exception ex)
            {
                ex.LogStackTraceToFile();
                AppState.Instance.StatusMessage.Set((ex.Message, false));
            }
        }

        #endregion
    }
}
