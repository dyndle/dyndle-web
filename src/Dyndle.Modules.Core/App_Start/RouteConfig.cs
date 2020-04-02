using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DD4T.ContentModel.Contracts.Configuration;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core
{
    /// <summary>
    /// Class RouteConfig.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Regex pattern used as a default binary url pattern when it is not set in the web config.
        /// </summary>
        public const string DEFAULT_BINARY_URL_PATTERN = "^.*\\.(gif|jpg|jpeg|png|bmp|txt|doc|docx|xls|xlsx|ppt|pptx|pdf|mpg|mov|wav|mp3)$";
        
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Some URLs should not be handled by any Controller:
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.RouteExistingFiles = false;

            //Register attributes's before catch routes for area's
            routes.MapMvcAttributeRoutes();

            //Register area's before catch all route for pagecontroller
            // AreaRegistration.RegisterAllAreas(); // note: this should be done from the global.asax by the customer's web app

            // XPM blank page
            routes.MapRoute(
                "Core_Blank",
                "se_blank.html",
                new { controller = "Page", action = "Blank" }
            ).DataTokens.Add("area", "Core");

            // For resolving ids to urls
            routes.MapRoute(
               "Core_Resolve",
               "resolve/{*itemId}",
               new { controller = "Page", action = "Resolve" },
               new { itemId = @"^(.*)?$" }
            ).DataTokens.Add("area", "Core");
            routes.MapRoute(
               "Core_Resolve_Loc",
               "{localization}/resolve/{*itemId}",
               new { controller = "Page", action = "Resolve" },
               new { itemId = @"^(.*)?$" }
            ).DataTokens.Add("area", "Core");

            routes.MapRoute(
                "CME_Preview_publication",
                "{publication}/cme/preview",
                new { controller = "Page", action = "Preview" }
            ).DataTokens.Add("area", "Core");

            routes.MapRoute(
                "CME_Preview",
                "cme/preview",
                new { controller = "Page", action = "Preview" }
            ).DataTokens.Add("area", "Core");

        

            // route for binaries, based on a configurable pattern (see Dyndle.BinaryUrlPattern in Web.config)
            routes.MapRoute(
                "Core_Binaries",
                "{*url}",
                new { controller = "Binary", action = "Binary" },
                new { url = String.IsNullOrEmpty(DyndleConfig.BinaryUrlPattern) ? DEFAULT_BINARY_URL_PATTERN : DyndleConfig.BinaryUrlPattern });
            // TODO: find out why this route does not explicitly refer to the Core area

            // Tridion Page Route
            routes.MapRoute(
               "Core_PageModel",
               "{*page}",
               new { controller = PageControllerName, action = PageActionName },
               new { page = new RedirectInvalidPageConstraint() }
            ).DataTokens.Add("area", "Core");

            //// special route for http exception handling
            routes.MapRoute(
                "Core_HttpException",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Page", action = "HttpException" }
            ).DataTokens.Add("area", "Core");

            // look for a Default route like VS creates for new projects
            // we must move that to the bottom, otherwise the page and binary controllers won't be hit

            Route defaultRoute = null;
            var allRoutes = RouteTable.Routes.OfType<Route>().ToList();
            for (int i = 0; i < allRoutes.Count; i++)
            {
                var route = allRoutes[i];
                if (route.Url == "{controller}/{action}/{id}")
                {
                    defaultRoute = route;
                    RouteTable.Routes.Remove(route);
                }
            }

            if (defaultRoute != null)
            {
                RouteTable.Routes.Add(defaultRoute);
            }
            else
            {
                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
            }

        }

        private static string PageControllerName
        {
            get
            {
                return ConfigurationManager.AppSettings["DD4T.PageController"] ?? "Page";
            }
        }

        private static string PageActionName
        {
            get
            {
                return ConfigurationManager.AppSettings["DD4T.PageAction"] ?? "Page";
            }
        }
    }

    /// <summary>
    /// Class RedirectInvalidPageConstraint.
    /// Determines whether the URL parameter contains a valid value for this constraint.
    /// When the url does not match the clean format, the request is redirected instantly.
    /// </summary>
    /// <seealso cref="System.Web.Routing.IRouteConstraint" />
    public class RedirectInvalidPageConstraint : IRouteConstraint
    {
        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The object that this constraint belongs to.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        /// <param name="values">An object that contains the parameters for the URL.</param>
        /// <param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        /// <returns>true if the URL parameter contains a valid value; otherwise, false.</returns>
        public bool Match
            (
                HttpContextBase httpContext,
                Route route,
                string parameterName,
                RouteValueDictionary values,
                RouteDirection routeDirection
            )
        {
            if (routeDirection != RouteDirection.IncomingRequest)
            {
                return false;
            }

            var lastError = HttpContext.Current.Server.GetLastError();
            if (lastError != null)
            {
                return false;
            }

            var incomingPageUri = httpContext.Request.Url;
            var incomingPageLink = incomingPageUri.AbsolutePath;

            if (incomingPageLink == null)
            {
                return true;
            }
            var configuration = DependencyResolver.Current.GetService(typeof(IDD4TConfiguration)) as IDD4TConfiguration;

            var pageLink = incomingPageLink.CleanUrl(configuration.WelcomeFile);

            if (pageLink.Equals(incomingPageLink, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(incomingPageUri.Query))
                pageLink = string.Format("{0}/{1}", pageLink, incomingPageUri.Query);

            httpContext.Response.RedirectPermanent(pageLink);
            return false;
        }
    }
}