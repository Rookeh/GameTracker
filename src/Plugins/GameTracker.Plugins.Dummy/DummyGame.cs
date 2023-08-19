using GameTracker.Models;
using GameTracker.Models.Enums;

namespace GameTracker.Plugins.Dummy
{
    public class DummyGame : Game
    {
        private readonly string _title;

        internal DummyGame()
        {
            _title = "Dummy Title";
            Description = "Dummy Description";
            GameplayModes = new[] { GameplayMode.Singleplayer };
            Genres = new[] { Genre.Other };
            Image = new Image
            {
                Url = "https://upload.wikimedia.org/wikipedia/commons/2/21/Hello_World_Brian_Kernighan_1978.jpg",
                Width = 460,
                Height = 215
            };
            PlatformId = 0;
            Publisher = "Dummy Publisher";
            Reviews = new List<Review> { new Review { Score = new Random().Next(11), Critic = "Dummy Critic", UpperBound = 10 } };
            Studio = "Dummy Studio";
            ReleaseDate = DateTime.Today;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.KeyboardMouse };

        public override DateTime? LastPlayed => DateTime.UnixEpoch;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            NewTab = true,
            Icon = "Egg",
            Text = "Hello, world",
            Url = "https://en.wikipedia.org/wiki/%22Hello,_World!%22_program"
        };     

        public override MultiplayerAvailability[] MultiplayerAvailability => new[] { Models.Enums.MultiplayerAvailability.None };        

        public override Platforms Platforms => Platforms.Windows;

        public override TimeSpan? Playtime => TimeSpan.Zero;

        public override string ProviderName => "Dummy";

        public override string[] Tags => new[] { "test", "hello", "world" } ;        

        public override string Title => _title;
    }
}