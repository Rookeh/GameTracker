using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models.GOGApi
{
    public class OwnedGames
    {
        [JsonPropertyName("owned")]
        public int[] Owned { get; set; }
    }
}