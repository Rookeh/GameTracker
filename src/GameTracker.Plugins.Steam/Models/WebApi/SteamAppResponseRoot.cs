using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly:InternalsVisibleTo("GameTracker.Plugins.Steam.Tests")]
namespace GameTracker.Plugins.Steam.Models.WebApi
{
    internal class SteamAppResponseRoot
    {
        [JsonPropertyName("response")]
        public SteamAppResponse Response { get; set; }
    }
}