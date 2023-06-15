using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL
{
    public class Data
    {
        [JsonPropertyName("gameLibraryTitlesRetrieve")]
        public GameLibraryTitlesRetrieve GameLibraryTitlesRetrieve { get; set; }
    }
}