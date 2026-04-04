using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LogScraper.Log.Metadata
{
    public partial class UserControlMetadataFormatting : UserControl
    {
        public event EventHandler SelectionChanged;

        public UserControlMetadataFormatting()
        {
            InitializeComponent();
            ItemShowOriginalMetadata.CheckedChanged += ItemShowOriginalMetadata_CheckedChanged;
            ItemHideAllMetadata.Click += ItemHideAllMetadata_CheckedChanged;
            UpdateButtons();
        }

        private void ItemShowOriginalMetadata_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtons();
            OnSelectionChanged(e);
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
            OnSelectionChanged(e);
        }

        private void ChkPanelControls_CheckedChanged(object sender, EventArgs e)
        {
            OnSelectionChanged(e);
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

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            if (suppressItemHideAllMetadataCheckedChanged) return;

            SelectionChanged?.Invoke(this, e);
        }

        private void SplitButton1_Click(object sender, EventArgs e)
        {
            ItemShowOriginalMetadata.Checked = !ItemShowOriginalMetadata.Checked;
            UpdateButtons();
            OnSelectionChanged(e);
        }

        private void UpdateButtons()
        {
            bool metadataItemIsChecked = false;
            for (int index = 3; index < ContextMenuStrip1.Items.Count; index++)
            {
                if (ContextMenuStrip1.Items[index] is ToolStripMenuItem menuItem)
                {
                    menuItem.Enabled = !ItemShowOriginalMetadata.Checked;
                    if (menuItem.Checked) metadataItemIsChecked = true;
                }
            }

            ItemHideAllMetadata.Enabled = ItemShowOriginalMetadata.Checked || metadataItemIsChecked;
        }
    }
}
