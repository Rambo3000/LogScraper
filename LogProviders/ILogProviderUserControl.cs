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
        public event EventHandler<string, bool> StatusUpdate;

        /// <summary>
        /// Event triggered when the URI associated with the log provider changes.
        /// </summary>
        public event EventHandler<string> UriChanged;

        /// <summary>
        /// Event that is raised when the validity of the log source changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="isValid">A boolean indicating whether the log source is valid.</param>
        public event EventHandler<bool> IsSourceValidChanged;

        /// <summary>
        /// Gets a value indicating whether the log source is currently valid.
        /// </summary>
        public bool IsSourceValid { get; }

        /// <summary>
        /// Updates the URI associated with the log provider and triggers the UriChanged event.
        /// </summary>
        public void UpdateUri();
    }
}
