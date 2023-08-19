using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.IGDB.Models;
using System.Text;

namespace GameTracker.Plugins.IGDB.Helpers
{
    public static class QueryBuilder
    {
        public static string BuildQuery(IEnumerable<Game> games)
        {
            var stringBuilder = new StringBuilder();

            foreach (var game in games.DistinctBy(g => g.PlatformId))
            {
                var unpunctuatedTitle = new string(game.Title.Replace('-', ' ').Where(c => char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)).ToArray());

                stringBuilder.Append($@"query games ""{game.PlatformId}"" {{ fields first_release_date,game_modes,genres,multiplayer_modes,name,rating,summary,url; where name = ""{unpunctuatedTitle}"" ");
                if (game.Platforms > 0)
                {
                    stringBuilder.Append($@"& platforms = ({GetIGDBPlatforms(game.Platforms)}); }};");
                }
                else
                {
                    stringBuilder.Append("; };");
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        private static string GetIGDBPlatforms(Platforms platforms)
        {
            return string.Join(',', Enum.GetValues<Platforms>().Where(p => p != Platforms.None && platforms.HasFlag(p)).Select(p => (int)PlatformMapping[p]));
        }

        private static Dictionary<Platforms, IGDBPlatform> PlatformMapping => new Dictionary<Platforms, IGDBPlatform>()
        {
            { Platforms.Windows, IGDBPlatform.Windows },
            { Platforms.MacOS, IGDBPlatform.MacOS },
            { Platforms.Linux, IGDBPlatform.Linux },
            { Platforms.NintendoSwitch, IGDBPlatform.NintendoSwitch },
            { Platforms.PlayStation3, IGDBPlatform.PlayStation3 },
            { Platforms.PlayStation4, IGDBPlatform.PlayStation4 },
            { Platforms.PlayStation5, IGDBPlatform.PlayStation5 },
            { Platforms.Xbox360, IGDBPlatform.Xbox360 },
            { Platforms.XboxOne, IGDBPlatform.XboxOne },
            { Platforms.XboxSeries, IGDBPlatform.XboxSeries }
        };
    }
}
