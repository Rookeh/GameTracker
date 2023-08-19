using EpicGamesStoreNET.Models;
using GameTracker.Models;
using GameTracker.Models.Enums;
using System.Globalization;

namespace GameTracker.Plugins.EpicGames.Models
{
    public class EpicGame : Game
    {
        private readonly string _title;
        private readonly string _namespace;
        private readonly string _urlSlug;

        internal EpicGame(Element element)
        {
            var hexBytes = Convert.FromHexString(element.Id);
            PlatformId = BitConverter.ToInt32(hexBytes);
            Description = element.Description;
            GameplayModes = Array.Empty<GameplayMode>();
            Genres = Array.Empty<Genre>();
            Image = new Image
            {
                Url = element.KeyImages?.FirstOrDefault(i => i.Type == "OfferImageWide")?.Url ?? "img//placeholder.png",
                Width = 460,
                Height = 259
            };
            Publisher = element.Seller.Name;
            ReleaseDate = element.EffectiveDate;
            Studio = element.CustomAttributes.FirstOrDefault(ca => ca.Key == "developerName")?.Value ?? null;

            _namespace = element.Namespace;
            _title = element.Title;
            _urlSlug = element.UrlSlug;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => Array.Empty<ControlScheme>();

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => GetLaunchCommand();

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platforms Platforms => Platforms.Windows;

        public override TimeSpan? Playtime => null;

        public override string ProviderName => "Epic Games Store";

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

        #region Private methods

        private LaunchCommand GetLaunchCommand()
        {
            bool isLaunchable = !Guid.TryParse(_namespace, out var _);            

            if (isLaunchable)
            {
                var textInfo = CultureInfo.InvariantCulture.TextInfo;
                var nameSpace = textInfo.ToTitleCase(_namespace);

                return new LaunchCommand
                {
                    Icon = "Controller",
                    NewTab = false,
                    Text = "Launch via Epic Games Launcher",
                    Url = $"com.epicgames.launcher://apps/{nameSpace}?action=launch&silent=true"
                };
            }

            var locale = CultureInfo.CurrentCulture.Name.ToLower();

            return new LaunchCommand
            {
                Icon = "Bag",
                NewTab = true,
                Text = "Open in Epic Games Store",
                Url = $"https://store.epicgames.com/{locale}/p/{_urlSlug}"
            };
        }

        #endregion
    }
}