using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Nintendo.Helpers;
using GameTracker.Plugins.Nintendo.Models.JP;

namespace GameTracker.Plugins.Nintendo.Models
{
    public class JPNintendoGame : Game
    {
        private readonly string _imageUrl;
        private readonly LaunchCommand _launchCommand;
        private readonly DateTime? _releaseDate;
        private readonly Studio? _studio;
        private readonly string _title;

        public JPNintendoGame(TitleInfoListTitleInfo titleInfo)
        {
            _imageUrl = titleInfo.ScreenshotImgURL;
            _launchCommand = JPGameHelpers.LaunchCommandFromUrl(titleInfo.LinkURL);
            _releaseDate = JPGameHelpers.ReleaseDateFromSalesDateString(titleInfo.SalesDate);
            _studio = JPGameHelpers.StudioFromMakerName(titleInfo.MakerName);
            _title = titleInfo.TitleName;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.Controller };

        public override string Description => string.Empty;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => new Image
        {
            Width = 480,
            Height = 270,
            Url = _imageUrl
        };

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => _launchCommand;

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platform[] Platforms => new[] { Constants.Platforms.NintendoSwitch };

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => null;

        public override DateTime? ReleaseDate => _releaseDate;

        public override Review[] Reviews => Array.Empty<Review>();

        public override string StorefrontName => "Nintendo eShop";

        public override Studio? Studio => _studio;

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

        public override Task Preload()
        {
            return Task.CompletedTask;
        }
    }
}