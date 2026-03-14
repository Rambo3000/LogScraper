using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log
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
        public int ErrorCount;

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


        /// <summary>
        /// A dictionary that serves as a pool of shared LogMetadataValue objects. 
        /// This pool allows for efficient reuse of LogMetadataValue instances.
        /// </summary>
        private readonly Dictionary<(LogMetadataProperty, string), LogMetadataValue> _valuePool = [];

        /// <summary>
        /// Gets a shared instance of a LogMetadataValue for the specified metadata property and value.
        /// </summary>
        /// <remarks>This method uses a value pool to ensure that only one instance exists for each unique
        /// combination of property and value, which can improve memory efficiency when many duplicate metadata values
        /// are used.</remarks>
        /// <param name="property">The metadata property for which to retrieve or create a shared LogMetadataValue instance.</param>
        /// <param name="value">The string value associated with the specified metadata property.</param>
        /// <returns>A LogMetadataValue instance representing the specified property and value. If an instance for the given
        /// combination already exists, the existing instance is returned; otherwise, a new instance is created and
        /// returned.</returns>
        public LogMetadataValue GetSharedLogMetadataValueObject(LogMetadataProperty property, string value)
        {
            var key = (property, value);
            if (!_valuePool.TryGetValue(key, out LogMetadataValue existing))
            {
                existing = new LogMetadataValue(property, value);
                _valuePool[key] = existing;
                InvalidateMetadataValuesPerProperty();
            }
            return existing;
        }

        /// <summary>
        /// Gets the index of log metadata values organized by their associated metadata properties. This index allows for efficient retrieval of log metadata values based on their properties, facilitating quick access to relevant metadata values when filtering or analyzing log entries. Each entry in the index maps a log metadata property to a list of log metadata values that are associated with that property across the log collection.
        /// </summary>
        public Dictionary<LogMetadataProperty, List<LogMetadataValue>> MetadataValues
        {
            get
            {
                return field ??= BuildMetadataValuesPerProperty();
            }

            private set;
        }

        /// <summary>
        /// This index allows for efficient retrieval of log metadata values based on their properties, 
        /// facilitating quick access to relevant metadata values when filtering or analyzing log entries.
        /// </summary>
        /// <returns>A dictionary mapping each log metadata property to a list of log metadata values associated with that property.</returns>
        private Dictionary<LogMetadataProperty, List<LogMetadataValue>> BuildMetadataValuesPerProperty()
        { 
            return _valuePool.GroupBy(kvp => kvp.Key.Item1).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Value).ToList());
        }

        /// <summary>
        /// Invalidates the index of log metadata values organized by their associated metadata properties.
        /// This method sets the MetadataValuesPerMetadataProperty field to null.
        /// </summary>
        private void InvalidateMetadataValuesPerProperty()
        {
            MetadataValues = null;
        }
    }
}
