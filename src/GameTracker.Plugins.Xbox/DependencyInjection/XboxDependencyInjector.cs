using GameTracker.Interfaces.Plugins;
using GameTracker.Plugins.Xbox.Data;
using GameTracker.Plugins.Xbox.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Plugins.Xbox.DependencyInjection
{
    public class XboxDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IRateLimitingConfig, RateLimitingConfig>();
            services.AddScoped<IRateLimitedXboxHttpClient, RateLimitedXboxHttpClient>();
        }
    }
}