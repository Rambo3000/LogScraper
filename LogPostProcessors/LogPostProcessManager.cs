using System;
using System.Collections.Generic;``
using System.Threading;
using System.Threading.Tasks;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{

    public sealed class LogPostProcessManager
    {
        private static int isRunning; // atomic flag to enforce single manager run

        public event EventHandler ProcessingFinished;

        private readonly LogCollection logCollection;
        private readonly Dictionary<LogPostProcessorKind, ILogPostProcessor> processors;

        public LogPostProcessManager(LogCollection logCollection)
        {
            this.logCollection = logCollection ?? throw new ArgumentNullException(nameof(logCollection));
            processors = [];
            processors.Add(LogPostProcessorKind.JsonPrettyPrint, new Implementations.JsonPostProcessor());
            processors.Add(LogPostProcessorKind.XmlPrettyPrint, new Implementations.XmlPostProcessor());
        }

        /// <summary>
        /// Runs post-processing on the log entries in the specified range.
        /// Returns false if a manager is already running.
        /// </summary>
        public bool TryRun(int startIndex, int endIndex, List<LogPostProcessorKind> processorKinds, CancellationToken cancellationToken)
        {
            Volatile.Write(ref isRunning, 1);

            if (startIndex < 0 || endIndex >= logCollection.LogEntries.Count || startIndex > endIndex)
            {
                OnProcessingFinished();
                return false;
            }
            if (processorKinds == null || processorKinds.Count == 0)
            {
                throw new ArgumentException("At least one processor kind must be specified.", nameof(processorKinds));
            }

            if (Interlocked.CompareExchange(ref isRunning, 1, 0) != 0)
            {
                OnProcessingFinished();
                return false; // already running
            }

            Task.Run(() => RunInternal(startIndex, endIndex, processorKinds, cancellationToken), cancellationToken);
            return true;
        }

        private void RunInternal(int startIndex, int endIndex, List<LogPostProcessorKind> processorKinds, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var kvp in processors)
                {
                    LogPostProcessorKind kind = kvp.Key;
                    // skip unrequested processor
                    if (!processorKinds.Contains(kind)) continue;

                    ILogPostProcessor processor = kvp.Value;

                    // ensure store exists
                    var store = logCollection.PostProcessCollection.GetStore(kind);
                    int requestStart = startIndex;
                    int requestEnd = endIndex;

                    int processedStart = store.LastProcessedStartIndex;
                    int processedEnd = store.LastProcessedEndIndex;

                    // no previous processing
                    if (processedStart < 0 || processedEnd < 0)
                    {
                        ProcessRange(requestStart, requestEnd, processor, kind, store, cancellationToken);
                        store.MarkRangeProcessed(requestStart, requestEnd);
                        continue;
                    }

                    // no overlap at all → forget old range
                    if (requestStart > processedEnd || requestEnd < processedStart)
                    {
                        ProcessRange(requestStart, requestEnd, processor, kind, store, cancellationToken);
                        store.MarkRangeProcessed(requestStart, requestEnd);
                        continue;
                    }

                    // fully inside processed → skip everything
                    if (requestStart >= processedStart && requestEnd <= processedEnd)
                    {
                        continue;
                    }

                    // partial overlaps → up to two ranges

                    // before overlap
                    if (requestStart < processedStart)
                    {
                        int start = requestStart;
                        int end = processedStart - 1;
                        ProcessRange(start, end, processor, kind, store, cancellationToken);
                    }

                    // after overlap
                    if (requestEnd > processedEnd)
                    {
                        int start = processedEnd + 1;
                        int end = requestEnd;
                        ProcessRange(start, end, processor, kind, store, cancellationToken);
                    }

                    // merge ranges
                    int newProcessedStart = Math.Min(processedStart, requestStart);
                    int newProcessedEnd = Math.Max(processedEnd, requestEnd);

                    store.MarkRangeProcessed(newProcessedStart, newProcessedEnd);

                }
            }
            catch (OperationCanceledException)
            {
                // canceled, just exit silently
            }
            catch (Exception ex)
            {
                // optionally log errors
                Console.WriteLine($"PostProcessing error: {ex}");
            }
            finally
            {
                // release the run lock
                Volatile.Write(ref isRunning, 0);
                OnProcessingFinished();
            }
        }
        private void ProcessRange(int start, int end, ILogPostProcessor processor, LogPostProcessorKind kind, LogPostProcessStore store, CancellationToken cancellationToken)
        {
            if (start > end)
            {
                return;
            }

            ParallelOptions options = new()
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = 1
            };

            Parallel.For(start, end + 1, options,
                index =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    LogEntry entry = logCollection.LogEntries[index];

                    if (processor.TryProcess(entry, out string result))
                    {
                        store.Set(index, new LogEntryPostProcessResult(kind, result));
                    }
                });
        }
        private void OnProcessingFinished()
        {
            ProcessingFinished?.Invoke(this, EventArgs.Empty);
        }

    }

}
