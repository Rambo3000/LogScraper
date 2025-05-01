using System;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters
{
    /// <summary>
    /// Defines a contract for source adapters that retrieve log data from various sources.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface provide methods to fetch log data synchronously or asynchronously,
    /// and optionally track the timestamp of the last log trail.
    /// </remarks>
    public interface ISourceAdapter
    {
        /// <summary>
        /// Retrieves the log data synchronously.
        /// </summary>
        /// <returns>The log data as a string.</returns>
        string GetLog();

        /// <summary>
        /// Retrieves the log data asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the log data as a string.</returns>
        Task<string> GetLogAsync();

        /// <summary>
        /// Returns a string representation of the source adapter.
        /// </summary>
        /// <returns>A string describing the source adapter.</returns>
        string ToString();

        /// <summary>
        /// Gets the timestamp of the last log trail, if applicable.
        /// </summary>
        /// <returns>A nullable <see cref="DateTime"/> representing the last trail time, or <c>null</c> if not available.</returns>
        DateTime? GetLastTrailTime();
    }
}
