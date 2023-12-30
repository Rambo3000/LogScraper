using LogScraper.Sources.Adapters.File;
using LogScraper.Sources.Adapters.Http;

namespace LogScraper.Sources.Adapters
{
    class SourceAdapterFactory
    {
        public static ISourceAdapter CreateHttpSourceAdapter(string apiUrl, string credentialManagerUri)
        {
            return new HttpSourceAdapter(apiUrl, credentialManagerUri);
        }
        public static ISourceAdapter CreateFileSourceAdapter(string filePath)
        {
            return new FileSourceAdapter(filePath);
        }
    }
}
