using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{
    public sealed class LogPostProcessManager
    {
        /// <summary>
        /// The collection of logs to be managed and processed.
        /// </summary>
        private readonly LogCollection logCollection;

        /// <summary>
        /// Provides a mapping of log post processor kinds to their corresponding processor implementations.
        /// </summary>
        private readonly Dictionary<LogPostProcessorKind, ILogPostProcessor> processors;

        /// <summary>
        /// Indicates whether the manager is currently running.
        /// </summary>
        private static int isRunning;

        /// <summary>
        /// Indicates whether any log item has been processed during the current run.
        /// </summary>
        private readonly bool[] anyItemProcessed = new bool[Enum.GetValues<LogPostProcessorKind>().Length];

        /// <summary>
        /// Occurs when the processing operation has completed.
        /// </summary>
        /// <remarks>Subscribers can use this event to perform actions after processing finishes. The
        /// event is raised regardless of whether the operation succeeded or failed.</remarks>
        public event EventHandler ProcessingFinished;

        /// <summary>
        /// Initializes a new instance of the LogPostProcessManager class with the specified log collection.
        /// </summary>
        /// <remarks>This constructor sets up default post-processors for JSON and XML pretty-printing.
        /// Additional processors can be added after initialization as needed.</remarks>
        /// <param name="logCollection">The collection of logs to be managed and processed. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if the logCollection parameter is null.</exception>
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
        /// <param name="processorKinds">The kinds of processors to apply.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>True if processing started successfully; false if already running or no log entries to process.</returns>
        public bool TryRun(List<LogPostProcessorKind> processorKinds, CancellationToken cancellationToken)
        {
            if (logCollection == null || logCollection.LogEntries == null || logCollection.LogEntries.Count == 0)
            {
                OnProcessingFinished(false, new bool[Enum.GetValues<LogPostProcessorKind>().Length]);
                return false;
            }
            if (processorKinds == null || processorKinds.Count == 0)
            {
                throw new ArgumentException("At least one processor kind must be specified.", nameof(processorKinds));
            }

            if (Interlocked.CompareExchange(ref isRunning, 1, 0) != 0)
            {
                OnProcessingFinished(false, new bool[Enum.GetValues<LogPostProcessorKind>().Length]);
                return false; // already running
            }
            Volatile.Write(ref isRunning, 1);

            bool[] anyItemProcessed = new bool[Enum.GetValues<LogPostProcessorKind>().Length];

            Task.Run(() => RunInternal(0, logCollection.LogEntries.Count - 1, processorKinds, cancellationToken), cancellationToken);
            return true;
        }

        /// <summary>
        /// Internal method to run post-processing on the specified range of log entries.
        /// </summary>
        /// <param name="startIndex">The starting index of the log entries to process.</param>
        /// <param name="endIndex">The ending index of the log entries to process.</param>
        /// <param name="processorKinds">The kinds of processors to apply.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <remarks>This method processes log entries in parallel using the specified processors.
        private void RunInternal(int startIndex, int endIndex, List<LogPostProcessorKind> processorKinds, CancellationToken cancellationToken)
        {
            bool wasCanceled = false;
            try
            {
                foreach (var kvp in processors)
                {
                    LogPostProcessorKind kind = kvp.Key;

                    Interlocked.Exchange(ref anyItemProcessed[(int)kind], false);

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
                bool[] hasChanges = new bool[Enum.GetValues<LogPostProcessorKind>().Length];

                foreach (var kind in processorKinds)
                {
                    hasChanges[(int)kind] = Volatile.Read(ref anyItemProcessed[(int)kind]);
                }

                OnProcessingFinished(wasCanceled, hasChanges);
            }
        }

        /// <summary>
        /// Processes a range of log entries using the specified processor.
        /// </summary>
        /// <param name="start">The starting index of the log entries to process.</param>
        /// <param name="end">The ending index of the log entries to process.</param>
        /// <param name="processor">The log post-processor to apply.</param>
        /// <param name="kind">The kind of processor being applied.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <remarks>This method processes log entries in parallel and updates their post-processing results.
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
                        Interlocked.Exchange(ref anyItemProcessed[(int)kind], true);
                    }
                });
        }

        /// <summary>
        /// Raises the ProcessingFinished event.
        /// </summary>
        /// <param name="wasCanceled">Indicates whether the processing was canceled.</param>
        /// <param name="hasChanges">Indicates whether any changes were made during processing.</param>
        private void OnProcessingFinished(bool wasCanceled, bool[] hasChanges)
        {
            ProcessingFinished?.Invoke(this, new LogPostProcessingFinishedEventArgs(wasCanceled, hasChanges));
        }
    }
}