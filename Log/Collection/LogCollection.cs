using System.Collections.Generic;
using System.Threading;

namespace LogScraper.Log.Collection
{
    internal class LogCollection
    {
        private static LogCollection instance = null;
        private static readonly Lock padlock = new();

        public int ErrorCount { get; set; }

        public LogCollection()
        {
            LogLines = [];
        }

        public void Clear()
        {
            LogLines.Clear();
            ErrorCount = 0;
        }

        public static LogCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance
                    lock (padlock)
                    {
                        instance ??= new LogCollection();
                    }
                }
                return instance;
            }
        }

        public List<LogLine> LogLines { get; set; }
    }
}
