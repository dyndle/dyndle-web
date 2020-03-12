using System;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Interfaces;

namespace Trivident.Modules.Core.Extensions
{
	/// <summary>
	/// Register Modules
	/// </summary>
	public static class RegisterModules
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceCollection"></param>
		/// <typeparam name="T"></typeparam>
		/// <exception cref="ArgumentNullException"></exception>
		public static void RegisterModule<T>(this IServiceCollection serviceCollection) where T : IServiceCollectionModule, new()
		{
			if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
			IServiceCollectionModule module = Activator.CreateInstance<T>();

			module.RegisterTypes(serviceCollection);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceCollection"></param>
		/// <param name="module"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void RegisterModule(this IServiceCollection serviceCollection, IServiceCollectionModule module)
		{
			if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

			module.RegisterTypes(serviceCollection);
		}
	}
}
