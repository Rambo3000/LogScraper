using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using LogScraper.Controls.Generic;
using LogScraper.Log;
using LogScraper.Log.Filtering;
using LogScraper.Log.Generic.Extensions;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;
using LogScraper.UserData;

namespace LogScraper.Controls
{
    public partial class SaveLogControl : UserControl
    {
        public SaveLogControl()
        {
            InitializeComponent();
            ShortcutManager.Register(this, AppShortcut.SaveLog, () => BtnSave_Click(this, EventArgs.Empty));

            // Pre-populate the dropdown menu so it isn't empty before the first "Opening" event.
            // A ContextMenuStrip that has never had any items assigned before it is first shown
            // does not display correctly on that first show, requiring a second click.
            RecentFilesMenu_Opening(this, new System.ComponentModel.CancelEventArgs());
        }

        private void RecentFilesMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RecentFilesMenuHelper.BuildMenu(RecentFilesMenu, RecentFileListType.Saved);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            LogMetadataFilterResult metadataFilterResult = LogAppState.Instance.MetadataFilterResult.Value;
            if (metadataFilterResult == null || metadataFilterResult.LogEntries == null || metadataFilterResult.LogEntries.Count == 0) return;

            LogRenderSettings logRenderSettings = new()
            {
                LogLayout = LogAppState.Instance.Layout.Value,
                ShowOriginalMetadata = true
            };

            string renderedLog = LogRenderer.RenderLogEntriesAsString(LogAppState.Instance.FilterResultWithRange.Value, logRenderSettings);

            if (renderedLog != null)
            {
                List<LogEntry> logEntriesToRender = LogAppState.Instance.FilterResultWithRange.Value.LogEntries;
                using SaveFileDialog saveFileDialog = new()
                {
                    Filter = "Log files (*.log)|*.log|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    DefaultExt = "log",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    FileName = $"Log from {logEntriesToRender[0].TimeStamp:yyyyMMdd_HHmmss} to {logEntriesToRender[^1].TimeStamp:yyyyMMdd_HHmmss}.log",
                    AddExtension = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, renderedLog);
                        UserDataManager.AddRecentFile(RecentFileListType.Saved, saveFileDialog.FileName);

                        ProcessStartInfo processStartInfo = new()
                        {
                            FileName = "explorer.exe",
                            Arguments = $"/select,\"{saveFileDialog.FileName}\"",
                            UseShellExecute = true
                        };

                        Process.Start(processStartInfo);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving log: " + ex.Message, "Save failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ex.LogStackTraceToFile("Error saving log.");
                    }
                }
            }
        }
    }
}