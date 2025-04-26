using LogScraper.Extensions;
using LogScraper.Log.Collection;
using LogScraper.Log.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LogScraper
{
    internal partial class UserControlBeginEndFilter : UserControl
    {
        private const string DefaulSearchtText = "Filteren...";

        public event EventHandler FilterChanged;
        /// <summary>
        ///     Used for showing the error log entries for non-error LogContentProperties
        /// </summary>
        private LogContentProperty LogContentPropertyError;

        public UserControlBeginEndFilter()
        {
            InitializeComponent();
            SelectedItemBackColor = Brushes.Orange;
            //Preset the default search text
            TxtSearch_Leave(null, null);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush SelectedItemBackColor { get; set; }

        private List<LogEntry> LogEntriesLatestVersion;

        public void UpdateFilterTypes(List<LogContentProperty> logContentProperties)
        {
            CboLogContentType.Items.Clear();
            if (logContentProperties == null || logContentProperties.Count == 0) return;
            CboLogContentType.Items.AddRange([.. logContentProperties]);
            foreach (LogContentProperty logContentProperty in logContentProperties)
            {
                if (logContentProperty.Description == "Errors")
                {
                    LogContentPropertyError = logContentProperty;
                    break;
                }
            }
            if (CboLogContentType.Items.Count > 0) CboLogContentType.SelectedIndex = 0;
        }

        public void UpdateLogEntries(List<LogEntry> logEntries)
        {
            LogEntriesLatestVersion = logEntries;
            UpdateLogContentList();
        }
        private void UpdateLogContentList()
        {
            // If there are no log entries, clear the list and return
            if (LogEntriesLatestVersion == null)
            {
                LstLogContent.Items.Clear();
                return;
            }

            // Get the selected log content property
            LogContentProperty logContentProperty = (LogContentProperty)CboLogContentType.SelectedItem;

            // If no log content property is selected, return
            if (logContentProperty == null) return;

            // Create a list to store the log entries with overridden ToString method
            List<LogEntryWithToStringOverride> logEntryWithToStringOverrides = GetLogEntryWithToStringOverridesList(logContentProperty);

            UpdateOrRedrawList(logEntryWithToStringOverrides);
        }

        private void UpdateOrRedrawList(List<LogEntryWithToStringOverride> newLogEntries)
        {
            int currentCount = LstLogContent.Items.Count;
            int newCount = newLogEntries.Count;

            int compareCount = Math.Min(currentCount, newCount);
            bool startMatches = true;

            for (int i = 0; i < compareCount; i++)
            {
                if (!newLogEntries[i].Content.Equals(((LogEntryWithToStringOverride)LstLogContent.Items[i]).Content))
                {
                    startMatches = false;
                    break;
                }
            }

            // Case 1: Exactly same items — skip update
            if (startMatches && currentCount == newCount)
            {
                return;
            }

            // Case 2: New list extends existing — add only new items
            if (startMatches && newCount > currentCount)
            {
                LstLogContent.SuspendDrawing();
                for (int i = currentCount; i < newCount; i++)
                {
                    LstLogContent.Items.Add(newLogEntries[i]);
                }
                LstLogContent.ResumeDrawing();
                return;
            }

            // In all other cases: Mismatch — full redraw
            FullyRedrawList(newLogEntries);
        }

        private List<LogEntryWithToStringOverride> GetLogEntryWithToStringOverridesList(LogContentProperty logContentProperty)
        {
            List<LogEntryWithToStringOverride> logEntryWithToStringOverrides = [];

            // Get the search filter text
            string filter = txtSearch.Text.Trim();
            if (filter == DefaulSearchtText) filter = null;

            // Iterate through the latest version of log entries
            foreach (LogEntry logEntry in LogEntriesLatestVersion)
            {
                // If the log entry has no content properties, continue to the next log entry
                if (logEntry.LogContentProperties == null) continue;

                // Try to get the content for the selected log content property
                logEntry.LogContentProperties.TryGetValue(logContentProperty, out string content);
                bool isError = false;

                // If the content is null, check for errors if the "Show Errors" checkbox is checked
                if (content == null)
                {
                    if (chkShowErrors.Checked) logEntry.LogContentProperties.TryGetValue(LogContentPropertyError, out content);
                    if (content == null) continue;
                    isError = true;
                }

                // If the content is not an error and a filter is applied, check if the content contains the filter text
                if (!isError && !string.IsNullOrEmpty(filter))
                {
                    if (!content.Contains(filter, StringComparison.InvariantCultureIgnoreCase)) continue;
                }

                // Create a new LogEntryWithToStringOverride object and add it to the list
                LogEntryWithToStringOverride logEntryWithToStringOverride = new()
                {
                    OriginalLogEntry = logEntry,
                    Content = isError ? content[0..8] + " ERROR" : content,
                    IsError = isError
                };
                logEntryWithToStringOverrides.Add(logEntryWithToStringOverride);
            }
            return logEntryWithToStringOverrides;
        }

        private void FullyRedrawList(List<LogEntryWithToStringOverride> newLogEntries)
        {
            // Store the currently selected log entry
            LogEntryWithToStringOverride selectedLogEntry = (LogEntryWithToStringOverride)LstLogContent.SelectedItem;
            // Store the current top index of the list
            int topIndex = LstLogContent.TopIndex;

            // Suspend drawing of the list to improve performance
            LstLogContent.SuspendDrawing();
            // Clear the list and add the new log entries
            LstLogContent.Items.Clear();
            LstLogContent.Items.AddRange([.. newLogEntries]);
            // Restore the top index of the list
            LstLogContent.TopIndex = topIndex > LstLogContent.Items.Count - 1 ? LstLogContent.Items.Count - 1 : topIndex;

            // If no log entry was previously selected, resume drawing and return
            if (selectedLogEntry == null)
            {
                LstLogContent.ResumeDrawing();
                return;
            }

            // Iterate through the new log entries and select the previously selected log entry if it exists
            foreach (LogEntryWithToStringOverride logEntriestringOverride in newLogEntries)
            {
                if (logEntriestringOverride.OriginalLogEntry == selectedLogEntry.OriginalLogEntry)
                {
                    try
                    {
                        ignoreSelectedItemChanged = true;
                        LstLogContent.SelectedItem = logEntriestringOverride;
                    }
                    finally
                    {
                        ignoreSelectedItemChanged = false;
                    }
                    break;
                }
            }
            // Resume drawing of the list
            LstLogContent.ResumeDrawing();
        }

        private void CboLogContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLogContentList();
            OnFilterChanged(EventArgs.Empty);
        }

        private class LogEntryWithToStringOverride
        {
            public LogEntry OriginalLogEntry { get; set; }
            public string Content { get; set; }
            public bool IsError { get; set; }
            public override string ToString()
            { return Content; }
        }

        public LogEntry SelectedLogEntry
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogEntryWithToStringOverride)LstLogContent.SelectedItem).OriginalLogEntry;
            }
        }

        public string SelectedItem
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogEntryWithToStringOverride)LstLogContent.SelectedItem).Content;
            }
        }
        public int ExtraLogEntryCount
        {
            get
            {
                if (!ChkShowExtraLogEntries.Checked) return 0;

                if (int.TryParse(TxtExtraLogEntries.Text, out int extraLogEntryCount)) return extraLogEntryCount;

                return 0;
            }
        }

        public bool FilterIsEnabled { get { return LstLogContent.SelectedIndex != -1; } }

        // Method to raise the custom FilterChanged event.
        protected virtual void OnFilterChanged(EventArgs e)
        {
            FilterChanged?.Invoke(this, e);
        }

        private bool ignoreSelectedItemChanged = false;

        private void LstLogContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreSelectedItemChanged == false) OnFilterChanged(EventArgs.Empty);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (LstLogContent.Items.Count > 0) LstLogContent.SelectedIndex = -1;
        }

        private void ChkShowExtraLogEntries_CheckedChanged(object sender, EventArgs e)
        {
            TxtExtraLogEntries.Visible = ChkShowExtraLogEntries.Checked;
            OnFilterChanged(EventArgs.Empty);
        }

        private void TxtExtraLogEntries_TextChanged(object sender, EventArgs e)
        {
            OnFilterChanged(EventArgs.Empty);
        }

        private void ChkShowErrors_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLogContentList();
        }

        private void LstLogContent_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                // Clear the item's background
                e.DrawBackground();
                // Get the item from the ListBox
                LogEntryWithToStringOverride item = LstLogContent.Items[e.Index] as LogEntryWithToStringOverride;

                // If the item is selected, draw a focus rectangle around it
                if (isSelected)
                {
                    // Set the text color to dark red for items with IsError = true
                    e.Graphics.FillRectangle(SelectedItemBackColor, e.Bounds);
                }
                // Truncate the text to fit within the width of the ListBox
                string truncatedText = TruncateTextToFit(item.ToString(), e.Graphics, e.Bounds.Width);

                // Check if the item is not null and IsError is true
                if (item != null && item.IsError)
                {
                    // Use the default text color for other items
                    e.Graphics.DrawString(truncatedText, LstLogContent.Font, Brushes.DarkRed, e.Bounds);
                }
                else
                {

                    // Use the default text color for other items
                    e.Graphics.DrawString(truncatedText, LstLogContent.Font, SystemBrushes.ControlText, e.Bounds);
                }
            }
        }
        private string TruncateTextToFit(string text, Graphics graphics, int maxWidth)
        {
            string truncatedText = text;
            int ellipsisWidth = TextRenderer.MeasureText(graphics, ".....", LstLogContent.Font).Width;

            while (TextRenderer.MeasureText(graphics, truncatedText, LstLogContent.Font).Width + ellipsisWidth > maxWidth)
            {
                truncatedText = truncatedText[..^1];
            }

            if (truncatedText.Length < text.Length)
            {
                truncatedText += "...";
            }

            return truncatedText;
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == DefaulSearchtText)
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = SystemColors.ControlText;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = DefaulSearchtText;
                txtSearch.ForeColor = Color.DarkGray;
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private string lastSearch = "";
        private void PerformSearch()
        {
            string searchString = txtSearch.Text.Trim();
            if (searchString == DefaulSearchtText) return;
            if (searchString != lastSearch)
            {
                UpdateLogContentList();
                lastSearch = searchString;
            }
        }
    }
}
