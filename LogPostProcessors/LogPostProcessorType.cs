namespace LogScraper.LogPostProcessors
{
    public enum LogPostProcessorKind
    {
        JsonPrettyPrint = 0,
        XmlPrettyPrint = 1
    }
    public static class LogPostProcessorKindExtensions
    {
        public static string ToPrettyName(this LogPostProcessorKind kind)
        {
            return kind switch
            {
                LogPostProcessorKind.JsonPrettyPrint => "json",
                LogPostProcessorKind.XmlPrettyPrint => "xml",
                _ => kind.ToString(),
            };
        }
    }
}
