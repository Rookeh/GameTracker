
using Newtonsoft.Json;

public class IGDBQueryResult
{
    [JsonProperty("name")]
    public string Id { get; set; }

    [JsonProperty("result")]
    public IGDBGame[] Game { get; set; }
}
