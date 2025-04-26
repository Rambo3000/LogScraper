using System.Collections.Generic;
using System.Threading;

namespace LogScraper.Log.Collection
{
    /// <summary>
    /// Represents a collection of log entries.
    /// </summary>
    public class LogCollection
    {
        // Singleton instance of LogCollection.
        private static LogCollection instance = null;

        // Lock object to ensure thread safety when creating the singleton instance.
        private static readonly Lock padlock = new();

        /// <summary>
        /// Gets or sets the count of errors in the log collection.
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Clears the log collection by removing all log entries and resetting the error count.
        /// </summary>
        public void Clear()
        {
            LogEntries.Clear();
            ErrorCount = 0;
        }

        /// <summary>
        /// Gets the singleton instance of the LogCollection class.
        /// Ensures that only one instance is created, even in a multithreaded environment.
        /// </summary>
        public static LogCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance.
                    lock (padlock)
                    {
                        instance ??= new LogCollection();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// A list of log entries stored in the collection.
        /// </summary>
        public List<LogEntry> LogEntries { get; set; } = [];
    }
}
