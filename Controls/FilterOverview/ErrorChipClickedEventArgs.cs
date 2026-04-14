using System;
using System.Collections.Generic;
using LogScraper.Log;

namespace LogScraper.Controls.FilterOverview
{
    /// <summary>
    /// Provides data for the <see cref="ActiveFilterOverviewControl.ErrorChipClicked"/> event.
    /// </summary>
    public class ErrorChipClickedEventArgs : EventArgs
    {
        /// <summary>
        /// The error log entries that triggered the chip.
        /// </summary>
        public IReadOnlyList<LogEntry> ErrorEntries { get; }

        public ErrorChipClickedEventArgs(IReadOnlyList<LogEntry> entries)
        {
            ErrorEntries = entries;
        }
    }
}
