using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Configuration;
using Trivident.Modules.Navigation.Binders;
using Trivident.Modules.Navigation.Providers;

namespace Trivident.Modules.Navigation
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
			serviceCollection.AddSingleton(typeof(IModelBinderProvider), typeof(SitemapItemModelBinder));
		}
	}
}