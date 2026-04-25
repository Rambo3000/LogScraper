using System;
using LogScraper.Log.Filtering;
using LogScraper.Log.Rendering;

namespace LogScraper.Log.LogAppState
{
    /// <summary>
    /// Central observable state container for the application.
    /// Owns the core derived-state objects and raises events when any of them changes,
    /// so controls can react without the form manually pushing updates to each one.
    /// </summary>
    public sealed class LogAppState
    {
        public static LogAppState Instance { get; } = new();
        private LogAppState() { }

        // ── raw data ────────────────────────────────────────────────────────────
        public StateSlice<LogCollection> LogCollection { get; } = new();
        public bool LogCollectionIsAvailable => LogCollection.Value != null;

        // ── after metadata filters applied ──────────────────────────────────────
        public StateSlice<LogMetadataFilterResult> MetadataFilterResult { get; } = new();

        // ── log range (begin/end viewport selection) ────────────────────────────
        public StateSlice<LogRange> LogRange { get; } = new();

        // ── after range applied to metadata filter result ───────────────────────
        internal StateSlice<LogFilterResultWithRange> FilterResultWithRange { get; } = new();

        // ── render settings (how entries are formatted on screen) ───────────────
        public StateSlice<LogRenderSettings> RenderSettings { get; } = new();

        // ── reset ───────────────────────────────────────────────────────────────
        /// <summary>
        /// Raised when a reset is requested.
        /// Subscribe to this event in controls or forms to perform their own cleanup.
        /// </summary>
        public event EventHandler<ResetEventArgs> ResetRequested;

        /// <summary>
        /// Resets the application state and raises <see cref="ResetRequested"/> so all subscribers
        /// can perform their own cleanup.
        /// </summary>
        /// <param name="keepFilters">
        /// <c>true</c> = soft reset: log data is cleared, active filters are preserved.
        /// <c>false</c> = hard reset: log data and all filters are cleared.
        /// </param>
        public void Reset(bool keepFilters)
        {
            LogCollection.Value?.Clear();
            LogCollection.ForceSet(null);
            MetadataFilterResult.ForceSet(null);
            FilterResultWithRange.ForceSet(null);
            LogRange.ForceSet(new Rendering.LogRange());
            RenderSettings.ForceSet(null);

            ResetRequested?.Invoke(this, new ResetEventArgs(keepFilters));
        }
    }
}
