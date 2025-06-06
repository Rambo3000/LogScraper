using System;
using LogScraper.Sources.Adapters.File;
using LogScraper.Sources.Adapters.Http;

namespace LogScraper.Sources.Adapters
{
    /// <summary>
    /// A factory class for creating instances of source adapters.
    /// </summary>
    /// <remarks>
    /// This class provides static methods to create specific implementations of the <see cref="ISourceAdapter"/> interface,
    /// such as HTTP and file-based source adapters.
    /// </remarks>
    class SourceAdapterFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="HttpSourceAdapter"/> for retrieving logs from an HTTP source.
        /// </summary>
        /// <param name="apiUrl">The base URL of the HTTP API.</param>
        /// <param name="credentialManagerUri">The URI used to retrieve credentials from the credential manager.</param>
        /// <param name="timeoutSeconds">The timeout duration for HTTP requests, in seconds.</param>
        /// <param name="trailType">The type of trailing log query to use (e.g., Kubernetes).</param>
        /// <param name="lastTrailTime">The timestamp of the last log trail, if applicable.</param>
        /// <returns>An instance of <see cref="HttpSourceAdapter"/>.</returns>
        public static ISourceAdapter CreateHttpSourceAdapter(string apiUrl, string credentialManagerUri, int timeoutSeconds, HttpAuthenticationSettings httpAuthenticationSettings, TrailType trailType, DateTime? lastTrailTime = null, bool authenticate = true)
        {
            HttpSourceAdapter httpSourceAdapter = new(apiUrl, credentialManagerUri, httpAuthenticationSettings, timeoutSeconds, trailType, lastTrailTime);
            if (authenticate) httpSourceAdapter.InitiateClientAndAuthenticate();
            return httpSourceAdapter;
        }

        /// <summary>
        /// Creates an instance of <see cref="FileSourceAdapter"/> for retrieving logs from a file.
        /// </summary>
        /// <param name="filePath">The path to the log file.</param>
        /// <returns>An instance of <see cref="FileSourceAdapter"/>.</returns>
        public static ISourceAdapter CreateFileSourceAdapter(string filePath)
        {
            return new FileSourceAdapter(filePath);
        }
    }
}
