using LogScraper.Log.Metadata;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using LogScraper.Extensions;

namespace LogScraper
{
    public partial class UserControlLogMetadataFilter : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Collapsed { get; set; }

        public event EventHandler FilterChanged;
        public event EventHandler CollapseChanged;
        private LogMetadataPropertyAndValues LogMetadataPropertyAndValues;

        public UserControlLogMetadataFilter(string description)
        {
            InitializeComponent();
            Collapsed = false;

            LblLogFilterDescription.Text = description;
        }

        private bool UpdateListViewInProgres;

        public void UpdateListView(LogMetadataPropertyAndValues logMetadataPropertyAndValuesNew)
        {

            if (LogMetadataPropertyAndValues != null)
            {
                bool keysAreEqual =
                    logMetadataPropertyAndValuesNew.LogMetadataValues.Count == LogMetadataPropertyAndValues.LogMetadataValues.Count &&
                    logMetadataPropertyAndValuesNew.LogMetadataValues.Keys.All(LogMetadataPropertyAndValues.LogMetadataValues.ContainsKey);

                if (keysAreEqual)
                {
                    UpdateCountInListView(logMetadataPropertyAndValuesNew);
                    return;
                }
            }

            UpdateListViewInProgres = true;

            Dictionary<string, bool> checkboxStates = [];
            foreach (UserControlLogMetadataFilterItem item in FlowLayoutPanelItems.Controls)
            {
                checkboxStates[item.Description] = item.IsChecked;
            }

            FlowLayoutPanelItems.SuspendDrawing();
            FlowLayoutPanelItems.Controls.Clear();

            // Sort by Value property
            List<LogMetadataValue> sortedValues = [.. logMetadataPropertyAndValuesNew.LogMetadataValues.Keys.OrderBy(lmv => lmv.Value)];

            foreach (LogMetadataValue logMetadataValue in sortedValues)
            {
                int count = logMetadataValue.Count;
                string description = logMetadataValue.Value;

                UserControlLogMetadataFilterItem item = new(description, count, logMetadataValue.IsFilterEnabled)
                {
                    Tag = logMetadataValue,
                    ForeColor = count == 0 ? Color.Gray : Color.Black,
                    Width = FlowLayoutPanelItems.Width - 5,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

                if (checkboxStates.TryGetValue(logMetadataValue.ToString(), out var isChecked))
                {
                    item.IsChecked = isChecked;
                }

                item.CheckedChanged += (sender, e) => OnFilterChanged(EventArgs.Empty);

                FlowLayoutPanelItems.Controls.Add(item);
            }

            LogMetadataPropertyAndValues = logMetadataPropertyAndValuesNew;
            UpdateListViewInProgres = false;

            ResizeVertically();

            FlowLayoutPanelItems.ResumeDrawing();
        }

        public void UpdateCountInListView(LogMetadataPropertyAndValues logMetadataPropertyAndValues)
        {
            foreach (UserControlLogMetadataFilterItem item in FlowLayoutPanelItems.Controls)
            {
                LogMetadataValue logMetadataValue = (LogMetadataValue)item.Tag;
                if (logMetadataValue != null)
                {
                    foreach (var kvp in logMetadataPropertyAndValues.LogMetadataValues)
                    {
                        if (kvp.Key == logMetadataValue)
                        {
                            logMetadataValue.Count = kvp.Key.Count;
                            item.Count = logMetadataValue.Count;
                            break;
                        }
                    }
                }
            }
        }

        public LogMetadataPropertyAndValues GetCurrentLogMetadataPropertyAndValues()
        {
            foreach (UserControlLogMetadataFilterItem item in FlowLayoutPanelItems.Controls)
            {
                ((LogMetadataValue)item.Tag).IsFilterEnabled = item.IsChecked;
            }

            return LogMetadataPropertyAndValues;
        }

        private void ResizeVertically()
        {
            int totalHeight = FlowLayoutPanelItems.Controls.Cast<Control>().Sum(c => c.Height);

            int newHeight = FlowLayoutPanelItems.Top + totalHeight + Padding.Bottom;

            if (Height != newHeight) Height = newHeight;
            if (FlowLayoutPanelItems.Height != totalHeight) FlowLayoutPanelItems.Height = totalHeight;
        }

        protected virtual void OnFilterChanged(EventArgs e)
        {
            if (!UpdateListViewInProgres) FilterChanged?.Invoke(this, e);
        }

        protected virtual void OnCollapseChanged(EventArgs e)
        {
            CollapseChanged?.Invoke(this, e);
        }

        private void FlowLayoutPanelItems_Resize(object sender, EventArgs e)
        {
            FlowLayoutPanelItems.SuspendDrawing();
            foreach (Control control in FlowLayoutPanelItems.Controls)
            {
                control.Width = FlowLayoutPanelItems.Width - 5;
            }
            FlowLayoutPanelItems.ResumeDrawing();
        }
    }
}
