using System;

namespace LogScraper.LogPostProcessors
{
    /// <summary>
    /// Provides data for an event that signals the completion of a post-processing operation.
    /// </summary>
    /// <param name="wasCanceled">A value indicating whether the post-processing operation was canceled before completion.</param>
    /// <param name="hasChanges">A value indicating whether the post-processing operation resulted in any changes.</param>
    public sealed class PostProcessingFinishedEventArgs(bool wasCanceled, bool hasChanges) : EventArgs
    {
        public bool WasCanceled { get; } = wasCanceled;
        public bool HasChanges { get; } = hasChanges;
    }
}