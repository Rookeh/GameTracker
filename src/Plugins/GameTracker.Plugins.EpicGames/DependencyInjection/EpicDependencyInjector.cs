using GameTracker.Interfaces.Plugins;
using GameTracker.Plugins.EpicGames.Interfaces;
using GameTracker.Plugins.EpicGames.Wrappers;
using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Plugins.EpicGames.DependencyInjection
{
    public class EpicDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IEpicGamesStore, EpicGamesStoreWrapper>();
        }
    }
}