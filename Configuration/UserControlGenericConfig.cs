using System;
using System.Windows.Forms;
using LogScraper.LogProviders;

namespace LogScraper.Configuration
{
    public partial class UserControlGenericConfig : UserControl
    {
        public UserControlGenericConfig()
        {
            InitializeComponent();
            CboLogProviderType.DataSource = Enum.GetValues<LogProviderType>();
        }
        internal void SetGenericConfig(LogScraperConfig config)
        {
            CboLogProviderType.SelectedItem = config.LogProviderTypeDefault;
            ChkExportToFile.Checked = config.ExportToFile;
            TxtEditorLocation.Text = config.EditorFileName;
            TxtEditorDescription.Text = config.EditorName;
            TxtExportFileName.Text = config.ExportFileName;
            TxtTimeOut.Text = config.HttpCLientTimeOUtSeconds.ToString();
        }

        private void ChkExportToFile_CheckedChanged(object sender, EventArgs e)
        {
            GrpExportSettings.Enabled = ChkExportToFile.Checked;
        }

        internal bool TryGetGenericConfig(out LogScraperConfig config)
        {
            config = GetLogScraperConfig();
            if (config.HttpCLientTimeOUtSeconds == 0)
            {
                MessageBox.Show("De timeout voor downloaden moet groter dan nul zijn.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (config.ExportToFile)
            {
                if (string.IsNullOrWhiteSpace(config.EditorName))
                {
                    MessageBox.Show("De omschrijving van de editor is verplicht", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(config.EditorFileName))
                {
                    MessageBox.Show("De locatie van de editor is verplicht.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(config.ExportFileName))
                {
                    MessageBox.Show("De bestandsnaam is verplicht.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        internal LogScraperConfig GetLogScraperConfig()
        {
            LogScraperConfig config = new()
            {
                EditorFileName = TxtEditorLocation.Text,
                EditorName = TxtEditorDescription.Text,
                ExportFileName = TxtExportFileName.Text,
                ExportToFile = ChkExportToFile.Checked,
                LogProviderTypeDefault = (LogProviderType)CboLogProviderType.SelectedItem,
                HttpCLientTimeOUtSeconds = int.TryParse(TxtTimeOut.Text, out int timeout) ? timeout : 0
            };

            return config;
        }
    }
}
