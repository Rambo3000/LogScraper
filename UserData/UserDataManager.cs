using System;
using System.Collections.Generic;
using System.IO;
using LogScraper.Configuration;
using LogScraper.Log.LogAppState;
using LogScraper.Utilities.Extensions;

namespace LogScraper.UserData
{
    /// <summary>
    /// Identifies which recent-files list a <see cref="RecentFileEntry"/> belongs to.
    /// </summary>
    internal enum RecentFileListType
    {
        /// <summary>Files written via the "save log" action.</summary>
        Saved,

        /// <summary>Files written to be opened in an external text editor.</summary>
        Editor
    }

    /// <summary>
    /// Central manager for user-specific data (as opposed to application configuration),
    /// persisted to <c>LogScraperUser.json</c>. Currently tracks the most recently
    /// saved/exported log files so the UI can offer to reopen, reveal, or remove them.
    /// </summary>
    internal static class UserDataManager
    {
        private const string UserDataFileName = "LogScraperUser.json";
        private const int MaxRecentFiles = 15;

        /// <summary>
        /// Observable state holding the current <see cref="UserData"/>. Controls can
        /// subscribe to <see cref="StateSlice{T}.Changed"/> to refresh their UI whenever
        /// the recent-files lists change.
        /// </summary>
        public static StateSlice<UserData> State { get; } = new();

        /// <summary>
        /// Loads user data from disk (if present) and initializes <see cref="State"/>.
        /// Must be called once at application startup.
        /// </summary>
        public static void Initialize()
        {
            UserData userData;
            try
            {
                userData = File.Exists(UserDataFileName)
                    ? ConfigurationManager.LoadFromFile<UserData>(UserDataFileName)
                    : new UserData();
            }
            catch (Exception ex)
            {
                ex.LogStackTraceToFile("Error loading user data from file: " + UserDataFileName);
                userData = new UserData();
            }

            userData ??= new UserData();
            userData.RecentSavedFiles ??= [];
            userData.RecentEditorFiles ??= [];

            State.ForceSet(userData);
        }

        /// <summary>
        /// Adds (or refreshes) an entry at the top of the given recent-files list, persists
        /// the change to disk, and notifies subscribers. The list is capped to the most
        /// recent <see cref="MaxRecentFiles"/> entries.
        /// </summary>
        /// <param name="listType">Which recent-files list to update.</param>
        /// <param name="filePath">The full path of the file that was written.</param>
        public static void AddRecentFile(RecentFileListType listType, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;

            UserData userData = State.Value ?? new UserData();
            List<RecentFileEntry> list = GetList(userData, listType);

            list.RemoveAll(entry => string.Equals(entry.FilePath, filePath, StringComparison.OrdinalIgnoreCase));
            list.Insert(0, new RecentFileEntry { FilePath = filePath, Timestamp = DateTime.Now });

            if (list.Count > MaxRecentFiles)
            {
                list.RemoveRange(MaxRecentFiles, list.Count - MaxRecentFiles);
            }

            Save(userData);
        }

        /// <summary>
        /// Removes an entry from the given recent-files list (without touching the file on disk),
        /// persists the change, and notifies subscribers.
        /// </summary>
        public static void RemoveRecentFile(RecentFileListType listType, string filePath)
        {
            UserData userData = State.Value ?? new UserData();
            List<RecentFileEntry> list = GetList(userData, listType);

            int removed = list.RemoveAll(entry => string.Equals(entry.FilePath, filePath, StringComparison.OrdinalIgnoreCase));
            if (removed == 0) return;

            Save(userData);
        }

        /// <summary>
        /// Clears the given recent-files list (without touching any files on disk),
        /// persists the change, and notifies subscribers.
        /// </summary>
        public static void ClearRecentFiles(RecentFileListType listType)
        {
            UserData userData = State.Value ?? new UserData();
            List<RecentFileEntry> list = GetList(userData, listType);

            if (list.Count == 0) return;

            list.Clear();
            Save(userData);
        }

        /// <summary>
        /// Returns a snapshot of the requested recent-files list, most recent first.
        /// </summary>
        public static IReadOnlyList<RecentFileEntry> GetRecentFiles(RecentFileListType listType)
        {
            UserData userData = State.Value;
            if (userData == null) return [];

            return [.. GetList(userData, listType)];
        }

        /// <summary>
        /// Gets whether the "open in text editor" action should silently overwrite the
        /// previously selected file instead of showing the save dialog again.
        /// </summary>
        public static bool GetOverwritePreviousEditorFileWithoutPrompt()
        {
            return State.Value?.OverwritePreviousEditorFileWithoutPrompt ?? false;
        }

        /// <summary>
        /// Sets whether the "open in text editor" action should silently overwrite the
        /// previously selected file instead of showing the save dialog again, persists the
        /// change, and notifies subscribers.
        /// </summary>
        public static void SetOverwritePreviousEditorFileWithoutPrompt(bool value)
        {
            UserData userData = State.Value ?? new UserData();
            if (userData.OverwritePreviousEditorFileWithoutPrompt == value) return;

            userData.OverwritePreviousEditorFileWithoutPrompt = value;
            Save(userData);
        }

        private static List<RecentFileEntry> GetList(UserData userData, RecentFileListType listType)
        {
            return listType switch
            {
                RecentFileListType.Saved => userData.RecentSavedFiles ??= [],
                RecentFileListType.Editor => userData.RecentEditorFiles ??= [],
                _ => throw new ArgumentOutOfRangeException(nameof(listType), listType, null)
            };
        }

        private static void Save(UserData userData)
        {
            try
            {
                ConfigurationManager.SaveToFile(UserDataFileName, userData, false);
            }
            catch (Exception ex)
            {
                ex.LogStackTraceToFile("Error saving user data to file: " + UserDataFileName);
            }

            // Force-set so Changed always fires, even if callers mutated the same instance in place.
            State.ForceSet(userData);
        }
    }
}
