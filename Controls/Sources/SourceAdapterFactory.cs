using System;
using System.Net.Http;
using LogScraper.Credentials;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Adapters.File;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Sources.Adapters.Http.Authentication;

namespace LogScraper.Controls.Sources
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
        /// <param name="credentialsPrompt">
        /// Used to ask the user for credentials when authentication fails. Defaults to a Windows Forms based
        /// prompt (<see cref="WinFormsCredentialsPrompt"/>) when not specified.
        /// </param>
        /// <param name="credentialStore">
        /// Used to persist and retrieve authentication credentials. Defaults to a Windows Credential Manager
        /// based store (<see cref="WindowsCredentialStore"/>) when not specified.
        /// </param>
        /// <returns>An instance of <see cref="HttpSourceAdapter"/>.</returns>
        public static ISourceAdapter CreateHttpSourceAdapter(string apiUrl, string credentialManagerUri, int timeoutSeconds, HttpAuthenticationSettings httpAuthenticationSettings, TrailType trailType, DateTime? lastTrailTime = null, bool authenticate = true, ICredentialsPrompt credentialsPrompt = null, ICredentialStore credentialStore = null)
        {
            credentialsPrompt ??= new WinFormsCredentialsPrompt();
            credentialStore ??= new WindowsCredentialStore();
            HttpSourceAdapter httpSourceAdapter = new(apiUrl, credentialManagerUri, httpAuthenticationSettings, timeoutSeconds, trailType, credentialsPrompt, credentialStore, lastTrailTime);
            if (authenticate) httpSourceAdapter.TryInitiateClientAndAuthenticate(out HttpResponseMessage _, out string _);
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
