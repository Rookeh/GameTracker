using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models
{
    public class OwnedGames
    {
        [JsonPropertyName("owned")]
        public int[] Owned { get; set; }
    }
}