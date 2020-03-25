using System.Web.Mvc;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Navigation.Providers;
using Dyndle.Modules.Navigation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Navigation
{
	/// <summary>
	/// Registration of Navigation Area on the core..
	/// </summary>
	public class NavigationAreaRegistration : BaseModuleAreaRegistration
	{
		/// <summary>
		/// 
		/// </summary>
		public override string AreaName => "Navigation";

		/// <inheritdoc />
		public override void RegisterTypes(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton(typeof(INavigationProvider), typeof(NavigationProvider));
			serviceCollection.AddSingleton(typeof(INavigationService), typeof(NavigationService));
		}
	}
}