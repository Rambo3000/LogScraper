using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LogScraper.SourceAdapters;
using LogScraper.Sources.Adapters.Http.Authenticators;

namespace LogScraper.Sources.Adapters.Http
{
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
                    HttpAuthenticationHelper.SaveAuthenticationDataToCredentialStore(AuthenticationData, credentialManagerUri);
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

        public async Task<string> GetLogAsync()
        {
            HttpResponseMessage response = await GetLogWithHttpStatusAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(ConvertHttpStatusCodeToString(response));
            }
            return await response.Content.ReadAsStringAsync();
        }

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

        public string GetLog()
        {
            HttpResponseMessage response = GetLogWithHttpStatus();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(ConvertHttpStatusCodeToString(response));
            }
            return response.Content.ReadAsStringAsync().Result;
        }

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
