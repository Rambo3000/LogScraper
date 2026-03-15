using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LogScraper.Log.Layout;
using LogScraper.Utilities.Extensions;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// A user control that provides an overview of metadata filters for log entries.
    /// Allows updating, resetting, and managing metadata filters.
    /// </summary>
    public partial class UserControlMetadataFilterOverview : UserControl
    {
        // Dictionary to store per-property filter controls for quick access.
        private readonly Dictionary<LogMetadataProperty, UserControlLogMetadataFilter> filterControls = [];

        /// <summary>
        /// Event triggered when a filter is changed.
        /// </summary>
        public event EventHandler FilterChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlMetadataFilterOverview"/> class.
        /// </summary>
        public UserControlMetadataFilterOverview()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The currently selected log entry. Set to null to deselect.
        /// Updates the indicator in each child control to show which value matches the selected line.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogEntry SelectedLogEntry
        {
            get => selectedLogEntry;
            set
            {
                selectedLogEntry = value;
                foreach (UserControlLogMetadataFilter control in filterControls.Values)
                    control.SelectedLogEntry = selectedLogEntry;
            }
        }

        private int previousWidth = 0;
        private bool isResetFiltersInProgress = false;
        private LogEntry selectedLogEntry;

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
        /// <param name="stats">Current filter stats (counts per value). Pass null when no filters are active.</param>
        public void UpdateFilterControls(LogLayout logLayout, LogCollection logCollection, List<LogMetadataFilterStats> stats)
        {
            this.SuspendDrawing();

            UserControlLogMetadataFilter previousControl = null;

            foreach (LogMetadataProperty property in logLayout.LogMetadataProperties)
            {
                if (!logCollection.MetadataValues.TryGetValue(property, out List<LogMetadataValue> allValues))
                    continue;

                LogMetadataFilterStats propertyStats = stats?.Find(s => s.Property == property);

                if (!filterControls.TryGetValue(property, out UserControlLogMetadataFilter control))
                {
                    control = new UserControlLogMetadataFilter(property.Description)
                    {
                        Width = ClientSize.Width
                    };
                    control.FilterChanged += OnFilterChanged;
                    Controls.Add(control);
                    filterControls[property] = control;
                }

                control.UpdateListView(property, allValues, propertyStats);

                if (previousControl != null)
                    control.Top = previousControl.Bottom + 5;

                previousControl = control;
            }

            this.ResumeDrawing();
        }

        /// <summary>
        /// Updates only the counts in all filter controls without rebuilding the value lists.
        /// Call this after filtering to reflect the new counts.
        /// </summary>
        /// <param name="stats">Updated filter stats per property.</param>
        public void UpdateFilterControlsCount(List<LogMetadataFilterStats> stats)
        {
            this.SuspendDrawing();

            foreach (LogMetadataFilterStats propertyStats in stats)
            {
                if (filterControls.TryGetValue(propertyStats.Property, out UserControlLogMetadataFilter control))
                    control.UpdateCounts(propertyStats);
            }

            this.ResumeDrawing();
        }

        /// <summary>
        /// Retrieves the current metadata filters from all child controls.
        /// Only returns filters that have active values.
        /// </summary>
        public List<LogMetadataFilter> GetActiveFilters()
        {
            List<LogMetadataFilter> filters = [];

            foreach (UserControlLogMetadataFilter control in filterControls.Values)
            {
                LogMetadataFilter filter = control.GetCurrentFilter();
                if (filter.ActiveValues.Count > 0)
                    filters.Add(filter);
            }

            return filters;
        }

        /// <summary>
        /// Enables filtering on a specific metadata value for a given property, in the specified mode.
        /// </summary>
        /// <param name="property">The metadata property to target.</param>
        /// <param name="value">The specific value to enable or disable filtering on.</param>
        /// <param name="isEnabled">Whether to enable or disable the filter.</param>
        public void EnableFilterOnSpecificMetadataValue(LogMetadataProperty property, LogMetadataValue value, bool isEnabled)
        {
            if (filterControls.TryGetValue(property, out UserControlLogMetadataFilter control))
                control.EnableDisableFilterOnSpecificMetadataValue(value, isEnabled);
        }

        /// <summary>
        /// Resets the filter controls by clearing all child controls and their event handlers.
        /// </summary>
        public void Reset()
        {
            this.SuspendDrawing();

            foreach (Control control in Controls)
            {
                if (control is UserControlLogMetadataFilter filterControl)
                    filterControl.FilterChanged -= OnFilterChanged;
            }

            Controls.Clear();
            filterControls.Clear();

            this.ResumeDrawing();
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
                if (control is UserControlLogMetadataFilter filterControl)
                    filterControl.ResetFilters();
            }

            isResetFiltersInProgress = false;
            OnFilterChanged(this, EventArgs.Empty);
            this.ResumeDrawing();
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            if (!isResetFiltersInProgress) FilterChanged?.Invoke(this, e);
        }

        protected override Point ScrollToControl(Control activeControl) => AutoScrollPosition;
    }
}