using System;
using Dyndle.Modules.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Extensions
{
    /// <summary>
    /// Register Modules
    /// </summary>
    public static class RegisterModules
	{
        /// <summary>
        /// Registers the module.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection">The service collection.</param>
        /// <exception cref="ArgumentNullException">serviceCollection</exception>
        public static void RegisterModule<T>(this IServiceCollection serviceCollection) where T : IServiceCollectionModule, new()
		{
			if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
			IServiceCollectionModule module = Activator.CreateInstance<T>();

			module.RegisterTypes(serviceCollection);
		}

        /// <summary>
        /// Registers the module.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="module">The module.</param>
        /// <exception cref="ArgumentNullException">serviceCollection</exception>
        public static void RegisterModule(this IServiceCollection serviceCollection, IServiceCollectionModule module)
		{
			if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

			module.RegisterTypes(serviceCollection);
		}
	}
}
