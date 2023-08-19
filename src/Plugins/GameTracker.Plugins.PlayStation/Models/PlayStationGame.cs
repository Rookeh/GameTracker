using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.PlayStation.Helpers;
using System;
using System.Globalization;
using Game = GameTracker.Models.Game;
using Image = GameTracker.Models.Image;
using PSNGame = GameTracker.Plugins.PlayStation.Models.GraphQL.GameLibrary.Game;

namespace GameTracker.Plugins.PlayStation.Models
{
    public class PlayStationGame : Game
    {
        private readonly DateTime? _lastPlayed;
        private readonly Platforms _platforms;
        private readonly string _launchUrl;
        private readonly string _title;
        internal PlayStationGame(PSNGame psnGame, string[] genres)
        {
            Description = string.Empty;
            GameplayModes = Array.Empty<GameplayMode>();
            Genres = PlayStationGameHelpers.GenresFromGenreStrings(genres).Distinct().ToArray();
            Image = new Image
            {
                Url = psnGame.Image.URL,
                Width = 338,
                Height = 338
            };
            PlatformId = Convert.ToInt32(psnGame.ConceptId);

            _lastPlayed = psnGame.LastPlayedDateTime;
            _launchUrl = string.IsNullOrEmpty(psnGame.ProductId)
                ? string.Format(Constants.LaunchCommands.Media, CultureInfo.CurrentCulture.Name.ToLower(), psnGame.Platform.ToLower())
                : string.Format(Constants.LaunchCommands.Game, CultureInfo.CurrentCulture.Name.ToLower(), psnGame.ProductId);
            _platforms = GetPlatforms(psnGame.Platform);
            _title = psnGame.Name;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.Controller };

        public override DateTime? LastPlayed => _lastPlayed;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            Icon = "Playstation",
            NewTab = true,
            Text = "View in PlayStation Store",
            Url = _launchUrl
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();        

        public override Platforms Platforms => _platforms;

        public override TimeSpan? Playtime => null;

        public override string ProviderName => "PlayStation";

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

        #region Private methods

        private static Platforms GetPlatforms(string platform)
        {
            switch (platform)
            {
                case Constants.Consoles.PS3:
                    return Platforms.PlayStation3;
                case Constants.Consoles.PS4:
                    return Platforms.PlayStation4;
                case Constants.Consoles.PS5:
                    return Platforms.PlayStation5;
                default:
                    return Platforms.None;
            }
        }

        #endregion
    }
}