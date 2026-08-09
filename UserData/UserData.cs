using System.Collections.Generic;
using LogScraper.Configuration;

namespace LogScraper.UserData
{
    /// <summary>
    /// Represents the persisted per-user data for the application, stored in
    /// <c>LogScraperUser.json</c>. This is separate from <see cref="GenericConfig"/> and
    /// the other configuration files, since it tracks transient/user-specific data
    /// (such as recently written files) rather than application settings.
    /// </summary>
    internal class UserData
    {
        /// <summary>
        /// Gets or sets the list of files most recently saved via the "save log" action,
        /// most recent first.
        /// </summary>
        public List<RecentFileEntry> RecentSavedFiles { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of files most recently written to open in an external
        /// text editor, most recent first.
        /// </summary>
        public List<RecentFileEntry> RecentEditorFiles { get; set; } = [];
    }
}
