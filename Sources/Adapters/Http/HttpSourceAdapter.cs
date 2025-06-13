using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LogScraper.SourceAdapters;
using LogScraper.Sources.Adapters.Http.Authenticators;

namespace LogScraper.Sources.Adapters.Http
{
    /// <summary>
    /// Represents an adapter for interacting with an HTTP-based source, providing functionality for authentication, log
    /// retrieval, and trail-based querying.
    /// </summary>
    /// <remarks>This class is designed to facilitate communication with HTTP endpoints that require
    /// authentication and support log retrieval based on trail types. It supports multiple authentication mechanisms,
    /// including form-based login with CSRF protection. The adapter maintains state related to authentication and trail
    /// querying, and provides methods for retrieving logs and handling HTTP responses.</remarks>
    /// <param name="apiUrl"></param>
    /// <param name="credentialManagerUri"></param>
    /// <param name="httpAuthenticationSettings"></param>
    /// <param name="timeoutSeconds"></param>
    /// <param name="trailType"></param>
    /// <param name="lastLogTrailTime"></param>
    internal class HttpSourceAdapter(string apiUrl, string credentialManagerUri, HttpAuthenticationSettings httpAuthenticationSettings, int timeoutSeconds, TrailType trailType, DateTime? lastLogTrailTime = null) : ISourceAdapter
    {
        private readonly string apiUrl = apiUrl;
        private readonly string credentialManagerUri = credentialManagerUri;
        private readonly int timeoutSeconds = timeoutSeconds;
        private readonly TrailType trailType = trailType;
        private DateTime? lastLogTrailTime = lastLogTrailTime;
        HttpClient client = null;

        public HttpAuthenticationData AuthenticationData { get; private set; }
        public HttpAuthenticationSettings httpAuthenticationSettings = httpAuthenticationSettings;

        public DateTime? GetLastTrailTime() => lastLogTrailTime;

        /// <summary>
        /// Attempts to initialize the HTTP client and authenticate with the specified settings.
        /// </summary>
        /// <remarks>This method performs up to three authentication attempts using the provided
        /// credentials and settings. If authentication fails, it prompts the user to update the credentials via a
        /// dialog. The method supports multiple authentication types, including form-based login with CSRF
        /// protection.</remarks>
        /// <param name="httpResponseMessage">When the method returns, contains the HTTP response message from the last authentication attempt,  or <see
        /// langword="null"/> if no response was received.</param>
        /// <param name="errorMessage">When the method returns, contains an error message describing the reason for failure,  or an empty string if
        /// the operation was successful.</param>
        /// <returns><see langword="true"/> if the client was successfully authenticated; otherwise, <see langword="false"/>.</returns>
        public bool TryInitiateClientAndAuthenticate(out HttpResponseMessage httpResponseMessage, out string errorMessage)
        {
            errorMessage = string.Empty;
            httpResponseMessage = null;

            string lastHttpStatusDescription = string.Empty;
            for (int attempt = 0; attempt < 3; attempt++)
            {
                AuthenticationData ??= HttpAuthenticationHelper.GetAuthenticationDataFromCredentialStore(credentialManagerUri);
                if (httpAuthenticationSettings != null && httpAuthenticationSettings.EnforcedAuthenticationType == HttpAuthenticationType.FormLoginWithCsrf)
                {
                    AuthenticationData.Type = httpAuthenticationSettings.EnforcedAuthenticationType;
                }

                IHttpClientAuthenticator clientAuthenticator = HttpAuthenticatorFactory.GetAuthenticator(AuthenticationData.Type);

                // Initialize the HTTP client if not already done
                client ??= clientAuthenticator.GetHttpClient(timeoutSeconds);

                bool isAuthenticated = clientAuthenticator.AuthenticateAsync(client, httpAuthenticationSettings, AuthenticationData).Result;

                // If we have header based authentication we have to check by means of calling the real endpoint whether the connection can be made
                if (AuthenticationData.Type != HttpAuthenticationType.FormLoginWithCsrf)
                {
                    // Reduce the log to download to nothing
                    DateTime? originalTrailTime = lastLogTrailTime;
                    lastLogTrailTime = DateTime.Now;
                    httpResponseMessage = client.GetAsync(apiUrl + GetTrailQuery()).Result;
                    // Restore the trailtime for getting log situations
                    lastLogTrailTime = originalTrailTime;

                    isAuthenticated = httpResponseMessage.IsSuccessStatusCode;
                }

                if (isAuthenticated)
                {
                    if (AuthenticationData.Type != HttpAuthenticationType.None)
                    {
                        HttpAuthenticationHelper.SaveAuthenticationDataToCredentialStore(AuthenticationData, credentialManagerUri);
                    }
                    return true;
                }

                if (httpResponseMessage != null) lastHttpStatusDescription = ConvertHttpStatusCodeToString(httpResponseMessage);
                FormHttpCredentials form = new()
                {
                    HttpAuthenticationData = AuthenticationData,
                    Url = apiUrl
                };

                form.ShowDialog();
                if (form.CustomDialogResult != System.Windows.Forms.DialogResult.OK) break;
                AuthenticationData = form.HttpAuthenticationData;
            }

            errorMessage = string.IsNullOrEmpty(lastHttpStatusDescription) ? "Failed to connect. No HttpResponseMessage" : lastHttpStatusDescription;
            return false;
        }

        /// <summary>
        /// Asynchronously retrieves the log content from a remote source.
        /// </summary>
        /// <remarks>This method sends a request to a remote service and returns the log content as a
        /// string. If the response status code is not <see cref="HttpStatusCode.OK"/>, an exception is
        /// thrown.</remarks>
        /// <returns>A <see cref="string"/> containing the log content retrieved from the remote source.</returns>
        /// <exception cref="Exception">Thrown if the response status code is not <see cref="HttpStatusCode.OK"/>. The exception message contains
        /// the string representation of the HTTP status code.</exception>
        public async Task<string> GetLogAsync()
        {
            HttpResponseMessage response = await GetLogWithHttpStatusAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(ConvertHttpStatusCodeToString(response));
            }
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Sends an HTTP GET request to retrieve log data from the configured API endpoint.
        /// </summary>
        /// <remarks>This method constructs the request URL using the configured <c>apiUrl</c> and the
        /// result of <c>GetTrailQuery()</c>. It requires the HTTP client to be initialized before invocation.</remarks>
        /// <returns>An <see cref="HttpResponseMessage"/> containing the HTTP response from the API.</returns>
        /// <exception cref="Exception">Thrown if the connection fails, with additional details about the error.</exception>
        public async Task<HttpResponseMessage> GetLogWithHttpStatusAsync()
        {
            try
            {
                if (client == null) throw new InvalidOperationException("HTTP client is not initialized.");

                return await client.GetAsync(apiUrl + GetTrailQuery());
            }
            catch (Exception e)
            {
                throw new Exception("Connection failed with error: " + e.Message);
            }
        }

        /// <summary>
        /// Retrieves the log content as a string.
        /// </summary>
        /// <remarks>This method sends a request to fetch the log and returns the content as a string. If
        /// the HTTP response status code is not <see cref="HttpStatusCode.OK"/>, an exception is thrown.</remarks>
        /// <returns>A string containing the log content retrieved from the server.</returns>
        /// <exception cref="Exception">Thrown if the HTTP response status code is not <see cref="HttpStatusCode.OK"/>. The exception message
        /// contains the string representation of the HTTP status code.</exception>
        public string GetLog()
        {
            HttpResponseMessage response = GetLogWithHttpStatus();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(ConvertHttpStatusCodeToString(response));
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Retrieves log data from the specified API endpoint and returns the HTTP response.
        /// </summary>
        /// <remarks>This method performs a synchronous HTTP GET request to the API endpoint constructed
        /// using the base URL and query parameters. Ensure that the HTTP client is properly initialized before calling
        /// this method.</remarks>
        /// <returns>An <see cref="HttpResponseMessage"/> containing the HTTP response from the API.</returns>
        /// <exception cref="Exception">Thrown if the connection fails, with the error message indicating the cause of the failure.</exception>
        private HttpResponseMessage GetLogWithHttpStatus()
        {
            try
            {
                if (client == null) throw new InvalidOperationException("HTTP client is not initialized.");

                return client.GetAsync(apiUrl + GetTrailQuery()).Result;
            }
            catch (Exception e)
            {
                throw new Exception("Connection failed with error: " + e.Message);
            }
        }
        /// <summary>
        /// Generates a query string for retrieving log trails based on the current trail type and elapsed time.
        /// </summary>
        /// <remarks>This method constructs a query string specific to the trail type. If the trail type
        /// is set to  <see cref="TrailType.Kubernetes"/>, the query includes the elapsed time since the last log trail
        /// retrieval. If the trail type is <see cref="TrailType.None"/>, an empty string is returned.</remarks>
        /// <returns>A query string for retrieving log trails. Returns an empty string if the trail type is <see
        /// cref="TrailType.None"/> or if no previous log trail retrieval time is available.</returns>
        private string GetTrailQuery()
        {
            if (trailType == TrailType.None) return string.Empty;

            DateTime now = DateTime.Now;
            int elapsedSeconds = lastLogTrailTime == null ? -1 : (int)(now - (DateTime)lastLogTrailTime).TotalSeconds + 1;

            string query = string.Empty;
            if (trailType == TrailType.Kubernetes)
            {
                query = elapsedSeconds == -1 ? string.Empty : "?sinceSeconds=" + elapsedSeconds;
            }

            lastLogTrailTime = now;
            return query;
        }
        /// <summary>
        /// Converts the HTTP status code of the provided response into a descriptive string.
        /// </summary>
        /// <param name="response">The HTTP response message containing the status code to convert. Cannot be null.</param>
        /// <returns>A string describing the HTTP status code. For example, "Unauthorized: Check authentication." for  <see
        /// cref="HttpStatusCode.Unauthorized"/> or "Internal Server Error: The server encountered an error."  for <see
        /// cref="HttpStatusCode.InternalServerError"/>. For other status codes, the string includes the  status code
        /// and its reason phrase.</returns>
        public static string ConvertHttpStatusCodeToString(HttpResponseMessage response)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Unauthorized: Check authentication.",
                HttpStatusCode.InternalServerError => "Internal Server Error: The server encountered an error.",
                _ => $"Error: {response.StatusCode} - {response.ReasonPhrase}",
            };
        }
    }
}
