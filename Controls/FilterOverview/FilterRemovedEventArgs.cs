using System;
using LogScraper.Log.Metadata;

namespace LogScraper.Controls.FilterOverview
{
    /// <summary>
    /// Provides data for the <see cref="ActiveFilterOverviewControl.FilterRemoved"/> event.
    /// </summary>
    public class FilterRemovedEventArgs : EventArgs
    {
        /// <summary>
        /// The metadata property whose filter chip was removed.
        /// </summary>
        public LogMetadataProperty Property { get; }

        /// <summary>
        /// The single active value when the filter had exactly one value selected; otherwise <see langword="null"/>,
        /// indicating all values for the property should be cleared.
        /// </summary>
        public LogMetadataValue SingleValue { get; }

        public FilterRemovedEventArgs(LogMetadataProperty property, LogMetadataValue singleValue = null)
        {
            Property = property;
            SingleValue = singleValue;
        }
    }
}
