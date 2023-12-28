namespace LogScraper.LogTransformers
{
    internal interface ILogTransformer
    {
        void Transform(string[] loglines);
    }
}
