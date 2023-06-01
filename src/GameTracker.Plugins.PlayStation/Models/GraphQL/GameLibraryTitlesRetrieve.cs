using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL
{
    public class GameLibraryTitlesRetrieve
    {
        [JsonPropertyName("games")]
        public Game[] Games { get; set; }
    }
}