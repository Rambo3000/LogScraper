namespace LogScraper.Log
{
    /// <summary>
    /// Represents criteria for filtering log content or metadata.
    /// </summary>
    public class FilterCriteria
    {
        /// <summary>
        /// A phrase that marks the beginning of the filter range.
        /// </summary>
        public string BeforePhrase { get; set; }

        /// <summary>
        /// A phrase that marks the end of the filter range.
        /// </summary>
        public string AfterPhrase { get; set; }
    }
}
