using System;
using System.Collections.Generic;

namespace LogScraper.LogPostProcessors
{
    /// <summary>
    /// Provides data for an event that signals the completion of a post-processing operation.
    /// </summary>
    /// <param name="wasCanceled">A value indicating whether the post-processing operation was canceled before completion.</param>
    /// <param name="hasChanges">An array indicating whether each post-processor made changes to the log entries.</param>
    public sealed class LogPostProcessingFinishedEventArgs(bool wasCanceled, bool[] hasChanges) : EventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the operation was canceled.
        /// </summary>
        public bool WasCanceled { get; } = wasCanceled;
        /// <summary>
        /// Gets an array indicating whether the operation resulted in any changes for each post-processor.
        /// </summary>
        public bool[] HasChanges { get; } = hasChanges;
    }
}