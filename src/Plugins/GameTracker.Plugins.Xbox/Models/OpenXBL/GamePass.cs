using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Xbox.Models.OpenXBL
{
    public class GamePass
    {
        [JsonPropertyName("isGamePass")]
        public bool IsGamePass { get; set; }
    }
}