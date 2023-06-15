using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.PlayStation.Helpers;
using System.Globalization;
using PSNGame = GameTracker.Plugins.PlayStation.Models.GraphQL.Game;

namespace GameTracker.Plugins.PlayStation.Models
{
    public class PlayStationGame : Game
    {
        private readonly string _imageUrl;
        private readonly DateTime? _lastPlayed;
        private readonly Platform[] _platforms;
        private readonly string _launchUrl;
        private readonly string _title;

        internal PlayStationGame(PSNGame psnGame)
        {            
            PlatformId = Convert.ToInt32(psnGame.ConceptId);
            _imageUrl = psnGame.Image.URL;
            _lastPlayed = psnGame.LastPlayedDateTime;
            _launchUrl = string.IsNullOrEmpty(psnGame.ProductId)
                ? string.Format(Constants.LaunchCommands.Media, CultureInfo.CurrentCulture.Name.ToLower(), psnGame.Platform.ToLower())
                : string.Format(Constants.LaunchCommands.Game, CultureInfo.CurrentCulture.Name.ToLower(), psnGame.ProductId);
            _platforms = GetPlatforms(psnGame.Platform).ToArray();
            _title = psnGame.Name;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.Controller };

        public override string Description => string.Empty;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => new Image
        {
            Url = _imageUrl,
            Width = 338,
            Height = 338
        };

        public override DateTime? LastPlayed => _lastPlayed;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            Icon = "Playstation",
            NewTab = true,
            Text = "View in PlayStation Store",
            Url = _launchUrl
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();        

        public override Platform[] Platforms => _platforms;

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => null;

        public override DateTime? ReleaseDate => null;

        public override Review[] Reviews => Array.Empty<Review>();

        public override string StorefrontName => "PlayStation";

        public override Studio? Studio => null;

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

        #region Private methods

        private static IEnumerable<Platform> GetPlatforms(string platform)
        {
            switch (platform)
            {
                case Constants.Consoles.PS3:
                    yield return Constants.ConsolePlatforms.PlayStation3;
                    break;
                case Constants.Consoles.PS4:
                    yield return Constants.ConsolePlatforms.PlayStation4;
                    break;
                case Constants.Consoles.PS5:
                    yield return Constants.ConsolePlatforms.PlayStation5;
                    break;
                default:
                    yield break;
            }
        }

        #endregion
    }
}