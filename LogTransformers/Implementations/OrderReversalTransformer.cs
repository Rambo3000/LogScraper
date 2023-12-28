using System;

namespace LogScraper.LogTransformers.Implementations
{
    internal class OrderReversalTransformer : ILogTransformer
    {
        public void Transform(string[] loglines)
        {
            Array.Reverse(loglines); ;
        }
    }
}
