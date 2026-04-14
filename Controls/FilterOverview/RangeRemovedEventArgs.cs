using System;

namespace LogScraper.Controls.FilterOverview
{
    /// <summary>
    /// Provides data for the <see cref="ActiveFilterOverviewControl.RangeRemoved"/> event.
    /// </summary>
    public class RangeRemovedEventArgs : EventArgs
    {
        /// <summary>
        /// Which end of the range (Begin or End) was removed.
        /// </summary>
        public LogRangeChipVariant Variant { get; }

        public RangeRemovedEventArgs(LogRangeChipVariant variant)
        {
            Variant = variant;
        }
    }
}
