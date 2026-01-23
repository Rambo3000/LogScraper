using System;
using System.Collections.Generic;
using System.Threading;

namespace LogScraper.LogPostProcessors
{
    public class LogPostProcessCollection
    {
        private Dictionary<LogPostProcessorKind, LogPostProcessStore> stores;

        public LogPostProcessCollection()
        {
            CreateStores();
        }

        private void CreateStores()
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

        public bool TryGetResult(LogPostProcessorKind processorKind, int logIndex, out LogEntryPostProcessResult result)
        {
            return stores[processorKind].TryGet(logIndex, out result);
        }

        public void SetResult(LogPostProcessorKind processorKind, int logIndex, LogEntryPostProcessResult result)
        {
            stores[processorKind].Set(logIndex, result);
        }

        public LogPostProcessStore GetStore(LogPostProcessorKind processorKind)
        {
            return stores[processorKind];
        }

        public List<LogPostProcessStore> GetStores()
        {
            return [stores[LogPostProcessorKind.XmlPrettyPrint], stores[LogPostProcessorKind.JsonPrettyPrint]];
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

        internal void Clear()
        {
            CreateStores();
        }
    }

}
