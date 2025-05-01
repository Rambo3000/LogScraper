﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Extensions;
using LogScraper.Log;
using LogScraper.Log.Layout;

namespace LogScraper.Log.Metadata
{
    /// <summary>
    /// A user control that provides an overview of metadata filters for log entries.
    /// Allows updating, resetting, and managing metadata filters.
    /// </summary>
    public partial class UserControlMetadataFilterOverview : UserControl
    {
        // Dictionary to store metadata property controls for quick access.
        private readonly Dictionary<LogMetadataPropertyAndValues, UserControlLogMetadataFilter> logMetadataPropertyControls = [];

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

        // Tracks the previous width of the panel to avoid unnecessary resizing.
        private int previousWidth = 0;

        /// <summary>
        /// Handles the resize event of the control to adjust the width of child controls.
        /// </summary>
        private void UserControlMetadataFilterOverview_Resize(object sender, EventArgs e)
        {
            int newWidth = ClientSize.Width;
            if (newWidth == previousWidth) return;

            // Suspend layout updates to improve performance during resizing.
            this.SuspendDrawing();
            SuspendLayout();

            foreach (Control ctrl in Controls)
            {
                if (ctrl.Width != newWidth)
                {
                    ctrl.Width = newWidth;
                }
            }

            // Resume layout updates after resizing.
            ResumeLayout();
            this.ResumeDrawing();
            previousWidth = newWidth;
        }

        /// <summary>
        /// Updates the filter controls based on the provided log layout and log collection.
        /// </summary>
        /// <param name="logLayout">The layout of the log, including metadata properties.</param>
        /// <param name="logCollection">The collection of log entries to filter.</param>
        public void UpdateFilterControls(LogLayout logLayout, LogCollection logCollection)
        {
            // Get the list of metadata properties and their values from the log collection.
            List<LogMetadataPropertyAndValues> logMetadataPropertyAndValues =
                LogEntryClassifier.GetLogEntriesListOfMetadataPropertyAndValues(logCollection.LogEntries, logLayout.LogMetadataProperties);

            UserControlLogMetadataFilter previousFilter = null;

            // Suspend layout updates to improve performance during control updates.
            this.SuspendDrawing();

            foreach (LogMetadataPropertyAndValues filterProperty in logMetadataPropertyAndValues)
            {
                if (!logMetadataPropertyControls.TryGetValue(filterProperty, out var userControlLogFilter))
                {
                    // Create a new UserControlLogMetadataFilter and add it to the dictionary and the panel.
                    userControlLogFilter = new UserControlLogMetadataFilter(filterProperty.LogMetadataProperty.Description)
                    {
                        Width = ClientSize.Width
                    };
                    userControlLogFilter.FilterChanged += OnFilterChanged;

                    Controls.Add(userControlLogFilter);
                    logMetadataPropertyControls[filterProperty] = userControlLogFilter;
                }

                // Update the list view of the filter control with the current property values.
                userControlLogFilter.UpdateListView(filterProperty);

                // Position the filter control below the previous one.
                if (previousFilter != null)
                {
                    userControlLogFilter.Top = previousFilter.Bottom + 5;
                }
                previousFilter = userControlLogFilter;
            }

            this.ResumeDrawing();
        }

        /// <summary>
        /// Handles the FilterChanged event from child controls and propagates it to subscribers.
        /// </summary>
        private void OnFilterChanged(object sender, EventArgs e)
        {
            FilterChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Retrieves the current metadata properties and their values from all child controls.
        /// </summary>
        /// <returns>A list of metadata properties and their values.</returns>
        public List<LogMetadataPropertyAndValues> GetMetadataPropertyAndValues()
        {
            List<LogMetadataPropertyAndValues> logMetadataPropertyAndValuesList = [];

            foreach (UserControlLogMetadataFilter userControlLogFilter in Controls)
            {
                logMetadataPropertyAndValuesList.Add(userControlLogFilter.GetCurrentLogMetadataPropertyAndValues());
            }

            return logMetadataPropertyAndValuesList;
        }

        /// <summary>
        /// Updates the count of metadata property values in the filter controls.
        /// </summary>
        /// <param name="filterProperties">The list of metadata properties and their values to update.</param>
        public void UpdateFilterControlsCount(List<LogMetadataPropertyAndValues> filterProperties)
        {
            foreach (LogMetadataPropertyAndValues filterProperty in filterProperties)
            {
                if (logMetadataPropertyControls.TryGetValue(filterProperty, out var userControlLogFilter))
                {
                    userControlLogFilter.UpdateCountInListView(filterProperty);
                }
            }
        }

        /// <summary>
        /// Resets the filter controls by clearing all child controls and their event handlers.
        /// </summary>
        public void Reset()
        {
            foreach (Control control in Controls)
            {
                if (control is UserControlLogMetadataFilter userControlLogMetadataFilter)
                {
                    userControlLogMetadataFilter.FilterChanged -= OnFilterChanged;
                }
            }

            Controls.Clear();
            logMetadataPropertyControls.Clear();
        }
    }
}
