using GameTracker.Models;
using GameTracker.Models.Enums;
using System.Text.RegularExpressions;

namespace GameTracker.Plugins.Nintendo.Helpers
{
    internal class EUGameHelpers
    {
        internal static Genre[] GenresFromGameCategories(string[] gameCategories)
        {
            var genres = new List<Genre>();

            foreach (var category in gameCategories)
            {
                if (Constants.EURegion.GenreMappings.ContainsKey(category))
                {
                    genres.Add(Constants.EURegion.GenreMappings[category]);
                }
            }

            return genres.Distinct().ToArray();
        }

        internal static Image ImageFromImageUrl(string imageUrl)
        {
            return new Image
            {
                Width = 500,
                Height = 250,                
                Url = imageUrl
            };
        }

        internal static string StudioFromCopyright(string copyrightHolder)
        {
            var match = Regex.Match(copyrightHolder, Constants.EURegion.StudioRegex);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        internal static LaunchCommand LaunchCommandFromUrl(string url)
        {
            return new LaunchCommand
            {
                Icon = "NintendoSwitch",
                NewTab = true,
                Text = "Open in Nintendo EShop",
                Url = string.Format(Constants.EURegion.StoreUrlFormat, url)
            };
        }
    }
}