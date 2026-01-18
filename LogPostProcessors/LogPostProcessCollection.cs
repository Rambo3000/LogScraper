using System.Collections.Generic;
using System.Threading;

namespace LogScraper.LogPostProcessors
{
    public class LogPostProcessCollection
    {
        private readonly object syncRoot = new object();
        private readonly Dictionary<LogPostProcessorKind, LogPostProcessStore> stores;

        public LogPostProcessCollection(
            int logEntryCount)
        {
            stores = new Dictionary<LogPostProcessorKind, LogPostProcessStore>
        {
            {
                LogPostProcessorKind.JsonPrettyPrint,
                new LogPostProcessStore(LogPostProcessorKind.JsonPrettyPrint)
            },
            {
                LogPostProcessorKind.XmlPrettyPrint,
                new LogPostProcessStore(LogPostProcessorKind.XmlPrettyPrint)
            }
        };
        }

        public bool TryGetResult(
            LogPostProcessorKind processorKind,
            int logIndex,
            out LogPostProcessResult result)
        {
            lock (syncRoot)
            {
                return stores[processorKind].TryGet(logIndex, out result);
            }
        }

        public void SetResult(
            LogPostProcessorKind processorKind,
            int logIndex,
            LogPostProcessResult result)
        {
            lock (syncRoot)
            {
                stores[processorKind].Set(logIndex, result);
            }
        }

        public LogPostProcessStore GetStore(
            LogPostProcessorKind processorKind)
        {
            return stores[processorKind];
        }

        private int isProcessing;

        public bool TryBeginProcessing()
        {
            return Interlocked.CompareExchange(ref isProcessing, 1, 0) == 0;
        }

        public void EndProcessing()
        {
            Volatile.Write(ref isProcessing, 0);
        }
    }

}
