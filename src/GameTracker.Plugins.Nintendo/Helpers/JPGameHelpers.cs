using GameTracker.Models;
using System.Globalization;

namespace GameTracker.Plugins.Nintendo.Helpers
{
    internal static class JPGameHelpers
    {
        internal static LaunchCommand LaunchCommandFromUrl(string url)
        {
            return new LaunchCommand
            {
                Icon = "NintendoSwitch",
                NewTab = true,
                Text = "Open in Nintendo Store",
                Uri = string.Format(Constants.JPRegion.StoreUrlFormat, url.Remove(0, 8))
            };
        }

        internal static DateTime? ReleaseDateFromSalesDateString(string salesDateString)
        {
            if (string.IsNullOrEmpty(salesDateString)) return null;

            return DateTime.ParseExact(salesDateString, "yyyy.M.d", CultureInfo.InvariantCulture);
        }

        internal static Studio? StudioFromMakerName(string makerName)
        {
            if (string.IsNullOrEmpty(makerName)) return null;

            return new Studio
            {
                Name = makerName,
            };
        }
    }
}