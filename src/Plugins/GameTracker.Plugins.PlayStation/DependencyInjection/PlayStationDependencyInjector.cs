using GameTracker.Interfaces.Plugins;
using GameTracker.Plugins.Common.Factories;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.PlayStation.Helpers;
using GameTracker.Plugins.PlayStation.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Plugins.PlayStation.DependencyInjection
{
    public class PlayStationDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IHttpClientWrapperFactory, HttpClientWrapperFactory>();
            services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();
        }
    }
}