namespace LogScraper.LogTransformers
{
    public interface ILogTransformer
    {
        void Transform(string[] logEntries);
    }
}
