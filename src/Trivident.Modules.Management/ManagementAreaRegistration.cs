using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Configuration;
using Trivident.Modules.Management.Providers;
using Trivident.Modules.Management.Contracts;

namespace Trivident.Modules.Management
{
	/// <summary>
	/// Registration of Navigation Area on the core..
	/// </summary>
	public class ManagementAreaRegistration : BaseModuleAreaRegistration
	{
		/// <summary>
		/// 
		/// </summary>
		public override string AreaName => "Management";

		/// <inheritdoc />
		public override void RegisterTypes(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton(typeof(ICacheProvider), typeof(MemoryCacheProvider));           
		}

        public override void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
               AreaName + "_Javascript",
               "admin/debug.js",
               new { controller = "Debug", action = "Javascript" });
            context.MapRoute(
                AreaName + "_CacheList",
                "admin/cache",
                new { controller = "Cache", action = "List" });
            context.MapRoute(
                AreaName + "_CacheItem",
                "admin/cache/item/{key}",
                new { controller = "Cache", action = "Item" });
            context.MapRoute(
                AreaName + "_CacheRemove",
                "admin/cache/remove/{key}",
                new { controller = "Cache", action = "Remove" });
            context.MapRoute(
               AreaName + "_CacheRemoveAll",
               "admin/cache/removeall",
               new { controller = "Cache", action = "RemoveAll" });

            base.RegisterRoutes(context);
        }
    }
}