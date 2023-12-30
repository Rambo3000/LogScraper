using System;
using LogScraper.Sources.Adapters;

namespace LogScraper.LogProviders
{
    internal interface ILogProviderUserControl
    {
        public ISourceAdapter GetSourceAdapter();

        public event EventHandler SourceSelectionChanged;

        public event Action<string, bool> StatusUpdate;
    }
}
