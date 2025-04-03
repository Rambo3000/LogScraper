using System;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters.File
{
    class FileSourceAdapter(string filePath) : ISourceAdapter
    {
        private readonly string filePath = filePath;

        public string GetLog()
        {
            return System.IO.File.ReadAllText(filePath);
        }
        public async Task<string> GetLogAsync()
        {
            return await System.IO.File.ReadAllTextAsync(filePath);
        }
        public DateTime? GetLastTrailTime()
        {
            return null;
        }
    }
}
