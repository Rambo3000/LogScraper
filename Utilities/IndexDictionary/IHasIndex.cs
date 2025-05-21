namespace LogScraper.Utilities.IndexDictionary
{
    /// <summary>
    /// Interface for objects that have an index. This is used in combination with the <see cref="IndexDictionary{TKey,TValue}"/> class.
    /// </summary>
    public interface IHasIndex
    {
        /// <summary>
        /// Gets the index of the object in the list of objects. This is used in combination with the <see cref="IndexDictionary{TKey,TValue}"/> class.
        /// </summary>
        int Index { get; set; }
    }
}
