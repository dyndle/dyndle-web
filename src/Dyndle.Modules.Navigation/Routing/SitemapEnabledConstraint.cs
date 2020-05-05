using System;
using System.Configuration;
using System.Web;
using System.Web.Routing;

namespace Dyndle.Modules.Navigation.Routing
{
    /// <summary>
    /// Sitemap enabled constraint.
    /// </summary>
    public class SitemapEnabledConstraint : IRouteConstraint
    {
        private readonly bool _sitemapEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapEnabledConstraint" /> class.
        /// </summary>
        public SitemapEnabledConstraint()
        {
            string setting = ConfigurationManager.AppSettings[NavigationConstants.Settings.SitemapEnabled];
            _sitemapEnabled = !string.IsNullOrEmpty(setting) && Convert.ToBoolean(setting);
        }

        /// <summary>
        /// Method to determine enabling of sitemap.
        /// </summary>
        /// <param name="httpContext">The http context</param>
        /// <param name="route">The route</param>
        /// <param name="parameterName">The paramter name</param>
        /// <param name="values">The values</param>
        /// <param name="routeDirection">The routedirection</param>
        /// <returns>Returns boolean indicating enabled sitemap.</returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return _sitemapEnabled;
        }

    }
}