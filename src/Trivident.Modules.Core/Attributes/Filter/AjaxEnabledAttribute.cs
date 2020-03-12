using System.Web.Mvc;
using System;

namespace Trivident.Modules.Core.Attributes.Filter
{
    /// <summary>
    /// Enables controller actions to be 'ajax enabled' - meaning that the /{localization}/ajax/{controller}/{action}/{view} route can be invoked without errors
    /// </summary>
    public class AjaxEnabledAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Adds appropriate area route data so views can be located
        /// </summary>
        /// <param name="filterContext">The filter context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.RouteData.DataTokens.ContainsKey("area"))
            {
                string area = null;
                var routeValues = filterContext.RouteData.Values;
                //Look for a specific area in the route
                if (routeValues.ContainsKey("area"))
                {
                    area = routeValues["area"].ToString();
                }
                //Fallback - default is to take the area name = controller name
                else if (routeValues.ContainsKey("controller"))
                {
                    area = routeValues["controller"].ToString();
                }
                if (!string.IsNullOrEmpty(area))
                {
                    filterContext.RouteData.DataTokens.Add("area", area);
                }
                else
                {
                    throw new Exception("Cannot determine area from route data, ensure that either an area or controller segment is present in the URL");
                }
            }
        }
    }
}
