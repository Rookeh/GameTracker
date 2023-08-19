using GameTracker.Interfaces;
using GameTracker.Interfaces.Plugins;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GameTracker.Core
{
    public class DataProviderFactory : IDataProviderFactory
    {
        private List<IDataProvider> _cachedProviders;

        public DataProviderFactory()

        {
            _cachedProviders = new List<IDataProvider>();
        }

        public IEnumerable<IDataProvider> GetAllProviders()
        {
            try
            {
                if (!_cachedProviders.Any())
                {
                    _cachedProviders = ActivateProviderTypesFromPlugins().ToList();
                }

                return _cachedProviders;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Enumerable.Empty<IDataProvider>();
            }
        }

        public IEnumerable<IGameProvider> GetGameProviders()
        {
            try
            {
                if (!_cachedProviders.Any())
                {
                    _cachedProviders = ActivateProviderTypesFromPlugins().ToList();
                }

                return _cachedProviders.Where(p => p is IGameProvider).Select(p => (IGameProvider)p);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Enumerable.Empty<IGameProvider>();
            }
        }

        public IEnumerable<IReviewProvider> GetReviewProviders()
        {
            try
            {
                if (!_cachedProviders.Any())
                {
                    _cachedProviders = ActivateProviderTypesFromPlugins().ToList();
                }

                return _cachedProviders.Where(p => p is IReviewProvider).Select(p => (IReviewProvider)p);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Enumerable.Empty<IReviewProvider>();
            }
        }

        private IEnumerable<IDataProvider?> ActivateProviderTypesFromPlugins()
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
                foreach (var pluginDependencyInjector in pluginDependencyInjectors)
                {
                    var injectorInstance = Activator.CreateInstance(pluginDependencyInjector) as IDependencyInjector;
                    injectorInstance.InjectDependencies(pluginServiceCollection);
                }

                var pluginDataProviders = pluginAssembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(IDataProvider)));
                foreach (var pluginDataProvider in pluginDataProviders)
                {
                    if (pluginDataProvider.GetConstructors().Any(c => c.GetParameters().Any()))
                    {
                        var pluginServiceProvider = pluginServiceCollection.BuildServiceProvider();
                        yield return ActivatorUtilities.CreateInstance(pluginServiceProvider, pluginDataProvider) as IDataProvider;
                    }
                    else
                    {
                        yield return Activator.CreateInstance(pluginDataProvider) as IDataProvider;
                    }
                }
            }
        }
    }
}