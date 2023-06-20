using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.GameLibrary
{
    public class GameResponse
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
}