using System.Threading.Tasks;

namespace LogScraper.SourceAdapters
{
    public interface ISourceAdapter
    {
        string GetLog();
        Task<string> GetLogAsync();
        string ToString();
    }
}
