
using System.Collections.Generic;

namespace LogScraper.LogPostProcessors
{
    public sealed class LogPostProcessStore(LogPostProcessorKind processorKind)
    {
        private const int BlockSize = 1024;

        private readonly List<LogEntryPostProcessResult[]> blocks = [];

        public LogPostProcessorKind ProcessorKind { get; } = processorKind;

        public int LastProcessedStartIndex { get { return lastProcessedStartIndex; } }
        public int LastProcessedEndIndex { get { return lastProcessedEndIndex; } }

        public bool TryGet(int logIndex, out LogEntryPostProcessResult result)
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


        public void Set(int logIndex, LogEntryPostProcessResult result)
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
                blocks.Add(new LogEntryPostProcessResult[BlockSize]);
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