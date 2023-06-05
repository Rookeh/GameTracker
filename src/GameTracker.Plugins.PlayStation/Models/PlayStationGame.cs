using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.PlayStation.Helpers;
using System.Globalization;
using PSNGame = GameTracker.Plugins.PlayStation.Models.GraphQL.Game;

namespace GameTracker.Plugins.PlayStation.Models
{
    public class PlayStationGame : Game
    {
        private readonly PSNGame _psnGame;
        private readonly string _locale;

        internal PlayStationGame(PSNGame psnGame)
        {
            _locale = CultureInfo.CurrentCulture.Name.ToLower();
            _psnGame = psnGame;
            PlatformId = Convert.ToInt32(psnGame.ConceptId);
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override TimeSpan? Playtime => null;

        public override string Title => _psnGame.Name;

        public override string Description => string.Empty;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => new Image
        {
            Url = _psnGame.Image.URL,
            Width = 338,
            Height = 338
        };

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            Icon = "Playstation",
            NewTab = true,
            Text = "View in PlayStation Store",
            Uri = GetLaunchUri()
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override DateTime? LastPlayed => _psnGame.LastPlayedDateTime;

        public override Platform[] Platforms => GetPlatforms().ToArray();

        public override Publisher? Publisher => null;

        public override DateTime? ReleaseDate => null;

        public override Review[] Reviews => Array.Empty<Review>();

        public override Studio? Studio => null;

        public override string[] Tags => Array.Empty<string>();

        #region Private methods

        private string GetLaunchUri()
        {
            return string.IsNullOrEmpty(_psnGame.ProductId)
                ? string.Format(Constants.LaunchCommands.Media, _locale, _psnGame.Platform.ToLower())
                : string.Format(Constants.LaunchCommands.Game, _locale, _psnGame.ProductId);
        }

        private IEnumerable<Platform> GetPlatforms()
        {
            switch (_psnGame.Platform)
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