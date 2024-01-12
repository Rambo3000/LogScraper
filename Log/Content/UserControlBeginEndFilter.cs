using LogScraper.Extensions;
using LogScraper.Log.Collection;
using LogScraper.Log.Content;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LogScraper
{
    internal partial class UserControlBeginEndFilter : UserControl
    {
        private const string DefaulSearchtText = "Filteren...";

        public event EventHandler FilterChanged;
        /// <summary>
        ///     Used for showing the error lines for non-error LogContentProperties
        /// </summary>
        private LogContentProperty LogContentPropertyError;

        public UserControlBeginEndFilter()
        {
            InitializeComponent();
            SelectedItemBackColor = Brushes.Orange;
            //Preset the default search text
            TxtSearch_Leave(null, null);
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

            List<LogLineWithToStringOverride> logLineWithToStringOverrides = [];

            string filter = txtSearch.Text.Trim();
            if (filter == DefaulSearchtText) filter = null;

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

                if (!isError && !string.IsNullOrEmpty(filter))
                {
                    if (!content.Contains(filter, StringComparison.InvariantCultureIgnoreCase)) continue;
                }

                LogLineWithToStringOverride logLineWithToStringOverride = new()
                {
                    OriginalLogLine = logLine,
                    Content = isError ? content[0..8] + " ERROR" : content,
                    IsError = isError
                };
                logLineWithToStringOverrides.Add(logLineWithToStringOverride);
            }

            LstLogContent.SuspendDrawing();
            LstLogContent.Items.Clear();
            LstLogContent.Items.AddRange(logLineWithToStringOverrides.ToArray());
            LstLogContent.TopIndex = topIndex > LstLogContent.Items.Count - 1 ? LstLogContent.Items.Count - 1 : topIndex;

            if (selectedLogLine == null)
            {
                LstLogContent.ResumeDrawing();
                return;
            }

            foreach (LogLineWithToStringOverride logLineStringOverride in logLineWithToStringOverrides)
            {
                if (logLineStringOverride.OriginalLogLine == selectedLogLine.OriginalLogLine)
                {
                    try
                    {
                        ignoreSelectedItemChanged = true;
                        LstLogContent.SelectedItem = logLineStringOverride;
                    }
                    finally
                    {
                        ignoreSelectedItemChanged = false;
                    }
                    break;
                }
            }
            LstLogContent.ResumeDrawing();
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
