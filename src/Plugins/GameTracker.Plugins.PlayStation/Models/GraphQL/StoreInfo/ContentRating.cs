using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.StoreInfo
{
    public class ContentRating
    {
        [JsonPropertyName("__typename")]
        public string TypeName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}