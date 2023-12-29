using LogScraper.Log.Collection;
using LogScraper.Log.Content;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LogScraper
{
    internal partial class UserControlBeginEndFilter : UserControl
    {
        public event EventHandler FilterChanged;
        /// <summary>
        ///     Used for showing the error lines for non-error LogContentProperties
        /// </summary>
        private LogContentProperty LogContentPropertyError;

        public UserControlBeginEndFilter()
        {
            InitializeComponent();
            SelectedItemBackColor = Brushes.Yellow;
        }

        public Brush SelectedItemBackColor { get; set; }

        private List<LogLine> LogLinesLatestVersion;

        public void UpdateFilterTypes(List<LogContentProperty> logContentProperties)
        {
            CboLogContentType.Items.Clear();
            if (logContentProperties == null || logContentProperties.Count == 0) return;
            CboLogContentType.Items.AddRange(logContentProperties.ToArray());
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

        public void UpdateLogLines(List<LogLine> logLines)
        {
            LogLinesLatestVersion = logLines;
            UpdateLogContentList();
        }
        private void UpdateLogContentList()
        {
            LogLineWithToStringOverride selectedLogLine = (LogLineWithToStringOverride)LstLogContent.SelectedItem;

            if (LogLinesLatestVersion == null)
            {
                LstLogContent.Items.Clear();
                return;
            }

            LogContentProperty logContentProperty = (LogContentProperty)CboLogContentType.SelectedItem;

            if (logContentProperty == null) return;

            int topIndex = LstLogContent.TopIndex;

            List<LogLineWithToStringOverride> logLineWithToStringOverrides = new();

            foreach (LogLine logLine in LogLinesLatestVersion)
            {
                if (logLine.LogContentProperties == null) continue;
                logLine.LogContentProperties.TryGetValue(logContentProperty, out string content);
                bool isError = false;
                if (content == null)
                {
                    if (chkShowErrors.Checked) logLine.LogContentProperties.TryGetValue(LogContentPropertyError, out content);
                    if (content == null) continue;
                    isError = true;
                }
                LogLineWithToStringOverride logLineWithToStringOverride = new()
                {
                    OriginalLogLine = logLine,
                    Content = isError ? content[0..8] + " ERROR" : content,
                    IsError = isError
                };
                logLineWithToStringOverrides.Add(logLineWithToStringOverride);
            }

            LstLogContent.BeginUpdate();
            LstLogContent.Items.Clear();
            LstLogContent.Items.AddRange(logLineWithToStringOverrides.ToArray());
            LstLogContent.TopIndex = topIndex > LstLogContent.Items.Count - 1 ? LstLogContent.Items.Count - 1 : topIndex;
            LstLogContent.EndUpdate();

            if (selectedLogLine == null) return;

            foreach (LogLineWithToStringOverride logLineStringOverride in logLineWithToStringOverrides)
            {
                if (logLineStringOverride.OriginalLogLine == selectedLogLine.OriginalLogLine)
                {
                    LstLogContent.SelectedItem = logLineStringOverride;
                    break;
                }
            }
        }

        private void CboLogContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLogContentList();
            OnFilterChanged(EventArgs.Empty);
        }

        private class LogLineWithToStringOverride
        {
            public LogLine OriginalLogLine { get; set; }
            public string Content { get; set; }
            public bool IsError { get; set; }
            public override string ToString()
            { return Content; }
        }

        public LogLine SelectedLogLine
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogLineWithToStringOverride)LstLogContent.SelectedItem).OriginalLogLine;
            }
        }

        public string SelectedItem
        {
            get
            {
                if (LstLogContent.SelectedItem == null) return null;
                return ((LogLineWithToStringOverride)LstLogContent.SelectedItem).Content;
            }
        }
        public int ExtraLineCount
        {
            get
            {
                if (!ChkShowExtraLines.Checked) return 0;

                if (int.TryParse(TxtExtraLines.Text, out int extraLinesCount)) return extraLinesCount;

                return 0;
            }
        }
        // Method to raise the custom FilterChanged event.
        protected virtual void OnFilterChanged(EventArgs e)
        {
            FilterChanged?.Invoke(this, e);
        }

        private void LstLogContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnFilterChanged(EventArgs.Empty);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (LstLogContent.Items.Count > 0) LstLogContent.SelectedIndex = -1;
        }

        private void ChkShowExtraLines_CheckedChanged(object sender, EventArgs e)
        {
            TxtExtraLines.Visible = ChkShowExtraLines.Checked;
            OnFilterChanged(EventArgs.Empty);
        }

        private void TxtExtraLines_TextChanged(object sender, EventArgs e)
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
                LogLineWithToStringOverride item = LstLogContent.Items[e.Index] as LogLineWithToStringOverride;

                // If the item is selected, draw a focus rectangle around it
                if (isSelected)
                {
                    // Set the text color to dark red for items with IsError = true
                    e.Graphics.FillRectangle(SelectedItemBackColor, e.Bounds);
                }
                // Check if the item is not null and IsError is true
                if (item != null && item.IsError)
                {
                    // Use the default text color for other items
                    e.Graphics.DrawString(item.ToString(), LstLogContent.Font, Brushes.DarkRed, e.Bounds);
                }
                else
                {
                    // Use the default text color for other items
                    e.Graphics.DrawString(item.ToString(), LstLogContent.Font, SystemBrushes.ControlText, e.Bounds);
                }
            }
        }
    }
}
