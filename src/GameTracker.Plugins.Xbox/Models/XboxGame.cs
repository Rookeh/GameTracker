using GameTracker.Models;
using GameTracker.Models.Constants;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Xbox.Helpers;
using GameTracker.Plugins.Xbox.Models.OpenXBL;
using System.Text.RegularExpressions;

namespace GameTracker.Plugins.Xbox.Models
{
    public class XboxGame : Game
    {
        private readonly Title _xboxTitle;

        public XboxGame(Title xboxTitle)
        {
            PlatformId = Convert.ToInt32(xboxTitle.TitleId);
            _xboxTitle = xboxTitle;
        }

        public override string Title => _xboxTitle.Name;

        public override string Description => string.Empty;

        public override Genre[] Genres => Array.Empty<Genre>();

        public override string Image => _xboxTitle.DisplayImage;

        public override DateTime? LastPlayed => _xboxTitle.TitleHistory.LastTimePlayed;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            Icon = "Xbox",
            NewTab = false,
            Text = "Launch via Microsoft Store",
            Uri = $"ms-windows-store://pdp/?PFN={_xboxTitle.PFN}"
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Platform[] Platforms => GetPlatforms(_xboxTitle.Devices).ToArray();

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => GetPublisher(_xboxTitle.PFN);

        public override DateTime? ReleaseDate => null;

        public override Review[] Reviews => Array.Empty<Review>();

        public override Studio? Studio => null;

        public override string[] Tags => Array.Empty<string>();

        #region Private methods

        private IEnumerable<Platform> GetPlatforms(string[] devices)
        {
            foreach(var device in devices) 
            { 
                switch (device)
                {
                    case Constants.Devices.PC:
                    case Constants.Devices.Win32:
                        yield return WellKnownPlatforms.Windows;
                        break;
                    case Constants.Devices.Xbox360:
                        yield return Constants.ConsolePlatforms.Xbox360;
                        break;
                    case Constants.Devices.XboxOne:
                        yield return Constants.ConsolePlatforms.XboxOne;
                        break;
                    case Constants.Devices.XboxSeries:
                        yield return Constants.ConsolePlatforms.XboxSeries;
                        break;
                    default: yield break;
                }
            }
        }

        private Publisher? GetPublisher(string pfn)
        {
            if (string.IsNullOrEmpty(pfn) || Regex.IsMatch(pfn, "^\\d"))
            {
                return null;
            }

            var publisherName = "Unknown";

            var pfnArr = pfn.Split('.');
            if (pfnArr.Any())
            {
                publisherName = Regex.Replace(pfnArr[0], "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1 ");
            }

            return new Publisher
            {
                Name = publisherName
            };
        }

        #endregion
    }
}