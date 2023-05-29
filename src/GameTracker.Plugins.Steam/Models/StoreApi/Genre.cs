
using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.StoreApi
{
    public class Genre
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}