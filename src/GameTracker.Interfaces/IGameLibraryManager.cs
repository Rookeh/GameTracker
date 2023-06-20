﻿using GameTracker.Models;

namespace GameTracker.Interfaces
{
    public interface IGameLibraryManager
    {
        IEnumerable<Game> Games { get; }
        IEnumerable<IGrouping<string, Game>> GamesGroupedByTitle { get; }
        Task InitialiseProviders(string userId);
        Task RefreshProvider(string userId, Guid providerId, params object[] parameters);
    }
}