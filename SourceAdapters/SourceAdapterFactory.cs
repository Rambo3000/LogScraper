using LogScraper.SourceAdapters.File;
using LogScraper.SourceAdapters.Http;

namespace LogScraper.SourceAdapters
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
