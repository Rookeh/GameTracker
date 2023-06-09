using GameTracker.Interfaces.Plugins;
using GameTracker.Plugins.Steam.Data;
using GameTracker.Plugins.Steam.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Plugins.Steam.DependencyInjection
{
    public class SteamDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<ISteamGameDetailsRepository, SteamGameDetailsRepository>();
        }
    }
}