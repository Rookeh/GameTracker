using System.Globalization;

namespace GameTracker.Plugins.Steam.Helpers
{
    internal static class LocalizationHelper
    {
        internal static string GetSteamLocaleString()
        {
            return CultureInfo.CurrentCulture.EnglishName.Split(' ').First().ToLower();
        }
    }
}