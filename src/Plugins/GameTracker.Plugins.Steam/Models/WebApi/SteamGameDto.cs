using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("GameTracker.Plugins.Steam.Tests")]
namespace GameTracker.Plugins.Steam.Models.WebApi
{
    public class SteamGameDto
    {
        private int _appId;

        [JsonPropertyName("appid")]
        public int AppId
        {
            get
            {
                return _appId;
            }
            set
            {
                _appId = Convert.ToInt32(value);
            }
        }

        [JsonPropertyName("playtime_forever")]
        public int Playtime { get; set; }

        [JsonPropertyName("playtime_windows_forever")]
        public int PlaytimeWindows { get; set; }

        [JsonPropertyName("playtime_mac_forever")]
        public int PlaytimeMac { get; set; }

        [JsonPropertyName("playtime_linux_forever")]
        public int PlaytimeLinux { get; set; }

        [JsonPropertyName("rtime_last_played")]
        public long LastPlayedTimestamp { get; set; }
    }
}