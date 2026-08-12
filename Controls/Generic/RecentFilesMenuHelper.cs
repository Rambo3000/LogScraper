using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogScraper.Log.Generic.Extensions;
using LogScraper.UserData;

namespace LogScraper.Controls.Generic
{
    /// <summary>
    /// Builds and maintains the dropdown context menu used by <see cref="SplitButton"/> controls
    /// that expose a "recent files" list (e.g. recently saved logs or recently opened editor
    /// exports). Each entry can be opened directly, revealed in Explorer, removed from the list
    /// only, or permanently deleted from disk (with confirmation).
    /// </summary>
    internal static class RecentFilesMenuHelper
    {
        /// <summary>
        /// Rebuilds the given <see cref="ContextMenuStrip"/> from the current recent-files list
        /// of the given <paramref name="listType"/>.
        /// </summary>
        public static void BuildMenu(ContextMenuStrip menu, RecentFileListType listType)
        {
            menu.Items.Clear();

            var entries = UserDataManager.GetRecentFiles(listType);
            if (entries.Count == 0)
            {
                menu.Items.Add(new ToolStripMenuItem("(No recent files)") { Enabled = false });
                return;
            }

            if (listType == RecentFileListType.Saved) menu.Items.Add(new ToolStripLabel("Recently saved files:"));
            if (listType == RecentFileListType.Editor) menu.Items.Add(new ToolStripLabel("Recent files viewed in editor:"));
            //menu.Items.Add(new ToolStripSeparator());

            foreach (RecentFileEntry entry in entries)
            {
                string filePath = entry.FilePath;

                ToolStripMenuItem fileItem = new($"{PathDisplayHelper.ShortenPath(filePath)}    ({entry.Timestamp:g})")
                {
                    ToolTipText = filePath
                };
                // Clicking the entry itself opens the file directly.
                fileItem.Click += (s, e) => OpenInEditor(filePath);

                ToolStripMenuItem openItem = new("Open in editor", null, (s, e) => OpenInEditor(filePath));
                ToolStripMenuItem explorerItem = new("Show in Explorer", null, (s, e) => ShowInExplorer(filePath));
                ToolStripMenuItem deleteFromListItem = new("Delete from list", null, (s, e) => UserDataManager.RemoveRecentFile(listType, filePath));
                ToolStripMenuItem removeItem = new("Remove file...", null, (s, e) => RemoveFile(listType, filePath));

                fileItem.DropDownItems.AddRange([openItem, explorerItem, new ToolStripSeparator(), deleteFromListItem, removeItem]);
                menu.Items.Add(fileItem);
            }

            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Clear list (keep files on disk)", null, (s, e) => ClearList(listType)));
            menu.Items.Add(new ToolStripMenuItem("Remove all listed files from disk...", null, (s, e) => RemoveAllFiles(listType, entries)));
        }

        /// <summary>
        /// Opens the file using the OS-registered default application for its file type.
        /// </summary>
        private static void OpenInEditor(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"The file no longer exists:\n{filePath}", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenLogInTextEditor.OpenFileInTextEditor(filePath);
        }

        /// <summary>
        /// Reveals the file in Windows Explorer, with the file pre-selected.
        /// </summary>
        private static void ShowInExplorer(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"The file no longer exists:\n{filePath}", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ProcessStartInfo processStartInfo = new()
                {
                    FileName = "explorer.exe",
                    Arguments = $"/select,\"{filePath}\"",
                    UseShellExecute = true
                };
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing file in Explorer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.LogStackTraceToFile("Error showing recent file in Explorer: " + filePath);
            }
        }

        /// <summary>
        /// Permanently deletes the file from disk (after confirmation) and removes it from the recent-files list.
        /// </summary>
        private static void RemoveFile(RecentFileListType listType, string filePath)
        {
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to permanently delete this file?\n{filePath}",
                "Remove file",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.LogStackTraceToFile("Error deleting recent file: " + filePath);
                return;
            }

            UserDataManager.RemoveRecentFile(listType, filePath);
        }

        /// <summary>
        /// Clears the recent-files list without touching any of the files on disk.
        /// </summary>
        private static void ClearList(RecentFileListType listType)
        {
            UserDataManager.ClearRecentFiles(listType);
        }

        /// <summary>
        /// Permanently deletes every file currently in the recent-files list (after confirmation,
        /// which lists all the affected file paths), then clears the list.
        /// </summary>
        private static void RemoveAllFiles(RecentFileListType listType, IReadOnlyList<RecentFileEntry> entries)
        {
            if (entries.Count == 0) return;

            StringBuilder message = new();
            message.AppendLine("Are you sure you want to permanently delete all of the following files?");
            message.AppendLine();
            foreach (RecentFileEntry entry in entries)
            {
                message.AppendLine(entry.FilePath);
            }

            DialogResult result = MessageBox.Show(
                message.ToString(),
                "Remove all files",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            List<string> failedPaths = [];
            foreach (RecentFileEntry entry in entries)
            {
                try
                {
                    if (File.Exists(entry.FilePath))
                    {
                        File.Delete(entry.FilePath);
                    }
                }
                catch (Exception ex)
                {
                    failedPaths.Add(entry.FilePath);
                    ex.LogStackTraceToFile("Error deleting recent file: " + entry.FilePath);
                }
            }

            UserDataManager.ClearRecentFiles(listType);

            if (failedPaths.Count > 0)
            {
                MessageBox.Show(
                    "The following files could not be deleted:\n" + string.Join("\n", failedPaths),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
