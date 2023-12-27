namespace LogScraper.SourceAdapters.Http
{
    public class HttpAuthenticationData
    {
        public HttpAuthenticationType Type { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BearerToken { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
    }
}