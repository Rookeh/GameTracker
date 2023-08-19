using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Nintendo.Helpers;
using GameTracker.Plugins.Nintendo.Models.JP;

namespace GameTracker.Plugins.Nintendo.Models
{
    public class JPNintendoGame : Game
    {
        private readonly LaunchCommand _launchCommand;
        private readonly string _title;

        public JPNintendoGame(TitleInfoListTitleInfo titleInfo)
        {
            Description = string.Empty;
            GameplayModes = Array.Empty<GameplayMode>();
            Genres = Array.Empty<Genre>();
            Image = new Image
            {
                Width = 480,
                Height = 270,
                Url = titleInfo.ScreenshotImgURL
            };
            ReleaseDate = JPGameHelpers.ReleaseDateFromSalesDateString(titleInfo.SalesDate);
            Studio = titleInfo.MakerName;

            _launchCommand = JPGameHelpers.LaunchCommandFromUrl(titleInfo.LinkURL);            
            _title = titleInfo.TitleName;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.Controller };

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => _launchCommand;

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platforms Platforms => Platforms.NintendoSwitch;

        public override TimeSpan? Playtime => null;

        public override string ProviderName => "Nintendo eShop";

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

        public override Task Preload()
        {
            return Task.CompletedTask;
        }
    }
}