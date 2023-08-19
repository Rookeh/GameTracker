using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Nintendo.Helpers;
using GameTracker.Plugins.Nintendo.Models.EU;

namespace GameTracker.Plugins.Nintendo.Models
{
    public class EUNintendoGame : Game
    {
        private readonly LaunchCommand _launchCommand;
        private readonly string _title;

        public EUNintendoGame(EUDocument gameDocument)
        {
            Description = gameDocument.ProductCatalogDescription;
            GameplayModes = Array.Empty<GameplayMode>();
            Genres = EUGameHelpers.GenresFromGameCategories(gameDocument.GameCategories);
            Image = EUGameHelpers.ImageFromImageUrl(gameDocument.ImageUrlH2X1);
            ReleaseDate = gameDocument.DatesReleased.Min();
            Studio = EUGameHelpers.StudioFromCopyright(gameDocument.Copyright);

            _launchCommand = EUGameHelpers.LaunchCommandFromUrl(gameDocument.Url);
            _title = gameDocument.Title;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.Controller } ;

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => _launchCommand;

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platforms Platforms => Platforms.NintendoSwitch;

        public override TimeSpan? Playtime => null;

        public override string ProviderName => "Nintendo eShop";

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

        public override Task Preload()
        {
            return Task.CompletedTask;
        }
    }
}