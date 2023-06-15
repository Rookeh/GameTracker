using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Xbox.Helpers;
using GameTracker.Plugins.Xbox.Models.OpenXBL;

namespace GameTracker.Plugins.Xbox.Models
{
    public class XboxGame : Game
    {
        private readonly ControlScheme[] _controlSchemes;
        private readonly Image _image;
        private readonly DateTime? _lastPlayed;
        private readonly LaunchCommand _launchCommand;
        private readonly Platform[] _platforms;
        private readonly Publisher? _publisher;
        private readonly string _title;

        public XboxGame(Title xboxTitle)
        {
            PlatformId = Convert.ToInt32(xboxTitle.TitleId);
            _controlSchemes = XboxGameHelpers.GetControlSchemesFromDevices(xboxTitle.Devices).Distinct().ToArray();
            _image = XboxGameHelpers.GetImageFromUrl(xboxTitle.DisplayImage);
            _lastPlayed = xboxTitle.TitleHistory.LastTimePlayed;
            _launchCommand = XboxGameHelpers.GetLaunchCommand(xboxTitle.PFN);
            _platforms = XboxGameHelpers.GetPlatforms(xboxTitle.Devices).ToArray();
            _publisher = XboxGameHelpers.GetPublisher(xboxTitle.PFN);
            _title = xboxTitle.Name;

        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => _controlSchemes;

        public override string Description => string.Empty;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => _image;

        public override DateTime? LastPlayed => _lastPlayed;

        public override LaunchCommand LaunchCommand => _launchCommand;

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platform[] Platforms => _platforms;

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => _publisher;

        public override DateTime? ReleaseDate => null;

        public override Review[] Reviews => Array.Empty<Review>();

        public override string StorefrontName => "Xbox";

        public override Studio? Studio => null;

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;
    }
}