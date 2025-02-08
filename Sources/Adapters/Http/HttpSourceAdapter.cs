using System;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LogScraper.Sources.Adapters;
using LogScraper.SourceAdapters;

namespace LogScraper.Sources.Adapters.Http
{
    internal class HttpSourceAdapter(string apiUrl, string credentialManagerUri, int timeoutSeconds) : ISourceAdapter
    {
        private readonly string apiUrl = apiUrl;
        private readonly string credentialManagerUri = credentialManagerUri;
        private readonly int timeoutSeconds = timeoutSeconds;
        public HttpAuthenticationData AuthenticationData { get; private set; }

        public HttpResponseMessage TestConnectionAndAskForAuthorisation()
        {
            HttpResponseMessage httpResponseMessage = GetLogWithHttpStatus();
            if (httpResponseMessage == null) return null;
            // Only in case of aunauthorized and forbidden ask for new credentials.
            // Not that the forbidden status sometimes occurs in cases of automatic single sign on
            if (httpResponseMessage.StatusCode != HttpStatusCode.Unauthorized && httpResponseMessage.StatusCode != HttpStatusCode.Forbidden) return httpResponseMessage;

            //We are now in the position where we are unauthorized, all other conditions are returned.
            FormHttpCredentials formHttpCredentials = new()
            {
                HttpAuthenticationData = AuthenticationData,
                Url = apiUrl
            };

            while (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized || httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                formHttpCredentials.ShowDialog();
                if (formHttpCredentials.CustomDialogResult != System.Windows.Forms.DialogResult.OK) break;

                //Get the new credential information
                AuthenticationData = formHttpCredentials.HttpAuthenticationData;

                httpResponseMessage = GetLogWithHttpStatus();
                if (httpResponseMessage == null) return null;
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                HttpAuthenticationHelper.SaveAuthenticationDataToCredentialStore(AuthenticationData, credentialManagerUri);
            }
            formHttpCredentials.Dispose();

            return httpResponseMessage;
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
                //If we do not have any credentials specified, check if there are any stored in the Credential Store
                AuthenticationData ??= HttpAuthenticationHelper.GetAuthenticationDataFromCredentialStore(credentialManagerUri);

                using var client = new HttpClient();

                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds); // Set a timeout for the entire request

                CreateAuthenticationHeader(client, AuthenticationData);

                return await client.GetAsync(apiUrl);
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
        public HttpResponseMessage GetLogWithHttpStatus()
        {
            try
            {
                //If we do not have any credentials specified, check if there are any stored in the Credential Store
                AuthenticationData ??= HttpAuthenticationHelper.GetAuthenticationDataFromCredentialStore(credentialManagerUri);

                using var client = new HttpClient();

                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds); // Set a timeout for the entire request

                CreateAuthenticationHeader(client, AuthenticationData);

                return client.GetAsync(apiUrl).Result;
            }
            catch (Exception e)
            {
                throw new Exception("Connection failed with error: " + e.Message);
            }
        }

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
                    // No additional headers for None AuthenticationData type
                    break;
            }
        }

        public static string ConvertHttpStatusCodeToString(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return "Unauthorized: Check authentication.";
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return "Internal Server Error: The server encountered an error.";
            }
            else
            {
                return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
            }
        }
    }
}
