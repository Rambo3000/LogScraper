namespace LogScraper.LogPostProcessors
{
    public enum LogPostProcessorKind
    {
        JsonPrettyPrint = 1,
        XmlPrettyPrint = 2
    }
    public static class LogPostProcessorKindExtensions
    {
        public static string ToPrettyName(this LogPostProcessorKind kind)
        {
            switch (kind)
            {
                case LogPostProcessorKind.JsonPrettyPrint:
                    return "json";

                case LogPostProcessorKind.XmlPrettyPrint:
                    return "xml";

                default:
                    return kind.ToString();
            }
        }
    }
}
