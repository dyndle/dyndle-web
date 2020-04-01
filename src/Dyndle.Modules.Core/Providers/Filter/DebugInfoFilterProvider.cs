using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyndle.Modules.Core.Attributes.Filter;
using Dyndle.Modules.Core.Controllers;

namespace Dyndle.Modules.Core.Providers.Filter
{
    /// <summary>
    /// Class DebugInfoFilterProvider.
    /// Provides instances of type <see cref="DebugInfoAttribute"/> 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.IFilterProvider" />
    public class DebugInfoFilterProvider : IFilterProvider
    {
        /// <summary>
        /// Returns an enumerator that contains all the <see cref="T:System.Web.Mvc.IFilterProvider" /> instances in the service locator.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>The enumerator that contains all the <see cref="T:System.Web.Mvc.IFilterProvider" /> instances in the service locator.</returns>
        public IEnumerable<System.Web.Mvc.Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {

            if (ShouldFilter(controllerContext))
            {
                return new[] { new System.Web.Mvc.Filter(new DebugInfoAttribute(DebugInfoNames), FilterScope.Global, 0) };
            }
            else
            {
                return Enumerable.Empty<System.Web.Mvc.Filter>();
            }
        }

        /// <summary>
        /// Should we filter on this request?.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if request is local and contains debug=true, <c>false</c> otherwise.</returns>
        private static bool ShouldFilter(ControllerContext context)
        {
            // only set the cookie if we are dealing with the page controller or the entity controller 
            if (!(typeof(PageController).IsAssignableFrom(context.Controller.GetType()) || typeof(EntityController).IsAssignableFrom(context.Controller.GetType())))
            {
                return false;
            }

            // check if the user has typed 'debug=true / this is not needed if they clicked on one of the buttons, in that case the cookie is set by javascript
            if ((!context.IsChildAction) && context.HttpContext.Request.QueryString["debug"] != null) // RouteData.Values["model"] == null 
            {
                try
                {
                    if (context.HttpContext.Request.QueryString["debug"].ToLower() == "false")
                    {
                        context.HttpContext.Response.Cookies.Add(new HttpCookie("debuginfo", ""));
                    }
                    else
                    {
                        context.HttpContext.Response.Cookies.Add(new HttpCookie("debuginfo", "/performance/general"));
                    }
                    context.HttpContext.Response.Redirect(context.HttpContext.Request.Url.AbsolutePath);
                }
                catch (HttpException)
                {
                    // fail silently, this probably means that we are trying to write headers after the first byte of the body has been written
                    // since this is non-essential functionality, we can simply continue
                }
            }

            return IsDebugMode(context.HttpContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsDebugMode(HttpContextBase context)
        {
            return GetCookieByName("debuginfo").Length > 0 &&
                        !context.Response.IsRequestBeingRedirected;
        }

        private static string GetCookieByName(string name)
        {
            var rawValue = HttpContext.Current.Request.Cookies.Get(name)?.Value;
            if (rawValue == null)
            {
                return string.Empty;
            }
            return HttpContext.Current.Server.UrlDecode(rawValue);
        }

        private static IEnumerable<string> DebugInfoNames
        {
            get
            {
                var debuginfo = GetCookieByName("debuginfo");
                if (! string.IsNullOrEmpty(debuginfo))
                {
                    return debuginfo.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                }
                return new List<string>();
            }
        }
    }
}
