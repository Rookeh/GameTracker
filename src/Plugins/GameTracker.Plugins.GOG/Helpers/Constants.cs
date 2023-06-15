
using GameTracker.Plugins.GOG.Models.GOGApi;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("GameTracker.Plugins.GOG.Tests")]
namespace GameTracker.Plugins.GOG.Helpers
{
    internal class Constants
    {
        internal class Authentication
        {
            internal static string ApiDocumentation => "https://gogapidocs.readthedocs.io/en/latest/auth.html";
            internal static string AuthenticationBaseUrl => "https://auth.gog.com";
            internal static string AuthenticationCodeUrl => "https://auth.gog.com/auth?client_id=46899977096215655&redirect_uri=https%3A%2F%2Fembed.gog.com%2Fon_login_success%3Forigin%3Dclient&response_type=code&layout=client2";
        }

        internal class Requests
        {
            internal const string GameDetailsRequestUrlFormat = "https://api.gog.com/products?ids={0}?expand=downloads,description,changelog";
            internal const string OwnedGamesRequestUrl = "https://embed.gog.com/user/data/games";
        }

        internal static GameDetails DefaultGameDetails => new GameDetails
        {
            IsDefaultValue = true,
            Changelog = "Pending...",
            ContentSystemCompatibility = new ContentSystemCompatibility
            {
                Linux = false,
                Windows = false,
                OSX = false,
            },
            Description = new Description
            {
                Full = "Pending...",
                Lead = "Pending...",
                WhatsCoolAboutIt = "Pending..."
            },
            Downloads = new Downloads
            {
                BonusContent = Array.Empty<BonusContent>(),
                Installers = Array.Empty<Installer>(),
                LanguagePacks = Array.Empty<object>(),
                Patches = Array.Empty<object>()
            },
            GameType = "Pending...",
            Id = 0,
            Images = new Images
            {
                Background = "img\\placeholder.png"
            },
            InDevelopment = new InDevelopment
            {
                Active = false
            },
            IsInstallable = false,
            IsPreOrder = false,
            IsSecret = false,
            Links = new Links
            {
                Forum = "https://www.gog.com/forum",
                Support = "https://support.gog.com"
            },
            ReleaseDate = DateTime.MaxValue.ToString(),
            PurchaseLink = "https://gog.com",
            Slug = "Pending...",
            Title = "Pending..."
        };
    }
}