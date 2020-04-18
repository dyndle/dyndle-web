using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Dyndle.Modules.Navigation.Routing
{
    public class SitemapEnabledConstraint : IRouteConstraint
    {
        private bool _sitemapEnabled;
        public SitemapEnabledConstraint()
        {
            string setting = ConfigurationManager.AppSettings[NavigationConstants.Settings.SitemapEnabled];
            _sitemapEnabled = !string.IsNullOrEmpty(setting) && Convert.ToBoolean(setting);
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return _sitemapEnabled;
        }

    }
}