using System;
using System.Collections.Generic;
using System.Linq;

namespace LogScraper.Sources.Adapters.Http.Authenticators
{
    /// <summary>
    /// Provides a factory for creating HTTP client authenticators based on the specified authentication type.
    /// </summary>
    /// <remarks>This class supports multiple authentication mechanisms and selects an appropriate
    /// authenticator based on the provided <see cref="HttpAuthenticationType"/>. If no suitable authenticator is found,
    /// an exception is thrown.</remarks>
    public static class HttpAuthenticatorFactory
    {
        /// <summary>
        /// Represents a collection of HTTP client authenticators used to handle authentication.
        /// </summary>
        /// <remarks>This static readonly list contains predefined authenticators, such as header-based
        /// and form-login authenticators. It is intended to provide a centralized set of authenticators for HTTP client
        /// operations.</remarks>
        private static readonly List<IHttpClientAuthenticator > authenticators =
        [
            new HeaderBasedAuthenticator(),
            new FormLoginAuthenticator()
        ];

        /// <summary>
        /// Retrieves an HTTP client authenticator that matches the specified authentication type.
        /// </summary>
        /// <remarks>This method searches for an authenticator that is applicable to the provided
        /// authentication type. If no matching authenticator is found, a <see cref="NotSupportedException"/> is
        /// thrown.</remarks>
        /// <param name="type">The type of authentication to be used. This must be a valid <see cref="HttpAuthenticationType"/> value.</param>
        /// <returns>An implementation of <see cref="IHttpClientAuthenticator"/> that supports the specified authentication type.</returns>
        /// <exception cref="NotSupportedException">Thrown if no authenticator is available for the specified <paramref name="type"/>.</exception>
        public static IHttpClientAuthenticator  GetAuthenticator(HttpAuthenticationType type)
        {
            return authenticators.FirstOrDefault(a => a.IsApplicable(type))
                   ?? throw new NotSupportedException($"Authentication type '{type}' not supported.");
        }
    }

}