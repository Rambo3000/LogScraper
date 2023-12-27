using LogScraper.Credentials;
using System;
using System.Net;

namespace LogScraper.SourceAdapters.Http
{
    internal class HttpAuthenticationHelper
    {
        private const string AuhtenticationTypeAddendumUri = "AuthenticationType";
        public static HttpAuthenticationData GetAuthenticationDataFromCredentialStore(string credentialManagerUri)
        {
            HttpAuthenticationData authenticationData = new()
            {
                Type = GetCredentialTypeFromCredentialStore(credentialManagerUri)
            };
            if (authenticationData.Type == HttpAuthenticationType.None) return authenticationData;

            NetworkCredential existingToken = CredentialManager.GetCredentials(credentialManagerUri);
            if (existingToken == null) return authenticationData;

            switch (authenticationData.Type)
            {
                case HttpAuthenticationType.ApiKey:
                    authenticationData.Key = existingToken.UserName;
                    authenticationData.Secret = existingToken.Password;
                    break;
                case HttpAuthenticationType.BearerToken:
                    authenticationData.BearerToken = existingToken.Password;
                    break;
                case HttpAuthenticationType.BasicAuthentication:
                    authenticationData.UserName = existingToken.UserName;
                    authenticationData.Password = existingToken.Password;
                    break;
                default:
                    break;
            }
            return authenticationData;
        }
        private static HttpAuthenticationType GetCredentialTypeFromCredentialStore(string credentialManagerUri)
        {
            NetworkCredential existingToken = CredentialManager.GetCredentials(credentialManagerUri + ":" + AuhtenticationTypeAddendumUri);
            if (existingToken == null) return HttpAuthenticationType.None;

            return ConvertHttpAuthenticationStringToType(existingToken.UserName);
        }
        public static void SaveAuthenticationDataToCredentialStore(HttpAuthenticationData authenticationData, string credentialManagerUri)
        {
            CredentialManager.SaveCredentials(credentialManagerUri + ":" + AuhtenticationTypeAddendumUri, ConvertHttpAuthenticationTypeToString(authenticationData.Type), "");

            switch (authenticationData.Type)
            {
                case HttpAuthenticationType.ApiKey:
                    CredentialManager.SaveCredentials(credentialManagerUri, authenticationData.Key, authenticationData.Secret);
                    break;
                case HttpAuthenticationType.BearerToken:
                    CredentialManager.SaveCredentials(credentialManagerUri, "", authenticationData.BearerToken);
                    break;
                case HttpAuthenticationType.BasicAuthentication:
                    CredentialManager.SaveCredentials(credentialManagerUri, authenticationData.UserName, authenticationData.Password);
                    break;
                default:
                    break;
            }
            return;
        }

        private static string ConvertHttpAuthenticationTypeToString(HttpAuthenticationType authenticationType)
        {
            return authenticationType switch
            {
                HttpAuthenticationType.ApiKey => "ApiKey",
                HttpAuthenticationType.BearerToken => "BearerToken",
                HttpAuthenticationType.BasicAuthentication => "Basic",
                _ => throw new Exception("Invalid conversion for authentication type"),
            };
        }
        private static HttpAuthenticationType ConvertHttpAuthenticationStringToType(string authenticationTypeString)
        {
            if (string.IsNullOrEmpty(authenticationTypeString)) throw new ArgumentOutOfRangeException(nameof(authenticationTypeString));

            if (authenticationTypeString == "ApiKey") return HttpAuthenticationType.ApiKey;
            if (authenticationTypeString == "BearerToken") return HttpAuthenticationType.BearerToken;
            if (authenticationTypeString == "Basic") return HttpAuthenticationType.BasicAuthentication;
            return HttpAuthenticationType.None;
        }
    }
}
