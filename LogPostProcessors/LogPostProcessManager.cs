using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{

    public sealed class LogPostProcessManager
    {
        private static int isRunning; // atomic flag to enforce single manager run

        public event EventHandler ProcessingFinished;
        bool anyItemProcessed = false; // 0 = no, 1 = yes

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

            if (startIndex < 0 || endIndex >= logCollection.LogEntries.Count || startIndex > endIndex)
            {
                OnProcessingFinished(false, false);
                return false;
            }
            if (processorKinds == null || processorKinds.Count == 0)
            {
                throw new ArgumentException("At least one processor kind must be specified.", nameof(processorKinds));
            }

            if (Interlocked.CompareExchange(ref isRunning, 1, 0) != 0)
            {
                OnProcessingFinished(false, false);
                return false; // already running
            }
            Volatile.Write(ref isRunning, 1);
            Task.Run(() => RunInternal(startIndex, endIndex, processorKinds, cancellationToken), cancellationToken);
            return true;
        }

        private void RunInternal(int startIndex, int endIndex, List<LogPostProcessorKind> processorKinds, CancellationToken cancellationToken)
        {
            bool wasCanceled = false;
            Interlocked.Exchange(ref anyItemProcessed, false);
            try
            {
                foreach (var kvp in processors)
                {
                    LogPostProcessorKind kind = kvp.Key;
                    // skip unrequested processor
                    if (!processorKinds.Contains(kind)) continue;

                    ILogPostProcessor processor = kvp.Value;

                    ProcessRange(startIndex, endIndex, processor, kind, cancellationToken);

                }
            }
            catch (OperationCanceledException)
            {
                wasCanceled = true;
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

                bool hasChanges = Volatile.Read(ref anyItemProcessed);

                OnProcessingFinished(wasCanceled, hasChanges);
            }
        }
        private void ProcessRange(int start, int end, ILogPostProcessor processor, LogPostProcessorKind kind, CancellationToken cancellationToken)
        {
            if (start > end)
            {
                return;
            }

            ParallelOptions options = new()
            {
                CancellationToken = cancellationToken,
                //Make sure not to overload the CPU
                MaxDegreeOfParallelism = Math.Min(1, Environment.ProcessorCount / 2)
            };

            Parallel.For(start, end + 1, options,
                index =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    LogEntry entry = logCollection.LogEntries[index];

                    // skip if already processed with this processor, also to avoid threading issues
                    if (entry.LogPostProcessResults != null && entry.LogPostProcessResults.Results[(int)kind] != null) return;

                    if (processor.TryProcess(entry, out string result))
                    {
                        entry.LogPostProcessResults ??= new LogPostProcessResults();
                        entry.LogPostProcessResults.Set(kind, new LogPostProcessResult(kind, result));

                        // mark that something changed
                        Interlocked.Exchange(ref anyItemProcessed, true);
                    }
                });
        }
        private void OnProcessingFinished(bool wasCanceled, bool hasChanges)
        {
            ProcessingFinished?.Invoke(this, new LogPostProcessingFinishedEventArgs(wasCanceled, hasChanges));
        }
    }
}
