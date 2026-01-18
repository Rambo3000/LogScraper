
using System.Collections.Generic;

namespace LogScraper.LogPostProcessors
{
    public sealed class LogPostProcessStore
    {
        private const int BlockSize = 1024;

        private readonly List<LogPostProcessResult[]> blocks = [];

        public LogPostProcessorKind ProcessorKind { get; }

        public int LastProcessedStartIndex { get; private set; } = -1;
        public int LastProcessedEndIndex { get; private set; } = -1;

        public LogPostProcessStore(LogPostProcessorKind processorKind)
        {
            ProcessorKind = processorKind;
        }

        public bool TryGet(int logIndex, out LogPostProcessResult result)
        {
            int blockIndex = logIndex / BlockSize;
            int offset = logIndex % BlockSize;

            if (blockIndex >= blocks.Count)
            {
                result = null;
                return false;
            }

            result = blocks[blockIndex][offset];
            return result != null;
        }


        public void Set(int logIndex, LogPostProcessResult result)
        {
            int blockIndex = logIndex / BlockSize;
            int offset = logIndex % BlockSize;

            EnsureBlockExists(blockIndex);
            blocks[blockIndex][offset] = result;
        }

        private void EnsureBlockExists(int blockIndex)
        {
            while (blocks.Count <= blockIndex)
            {
                blocks.Add(new LogPostProcessResult[BlockSize]);
            }
        }
        private volatile int lastProcessedStartIndex = -1;
        private volatile int lastProcessedEndIndex = -1;

        public void MarkRangeProcessed(int startIndex, int endIndex)
        {
            if (startIndex > lastProcessedStartIndex)
            {
                lastProcessedStartIndex = startIndex;
            }

            if (endIndex > lastProcessedEndIndex)
            {
                lastProcessedEndIndex = endIndex;
            }
        }
    }
}