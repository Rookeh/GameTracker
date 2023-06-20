using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.StoreInfo
{
    public class Edition
    {
        [JsonPropertyName("__typename")]
        public string TypeName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}