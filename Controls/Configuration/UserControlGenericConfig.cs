using System;
using System.IO;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.Generic.Extensions;
using LogScraper.Log.LogProviders;

namespace LogScraper.Controls.Configuration
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
            ChkShowErrorsInBeginAndEndFilters.Checked = config.ShowErrorLinesInBeginAndEndFilters;
            ChkShowTimeLine.Checked = config.ShowTimelineByDefault;
            ChkPinLogProvidersByDefault.Checked = !config.PinLogProvidersByDefault;
            ChkIsDebugModeEnabled.Checked = config.IsDebugModeEnabled;
            ChkCheckForExternalViewingFilesOnExit.Checked = config.CheckForExternalViewingFilesOnExit;
            TxtEditorLocation.Text = config.EditorFileName;
            TxtExportFileName.Text = config.ExportFileName;
            TxtTimeOut.Text = config.HttpCLientTimeOUtSeconds.ToString();
            ChkIncludePrereleaseUpdates.Checked = config.IncludeBetaUpdates;
        }

        internal bool TryGetGenericConfig(out GenericConfig config)
        {
            config = GetLogScraperConfig();
            if (config.HttpCLientTimeOUtSeconds == 0)
            {
                MessageBox.Show("The download timeout must be greater than zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(config.EditorFileName))
            {
                MessageBox.Show("The editor location is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        internal GenericConfig GetLogScraperConfig()
        {
            GenericConfig config = new()
            {
                EditorFileName = TxtEditorLocation.Text,
                ExportFileName = TxtExportFileName.Text,
                PinLogProvidersByDefault = !ChkPinLogProvidersByDefault.Checked,
                ShowErrorLinesInBeginAndEndFilters = ChkShowErrorsInBeginAndEndFilters.Checked,
                ShowTimelineByDefault = ChkShowTimeLine.Checked,
                IsDebugModeEnabled = ChkIsDebugModeEnabled.Checked,
                AutomaticReadTimeMinutes = int.TryParse(CboAutomaticReadTime.SelectedItem.ToString(), out int automaticReadTime) ? automaticReadTime : 1,
                LogProviderTypeDefault = (LogProviderType)CboLogProviderType.SelectedItem,
                HttpCLientTimeOUtSeconds = int.TryParse(TxtTimeOut.Text, out int timeout) ? timeout : 0,
                IncludeBetaUpdates = ChkIncludePrereleaseUpdates.Checked,
                CheckForExternalViewingFilesOnExit = ChkCheckForExternalViewingFilesOnExit.Checked
            };

            return config;
        }

        private void BtnBrowseExportFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Title = "Select export file",
                Filter = "Log files (*.log, *.txt)|*.log;*.txt;|All files (*.*)|*.*",
                CheckFileExists = false,
                CheckPathExists = true
            };

            string currentPath = TxtExportFileName.Text;

            if (!string.IsNullOrWhiteSpace(currentPath))
            {
                try
                {
                    string directory = Path.GetDirectoryName(currentPath);
                    if (Directory.Exists(directory))
                    {
                        dialog.InitialDirectory = directory;
                    }
                }
                catch (Exception ex)
                {
                    // The current path is invalid, fall back to the default behaviour.
                    ex.LogStackTraceToFile("Error setting initial directory in export file dialog.");
                }
            }

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                TxtExportFileName.Text = dialog.FileName;
            }
        }

        private void BtnBrowseEditor_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Title = "Select a text editor",
                Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true
            };

            string currentPath = TxtEditorLocation.Text;

            if (!string.IsNullOrWhiteSpace(currentPath))
            {
                try
                {
                    string directory = Path.GetDirectoryName(currentPath);
                    if (Directory.Exists(directory))
                    {
                        dialog.InitialDirectory = directory;
                    }

                    string filename = Path.GetFileName(currentPath);
                    if (!string.IsNullOrWhiteSpace(filename))
                    {
                        dialog.FileName = filename;
                    }
                }
                catch (Exception ex)
                {
                    // invalid path, ignore
                    ex.LogStackTraceToFile("Error setting initial directory or file name in editor location dialog.");
                }
            }

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                TxtEditorLocation.Text = dialog.FileName;
            }
        }
    }
}
