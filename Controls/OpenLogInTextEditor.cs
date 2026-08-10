using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Controls.Generic;
using LogScraper.Export;
using LogScraper.Log.Filtering;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;
using LogScraper.UserData;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;

namespace LogScraper.Controls
{
    public partial class OpenLogInTextEditor : UserControl
    {
        /// <summary>
        /// Tracks the file path chosen via the save dialog during the current application
        /// session. Reset on application restart, so the save dialog is always shown at
        /// least once per session.
        /// </summary>
        private static string _sessionExportFilePath;

        /// <summary>
        /// Offers to remove any files still on disk from the recent "saved" and "editor" files
        /// lists, since they may contain sensitive log data that was written on demand.
        /// Called on application exit.
        /// </summary>
        public static void AskToRemoveTemporaryFile()
        {
            try
            {
                List<(RecentFileListType ListType, string FilePath)> existingFiles = [];

                foreach (RecentFileEntry entry in UserDataManager.GetRecentFiles(RecentFileListType.Editor))
                {
                    if (File.Exists(entry.FilePath))
                    {
                        existingFiles.Add((RecentFileListType.Editor, entry.FilePath));
                    }
                }

                if (existingFiles.Count == 0) return;

                StringBuilder message = new();
                message.AppendLine("The following log files used for viewing files in the external editor still exist:");
                message.AppendLine();
                foreach (var (_, filePath) in existingFiles)
                {
                    message.AppendLine(filePath);
                }
                message.AppendLine();
                message.AppendLine("Do you want to remove them now?");

                DialogResult result = MessageBox.Show(
                   message.ToString(),
                   "Remove exported log files",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                foreach (var (listType, filePath) in existingFiles)
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        ex.LogStackTraceToFile("Error while trying to remove exported log file: " + filePath);
                    }

                    UserDataManager.RemoveRecentFile(listType, filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while trying to remove exported log files: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.LogStackTraceToFile("Error while trying to remove exported log files on application close.");
            }
        }

        public OpenLogInTextEditor()
        {
            InitializeComponent();
            LogAppState.Instance.MetadataFilterResult.Changed += (s, e) => UpdateButtons();
            ShortcutManager.Register(this, AppShortcut.OpenLogInEditor, OpenFileInExternalEditor);

            // Pre-populate the dropdown menu so it isn't empty before the first "Opening" event.
            // A ContextMenuStrip that has never had any items assigned before it is first shown
            // does not display correctly on that first show, requiring a second click.
            RecentFilesMenu_Opening(this, new System.ComponentModel.CancelEventArgs());
        }

        private void OpenLogInTextEditor_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
        }

        private void BtnOpenWithEditor_Click(object sender, EventArgs e)
        {
            LogMetadataFilterResult filterResult = LogAppState.Instance.MetadataFilterResult.Value;
            if (filterResult?.LogEntries == null || filterResult.LogEntries.Count == 0) return;
            OpenFileInExternalEditor();
        }

        private void RecentFilesMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RecentFilesMenuHelper.BuildMenu(RecentFilesMenu, RecentFileListType.Editor);

            ToolStripMenuItem overwriteItem = new("Overwrite previously selected file without asking")
            {
                CheckOnClick = true,
                Checked = UserDataManager.GetOverwritePreviousEditorFileWithoutPrompt()
            };
            overwriteItem.Click += (s, args) => UserDataManager.SetOverwritePreviousEditorFileWithoutPrompt(overwriteItem.Checked);

            RecentFilesMenu.Items.Insert(0, overwriteItem);
            RecentFilesMenu.Items.Insert(1, new ToolStripSeparator());
        }

        /// <summary>
        /// Prompts the user for a destination file, writes the currently filtered log to it, and
        /// opens the resulting file in the configured external editor. The log is only ever written
        /// to disk on demand, when the user explicitly triggers this action.
        /// </summary>
        private async void OpenFileInExternalEditor()
        {
            LogMetadataFilterResult filterResult = LogAppState.Instance.MetadataFilterResult.Value;
            if (filterResult == null || filterResult.LogEntries == null || filterResult.LogEntries.Count == 0) return;

            LogRenderSettings logRenderSettings = new()
            {
                LogLayout = LogAppState.Instance.Layout.Value,
                ShowOriginalMetadata = true
            };

            string renderedLog = LogRenderer.RenderLogEntriesAsString(LogAppState.Instance.FilterResultWithRange.Value, logRenderSettings);
            if (string.IsNullOrEmpty(renderedLog)) return;

            string filePath;

            if (!string.IsNullOrWhiteSpace(_sessionExportFilePath) && UserDataManager.GetOverwritePreviousEditorFileWithoutPrompt())
            {
                filePath = _sessionExportFilePath;
            }
            else
            {
                string defaultExportFileName = !string.IsNullOrWhiteSpace(_sessionExportFilePath)
                    ? _sessionExportFilePath
                    : ConfigAppState.Instance.GenericConfig.Value.ExportFileName;

                using SaveFileDialog saveFileDialog = new()
                {
                    Title = "Select where to save the log to open it in the text editor",
                    Filter = "Log files (*.log)|*.log|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    DefaultExt = "log",
                    AddExtension = true
                };

                if (!string.IsNullOrWhiteSpace(defaultExportFileName))
                {
                    try
                    {
                        string directory = Path.GetDirectoryName(defaultExportFileName);
                        if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
                        {
                            saveFileDialog.InitialDirectory = directory;
                        }

                        string fileName = Path.GetFileName(defaultExportFileName);
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            saveFileDialog.FileName = fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        // The configured default path is invalid, fall back to the default behaviour.
                        ex.LogStackTraceToFile("Error setting default export path in save dialog.");
                    }
                }

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                filePath = saveFileDialog.FileName;
                _sessionExportFilePath = filePath;
            }

            btnOpenWithEditor.Enabled = false;
            try
            {
                bool success = await LogExportWorkerManager.WriteToFileAsync(renderedLog, filePath);
                if (!success)
                {
                    MessageBox.Show($"Failed to write the log to: {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UserDataManager.AddRecentFile(RecentFileListType.Editor, filePath);

                OpenFileInTextEditor(filePath);
            }
            finally
            {
                btnOpenWithEditor.Enabled = true;
                UpdateButtons();
            }
        }

        public static void OpenFileInTextEditor(string filePath)
        {
            try
            {
                // Open the file in the configured external editor.
                Process.Start(ConfigAppState.Instance.GenericConfig.Value.EditorFileName, "\"" + filePath + "\"");
            }
            catch (Exception ex)
            {
                // Show an error message if the file cannot be opened.
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.LogStackTraceToFile("Error when trying to open the exported log file in the external editor.");
            }
        }
    }
}

