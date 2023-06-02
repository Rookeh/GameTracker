using GameTracker.Plugins.GOG.Models;
using GameTracker.Plugins.GOG.Models.GOGApi;

namespace GameTracker.Plugins.GOG.Helpers
{
    internal class Constants
    {
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