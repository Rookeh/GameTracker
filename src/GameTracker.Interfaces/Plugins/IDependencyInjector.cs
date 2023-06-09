using Microsoft.Extensions.DependencyInjection;

namespace GameTracker.Interfaces.Plugins
{
    public interface IDependencyInjector
    {
        void InjectDependencies(IServiceCollection services);
    }
}