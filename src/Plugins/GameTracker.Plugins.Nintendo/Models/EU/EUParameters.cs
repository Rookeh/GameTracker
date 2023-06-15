using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Nintendo.Models.EU
{
    public class EUParameters
    {
        [JsonPropertyName("q")]
        public string Q { get; set; }

        [JsonPropertyName("start")]
        public string Start { get; set; }

        [JsonPropertyName("fq")]
        public string Fq { get; set; }

        [JsonPropertyName("sort")]
        public string Sort { get; set; }

        [JsonPropertyName("rows")]
        public string Rows { get; set; }

        [JsonPropertyName("wt")]
        public string Wt { get; set; }
    }
}