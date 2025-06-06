using System.Net.Http;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters.Http.Authenticators
{
    public interface IHttpClientAuthenticator 
    {
        HttpClient GetHttpClient(int timeoutSeconds);
        Task<bool> AuthenticateAsync(HttpClient client, HttpAuthenticationSettings httpAuthenticationSettings, HttpAuthenticationData data, string loginUrl);
        bool IsApplicable(HttpAuthenticationType type);
    }

}