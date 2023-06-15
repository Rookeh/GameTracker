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
                Description = "The Xbox 360 is a seventh-generation game console, developed and released by Microsoft in 2005."
            };

            internal static Platform XboxOne => new Platform()
            {
                Name = "Xbox One",
                Icon = "Xbox",
                Description = "The Xbox One is an eighth-generation game console, developed by Microsoft as a successor to the Xbox 360 and released in 2013. " +
                              "Later in its lifecycle, two new variants were introduced; the Xbox One S and Xbox One X. The former replaced the original variant as an entry-level " + 
                              "model and the One X was introduced as a higher-powered version, offering native 4K output and improved visual fidelity, to compete with the PlayStation 4 Pro."
            };

            internal static Platform XboxSeries => new Platform()
            {
                Name = "Xbox Series",
                Icon = "Xbox",
                Description = "The Xbox Series S and Series X are ninth-generation home video game consoles developed by Microsoft as successors to the Xbox One S and One X, respectively. " +
                              "They were released in 2020. Both systems offer improvements in compute and graphical fidelity over their predecessors, although the Series X is the more " +
                              "powerful of the two variants."
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
            internal const string URL = "https://xbl.io/";
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
            Description = "The Xbox network is an online gaming and digital storefront operated by Microsoft. It was previously branded as Xbox Live, " +
                          "which was introduced in 2002 as an online platform for the original Xbox console. Starting with the Xbox 360, the service was " +
                          "updated to include the ability to purchase digital titles, and over the years it has evolved further to offer apps, streaming content, " +
                          "and a subscription service called Game Pass which offers subscribers access to a rotating library of titles at no additional cost. In " +
                          "2020, Xbox Cloud Gaming was launched, offering the ability to stream Xbox titles to Xbox consoles and web browsers.",
            ExtendedInformation = @$"This integration uses <a href=""{OpenXBL.URL}"">OpenXBL</a> to fetch data from your Xbox profile. " +
                                  $@"You will need to sign up, and then copy the personal API key from your OpenXBL profile page.<br><br> " +
                                    "You can opt to include or exclude both Game Pass titles and legacy (Xbox 360 / Games for Windows Live) titles from the results.",
            Links = new[]
            {                                
                new SocialLink { LinkPlatform = LinkType.Reddit, LinkTarget = "xbox" },
                new SocialLink { LinkPlatform = LinkType.Twitter, LinkTarget = "@Xbox" },
                new SocialLink { LinkPlatform = LinkType.Web, LinkTarget = "https://www.xbox.com" },
                new SocialLink { LinkPlatform = LinkType.YouTube, LinkTarget = "@xbox" }
            } 
        };
    }
}