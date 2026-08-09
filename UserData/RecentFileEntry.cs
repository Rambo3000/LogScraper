using System;

namespace LogScraper.UserData
{
    /// <summary>
    /// Represents a single file that was written to disk on demand by the application
    /// (either saved by the user or opened in an external text editor), so it can be
    /// listed, reopened, or cleaned up later.
    /// </summary>
    public class RecentFileEntry : IEquatable<RecentFileEntry>
    {
        /// <summary>
        /// Gets or sets the full path of the file on disk.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the moment the file was written/last accessed by the application.
        /// </summary>
        public DateTime Timestamp { get; set; }

        public bool Equals(RecentFileEntry other)
        {
            if (other is null) return false;
            return string.Equals(FilePath, other.FilePath, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) => Equals(obj as RecentFileEntry);

        public override int GetHashCode() => FilePath?.ToUpperInvariant().GetHashCode() ?? 0;
    }
}
