using LogScraper.Log.Metadata;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LogScraper
{
    public partial class UserControlLogMetadataFilter : UserControl
    {
        public bool Collapsed { get; set; }

        // Define a custom event that will be triggered when a checkbox item is changed.
        public event EventHandler FilterChanged;
        public event EventHandler CollapseChanged;
        private LogMetadataPropertyAndValues LogMetadataPropertyAndValues;

        public class MyData
        {
            public string Description { get; set; }
            public int Count { get; set; }
        }
        public UserControlLogMetadataFilter(string description)
        {
            InitializeComponent();
            Collapsed = false;

            LblLogFilterDescription.Text = description;

            // Add columns to the ListView
            LstFilterValues.Columns.Add("Description", 200); // Adjust the width as needed
            LstFilterValues.Columns.Add("Count", 100); // Adjust the width as needed

            // Set the ListView to automatically resize columns
            LstFilterValues.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            LstFilterValues.Columns[1].TextAlign = HorizontalAlignment.Right;
        }
        private bool UpdateListViewInProgres;
        public void UpdateListView(LogMetadataPropertyAndValues logMetadataPropertyAndValues)
        {
            UpdateListViewInProgres = true;

            // Create a dictionary to save the state of the checkboxes
            Dictionary<string, bool> checkboxStates = [];

            foreach (ListViewItem item in LstFilterValues.Items)
            {
                checkboxStates[item.Text] = item.Checked;
            }

            // Create a list to hold the new items
            List<ListViewItem> newItems = [];

            foreach (KeyValuePair<LogMetadataValue, string> kvp in logMetadataPropertyAndValues.LogMetadataValues)
            {
                LogMetadataValue logMetadataValue = kvp.Key;
                int count = logMetadataValue.Count; // Get the count from LogMetadataValues.Count
                string description = logMetadataValue.Value; // Get the description from LogMetadataValues.Description

                // Create a ListViewItem for the LogMetadataValues
                ListViewItem item = new(description)
                {
                    Tag = logMetadataValue,
                    ForeColor = count == 0 ? Color.Gray : Color.Black
                };

                // Add sub-items for Count and IsFilterEnabled
                item.SubItems.Add(FormatCount(count));

                // Restore the state of the checkbox
                if (checkboxStates.TryGetValue(logMetadataValue.ToString(), out var isChecked))
                {
                    item.Checked = isChecked;
                }
                else
                {
                    // If not found in the saved state, use the default value from LogMetadataValues
                    item.Checked = logMetadataValue.IsFilterEnabled;
                }

                // Add the item to the list of new items
                newItems.Add(item);
            }

            // Clear the ListView and add the new items
            LstFilterValues.BeginUpdate();
            LstFilterValues.Items.Clear();
            LstFilterValues.Items.AddRange(newItems.ToArray());
            LstFilterValues.EndUpdate();

            LogMetadataPropertyAndValues = logMetadataPropertyAndValues;
            UpdateListViewInProgres = false;

            ResizeVertically();
        }

        public void UpdateCountInListView(LogMetadataPropertyAndValues logMetadataPropertyAndValues)
        {
            foreach (ListViewItem item in LstFilterValues.Items)
            {
                LogMetadataValue logMetadataValue = (LogMetadataValue)item.Tag;
                if (logMetadataValue != null)
                {
                    foreach (var kvp in logMetadataPropertyAndValues.LogMetadataValues)
                    {
                        if (kvp.Key == logMetadataValue)
                        {
                            // Update the count and forecolor
                            logMetadataValue.Count = kvp.Key.Count;
                            item.SubItems[1].Text = FormatCount(logMetadataValue.Count);
                            item.ForeColor = logMetadataValue.Count == 0 ? Color.Gray : Color.Black;
                            break;
                        }
                    }
                }
            }
        }

        public LogMetadataPropertyAndValues GetCurrentLogMetadataPropertyAndValues()
        {
            foreach (ListViewItem item in LstFilterValues.Items)
            {
                ((LogMetadataValue)item.Tag).IsFilterEnabled = item.Checked;
            }

            return LogMetadataPropertyAndValues;
        }

        private static string FormatCount(int count)
        {
            if (count > 99999)
            {
                // If the count is greater than 99,999, format it as "100k" notation
                return string.Format("{0:n0}k", count / 1000);
            }
            else
            {
                // Format the count with a thousand separator
                return string.Format("{0:n0}", count);
            }
        }
        private void ResizeVertically()
        {
            int totalHeightListView = 0;

            if (!Collapsed)
            {
                foreach (ListViewItem item in LstFilterValues.Items)
                {
                    totalHeightListView += item.Bounds.Height;
                }
            }

            // Calculate the new height based on the ListView's height
            int newHeightUserControl = LstFilterValues.Top + totalHeightListView + Padding.Bottom;

            // Set the user control's new height
            if (Height != newHeightUserControl) Height = newHeightUserControl;

            // Set the ListView's height
            if (LstFilterValues.Height != totalHeightListView) LstFilterValues.Height = totalHeightListView;
        }

        private void LstFilterValues_Resize(object sender, EventArgs e)
        {
            if (LstFilterValues.Columns.Count == 0) return;

            const int maximumCountWidth = 50;
            LstFilterValues.Columns[0].Width = LstFilterValues.Width - maximumCountWidth;
            LstFilterValues.Columns[1].Width = maximumCountWidth;
        }

        private void LstFilterValues_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (LstFilterValues.FocusedItem != null) LstFilterValues.FocusedItem.Focused = false;
            if (!e.Item.Selected) return;

            e.Item.Checked = !e.Item.Checked;
        }

        private void BtnOpenOrCollapse_Click(object sender, EventArgs e)
        {
            Collapsed = !Collapsed;
            BtnOpenOrCollapse.Image = Collapsed ? Properties.Resources.chevron_up : Properties.Resources.chevron_down;
            ResizeVertically();
            OnCollapseChanged(EventArgs.Empty);
        }
        private void LstFilterValues_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

            if (LstFilterValues.FocusedItem != null) LstFilterValues.FocusedItem.Focused = false;
            if (e.Item.Selected) e.Item.Selected = false;

            if (LstFilterValues.Items.Count == 0) return;

            OnFilterChanged(EventArgs.Empty);
        }

        // Method to raise the custom FilterChanged event.
        protected virtual void OnFilterChanged(EventArgs e)
        {
            if (!UpdateListViewInProgres) FilterChanged?.Invoke(this, e);
        }

        // Method to raise the custom CollapseChanged event.
        protected virtual void OnCollapseChanged(EventArgs e)
        {
            CollapseChanged?.Invoke(this, e);
        }

        private void LstFilterValues_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListView listview = sender as ListView;
            ListViewItem item = listview.Items[e.Index];
            item.Selected = false;

            //Store the datetime if none is set and continue as normal
            if (string.IsNullOrEmpty(item.ToolTipText))
            {
                item.ToolTipText = DateTime.Now.ToString();
                return;
            } 
            if (Convert.ToDateTime(item.ToolTipText) > DateTime.Now.AddMilliseconds(-1000))
            {
                //Within a small amount of time the item is checked again. Ignore this.
                //When moving the mouse when clicking the object this is sometimes registered as dragging and the 
                // checkbox checking is undone. It seems to have something to do with the DragItem event.
                //Disable updating the value. Also the user cannot do this within this timespan
                e.NewValue = e.CurrentValue;
                return;
            }

            item.ToolTipText = DateTime.Now.ToString();
        }
    }
}
