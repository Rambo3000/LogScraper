using System;

namespace LogScraper.Log.LogAppState
{
    /// <summary>
    /// Event arguments for the <see cref="LogAppState.ResetRequested"/> event.
    /// </summary>
    public sealed class ResetEventArgs : EventArgs
    {
        /// <summary>
        /// When <c>true</c> the active filters are preserved (soft reset / erase).
        /// When <c>false</c> all filters are cleared as well (hard reset).
        /// </summary>
        public bool KeepFilters { get; }

        public ResetEventArgs(bool keepFilters)
        {
            KeepFilters = keepFilters;
        }
    }
}
