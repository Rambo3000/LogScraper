using System;
using System.Collections.Generic;
using LogScraper.Log.Filtering;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Log.Rendering;
using LogScraper.LogPostProcessors;
using LogScraper.Sources.Workers;

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
        private LogAppState()
        {
            // Whenever the log collection, layout, or active metadata filters change, we need to recalculate the filtered result.
            LogCollection.Changed += (s, e) => UpdateMetadataFilterResult();
            Layout.Changed += (s, e) => UpdateMetadataFilterResult();
            MetadataFilters.Changed += (s, e) => UpdateMetadataFilterResult();

            // Whenever the log range or the metadata-filtered result changes, we need to recalculate the combined FilterResultWithRange.
            Range.Changed += (s, e) => UpdateFilterResultWithRange();
            MetadataFilterResult.Changed += (s, e) => UpdateFilterResultWithRange();

            // Combine the various render-related settings into a single LogRenderSettings object
            Layout.Changed += (s, e) => UpdateRenderSettings();
            RenderOriginalMetadata.Changed += (s, e) => UpdateRenderSettings();
            RenderSeperateMetadataProperties.Changed += (s, e) => UpdateRenderSettings();
            RenderProcessorKinds.Changed += (s, e) => UpdateRenderSettings();
            RenderFlowTreeSettings.Changed += (s, e) => UpdateRenderSettings();
        }

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
        public StateSlice<LogRange> Range { get; } = new();

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
        public StateSlice<LogLayout> Layout { get; } = new();

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
        /// The current settings for how the flow tree is rendered on screen (e.g. which columns are visible, their widths, etc).
        /// </summary>
        public StateSlice<LogFlowTreeRenderSettings> RenderFlowTreeSettings { get; } = new();

        /// <summary>
        /// The list of active metadata filters that are applied to the log collection to produce <see cref="MetadataFilterResult"/>.
        /// </summary>
        public StateSlice<List<LogMetadataFilter>> MetadataFilters { get; } = new();

        /// <summary>
        /// The log entry that is currently selected in the log viewport, if any.
        /// </summary>
        public StateSlice<LogEntry> ViewportSelectedLogEntry { get; } = new();

        /// <summary>
        /// The log entry that is currently at the top of the log viewport (i.e. the first visible log entry), if any.
        /// </summary>
        public StateSlice<LogRange> ViewportVisibleRange { get; } = new();

        /// <summary>
        /// The list of user-created bookmarks (log entries marked as important by the user for easy reference).
        /// </summary>
        public StateSlice<SortedList<int, LogEntry>> Bookmarks { get; } = new();

        /// <summary>
        /// The timestamp of the last fetched log entry, used for incremental (trail) fetching.
        /// </summary>
        public StateSlice<DateTime?> LastTrailTime { get; } = new();

        /// <summary>
        /// Whether a source processing worker is currently active.
        /// </summary>
        public StateSlice<bool> IsSourceProcessingActive { get; } = new();

        /// <summary>
        /// Whether the currently selected log source is valid and ready to fetch from.
        /// </summary>
        public StateSlice<bool> IsSourceValid { get; } = new();

        /// <summary>
        /// The current status message and whether it represents a success or an error.
        /// </summary>
        public StateSlice<(string Message, bool IsSuccess)> StatusMessage { get; } = new();

        /// <summary>
        /// Raised when a reset is requested.
        /// Subscribe to this event in controls or forms to perform their own cleanup.
        /// </summary>
        public event EventHandler<ResetEventArgs> ResetRequested;

        private bool _resetInProgress = false;

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
            //Track reset in progress so UpdateMetadataFilterResult is just forcely called once at the end of this method
            _resetInProgress = true;

            LogCollection.Value?.Clear();
            LogCollection.ForceSet(null);

            MetadataFilterResult.ForceSet(null);
            FilterResultWithRange.ForceSet(null);
            Range.ForceSet(LogRange.Full);
            RenderProcessorKinds.ForceSet([]);
            if (!keepFilters) MetadataFilters.ForceSet([]);

            ViewportSelectedLogEntry.ForceSet(null);
            ViewportVisibleRange.ForceSet(null);
            LastTrailTime.ForceSet(null);
            StatusMessage.ForceSet((string.Empty, true));

            _resetInProgress = false;

            UpdateMetadataFilterResult();
            UpdateRenderSettings();

            ResetRequested?.Invoke(this, new ResetEventArgs(keepFilters));

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Updates the current render settings based on the latest user interface values.
        /// </summary>
        private void UpdateRenderSettings()
        {
            if (_resetInProgress) return;

            LogRenderSettings logRenderSettings = new()
            {
                LogLayout = Layout.Value,
                ShowOriginalMetadata = RenderOriginalMetadata.Value,
                SelectedMetadataProperties = RenderSeperateMetadataProperties.Value,
                LogPostProcessorKinds = RenderProcessorKinds.Value,
                LogFlowTreeRenderSettings = RenderFlowTreeSettings.Value
            };
            RenderSettings.Set(logRenderSettings);
        }

        /// <summary>
        /// Updates the <see cref="MetadataFilterResult"/> by applying the active <see cref="MetadataFilters"/> to the <see cref="LogCollection"/>
        /// </summary>
        private void UpdateMetadataFilterResult()
        {
            if (_resetInProgress) return;

            if (!LogCollectionIsAvailable || Layout.Value == null || MetadataFilters.Value == null)
            {
                MetadataFilterResult.Set(null);
                return;
            }

            LogMetadataFilterResult metadataFilterResult = LogMetadataFilterEngine.Apply(LogCollection.Value, MetadataFilters.Value, Layout.Value);
           
            MetadataFilterResult.Set(metadataFilterResult);
        }

        /// <summary>
        /// Updates the <see cref="FilterResultWithRange"/> by combining the current <see cref="MetadataFilterResult"/> with the current <see cref="Range"/>.
        /// </summary>
        private void UpdateFilterResultWithRange()
        {
            if (_resetInProgress) return;

            FilterResultWithRange.Set(new(MetadataFilterResult.Value, Range.Value));
        }
    }
}
