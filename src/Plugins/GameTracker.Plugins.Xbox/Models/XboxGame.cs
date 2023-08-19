using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Xbox.Helpers;
using GameTracker.Plugins.Xbox.Models.OpenXBL;

namespace GameTracker.Plugins.Xbox.Models
{
    public class XboxGame : Game
    {
        private readonly ControlScheme[] _controlSchemes;
        private readonly DateTime? _lastPlayed;
        private readonly LaunchCommand _launchCommand;
        private readonly Platforms _platforms;
        private readonly string _title;

        public XboxGame(Title xboxTitle)
        {
            Description = string.Empty;
            GameplayModes = Array.Empty<GameplayMode>();
            Genres = Array.Empty<Genre>();
            Image = XboxGameHelpers.GetImageFromUrl(xboxTitle.DisplayImage);
            PlatformId = Convert.ToInt32(xboxTitle.TitleId);
            Publisher = XboxGameHelpers.GetPublisher(xboxTitle.PFN);

            _controlSchemes = XboxGameHelpers.GetControlSchemesFromDevices(xboxTitle.Devices).Distinct().ToArray();
            _lastPlayed = xboxTitle.TitleHistory.LastTimePlayed;
            _launchCommand = XboxGameHelpers.GetLaunchCommand(xboxTitle.PFN);
            _platforms = XboxGameHelpers.GetPlatforms(xboxTitle.Devices);            
            _title = xboxTitle.Name;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => _controlSchemes;

        public override DateTime? LastPlayed => _lastPlayed;

        public override LaunchCommand LaunchCommand => _launchCommand;

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platforms Platforms => _platforms;

        public override TimeSpan? Playtime => null;

        public override string ProviderName => "Xbox";

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;
    }
}