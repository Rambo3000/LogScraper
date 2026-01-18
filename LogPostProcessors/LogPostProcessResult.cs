namespace LogScraper.LogPostProcessors
{
    public class LogPostProcessResult
    {
        public LogPostProcessorKind ProcessorKind { get; }
        public string ProcessedText { get; }

        public LogPostProcessResult(LogPostProcessorKind processorKind, string processedText)
        {
            ProcessorKind = processorKind;
            ProcessedText = processedText;
        }
    }
}
