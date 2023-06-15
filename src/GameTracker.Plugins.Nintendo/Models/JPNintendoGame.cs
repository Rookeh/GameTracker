using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Nintendo.Helpers;
using GameTracker.Plugins.Nintendo.Models.JP;

namespace GameTracker.Plugins.Nintendo.Models
{
    public class JPNintendoGame : Game
    {
        private readonly TitleInfoListTitleInfo _titleInfo;

        public JPNintendoGame(TitleInfoListTitleInfo titleInfo)
        {
            _titleInfo = titleInfo;
        }

        public override ControlScheme[] ControlSchemes => Array.Empty<ControlScheme>();

        public override string Description => string.Empty;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => new Image
        {
            Width = 480,
            Height = 270,
            Url = _titleInfo.ScreenshotImgURL
        };

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => JPGameHelpers.LaunchCommandFromUrl(_titleInfo.LinkURL);

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platform[] Platforms => new[] { Constants.Platforms.NintendoSwitch };

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => null;

        public override DateTime? ReleaseDate => JPGameHelpers.ReleaseDateFromSalesDateString(_titleInfo.SalesDate);

        public override Review[] Reviews => Array.Empty<Review>();

        public override string StorefrontName => "Nintendo eShop";

        public override Studio? Studio => JPGameHelpers.StudioFromMakerName(_titleInfo.MakerName);

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _titleInfo.TitleName;

        public override Task Preload()
        {
            return Task.CompletedTask;
        }
    }
}