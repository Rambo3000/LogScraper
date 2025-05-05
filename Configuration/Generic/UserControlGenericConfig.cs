using System;
using System.Windows.Forms;
using LogScraper.Configuration.Generic;
using LogScraper.LogProviders;

namespace LogScraper.Configuration
{
    public partial class UserControlGenericConfig : UserControl
    {
        public UserControlGenericConfig()
        {
            InitializeComponent();
            CboLogProviderType.DataSource = Enum.GetValues<LogProviderType>();
            CboAutomaticReadTime.DataSource = new int[] { 1, 2, 3, 4, 5 };
        }
        internal void SetGenericConfig(GenericConfig config)
        {
            CboLogProviderType.SelectedItem = config.LogProviderTypeDefault;
            CboAutomaticReadTime.SelectedItem = config.AutomaticReadTimeMinutes;
            ChkExportToFile.Checked = config.ExportToFile;
            ChkShowErrorsInBeginAndEndFilters.Checked = config.ShowErrorLinesInBeginAndEndFilters;
            TxtEditorLocation.Text = config.EditorFileName;
            TxtExportFileName.Text = config.ExportFileName;
            TxtTimeOut.Text = config.HttpCLientTimeOUtSeconds.ToString();
        }

        private void ChkExportToFile_CheckedChanged(object sender, EventArgs e)
        {
            GrpExportSettings.Enabled = ChkExportToFile.Checked;
        }

        internal bool TryGetGenericConfig(out GenericConfig config)
        {
            config = GetLogScraperConfig();
            if (config.HttpCLientTimeOUtSeconds == 0)
            {
                MessageBox.Show("De timeout voor downloaden moet groter dan nul zijn.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (config.ExportToFile)
            {
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
        internal GenericConfig GetLogScraperConfig()
        {
            GenericConfig config = new()
            {
                EditorFileName = TxtEditorLocation.Text,
                ExportFileName = TxtExportFileName.Text,
                ExportToFile = ChkExportToFile.Checked,
                ShowErrorLinesInBeginAndEndFilters = ChkShowErrorsInBeginAndEndFilters.Checked,
                AutomaticReadTimeMinutes = int.TryParse(CboAutomaticReadTime.SelectedItem.ToString(), out int automaticReadTime) ? automaticReadTime : 1,
                LogProviderTypeDefault = (LogProviderType)CboLogProviderType.SelectedItem,
                HttpCLientTimeOUtSeconds = int.TryParse(TxtTimeOut.Text, out int timeout) ? timeout : 0
            };

            return config;
        }
    }
}
