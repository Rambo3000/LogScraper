namespace LogScraper.Log.Processing
{
    /// <summary>
    /// Represents the processing status of a log file.
    /// </summary>
    public enum LogProcessingStatus
    {
        /// <summary>
        /// The log file is currently being retrieved from the source.
        /// </summary>
        Retrieving,
        /// <summary>
        /// The log file is currently being processed, which may include parsing, analyzing, or extracting relevant information.
        /// </summary>
        Processing,
        /// <summary>
        /// The log file is waiting to be processed, which may occur if there are multiple log files in a queue or if the processing resources are currently occupied with other tasks.
        /// </summary>
        Waiting,
        /// <summary>
        /// There is no log file currently being processed, and the system is idle.
        /// </summary>
        Idle
    }
}
