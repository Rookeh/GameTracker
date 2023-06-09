using GameTracker.Interfaces;
using GameTracker.Interfaces.Plugins;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GameTracker.Core.Factories
{
    public class ProviderFactory : IProviderFactory
    {        
        private List<IGameProvider> _cachedProviders;

        public ProviderFactory()
        {
            _cachedProviders = new List<IGameProvider>();
        }

        public IEnumerable<IGameProvider> GetProviders()
        {
            try
            {
                if (_cachedProviders.Any())
                {
                    return _cachedProviders;
                }
                else
                {
                    return _cachedProviders = ActivateProviderTypesFromPlugins().ToList();
                }
            }
            catch (Exception)
            {
                return Enumerable.Empty<IGameProvider>();
            }
        }

        private IEnumerable<IGameProvider?> ActivateProviderTypesFromPlugins()
        {
            var enumerationOptions = new EnumerationOptions();
            enumerationOptions.RecurseSubdirectories = true;
            enumerationOptions.MaxRecursionDepth = 5;
            var pluginAssemblyPaths = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "GameTracker.Plugins.*.dll", enumerationOptions);

            foreach (var pluginAssemblyPath in pluginAssemblyPaths)
            {              
                var pluginAssembly = Assembly.LoadFrom(pluginAssemblyPath);
                var pluginServiceCollection = new ServiceCollection();

                var pluginDependencyInjectors = pluginAssembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(IDependencyInjector)));
                foreach(var pluginDependencyInjector in pluginDependencyInjectors)
                {
                    var injectorInstance = Activator.CreateInstance(pluginDependencyInjector) as IDependencyInjector;
                    injectorInstance.InjectDependencies(pluginServiceCollection);
                }

                var pluginGameProviders = pluginAssembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(IGameProvider)));
                foreach (var pluginGameProvider in pluginGameProviders)
                {
                    if (pluginGameProvider.GetConstructors().Any(c => c.GetParameters().Any()))
                    {
                        var pluginServiceProvider = pluginServiceCollection.BuildServiceProvider();
                        yield return ActivatorUtilities.CreateInstance(pluginServiceProvider, pluginGameProvider) as IGameProvider;
                    }
                    else
                    {
                        yield return Activator.CreateInstance(pluginGameProvider) as IGameProvider;
                    }
                }
            }
        }
    }
}