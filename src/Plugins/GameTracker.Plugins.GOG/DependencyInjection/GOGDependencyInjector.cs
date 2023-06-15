using GameTracker.Interfaces.Plugins;
using GameTracker.Plugins.Common.Factories;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.GOG.Helpers;
using GameTracker.Plugins.GOG.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Plugins.GOG.DependencyInjection
{
    public class GOGDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IHttpClientWrapperFactory, HttpClientWrapperFactory>();
            services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();
        }
    }
}