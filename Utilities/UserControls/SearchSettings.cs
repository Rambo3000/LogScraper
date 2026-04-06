using System;
using LogScraper.Log.Rendering;

namespace LogScraper.Utilities.UserControls
{
    public class SearchSettings : IEquatable<SearchSettings>
    {
        public LogRange LogRange { get; set; }
        public string SearchText { get; set; }
        public bool CaseSensitive { get; set; }
        public bool WholeWord { get; set; }
        public bool IsMetadataSearchEnabled { get; set; }

        /// <summary>
        /// The direction for Next/Previous navigation. Not included in equality comparison —
        /// direction alone does not constitute a meaningful change to the result set.
        /// </summary>
        public UserControlSearch.SearchDirection Direction { get; set; }

        /// <summary>
        /// Whether navigation should wrap around when reaching the end or beginning of the log.
        /// Applies to Next/Previous only; not relevant to the result list.
        /// </summary>
        public bool WrapAround { get; set; }

        /// <summary>
        /// Render settings used to produce display strings for result list items.
        /// Not included in equality comparison — render settings do not affect which entries match,
        /// only how they are displayed.
        /// </summary>
        public LogRenderSettings LogRenderSettings { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SearchSettings);
        }

        public bool Equals(SearchSettings other)
        {
            if (other == null) return false;

            bool logRangeEqual = (LogRange == null && other.LogRange == null) ||
                                 (LogRange != null && LogRange.Equals(other.LogRange));

            // Direction and WrapAround are intentionally excluded:
            // they don't affect the result list contents.
            return logRangeEqual &&
                   SearchText == other.SearchText &&
                   CaseSensitive == other.CaseSensitive &&
                   WholeWord == other.WholeWord &&
                   IsMetadataSearchEnabled == other.IsMetadataSearchEnabled;
        }

        public override int GetHashCode()
        {
            // Direction and WrapAround excluded to match Equals.
            return HashCode.Combine(LogRange, SearchText, CaseSensitive, WholeWord, IsMetadataSearchEnabled);
        }
    }
}