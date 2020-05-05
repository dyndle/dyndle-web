using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Navigation.Providers;
using Dyndle.Modules.Navigation.Routing;
using Dyndle.Modules.Navigation.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Mvc;

namespace Dyndle.Modules.Navigation
{
    /// <summary>
    /// Registration of Navigation Area on the core..
    /// </summary>
    public class NavigationAreaRegistration : BaseModuleAreaRegistration
    {
        /// <summary>
        /// Gets the name of the area.
        /// </summary>
        /// <value>The name of the area.</value>
        public override string AreaName => "Navigation";

        /// <inheritdoc />
        public override void RegisterTypes(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(INavigationProvider), typeof(NavigationProvider));
            serviceCollection.AddSingleton(typeof(INavigationService), typeof(NavigationService));
        }

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="context">The Arearegistration context</param>
        public override void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                   AreaName + "_Sitemap",
                   "sitemap.xml",
                   new { controller = "Sitemap", action = "Sitemap" },
                    new { enabled = new SitemapEnabledConstraint() });
            context.MapRoute(
                   AreaName + "_SitemapLevel1",
                   "{publication}/sitemap.xml",
                   new { controller = "Sitemap", action = "Sitemap" },
                    new { enabled = new SitemapEnabledConstraint() });
            context.MapRoute(
                   AreaName + "_SitemapLevel2",
                   "{publication}/{publevel2}/sitemap.xml",
                   new { controller = "Sitemap", action = "Sitemap" },
                    new { enabled = new SitemapEnabledConstraint() });
        }

    }
}