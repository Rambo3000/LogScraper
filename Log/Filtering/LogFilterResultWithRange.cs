using System;
using System.Collections;
using System.Collections.Generic;
using LogScraper.Log.Content;
using LogScraper.Log.Rendering;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log.Filtering
{
    public class LogFilterResultWithRange(LogMetadataFilterResult metadataFilterResult, LogRange range)
    {
        /// <summary>
        /// The result of applying metadata filters to a log collection, which includes the filtered log entries and a BitArray mask indicating which entries passed the filters.
        /// </summary>
        public LogMetadataFilterResult MetadataFilterResult { get; } = metadataFilterResult;

        /// <summary>
        /// The range of log entries to consider within the filtered results, defined by optional begin and end LogEntry references.
        /// </summary>
        public LogRange Range { get; } = range;

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
                if (_logEntriesCache != null || MetadataFilterResult == null) return _logEntriesCache;

                _logEntriesCache = LogRenderer.GetLogEntriesRange(MetadataFilterResult.LogEntries, Range);
                return _logEntriesCache;
            }
        }

        private BitArray _filteredAndRangedMask = null;

        /// <summary>
        /// Combines the FilteredLogEntriesMask from the metadata filter result with the range to produce a new BitArray
        /// </summary>
        public BitArray FilteredAndRangedMask
        {
            get
            {
                if (_filteredAndRangedMask != null) return _filteredAndRangedMask;

                BitArray filteredMask = MetadataFilterResult.FilteredLogEntriesMask;
                if (filteredMask == null) return new BitArray(0);

                if (Range == null || !Range.IsBeginOrEndSet)
                {
                    _filteredAndRangedMask = filteredMask;
                    return _filteredAndRangedMask;
                }

                int beginIndex = Range.Begin?.Index ?? 0;
                int endIndex = Range.End?.Index ?? (filteredMask.Length - 1);

                BitArray rangeMask = new(filteredMask.Length);
                for (int i = beginIndex; i <= endIndex && i < filteredMask.Length; i++)
                    rangeMask[i] = true;

                _filteredAndRangedMask = new BitArray(filteredMask).And(rangeMask);
                return _filteredAndRangedMask;
            }
        }

        private BitArray _errorMaskCache = null;

        /// <summary>
        /// Returns the count of log entries within the current range that are marked as errors in the source log collection's ErrorMask.
        /// </summary>
        /// <returns>The number of error log entries within the current range.</returns>
        public BitArray ErrorMask
        {
            get
            {
                if (_errorMaskCache != null) return _errorMaskCache;

                BitArray rangedMask = FilteredAndRangedMask;
                BitArray sourceErrorMask = MetadataFilterResult.SourceLogCollection.ErrorMask;

                // During continuous reading the source ErrorMask may have grown beyond the snapshot
                // captured in FilteredAndRangedMask. Normalize to rangedMask length before AND-ing.
                BitArray errorMask;
                if (sourceErrorMask.Length == rangedMask.Length)
                {
                    errorMask = new BitArray(sourceErrorMask);
                }
                else
                {
                    errorMask = new BitArray(rangedMask.Length);
                    int copyLength = Math.Min(sourceErrorMask.Length, rangedMask.Length);
                    for (int i = 0; i < copyLength; i++)
                        errorMask[i] = sourceErrorMask[i];
                }

                _errorMaskCache = errorMask.And(rangedMask);
                return _errorMaskCache;
            }
        }

        private List<LogEntry> _errorLogEntries = null;

        /// <summary>
        /// Returns the count of log entries within the current range that are marked as errors in the source log collection's ErrorMask.
        /// </summary>
        /// <returns>The number of error log entries within the current range.</returns>
        public List<LogEntry> ErrorLogEntries
        {
            get
            {
                if (_errorLogEntries != null) return _errorLogEntries;
                _errorLogEntries = [];

                foreach (LogEntry entry in LogEntries)
                {
                    if (ErrorMask[entry.Index]) _errorLogEntries.Add(entry);
                }
                return _errorLogEntries;
            }
        }

        /// <summary>
        /// A cache of log entries by content property, built lazily on demand by <see cref="GetLogEntriesOfContentProperty"/>.
        /// </summary>
        private IndexDictionary<LogContentProperty, List<LogEntry>> _entriesByContentPropertCache;

        /// <summary>
        /// Returns the log entries within the current range that have the specified content property.
        /// The result is lazily built and cached per property.
        /// </summary>
        public List<LogEntry> GetLogEntriesOfContentProperty(LogContentProperty property)
        {
            if (_entriesByContentPropertCache == null || !_entriesByContentPropertCache.ContainsKey(property))
                BuildEntriesByContentProperty(property);
            return _entriesByContentPropertCache.TryGetValue(property, out List<LogEntry> entries) ? entries : [];
        }

        /// <summary>
        /// Builds the entries list for a single content property by iterating <see cref="LogEntries"/>
        /// and checking the property's BitArray in <see cref="ContentPropertyMask"/>.
        /// </summary>
        private void BuildEntriesByContentProperty(LogContentProperty property)
        {
            if (_entriesByContentPropertCache == null)
            {
                int capacity = MetadataFilterResult.SourceLogCollection.ContentPropertyMask?.Capacity ?? property.Index + 1;
                _entriesByContentPropertCache = new(capacity);
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

            _entriesByContentPropertCache[property] = result;
        }
    }
}
