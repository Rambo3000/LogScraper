using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LogScraper.Controls.Generic;
using LogScraper.Log;
using LogScraper.Log.Filtering;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Metadata;

namespace LogScraper.Controls.Metadata
{
    /// <summary>
    /// A user control that provides an overview of metadata filters for log entries.
    /// Allows updating, resetting, and managing metadata filters.
    /// </summary>
    public partial class LogMetadataFiltersOverviewControl : UserControl
    {
        // Dictionary to store per-property filter controls for quick access.
        private readonly Dictionary<LogMetadataProperty, LogMetadataFilterControl> filterControls = [];

        // Ordered list of filter controls, used for reflowing positions after collapse/expand.
        private readonly List<LogMetadataFilterControl> orderedFilterControls = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMetadataFiltersOverviewControl"/> class.
        /// </summary>
        public LogMetadataFiltersOverviewControl()
        {
            InitializeComponent();
        }
        private void UserControlMetadataFilterOverview_Load(object sender, EventArgs e)
        {
            LogAppState.Instance.ResetRequested += OnResetRequested;
            LogAppState.Instance.LogCollection.Changed += (s, e) => UpdateFilterControls();
            LogAppState.Instance.Layout.Changed += (s, e) => UpdateFilterControls();
            LogAppState.Instance.MetadataFilterResult.Changed += (s, e) => UpdateFilterControlsCount();
        }

        private void OnResetRequested(object sender, ResetEventArgs e)
        {
            if (e.KeepFilters)
            {
                UpdateFilterControlsCountToZero();
            }
            else
            {
                Reset();
            }
        }

        private int previousWidth = 0;
        private bool isResetFiltersInProgress = false;

        /// <summary>
        /// Handles resize to adjust child control widths.
        /// </summary>
        private void UserControlMetadataFilterOverview_Resize(object sender, EventArgs e)
        {
            int newWidth = ClientSize.Width;
            if (newWidth == previousWidth) return;

            this.SuspendDrawing();
            SuspendLayout();

            foreach (Control ctrl in Controls)
            {
                if (ctrl.Width != newWidth)
                    ctrl.Width = newWidth;
            }

            ResumeLayout();
            this.ResumeDrawing();
            previousWidth = newWidth;
        }

        /// <summary>
        /// Updates the filter controls based on the provided log layout and log collection.
        /// Creates controls for new properties, updates values and counts for existing ones.
        /// </summary>
        /// <param name="logLayout">The layout of the log, including metadata properties.</param>
        /// <param name="logCollection">The collection of log entries, providing the value pool.</param>
        public void UpdateFilterControls()
        {
            LogLayout logLayout = LogAppState.Instance.Layout.Value;
            LogCollection logCollection = LogAppState.Instance.LogCollection.Value;

            if (logLayout == null || logCollection == null) return;

            // Skip if a write is in progress; LogCollection.Changed will retrigger after the write completes.
            if (!logCollection.TryAcquireReadAccess()) return;

            this.SuspendDrawing();

            try
            {
                LblExplenation.Visible = logLayout.LogMetadataProperties.Count == 0;

                LogMetadataFilterControl previousControl = null;

                foreach (LogMetadataProperty property in logLayout.LogMetadataProperties)
                {
                    if (!logCollection.MetadataValues.TryGetValue(property, out List<LogMetadataValue> allValues))
                        continue;

                    if (!filterControls.TryGetValue(property, out LogMetadataFilterControl control))
                    {
                        control = new LogMetadataFilterControl(property.Description)
                        {
                            Width = ClientSize.Width
                        };
                        control.FilterChanged += OnFilterChanged;
                        control.CollapseChanged += OnCollapseChanged;
                        Controls.Add(control);
                        filterControls[property] = control;
                        orderedFilterControls.Add(control);
                    }

                    control.UpdateListView(property, allValues);

                    if (previousControl != null)
                        control.Top = previousControl.Bottom + (previousControl.Collapsed ? 3 : 15);

                    previousControl = control;
                }
            }
            finally
            {
                logCollection.ReleaseReadAccess();
                UpdateFilterControlsCount();
                this.ResumeDrawing();
            }
        }

        /// <summary>
        /// Updates only the counts in all filter controls without rebuilding the value lists.
        /// Call this after filtering to reflect the new counts.
        /// </summary>
        /// <param name="stats">Updated filter stats per property.</param>
        private void UpdateFilterControlsCount()
        {
            if (LogAppState.Instance.MetadataFilterResult.Value == null || filterControls.Count == 0) return;

            this.SuspendDrawing();
            List<LogMetadataFilterStats> stats = [.. LogAppState.Instance.MetadataFilterResult.Value.FilterStats.Values];

            foreach (LogMetadataFilterStats propertyStats in stats)
            {
                if (filterControls.TryGetValue(propertyStats.Property, out LogMetadataFilterControl control))
                    control.UpdateCounts(propertyStats);
            }

            this.ResumeDrawing();
        }
        /// <summary>
        /// Updates only the counts in all filter controls without rebuilding the value lists.
        /// Call this after filtering to reflect the new counts.
        /// </summary>
        /// <param name="stats">Updated filter stats per property.</param>
        public void UpdateFilterControlsCountToZero()
        {
            this.SuspendDrawing();

            foreach (LogMetadataFilterControl control in filterControls.Values)
                control.UpdateCountsToZero();

            this.ResumeDrawing();
        }

        /// <summary>
        /// Retrieves the current metadata filters from all child controls.
        /// Only returns filters that have active values.
        /// </summary>
        private List<LogMetadataFilter> GetActiveFilters()
        {
            List<LogMetadataFilter> filters = [];

            foreach (LogMetadataFilterControl control in filterControls.Values)
            {
                LogMetadataFilter filter = control.GetCurrentFilter();
                if (filter.ActiveValues.Count > 0)
                    filters.Add(filter);
            }

            return filters;
        }

        /// <summary>
        /// Resets the filter controls by clearing all child controls and their event handlers.
        /// </summary>
        public void Reset()
        {
            this.SuspendDrawing();

            foreach (Control control in Controls)
            {
                if (control is LogMetadataFilterControl filterControl)
                {
                    filterControl.FilterChanged -= OnFilterChanged;
                    filterControl.CollapseChanged -= OnCollapseChanged;
                }
            }

            foreach (LogMetadataFilterControl control in orderedFilterControls)
                Controls.Remove(control);

            filterControls.Clear();
            orderedFilterControls.Clear();

            LblExplenation.Visible = true;
            this.ResumeDrawing();
        }

        /// <summary>
        /// Removes a specific filter value (or all values) for the given property.
        /// If <paramref name="specificValue"/> is non-null, only that value is deselected;
        /// otherwise all checked values for the property are cleared.
        /// </summary>
        public void RemoveFilter(LogMetadataProperty property, LogMetadataValue specificValue)
        {
            if (!filterControls.TryGetValue(property, out LogMetadataFilterControl control)) return;
            if (specificValue != null)
                control.DeselectValue(specificValue);
            else
                control.DeselectAllValues();
        }

        /// <summary>
        /// Resets all active filter selections without removing the controls.
        /// </summary>
        internal void ResetFilters()
        {
            this.SuspendDrawing();
            isResetFiltersInProgress = true;

            foreach (Control control in Controls)
            {
                if (control is LogMetadataFilterControl filterControl)
                    filterControl.ResetFilters();
            }

            isResetFiltersInProgress = false;
            OnFilterChanged(this, EventArgs.Empty);
            this.ResumeDrawing();
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            if (!isResetFiltersInProgress) LogAppState.Instance.MetadataFilters.Set(GetActiveFilters());
        }

        private void OnCollapseChanged(object sender, EventArgs e) => ReflowControls();

        private void ReflowControls()
        {
            this.SuspendDrawing();
            SuspendLayout();

            Point previousScrollPosition = AutoScrollPosition;
            AutoScrollPosition = Point.Empty;

            LogMetadataFilterControl previousControl = null;
            foreach (LogMetadataFilterControl control in orderedFilterControls)
            {
                if (previousControl != null)
                    control.Top = previousControl.Bottom + (previousControl.Collapsed ? 3 : 15);

                previousControl = control;
            }

            ResumeLayout();

            AutoScrollPosition = new Point(
                Math.Abs(previousScrollPosition.X),
                Math.Abs(previousScrollPosition.Y));

            this.ResumeDrawing();
        }

        protected override Point ScrollToControl(Control activeControl) => AutoScrollPosition;

    }
}