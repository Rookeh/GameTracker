using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Nintendo.Models.EU
{
    public class EUGameResponse
    {
        [JsonPropertyName("numFound")]
        public int NumFound { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("docs")]
        public EUDocument[] Docs { get; set; }
    }
}