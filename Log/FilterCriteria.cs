using System.Text.RegularExpressions;
using Newtonsoft.Json;

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

        /// <summary>
        /// Indicates whether the AfterPhrase is a regular expression.
        /// </summary>
        public bool IsRegex { get; set; }
        /// <summary>
        /// Gets or sets the compiled regular expression used to match phrases that occur after a specific condition.
        /// </summary>
        [JsonIgnore]
        public Regex RegexCompiled { get; set; }
    }
}
