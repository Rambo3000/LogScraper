using Newtonsoft.Json;

namespace LogScraper.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsEqualByJsonComparison(this object obj1, object obj2)
        {
            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json1 = JsonConvert.SerializeObject(obj1, settings);
            string json2 = JsonConvert.SerializeObject(obj2, settings);

            return json1 == json2;
        }
    }
}
