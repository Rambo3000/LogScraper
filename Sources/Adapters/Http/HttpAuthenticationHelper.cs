using System;
using System.Net;
using LogScraper.Credentials;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LogScraper.Sources.Adapters.Http
{
    /// <summary>
    /// Provides helper methods for managing HTTP authentication data using the credential store.
    /// </summary>
    internal class HttpAuthenticationHelper
    {
        // Addendum used to store the authentication type in the credential store.
        private const string AuhtenticationTypeAddendumUri = "AuthenticationType";

        /// <summary>
        /// Retrieves authentication data from the credential store for the specified URI.
        /// </summary>
        /// <param name="credentialManagerUri">The URI used to identify the credentials in the credential store.</param>
        /// <returns>An <see cref="HttpAuthenticationData"/> object containing the retrieved authentication data.</returns>
        public static HttpAuthenticationData GetAuthenticationDataFromCredentialStore(string credentialManagerUri)
        {
            // Initialize the authentication data with the type retrieved from the credential store.
            HttpAuthenticationData authenticationData = new()
            {
                Type = GetCredentialTypeFromCredentialStore(credentialManagerUri)
            };

            // If no authentication type is specified, return the default authentication data.
            if (authenticationData.Type == HttpAuthenticationType.None) return authenticationData;

            // Retrieve the credentials from the credential store.
            NetworkCredential existingToken = CredentialManager.GetCredentials(credentialManagerUri);
            if (existingToken == null) return authenticationData;

            // Populate the authentication data based on the retrieved credentials and authentication type.
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
                case HttpAuthenticationType.FormLoginWithCsrf:
                    authenticationData.UserName = existingToken.UserName;
                    authenticationData.Password = existingToken.Password;
                    break;
                default:
                    break;
            }
            return authenticationData;
        }

        /// <summary>
        /// Retrieves the authentication type from the credential store for the specified URI.
        /// </summary>
        /// <param name="credentialManagerUri">The URI used to identify the credentials in the credential store.</param>
        /// <returns>The <see cref="HttpAuthenticationType"/> retrieved from the credential store.</returns>
        private static HttpAuthenticationType GetCredentialTypeFromCredentialStore(string credentialManagerUri)
        {
            // Retrieve the credentials for the authentication type addendum.
            NetworkCredential existingToken = CredentialManager.GetCredentials(credentialManagerUri + ":" + AuhtenticationTypeAddendumUri);
            if (existingToken == null) return HttpAuthenticationType.None;

            // Convert the retrieved string to an authentication type.
            return ConvertHttpAuthenticationStringToType(existingToken.UserName);
        }

        /// <summary>
        /// Saves the specified authentication data to the credential store for the given URI.
        /// </summary>
        /// <param name="authenticationData">The authentication data to save.</param>
        /// <param name="credentialManagerUri">The URI used to identify the credentials in the credential store.</param>
        public static void SaveAuthenticationDataToCredentialStore(HttpAuthenticationData authenticationData, string credentialManagerUri)
        {
            // Save the authentication type to the credential store.
            CredentialManager.SaveCredentials(credentialManagerUri + ":" + AuhtenticationTypeAddendumUri, ConvertHttpAuthenticationTypeToString(authenticationData.Type), string.Empty);

            // Save the authentication data based on the specified authentication type.
            switch (authenticationData.Type)
            {
                case HttpAuthenticationType.ApiKey:
                    CredentialManager.SaveCredentials(credentialManagerUri, authenticationData.Key, authenticationData.Secret);
                    break;
                case HttpAuthenticationType.BearerToken:
                    CredentialManager.SaveCredentials(credentialManagerUri, string.Empty, authenticationData.BearerToken);
                    break;
                case HttpAuthenticationType.BasicAuthentication:
                    CredentialManager.SaveCredentials(credentialManagerUri, authenticationData.UserName, authenticationData.Password);
                    break;
                case HttpAuthenticationType.FormLoginWithCsrf:
                    CredentialManager.SaveCredentials(credentialManagerUri, authenticationData.UserName, authenticationData.Password);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Converts an <see cref="HttpAuthenticationType"/> to its string representation.
        /// </summary>
        /// <param name="authenticationType">The authentication type to convert.</param>
        /// <returns>A string representation of the authentication type.</returns>
        /// <exception cref="Exception">Thrown if the authentication type is invalid.</exception>
        private static string ConvertHttpAuthenticationTypeToString(HttpAuthenticationType authenticationType)
        {
            return authenticationType switch
            {
                HttpAuthenticationType.ApiKey => "ApiKey",
                HttpAuthenticationType.BearerToken => "BearerToken",
                HttpAuthenticationType.BasicAuthentication => "Basic",
                HttpAuthenticationType.FormLoginWithCsrf => "FormLoginWithCsrf",
                _ => throw new Exception("Invalid conversion for authentication type"),
            };
        }

        /// <summary>
        /// Converts a string representation of an authentication type to an <see cref="HttpAuthenticationType"/>.
        /// </summary>
        /// <param name="authenticationTypeString">The string representation of the authentication type.</param>
        /// <returns>The corresponding <see cref="HttpAuthenticationType"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the string is null or empty.</exception>
        private static HttpAuthenticationType ConvertHttpAuthenticationStringToType(string authenticationTypeString)
        {
            if (string.IsNullOrEmpty(authenticationTypeString)) throw new ArgumentOutOfRangeException(nameof(authenticationTypeString));

            if (authenticationTypeString == "ApiKey") return HttpAuthenticationType.ApiKey;
            if (authenticationTypeString == "BearerToken") return HttpAuthenticationType.BearerToken;
            if (authenticationTypeString == "Basic") return HttpAuthenticationType.BasicAuthentication;
            if (authenticationTypeString == "FormLoginWithCsrf") return HttpAuthenticationType.FormLoginWithCsrf;
            return HttpAuthenticationType.None;
        }
    }
}
