using GameTracker.Models;
using GameTracker.Models.Enums;

namespace GameTracker.Plugins.Nintendo.Helpers
{
    internal static class Constants
    {
        internal static class EURegion
        {
            internal const string StoreUrlFormat = "https://www.nintendo-europe.com{0}";
            internal const string GetGamesUrlFormat = "http://search.nintendo-europe.com/{0}/select";
            internal const string DefaultLocale = "en";
            internal const string MaxGames = "9999";
            internal const string StudioRegex = @"^(?:Copyright|©)\s+(?:\d{4}(?:-\d{4})?|\d{4})\s+([^.\n]+)(?!(?:Copyright|©))";

            internal static Dictionary<string, Genre> GenreMappings = new Dictionary<string, Genre>()
            {
                ["action"] = Genre.Action,
                ["shooter"] = Genre.Shooter,
                ["board_game"] = Genre.BoardGame,
                ["education"] = Genre.Other,
                ["puzzle"] = Genre.Puzzle,
                ["other"] = Genre.Other,
                ["arcade"] = Genre.Arcade,
                ["party"] = Genre.Party,
                ["racing"] = Genre.Racing,
                ["simulation"] = Genre.Simulation,
                ["music"] = Genre.Music,
                ["platformer"] = Genre.Platformer,
                ["adventure"] = Genre.Adventure,
                ["lifestyle"] = Genre.Casual,
                ["first_person_shooter"] = Genre.Shooter,
                ["fighting"] = Genre.Fighting,
                ["strategy"] = Genre.Strategy,
                ["utility"] = Genre.Other,
                ["sports"] = Genre.Sports,
                ["training"] = Genre.Other,
                ["rpg"] = Genre.RPG,
                ["golf"] = Genre.Sports,
                ["system_tool"] = Genre.Other,
                ["football"] = Genre.Sports,
                ["communication"] = Genre.Other,
                ["animal_life"] = Genre.Simulation,
                ["health_and_fitness"] = Genre.Other,
                ["extreme"] = Genre.Sports,
                ["tennis"] = Genre.Sports,
                ["cooking"] = Genre.Casual
            };
        }

        internal static class JPRegion
        {
            internal const string GetGamesUrl = "https://www.nintendo.co.jp/data/software/xml/switch.xml";
            internal const string StoreUrlFormat = "https://store-jp.nintendo.com/list/software/{0}.html";
        }

        internal static class Platforms
        {
            internal static Platform NintendoEShop => new Platform
            {
                Name = "Nintendo EShop",
                Description = "Nintendo EShop",
                Icon = "NintendoSwitch",
                ExtendedInformation = "This integration cannot access your Nintendo account; therefore, you must specify " +
                                      "which titles you own. Details for owned titles will be retrieved from the Nintendo EShop.<br><br> " +
                                      "You must also specify your region code. Currently, only Europe (EU) and Japan (JP) region codes are supported.",
                Links = new SocialLink[] 
                {
                    new SocialLink { LinkPlatform = LinkType.Web, LinkTarget = "https://www.nintendo.com/store/" }
                }
            };

            internal static Platform NintendoSwitch = new Platform
            {
                Name = "Nintendo Switch",
                Icon = "NintendoSwitch",
                Description = "The Nintendo Switch is a hybrid video game console developed by Nintendo and released worldwide in most regions on March 3, 2017.",
                Links = new SocialLink[]
                {
                    new SocialLink { LinkPlatform = LinkType.Twitter, LinkTarget = "@NintendoSwitch" },
                    new SocialLink { LinkPlatform = LinkType.Web, LinkTarget = "https://www.nintendo.com" }
                }
            };
        }
    }
}