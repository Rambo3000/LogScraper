using System;
using System.Collections.Generic;
using LogScraper.Log.Filtering;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Log.Rendering;
using LogScraper.LogPostProcessors;

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

        /// <summary>
        /// The main log collection
        /// </summary>
        public StateSlice<LogCollection> LogCollection { get; } = new();
        public bool LogCollectionIsAvailable => LogCollection.Value != null;

        /// <summary>
        /// The result of applying active filters to the log collection.
        /// </summary>
        public StateSlice<LogMetadataFilterResult> MetadataFilterResult { get; } = new();

        /// <summary>
        /// The currently active log range (start/end indices) for rendering.
        /// </summary>
        public StateSlice<LogRange> LogRange { get; } = new();

        /// <summary>
        /// The result of applying active filters to the log collection, along with the range of log entries it covers.
        /// </summary>
        internal StateSlice<LogFilterResultWithRange> FilterResultWithRange { get; } = new();

        /// <summary>
        /// The current settings for how log entries are rendered on screen (e.g. which columns are visible, their widths, etc).
        /// </summary>
        public StateSlice<LogRenderSettings> RenderSettings { get; } = new();

        /// <summary>
        /// The current layout of the log (e.g. which columns are visible, their order, etc).
        /// </summary>
        public StateSlice<LogLayout> LogLayout { get; } = new();

        /// <summary>
        /// Whether to render the original metadata in the log viewport.
        /// </summary>
        public StateSlice<bool> RenderOriginalMetadata { get; } = new();

        /// <summary>
        /// The list of metadata properties to render as separate columns in the log view when <see cref="RenderOriginalMetadata"/> is <c>false</c>.
        /// </summary>
        public StateSlice<List<LogMetadataProperty>> RenderSeperateMetadataProperties { get; } = new();

        /// <summary>
        /// The list of log post-processor kinds that are currently visible to the user and can be applied to log entries.
        /// </summary>
        public StateSlice<List<LogPostProcessorKind>> RenderProcessorKinds { get; } = new();

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
