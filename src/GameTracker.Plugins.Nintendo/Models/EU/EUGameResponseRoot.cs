using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Nintendo.Models.EU
{
    public class EUGameResponseRoot
    {
        [JsonPropertyName("responseHeader")]
        public EUGameResponseHeader ResponseHeader { get; set; }
        
        [JsonPropertyName("response")]
        public EUGameResponse Response { get; set; }
    }
}