using System.Collections;
using System.Collections.Generic;
using LogScraper.Log.Content;
using LogScraper.Log.Rendering;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log.Filtering
{
    internal class LogFilterResultWithRange(LogMetadataFilterResult metadataFilterResult, LogRange range)
    {
        public LogMetadataFilterResult MetadataFilterResult { get; } = metadataFilterResult;

        public LogRange Range { get; } = range;

        /// <summary>
        /// The inclusive source-collection index of the first log entry in the range.
        /// </summary>
        private int BeginIndex { get; } = range?.Begin?.Index ?? 0;

        /// <summary>
        /// The inclusive source-collection index of the last log entry in the range.
        /// </summary>
        private int EndIndex { get; } = range?.End?.Index ?? int.MaxValue;

        private List<LogEntry> _logEntriesCache;

        /// <summary>
        /// Returns the filtered log entries that fall within the range.
        /// Because <see cref="LogMetadataFilterResult.LogEntries"/> is ordered by source index,
        /// a binary search locates the start position so the full list is never scanned unnecessarily.
        /// The result is cached so the search is never repeated.
        /// </summary>
        public List<LogEntry> LogEntries
        {
            get
            {
                if (_logEntriesCache != null) return _logEntriesCache;

                List<LogEntry> entries = MetadataFilterResult.LogEntries;
                if (entries == null || entries.Count == 0) return _logEntriesCache = [];

                if (!Range.IsConstrained) return _logEntriesCache = entries;

                // Binary search for the first entry with Index >= BeginIndex
                int lo = 0, hi = entries.Count - 1, startPos = entries.Count;
                while (lo <= hi)
                {
                    int mid = (lo + hi) >> 1;
                    if (entries[mid].Index >= BeginIndex) { startPos = mid; hi = mid - 1; }
                    else lo = mid + 1;
                }

                List<LogEntry> result = [];
                for (int i = startPos; i < entries.Count; i++)
                {
                    if (entries[i].Index > EndIndex) break;
                    result.Add(entries[i]);
                }
                return _logEntriesCache = result;
            }
        }

        /// <summary>
        /// A cache of log entries by content property, built lazily on demand by <see cref="GetLogEntriesOfContentProperty"/>.
        /// </summary>
        private IndexDictionary<LogContentProperty, List<LogEntry>> _entriesByContentProperty;

        /// <summary>
        /// Builds the entries list for a single content property by iterating <see cref="LogEntries"/>
        /// and checking the property's BitArray in <see cref="ContentPropertyMask"/>.
        /// </summary>
        private void BuildEntriesByContentProperty(LogContentProperty property)
        {
            if (_entriesByContentProperty == null)
            {
                int capacity = MetadataFilterResult.SourceLogCollection.ContentPropertyMask?.Capacity ?? property.Index + 1;
                _entriesByContentProperty = new(capacity);
            }

            MetadataFilterResult.SourceLogCollection.ContentPropertyMask.TryGetValue(property, out BitArray logCollectionMask);
            BitArray filteredMask = MetadataFilterResult.FilteredLogEntriesMask;
            if (logCollectionMask == null || filteredMask == null || logCollectionMask.Length != filteredMask.Length) return;

            // Note: this BitArray spans the entire source log collection, so we can check it directly against the entry indices.
            BitArray contentPropertyMask = new BitArray(filteredMask).And(logCollectionMask);

            List<LogEntry> result = [];
            foreach (LogEntry entry in LogEntries)
            {
                if (contentPropertyMask[entry.Index]) result.Add(entry);
            }
                    
            _entriesByContentProperty[property] = result;
        }

        /// <summary>
        /// Returns the log entries within the current range that have the specified content property.
        /// The result is lazily built and cached per property.
        /// </summary>
        public List<LogEntry> GetLogEntriesOfContentProperty(LogContentProperty property)
        {
            if (!_entriesByContentProperty.ContainsKey(property))
                BuildEntriesByContentProperty(property);
            return _entriesByContentProperty.TryGetValue(property, out List<LogEntry> entries) ? entries : [];
        }
    }
}
