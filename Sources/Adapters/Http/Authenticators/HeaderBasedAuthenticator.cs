using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters.Http.Authenticators
{
    public class HeaderBasedAuthenticator : IHttpClientAuthenticator 
    {
        public bool IsApplicable(HttpAuthenticationType type)
        {
            return type is HttpAuthenticationType.None
                or HttpAuthenticationType.BasicAuthentication
                or HttpAuthenticationType.BearerToken
                or HttpAuthenticationType.ApiKey;
        }

        public Task<bool> AuthenticateAsync(HttpClient client, HttpAuthenticationSettings httpAuthenticationSettings, HttpAuthenticationData data, string loginUrl)
        {
            switch (data.Type)
            {
                case HttpAuthenticationType.BasicAuthentication:
                    string basicAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{data.UserName}:{data.Password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
                    break;

                case HttpAuthenticationType.BearerToken:
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.BearerToken);
                    break;

                case HttpAuthenticationType.ApiKey:
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{data.Key}:{data.Secret}");
                    break;

                case HttpAuthenticationType.None:
                default:
                    // Do nothing
                    break;
            }

            return Task.FromResult(true);
        }

        public HttpClient GetHttpClient(int timeoutSeconds)
        {
            return new()
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }
    }
}
