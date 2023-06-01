using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL
{
    public class GameResponse
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
}