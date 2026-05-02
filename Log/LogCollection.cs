using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.Extensions;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log
{
    /// <summary>
    /// Represents a collection of log entries.
    /// Thread-safe for concurrent access between background processing and UI reads.
    /// </summary>
    public class LogCollection
    {
        private readonly ReaderWriterLockSlim _lock = new();

        /// <summary>
        /// The internal list of log entries. Modified only during <see cref="CommitParsedEntries"/> and <see cref="Clear"/>.
        /// External code reads via <see cref="LogEntries"/>.
        /// </summary>
        private List<LogEntry> _logEntries = [];

        /// <summary>
        /// Read-only view of the log entries.
        /// </summary>
        public IReadOnlyList<LogEntry> LogEntries => _logEntries;

        private BitArray _errorMask = null;
        /// <summary>
        /// A BitArray indicating which log entries in the collection are error log entries.
        /// </summary>
        public BitArray ErrorMask => _errorMask;

        private IndexDictionary<LogContentProperty, BitArray> _contentPropertyMask = null;
        /// <summary>
        /// A dictionary mapping each content property to a BitArray indicating which log entries (by index) have that property.
        /// </summary>
        public IndexDictionary<LogContentProperty, BitArray> ContentPropertyMask => _contentPropertyMask;

        /// <summary>
        /// Total number of log entries in the collection.
        /// Safe to read from any thread without a lock.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Number of log entries that are classified as errors.
        /// Safe to read from any thread without a lock.
        /// </summary>
        public int ErrorCount { get; private set; }

        /// <summary>
        /// Clears the log collection by removing all log entries and resetting all state.
        /// </summary>
        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _logEntries.Clear();
                _errorMask = null;
                _contentPropertyMask = null;
                _valuePool = new();
                TotalCount = 0;
                ErrorCount = 0;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// A shared pool of <see cref="LogMetadataValue"/> objects, keyed by property and value.
        /// Ensures that identical metadata values are represented by the same object instance.
        /// </summary>
        private ConcurrentDictionary<(LogMetadataProperty, string), LogMetadataValue> _valuePool = new();

        /// <summary>
        /// Returns a shared <see cref="LogMetadataValue"/> instance for the given metadata property and value.
        /// If the combination has been seen before, the existing instance is returned.
        /// Otherwise a new instance is created, stored, and returned.
        /// </summary>
        public LogMetadataValue GetSharedLogMetadataValueObject(LogMetadataProperty property, string value)
        {
            var key = (property, value);

            if (_valuePool.TryGetValue(key, out LogMetadataValue existing))
                return existing;

            LogMetadataValue candidate = new(property, value);
            LogMetadataValue result = _valuePool.GetOrAdd(key, candidate);

            if (ReferenceEquals(result, candidate))
                InvalidateMetadataValuesPerProperty();

            return result;
        }

        /// <summary>
        /// An index of all known <see cref="LogMetadataValue"/> objects grouped by their associated <see cref="LogMetadataProperty"/>.
        /// Used for efficient lookup when filtering or analyzing log entries by metadata.
        /// Built on first access and invalidated whenever new metadata values are added.
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
        /// Builds the <see cref="MetadataValues"/> index from the current value pool.
        /// </summary>
        /// <returns>A dictionary mapping each log metadata property to its associated metadata values.</returns>
        private Dictionary<LogMetadataProperty, List<LogMetadataValue>> BuildMetadataValuesPerProperty()
        {
            return _valuePool.GroupBy(kvp => kvp.Key.Item1).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Value).ToList());
        }

        /// <summary>
        /// Invalidates the cached <see cref="MetadataValues"/> index so it is rebuilt on next access.
        /// </summary>
        private void InvalidateMetadataValuesPerProperty()
        {
            MetadataValues = null;
        }

        /// <summary>
        /// Returns the first and last log entry strings, used to determine which entries are new during parsing.
        /// Returns (null, null) if the collection is empty.
        /// </summary>
        internal (string FirstEntry, string LastEntry) GetAnchorsForParsing()
        {
            _lock.EnterReadLock();
            try
            {
                return _logEntries.Count == 0
                    ? (null, null)
                    : (_logEntries[0].Entry, _logEntries[^1].Entry);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Appends the parsed entries and replaces the masks in a single consistent update.
        /// Entry indices are assigned based on the actual collection size at the time of commit.
        /// </summary>
        internal void CommitParsedEntries(List<LogEntry> newEntries, IndexDictionary<LogContentProperty, BitArray> contentPropertyMask, BitArray errorMask)
        {
            _lock.EnterWriteLock();
            try
            {
                // Assign indices based on actual collection size at commit time,
                // correcting any offset captured earlier during parsing.
                int offset = _logEntries?.Count ?? 0;
                for (int i = 0; i < newEntries.Count; i++)
                    newEntries[i].Index = offset + i;

                _logEntries.AddRange(newEntries);
                _contentPropertyMask = contentPropertyMask;
                _errorMask = errorMask;
                TotalCount = _logEntries?.Count ?? 0;
                ErrorCount = errorMask?.CountSetBits() ?? 0;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Tries to acquire read access without blocking.
        /// Returns false if a write is currently in progress; in that case the caller may skip the
        /// operation, as the write completion will trigger a refresh.
        /// Must be paired with <see cref="ReleaseReadAccess"/> if it returns true.
        /// </summary>
        public bool TryAcquireReadAccess()
        {
            return _lock.TryEnterReadLock(0);
        }

        /// <summary>
        /// Acquires read access, blocking until any active write has completed.
        /// Use when the operation must not be skipped.
        /// Must be paired with <see cref="ReleaseReadAccess"/>.
        /// </summary>
        public void AcquireReadAccess()
        {
            _lock.EnterReadLock();
        }

        /// <summary>
        /// Releases read access previously acquired by <see cref="TryAcquireReadAccess"/> or <see cref="AcquireReadAccess"/>.
        /// </summary>
        public void ReleaseReadAccess()
        {
            _lock.ExitReadLock();
        }
    }
}