using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.StoreInfo
{
    public class StoreInfoRoot
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
}