using GameTracker.Interfaces.Plugins;
using GameTracker.Plugins.Common.Factories;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.Steam.ApiClients;
using GameTracker.Plugins.Steam.Data;
using GameTracker.Plugins.Steam.Interfaces.ApiClients;
using GameTracker.Plugins.Steam.Interfaces.Data;
using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Plugins.Steam.DependencyInjection
{
    public class SteamDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IHttpClientWrapperFactory, HttpClientWrapperFactory>();
            services.AddSingleton<IRateLimitedSteamApiClient, RateLimitedSteamApiClient>();
            services.AddSingleton<ICategoryMappingRepository, CategoryMappingRepository>();
            services.AddSingleton<IDeveloperMappingRepository, DeveloperMappingRepository>();
            services.AddSingleton<IGenreMappingRepository, GenreMappingRepository>();
            services.AddScoped<IPublisherMappingRepository, PublisherMappingRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IDeveloperRepository, DeveloperRepository>();
            services.AddSingleton<IGenreRepository, GenreRepository>();
            services.AddSingleton<IPublisherRepository, PublisherRepository>();
            services.AddSingleton<IMetacriticScoreRepository, MetacriticScoreRepository>();
            services.AddSingleton<IPlatformsRepository, PlatformsRepository>();
            services.AddSingleton<IReleaseDateRepository, ReleaseDateRepository>();
            services.AddSingleton<ISteamGameDetailsRepository, SteamGameDetailsRepository>();
        }
    }
}