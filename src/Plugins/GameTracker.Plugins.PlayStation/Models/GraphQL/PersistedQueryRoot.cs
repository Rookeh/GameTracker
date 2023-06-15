using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL
{
    public class PersistedQueryRoot
    {
        [JsonPropertyName("persistedQuery")]
        public PersistedQuery PersistedQuery { get; set; }
    }
}