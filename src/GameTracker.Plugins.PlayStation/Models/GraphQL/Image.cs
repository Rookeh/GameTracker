using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL
{
    public class Image
    {
        //public string __typename { get; set; }
        [JsonPropertyName("url")]
        public string URL { get; set; }
    }
}