using LogScraper.Sources.Adapters.File;
using LogScraper.Sources.Adapters.Http;

namespace LogScraper.Sources.Adapters
{
    class SourceAdapterFactory
    {
        public static ISourceAdapter CreateHttpSourceAdapter(string apiUrl, string credentialManagerUri, int timeoutSeconds)
        {
            return new HttpSourceAdapter(apiUrl, credentialManagerUri, timeoutSeconds);
        }
        public static ISourceAdapter CreateFileSourceAdapter(string filePath)
        {
            return new FileSourceAdapter(filePath);
        }
    }
}
