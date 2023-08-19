using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.GOG.Helpers;
using GameTracker.Plugins.GOG.Models.GOGApi;

namespace GameTracker.Plugins.GOG.Models
{
    public class GOGGame : Game
    {
        private readonly Platforms _platforms;
        private readonly string _title;

        internal GOGGame(GameDetails gameDetails)
        {
            PlatformId = gameDetails.Id;
            Description = gameDetails.Description?.Full ?? string.Empty;
            GameplayModes = Array.Empty<GameplayMode>();
            Genres = Array.Empty<Genre>();
            Image = new Image
            {
                Url = gameDetails.Images?.Logo2x ?? "img\\placeholder.png",
                Width = 200,
                Height = 120
            };
            _platforms = gameDetails.ContentSystemCompatibility.FromContentSystemCompatibility();
            ReleaseDate = string.IsNullOrEmpty(gameDetails.ReleaseDate)
                ? null
                : DateTime.Parse(gameDetails.ReleaseDate);
            _title = gameDetails.Title;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => Array.Empty<ControlScheme>() ;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            NewTab = false,
            Icon = "PcDisplay",
            Text = "Launch via GOG Galaxy",
            Url = $"goggalaxy://openGameView/{PlatformId}"
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();        

        public override DateTime? LastPlayed => null;

        public override Platforms Platforms => _platforms;

        public override TimeSpan? Playtime => null;

        public override string ProviderName => "GOG";

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;
    }
}