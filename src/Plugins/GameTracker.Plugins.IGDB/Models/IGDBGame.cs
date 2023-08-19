
using Newtonsoft.Json;

public class IGDBGame
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("first_release_date")]
    public int FirstReleaseDate { get; set; }

    [JsonProperty("game_modes")]
    public int[] GameModes { get; set; }

    [JsonProperty("genres")]
    public int[] Genres { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("rating")]
    public float Rating { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}