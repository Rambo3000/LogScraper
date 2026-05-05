namespace LogScraper.Log.Processing
{
    /// <summary>
    /// Represents the combined state of the active data retrieval process.
    /// </summary>
    public sealed class LogProcessingState
    {
        public static readonly LogProcessingState Inactive = new(false, LogProcessingStatus.Idle);
        public static LogProcessingState Active(bool isTimed) => new(isTimed, LogProcessingStatus.Retrieving);

        private LogProcessingState(bool isTimed, LogProcessingStatus status)
        {
            IsTimed = isTimed;
            Status = status;
        }

        /// <summary>
        /// Whether a fetch is currently running, derived from <see cref="Status"/>.
        /// </summary>
        public bool IsActive => Status != LogProcessingStatus.Idle;

        /// <summary>
        /// Whether the current fetch is a timed (continuous) fetch as opposed to a single fetch.
        /// Only meaningful when <see cref="IsActive"/> is <c>true</c>.
        /// </summary>
        public bool IsTimed { get; }

        /// <summary>
        /// The current processing status of the fetch.
        /// </summary>
        public LogProcessingStatus Status { get; }

        /// <summary>
        /// Returns a new <see cref="LogProcessingState"/> with the same <see cref="IsTimed"/> value but a different status.
        /// </summary>
        public LogProcessingState WithStatus(LogProcessingStatus status) => new(IsTimed, status);
    }
}
