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

        public HttpResponseMessage InitiateClientAndAuthenticate()
        {
            if ( string.IsNullOrEmpty(apiUrl) )
            {
                throw new ArgumentException("API URL cannot be null or empty.", nameof(apiUrl));
            }
            if ( string.IsNullOrEmpty(credentialManagerUri) )
            {
                throw new ArgumentException("Credential manager URI cannot be null or empty.", nameof(credentialManagerUri));
            }
            if (timeoutSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeoutSeconds), "Timeout must be greater than zero.");
            }
            
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

                bool isAuthenticated = clientAuthenticator.AuthenticateAsync(client, httpAuthenticationSettings, AuthenticationData, apiUrl).Result;
                HttpResponseMessage response = client.GetAsync(apiUrl + GetTrailQuery()).Result;

                if (isAuthenticated && response.IsSuccessStatusCode)
                {
                    HttpAuthenticationHelper.SaveAuthenticationDataToCredentialStore(AuthenticationData, credentialManagerUri);
                    return response;
                }

                FormHttpCredentials form = new()
                {
                    HttpAuthenticationData = AuthenticationData,
                    Url = apiUrl
                };

                form.ShowDialog();
                if (form.CustomDialogResult != System.Windows.Forms.DialogResult.OK) break;
                AuthenticationData = form.HttpAuthenticationData;
            }

            return null;
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
