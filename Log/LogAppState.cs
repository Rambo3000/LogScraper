using System;
using LogScraper.Log.Filtering;
using LogScraper.Log.Rendering;

namespace LogScraper.Log
{
    /// <summary>
    /// Central observable state container for the application.
    /// Owns the three core derived-state objects and raises events when any of them changes,
    /// so controls can react without the form manually pushing updates to each one.
    /// </summary>
    public sealed class LogAppState
    {
        public static LogAppState Instance { get; } = new();
        private LogAppState() { }

        // ── raw data ────────────────────────────────────────────────────────────
        public LogCollection LogCollection { get; private set; }
        public event EventHandler LogCollectionChanged;

        public void SetLogCollection(LogCollection collection)
        {
            LogCollection = collection;
            LogCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── after metadata filters applied ──────────────────────────────────────
        public LogMetadataFilterResult MetadataFilterResult { get; private set; }
        public event EventHandler MetadataFilterResultChanged;

        public void SetMetadataFilterResult(LogMetadataFilterResult result)
        {
            MetadataFilterResult = result;
            MetadataFilterResultChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── log range (begin/end viewport selection) ────────────────────────────
        public LogRange LogRange { get; private set; } = new();
        public event EventHandler LogRangeChanged;

        public void SetLogRange(LogRange range)
        {
            LogRange = range ?? new();
            LogRangeChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── after range applied to metadata filter result ───────────────────────
        internal LogFilterResultWithRange FilterResultWithRange { get; private set; }
        public event EventHandler FilterResultWithRangeChanged;

        internal void SetFilterResultWithRange(LogFilterResultWithRange result)
        {
            FilterResultWithRange = result;
            FilterResultWithRangeChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── render settings (how entries are formatted on screen) ───────────────
        public LogRenderSettings RenderSettings { get; private set; }
        public event EventHandler RenderSettingsChanged;

        public void SetRenderSettings(LogRenderSettings settings)
        {
            RenderSettings = settings;
            RenderSettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── full reset ───────────────────────────────────────────────────────────
        public void Clear()
        {
            LogCollection?.Clear();
            LogCollection = null;
            MetadataFilterResult = null;
            FilterResultWithRange = null;
            LogRange = new();
            RenderSettings = null;

            LogCollectionChanged?.Invoke(this, EventArgs.Empty);
            MetadataFilterResultChanged?.Invoke(this, EventArgs.Empty);
            FilterResultWithRangeChanged?.Invoke(this, EventArgs.Empty);
            LogRangeChanged?.Invoke(this, EventArgs.Empty);
            RenderSettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
