using LogScraper.Export;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LogScraper.Log.Metadata
{
    public partial class UserControlMetadataFormating : UserControl
    {
        public event EventHandler SelectionChanged;

        public UserControlMetadataFormating()
        {
            InitializeComponent();
        }

        private void ChkShowOriginalMetdata_CheckedChanged(object sender, EventArgs e)
        {
            pnlCheckBoxes.Enabled = !chkShowOriginalMetdata.Checked;
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
                CheckBox checkBox = new()
                {
                    Text = logMetadataProperty.Description,
                    Tag = logMetadataProperty,
                    Padding = new() { All = 0 },
                    Margin = new() { Top = 0, Bottom = 0, Left = 5, Right = 0 }

                };
                checkBox.CheckedChanged += ChkPanelControls_CheckedChanged;
                pnlCheckBoxes.Controls.Add(checkBox);
            }
            grpAddMetadata.Visible = logMetadataProperties.Count > 0;
        }
        public LogExportSettingsMetadata LogExportSettingsMetadata
        {
            get
            {
                LogExportSettingsMetadata LogExportSettingsMetadata = new()
                {
                    SelectedMetadataProperties = [],
                    ShowOriginalMetadata = chkShowOriginalMetdata.Checked
                };
                if (LogExportSettingsMetadata.ShowOriginalMetadata)
                {
                    return LogExportSettingsMetadata;
                }

                foreach (Control control in pnlCheckBoxes.Controls)
                {
                    if (!((CheckBox)control).Checked) continue;

                    LogExportSettingsMetadata.SelectedMetadataProperties.Add((LogMetadataProperty)control.Tag);
                }
                return LogExportSettingsMetadata;
            }
        }
        public void Clear()
        {
            pnlCheckBoxes.Controls.Clear();
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }
    }
}
