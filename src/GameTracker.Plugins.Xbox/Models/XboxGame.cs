using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Xbox.Helpers;
using GameTracker.Plugins.Xbox.Models.OpenXBL;

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

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => XboxGameHelpers.GetControlSchemesFromDevices(_xboxTitle.Devices).Distinct().ToArray();

        public override string Description => string.Empty;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => new Image
        {
            Url = _xboxTitle.DisplayImage,
            Width = 310,
            Height = 310
        };

        public override DateTime? LastPlayed => _xboxTitle.TitleHistory.LastTimePlayed;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            Icon = "Xbox",
            NewTab = false,
            Text = "Launch via Microsoft Store",
            Url = $"ms-windows-store://pdp/?PFN={_xboxTitle.PFN}"
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platform[] Platforms => XboxGameHelpers.GetPlatforms(_xboxTitle.Devices).ToArray();

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => XboxGameHelpers.GetPublisher(_xboxTitle.PFN);

        public override DateTime? ReleaseDate => null;

        public override Review[] Reviews => Array.Empty<Review>();

        public override string StorefrontName => "Xbox";

        public override Studio? Studio => null;

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _xboxTitle.Name;
    }
}