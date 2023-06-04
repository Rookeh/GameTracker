using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Xbox.Models.OpenXBL;

namespace GameTracker.Plugins.Xbox.Helpers
{
    internal static class Constants
    {
        internal static class ConsolePlatforms
        {
            internal static Platform Xbox360 => new Platform()
            {
                Name = "Xbox 360",
                Icon = "Xbox",
                Description = "The Xbox 360 is a home video game console developed by Microsoft."
            };

            internal static Platform XboxOne => new Platform()
            {
                Name = "Xbox One",
                Icon = "Xbox",
                Description = "The Xbox One S/X are home video game consoles developed by Microsoft."
            };

            internal static Platform XboxSeries => new Platform()
            {
                Name = "Xbox Series",
                Icon = "Xbox",
                Description = "The Xbox Series S/X are home video game consoles developed by Microsoft."
            };
        }

        internal static class Devices
        {
            internal const string PC = "PC";
            internal const string Win32 = "Win32";
            internal const string Xbox360 = "Xbox360";
            internal const string XboxOne = "XboxOne";
            internal const string XboxSeries = "XboxSeries";
        }

        internal static class OpenXBL
        {
            internal const string AuthHeader = "x-authorization";
            internal const string TitleHistoryUrl = "https://xbl.io/api/v2/player/titleHistory";
        }

        public static XboxLiveTitleResponse DefaultTitleResponse => new XboxLiveTitleResponse
        {
            XUID = Guid.Empty.ToString(),
            Titles = Array.Empty<Title>()
        };

        public static Platform XboxPlatform => new Platform
        {
            Name = "Xbox",
            Icon = "Xbox",
            Description = "The Xbox network, formerly and still sometimes branded as Xbox Live, is an online multiplayer gaming and digital media delivery service created and operated by Microsoft.",
            Links = new[]
            {                                
                new SocialLink { LinkPlatform = LinkType.Mastodon, LinkTarget = "@Xbox@mastodon.social" },
                new SocialLink { LinkPlatform = LinkType.Reddit, LinkTarget = "/r/xbox/" },
                new SocialLink { LinkPlatform = LinkType.Twitter, LinkTarget = "@Xbox" },
                new SocialLink { LinkPlatform = LinkType.Web, LinkTarget = "https://www.xbox.com" },
                new SocialLink { LinkPlatform = LinkType.YouTube, LinkTarget = "@xbox" }
            } 
        };
    }
}