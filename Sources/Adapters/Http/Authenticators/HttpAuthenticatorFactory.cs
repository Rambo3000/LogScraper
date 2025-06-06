using System;
using System.Collections.Generic;
using System.Linq;

namespace LogScraper.Sources.Adapters.Http.Authenticators
{
    public static class HttpAuthenticatorFactory
    {
        private static readonly List<IHttpClientAuthenticator > authenticators =
        [
            new HeaderBasedAuthenticator(),
            new FormLoginAuthenticator()
        ];

        public static IHttpClientAuthenticator  GetAuthenticator(HttpAuthenticationType type)
        {
            return authenticators.FirstOrDefault(a => a.IsApplicable(type))
                   ?? throw new NotSupportedException($"Authentication type '{type}' not supported.");
        }
    }

}