using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters
{
    public interface ISourceAdapter
    {
        string GetLog();
        Task<string> GetLogAsync();
        string ToString();
    }
}
