using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Nintendo.Helpers;
using GameTracker.Plugins.Nintendo.Models.EU;

namespace GameTracker.Plugins.Nintendo.Models
{
    public class EUNintendoGame : Game
    {
        private EUDocument _gameDocument;
        
        public EUNintendoGame(EUDocument gameDocument)
        {
            _gameDocument = gameDocument;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.Controller } ;

        public override string Description => _gameDocument.ProductCatalogDescription;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => EUGameHelpers.GenresFromGameCategories(_gameDocument.GameCategories);

        public override Image Image => EUGameHelpers.ImageFromImageUrl(_gameDocument.ImageUrlH2X1);

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => EUGameHelpers.LaunchCommandFromUrl(_gameDocument.Url);

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platform[] Platforms => new[] { Constants.Platforms.NintendoSwitch };

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => null;

        public override DateTime? ReleaseDate => _gameDocument.DatesReleased.Min();

        public override Review[] Reviews => Array.Empty<Review>();

        public override Studio? Studio => EUGameHelpers.StudioFromCopyright(_gameDocument.Copyright);

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _gameDocument.Title;

        public override Task Preload()
        {
            return Task.CompletedTask;
        }
    }
}