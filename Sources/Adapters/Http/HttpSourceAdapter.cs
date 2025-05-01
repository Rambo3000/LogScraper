using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LogScraper.SourceAdapters;

namespace LogScraper.Sources.Adapters.Http
{
    /// <summary>
    /// Provides an adapter for retrieving log data from an HTTP source.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="ISourceAdapter"/> interface to fetch logs from an HTTP API.
    /// It supports various authentication methods and handles trailing log queries.
    /// </remarks>
    internal class HttpSourceAdapter(string apiUrl, string credentialManagerUri, int timeoutSeconds, TrailType trailType, DateTime? lastLogTrailTime = null) : ISourceAdapter
    {
        // The base URL of the HTTP API.
        private readonly string apiUrl = apiUrl;

        // The URI used to retrieve credentials from the credential manager.
        private readonly string credentialManagerUri = credentialManagerUri;

        // The timeout duration for HTTP requests, in seconds.
        private readonly int timeoutSeconds = timeoutSeconds;

        /// <summary>
        /// Gets the authentication data used for HTTP requests.
        /// </summary>
        public HttpAuthenticationData AuthenticationData { get; private set; }

        // The type of trailing log query to use (e.g., Kubernetes).
        private readonly TrailType trailType = trailType;

        // The timestamp of the last log trail, used for trailing queries.
        private DateTime? lastLogTrailTime = lastLogTrailTime;

        /// <summary>
        /// Gets the timestamp of the last log trail.
        /// </summary>
        /// <returns>A nullable <see cref="DateTime"/> representing the last trail time.</returns>
        public DateTime? GetLastTrailTime()
        {
            return lastLogTrailTime;
        }

        /// <summary>
        /// Tests the connection to the HTTP source and prompts for authorization if needed.
        /// </summary>
        /// <returns>An <see cref="HttpResponseMessage"/> representing the response from the HTTP source.</returns>
        public HttpResponseMessage TestConnectionAndAskForAuthorisation()
        {
            HttpResponseMessage httpResponseMessage = GetLogWithHttpStatus();
            if (httpResponseMessage == null) return null;

            // Handle unauthorized or forbidden responses by prompting for new credentials.
            if (httpResponseMessage.StatusCode != HttpStatusCode.Unauthorized && httpResponseMessage.StatusCode != HttpStatusCode.Forbidden) return httpResponseMessage;

            FormHttpCredentials formHttpCredentials = new()
            {
                HttpAuthenticationData = AuthenticationData,
                Url = apiUrl
            };

            while (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized || httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                formHttpCredentials.ShowDialog();
                if (formHttpCredentials.CustomDialogResult != System.Windows.Forms.DialogResult.OK) break;

                // Update the authentication data with new credentials.
                AuthenticationData = formHttpCredentials.HttpAuthenticationData;

                httpResponseMessage = GetLogWithHttpStatus();
                if (httpResponseMessage == null) return null;
            }

            // Save the new credentials if the connection is successful.
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                HttpAuthenticationHelper.SaveAuthenticationDataToCredentialStore(AuthenticationData, credentialManagerUri);
            }
            formHttpCredentials.Dispose();

            return httpResponseMessage;
        }

        /// <summary>
        /// Asynchronously retrieves the log data from the HTTP source.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the log data as a string.</returns>
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
        /// Asynchronously retrieves the HTTP response from the log source.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the HTTP response.</returns>
        public async Task<HttpResponseMessage> GetLogWithHttpStatusAsync()
        {
            try
            {
                // Retrieve credentials from the credential store if not already set.
                AuthenticationData ??= HttpAuthenticationHelper.GetAuthenticationDataFromCredentialStore(credentialManagerUri);

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

                // Add authentication headers to the HTTP client.
                CreateAuthenticationHeader(client, AuthenticationData);

                return await client.GetAsync(apiUrl + GetTrailQuery());
            }
            catch (Exception e)
            {
                throw new Exception("Connection failed with error: " + e.Message);
            }
        }

        /// <summary>
        /// Constructs the query string for trailing log requests.
        /// </summary>
        /// <returns>A string representing the query parameters for the trailing log request.</returns>
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

            // Update the last trail time for the next request.
            lastLogTrailTime = now;

            return query;
        }

        /// <summary>
        /// Retrieves the log data from the HTTP source synchronously.
        /// </summary>
        /// <returns>The log data as a string.</returns>
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
        /// Retrieves the HTTP response from the log source synchronously.
        /// </summary>
        /// <returns>An <see cref="HttpResponseMessage"/> representing the HTTP response.</returns>
        public HttpResponseMessage GetLogWithHttpStatus()
        {
            try
            {
                // Retrieve credentials from the credential store if not already set.
                AuthenticationData ??= HttpAuthenticationHelper.GetAuthenticationDataFromCredentialStore(credentialManagerUri);

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

                // Add authentication headers to the HTTP client.
                CreateAuthenticationHeader(client, AuthenticationData);

                return client.GetAsync(apiUrl + GetTrailQuery()).Result;
            }
            catch (Exception e)
            {
                throw new Exception("Connection failed with error: " + e.Message);
            }
        }

        /// <summary>
        /// Adds the appropriate authentication headers to the HTTP client based on the authentication data.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> to add headers to.</param>
        /// <param name="authenticationData">The authentication data to use for creating headers.</param>
        private static void CreateAuthenticationHeader(HttpClient client, HttpAuthenticationData authenticationData)
        {
            switch (authenticationData.Type)
            {
                case HttpAuthenticationType.ApiKey:
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticationData.Key}:{authenticationData.Secret}");
                    break;
                case HttpAuthenticationType.BearerToken:
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticationData.BearerToken}");
                    break;
                case HttpAuthenticationType.BasicAuthentication:
                    var basicAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{authenticationData.UserName}:{authenticationData.Password}"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {basicAuth}");
                    break;
                case HttpAuthenticationType.None:
                default:
                    // No additional headers for None AuthenticationData type.
                    break;
            }
        }

        /// <summary>
        /// Converts an HTTP status code to a descriptive string.
        /// </summary>
        /// <param name="response">The HTTP response to convert.</param>
        /// <returns>A string describing the HTTP status code.</returns>
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
