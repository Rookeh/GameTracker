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
            PlatformId = 0;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.KeyboardMouse };

        public override string Description => "Dummy Description";

        public override GameplayMode[] GameplayModes => new[] { GameplayMode.Singleplayer };

        public override Genre[] Genres => new[] { Genre.Other };

        public override Image Image => new Image
        {
            Url = "https://upload.wikimedia.org/wikipedia/commons/2/21/Hello_World_Brian_Kernighan_1978.jpg",
            Width = 460,
            Height = 215
        };

        public override DateTime? LastPlayed => DateTime.UnixEpoch;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            NewTab = true,
            Icon = "Egg",
            Text = "Hello, world",
            Url = "https://en.wikipedia.org/wiki/%22Hello,_World!%22_program"
        };     

        public override MultiplayerAvailability[] MultiplayerAvailability => new[] { Models.Enums.MultiplayerAvailability.None };        

        public override Platform[] Platforms => new[] { new Platform { Name = "Dummy Platform", Icon = "EggFried" } } ;

        public override TimeSpan? Playtime => TimeSpan.Zero;

        public override Publisher Publisher => new Publisher { Name = "Dummy Publisher" };

        public override DateTime? ReleaseDate => DateTime.Today;

        public override Review[] Reviews => new[] { new Review { Score = new Random().Next(11), Critic = new Critic { Name = "Dummy Critic", UpperBound = 10 } } } ;

        public override string ProviderName => "Dummy";

        public override Studio Studio => new Studio { Name = "Dummy Studio" };

        public override string[] Tags => new[] { "test", "hello", "world" } ;        

        public override string Title => _title;
    }
}