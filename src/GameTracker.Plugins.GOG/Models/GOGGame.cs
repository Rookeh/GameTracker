using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.GOG.Helpers;
using GameTracker.Plugins.GOG.Models.GOGApi;

namespace GameTracker.Plugins.GOG.Models
{
    public class GOGGame : Game
    {
        private readonly GameDetails _gameDetails;

        public GOGGame(GameDetails gameDetails)
        {
            PlatformId = gameDetails.Id;
            _gameDetails = gameDetails;
        }

        public override string Description => _gameDetails.Description?.Full ?? string.Empty;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override string Image => _gameDetails.Images?.Background ?? "img\\placeholder.png";

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            NewTab = false,
            Icon = "PcDisplay",
            Text = "Launch via GOG Galaxy",
            Uri = $"goggalaxy://openGameView/{PlatformId}"
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();        

        public override DateTime? LastPlayed => null;

        public override Platform[] Platforms => _gameDetails.ContentSystemCompatibility.FromContentSystemCompatibility();

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => null;

        public override DateTime? ReleaseDate => string.IsNullOrEmpty(_gameDetails.ReleaseDate) 
            ? null 
            : DateTime.Parse(_gameDetails.ReleaseDate);

        public override Review[] Reviews => Array.Empty<Review>();

        public override Studio? Studio => null;

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _gameDetails.Title;        
    }
}