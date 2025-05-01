using System;
using LogScraper.Sources.Adapters;

namespace LogScraper.LogProviders
{
    /// <summary>
    /// Represents the interface for user controls that interact with log providers.
    /// Provides methods and events for managing log provider sources and their status.
    /// </summary>
    internal interface ILogProviderUserControl
    {
        /// <summary>
        /// Retrieves the source adapter associated with the log provider.
        /// The source adapter is responsible for fetching logs from the log provider.
        /// </summary>
        /// <returns>An instance of <see cref="ISourceAdapter"/> that interacts with the log provider.</returns>
        public ISourceAdapter GetSourceAdapter();

        /// <summary>
        /// Event triggered when the source selection changes in the user control.
        /// This can be used to notify other components about changes in the selected log source.
        /// </summary>
        public event EventHandler SourceSelectionChanged;

        /// <summary>
        /// Event triggered to update the status of the log provider.
        /// Provides a message and a success flag to indicate the status.
        /// </summary>
        public event Action<string, bool> StatusUpdate;
    }
}
