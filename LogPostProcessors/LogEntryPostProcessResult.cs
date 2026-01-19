namespace LogScraper.LogPostProcessors
{
    public class LogEntryPostProcessResult(LogPostProcessorKind processorKind, string processedText)
    {
        public LogPostProcessorKind ProcessorKind { get; } = processorKind;
        public string ProcessedText { get; } = processedText;
    }
}
