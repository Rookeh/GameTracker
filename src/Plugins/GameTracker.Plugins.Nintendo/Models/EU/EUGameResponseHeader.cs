using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Nintendo.Models.EU
{
    public class EUGameResponseHeader
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("QTime")]
        public int QTime { get; set; }

        [JsonPropertyName("_params")]
        public EUParameters Params { get; set; }
    }
}