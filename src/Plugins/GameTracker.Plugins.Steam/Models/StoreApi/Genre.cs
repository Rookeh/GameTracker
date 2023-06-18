
using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.StoreApi
{
    public class Genre
    {
        [JsonPropertyName("id")]
        public string IdString { get; set; }

        [JsonIgnore]
        public int Id => Convert.ToInt32(IdString);

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}