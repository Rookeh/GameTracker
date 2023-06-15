using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models.GOGApi
{
    public class GameDetailsRoot
    {
        [JsonPropertyOrder(0)]
        public GameDetails[] Games { get; set; }
    }
}