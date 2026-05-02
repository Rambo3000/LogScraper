using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Metadata;

namespace LogScraper.Controls.Metadata
{
    public partial class LogMetadataRenderOptionsControl : UserControl
    {
        public LogMetadataRenderOptionsControl()
        {
            InitializeComponent();
            ItemShowOriginalMetadata.CheckedChanged += ItemShowOriginalMetadata_CheckedChanged;
            ItemHideAllMetadata.Click += ItemHideAllMetadata_CheckedChanged;
            UpdateButtons();
            LogAppState.Instance.Layout.Changed += (s, e) => UpdateLogMetadataProperties(LogAppState.Instance.Layout.Value?.LogMetadataProperties ?? []);
        }



        private void ItemShowOriginalMetadata_CheckedChanged(object sender, EventArgs e)
        {
            LogAppState.Instance.RenderOriginalMetadata.Set(ItemShowOriginalMetadata.Checked);
            UpdateButtons();
            UpdateLogAppStateMetadataProperties();
        }

        /// <summary>
        /// Use this flag to temporarily suppress events while we update the state of the checkable items.
        /// </summary>
        bool suppressItemHideAllMetadataCheckedChanged = false;
        private void ItemHideAllMetadata_CheckedChanged(object sender, EventArgs e)
        {
            suppressItemHideAllMetadataCheckedChanged = true;
            ToolStripItemCollection items = ContextMenuStrip1.Items;

            for (int index = 0; index < items.Count; index++)
            {
                if (items[index] is not ToolStripMenuItem menuItem) continue;

                menuItem.Checked = false;
            }

            suppressItemHideAllMetadataCheckedChanged = false;

            UpdateButtons();
            UpdateLogAppStateMetadataProperties();
        }

        private void ChkPanelControls_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLogAppStateMetadataProperties();
            UpdateButtons();
        }

        public void UpdateLogMetadataProperties(List<LogMetadataProperty> logMetadataProperties)
        {
            Clear();

            foreach (var logMetadataProperty in logMetadataProperties)
            {
                ToolStripMenuItem item = new()
                {
                    CheckOnClick = true,
                    Name = logMetadataProperty.Index.ToString(),
                    Tag = logMetadataProperty,
                    Size = new System.Drawing.Size(235, 22),
                    Text = logMetadataProperty.Description,
                    Checked = logMetadataProperty.IsDefaultVisibleInLog
                };
                item.CheckedChanged += ChkPanelControls_CheckedChanged;
                ContextMenuStrip1.Items.Add(item);
            }
            toolStripSeparator1.Visible = logMetadataProperties.Count > 0;
        }
        public bool ShowOriginalMetadata => ItemShowOriginalMetadata.Checked;
        public List<LogMetadataProperty> SelectedMetadataProperties
        {
            get
            {
                List<LogMetadataProperty> selectedMetadataProperties = [];

                if (ShowOriginalMetadata) return selectedMetadataProperties;

                ToolStripItemCollection items = ContextMenuStrip1.Items;

                for (int index = 3; index < items.Count; index++)
                {
                    if (items[index] is not ToolStripMenuItem menuItem || !menuItem.Checked) continue;

                    if (menuItem.Tag is not LogMetadataProperty metadataProperty) continue;

                    selectedMetadataProperties.Add(metadataProperty);
                }

                return selectedMetadataProperties;
            }
        }

        public bool IsOriginalMetadataShown { get { return ItemShowOriginalMetadata.Checked; } }

        public void Clear()
        {
            ContextMenuStrip1.Items.Clear();
            ContextMenuStrip1.Items.AddRange([ItemShowOriginalMetadata, ItemHideAllMetadata, toolStripSeparator1]);
        }

        private void UpdateLogAppStateMetadataProperties()
        {
            // When we update the state of all the checkable items in ItemHideAllMetadata_CheckedChanged do not trigger for every check change.
            if (suppressItemHideAllMetadataCheckedChanged) return;

            LogAppState.Instance.RenderSeperateMetadataProperties.Set(SelectedMetadataProperties);
        }

        private void SplitButton1_Click(object sender, EventArgs e)
        {
            ItemShowOriginalMetadata.Checked = !ItemShowOriginalMetadata.Checked;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool metadataItemIsChecked = false;
            for (int index = 3; index < ContextMenuStrip1.Items.Count; index++)
            {
                if (ContextMenuStrip1.Items[index] is ToolStripMenuItem menuItem)
                {
                    if (menuItem.Checked) metadataItemIsChecked = true;
                    menuItem.Enabled = !ItemShowOriginalMetadata.Checked;
                }
            }

            ItemHideAllMetadata.Enabled = ItemShowOriginalMetadata.Checked || metadataItemIsChecked;
            splitButton1.ImageIndex = ItemShowOriginalMetadata.Checked ? 1 : 0;
        }
    }
}
