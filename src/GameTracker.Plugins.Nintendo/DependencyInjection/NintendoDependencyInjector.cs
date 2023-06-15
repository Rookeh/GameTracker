using GameTracker.Interfaces.Plugins;
using GameTracker.Plugins.Common.Factories;
using GameTracker.Plugins.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Plugins.Nintendo.DependencyInjection
{
    public class NintendoDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IHttpClientWrapperFactory, HttpClientWrapperFactory>();
        }
    }
}