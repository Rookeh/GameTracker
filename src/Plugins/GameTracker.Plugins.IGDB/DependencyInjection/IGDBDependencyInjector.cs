using GameTracker.Interfaces.Plugins;
using GameTracker.Plugins.IGDB.Factories;
using GameTracker.Plugins.IGDB.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Plugins.IGDB.DependencyInjection
{
    public class IGDBDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IIGDBClientWrapperFactory, IGDBClientWrapperFactory>();
        }
    }
}