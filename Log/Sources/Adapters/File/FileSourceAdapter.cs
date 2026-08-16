using System;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters.File
{
    /// <summary>
    /// Provides an adapter for reading log data from a file.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="ISourceAdapter"/> interface to retrieve log data
    /// from a specified file path, either synchronously or asynchronously.
    /// </remarks>
    class FileSourceAdapter(string filePath) : ISourceAdapter
    {
        // The file path from which the log data will be read.
        private readonly string filePath = filePath;

        /// <summary>
        /// Reads the entire log file content synchronously.
        /// </summary>
        /// <returns>The content of the log file as a string.</returns>
        public string GetLog()
        {
            return System.IO.File.ReadAllText(filePath);
        }

        /// <summary>
        /// Reads the entire log file content asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the log file content as a string.</returns>
        public async Task<string> GetLogAsync()
        {
            return await System.IO.File.ReadAllTextAsync(filePath);
        }

        /// <summary>
        /// Gets the timestamp of the last log trail.
        /// </summary>
        /// <remarks>
        /// This method currently returns <c>null</c>, indicating that the last trail time is not implemented.
        /// </remarks>
        /// <returns>A nullable <see cref="DateTime"/> representing the last trail time, or <c>null</c> if not available.</returns>
        public DateTime? GetLastTrailTime()
        {
            return null;
        }
    }
}
