using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.GameLibrary
{
    public class PersistedQuery
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("sha256Hash")]
        public string SHA256Hash { get; set; }
    }
}