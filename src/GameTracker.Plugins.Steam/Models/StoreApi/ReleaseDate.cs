
using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.StoreApi
{
    public class ReleaseDate
    {
        [JsonPropertyName("coming_soon")]
        public bool Unreleased { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }
    }
}