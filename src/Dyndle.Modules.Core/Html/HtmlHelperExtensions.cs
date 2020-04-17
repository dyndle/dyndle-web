using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Controllers;
using Dyndle.Modules.Core.Environment;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Filter;

namespace Dyndle.Modules.Core.Html
{
    /// <summary>
    /// Class HtmlHelperExtensions.
    /// 
    /// </summary>
    public static class HtmlHelperExtensions
    {
        #region Render methods

        /// <summary>
        /// Renders the entities in the collection using the route data provided by Tridion.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString RenderEntities(this HtmlHelper htmlHelper, IEnumerable<IEntityModel> entities)
        {
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var item in entities)
            {
                resultBuilder.Append(htmlHelper.RenderEntity(item));
            }
            return MvcHtmlString.Create(resultBuilder.ToString());
        }

        /// <summary>
        /// Renders the regions using the route data provided by Tridion.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="regions">The regions.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString RenderRegions(this HtmlHelper htmlHelper, IEnumerable<IRegionModel> regions)
        {
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var region in regions)
            {
                resultBuilder.Append(htmlHelper.RenderRegion(region));
            }
            return MvcHtmlString.Create(resultBuilder.ToString());
        }

        /// <summary>
        /// Renders the region using the route data provided by Tridion.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="region">The region.</param>
        /// <param name="regionView">The view name.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString RenderRegion(this HtmlHelper htmlHelper, IRegionModel region, string regionView = "")
        {
            if (region == null)
            {
                return MvcHtmlString.Empty;
            }
            var viewData = new ViewDataDictionary(htmlHelper.ViewData);
            if (region.RouteValues != null)
            {
                foreach (string key in region.RouteValues.Keys)
                {
                    viewData[key] = region.RouteValues[key];
                }
            }
            return htmlHelper.Action("Region", "Region", new { Region = region, RegionView = regionView });
        }

        /// <summary>
        /// Renders the entity using the route data provided by Tridion.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>MvcHtmlString.</returns>
        /// <exception cref="ArgumentNullException">When there is no route data available on the model</exception>
        public static MvcHtmlString RenderEntity(this HtmlHelper htmlHelper, IEntityModel entity)
        {
            if (entity == null)
            {
                return MvcHtmlString.Empty;
            }

            var mvcData = entity.MvcData;
            if (mvcData == null)
            {
                throw new ArgumentNullException(string.Format("Unable to render Entity Model [{0}], because it has no MVC data.", entity));
            }
            RouteValueDictionary parameters = new RouteValueDictionary();

            parameters["entity"] = entity;
            parameters["area"] = mvcData.Area;
            if (mvcData.RouteValues != null)
            {
                foreach (string key in mvcData.RouteValues.Keys)
                {
                    parameters[key] = mvcData.RouteValues[key];
                }
            }

            MvcHtmlString result = htmlHelper.Action(mvcData.Action, mvcData.Controller, parameters);

            return result;
        }
        /// <summary>
        /// Renders the includes using the <see cref="PageController"/>. The URL of the includes page is configured in the IncludesUrl appSetting.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="view">The view.</param>
        /// <returns>MvcHtmlString with rendered content.</returns>

        public static MvcHtmlString RenderIncludes(this HtmlHelper htmlHelper, string view = null)
        {
            string includesUrl = DyndleConfig.IncludesUrl;
            MvcHtmlString result = htmlHelper.Action("Includes", "Page", new { Page = includesUrl, View = view });
            return result;
        }

        /// <summary>
        /// Renders the includes using the <see cref="PageController"/>.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="includeName">Name of the page to include (will be injected into the IncludesUrlMask).</param>
        /// <param name="view">The view.</param>
        /// <returns>MvcHtmlString with rendered content.</returns>

        public static MvcHtmlString RenderIncludes(this HtmlHelper htmlHelper, string includeName, string view = null)
        {
            string includesUrl = string.Format(DyndleConfig.IncludesUrlMask, includeName);
            MvcHtmlString result = htmlHelper.Action("Includes", "Page", new { Page = includesUrl, View = view });
            return result;
        }

        /// <summary>
        /// Renders the includes using the <see cref="PageController"/>.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="region">The region.</param>
        /// <param name="view">The view.</param>
        /// <returns>MvcHtmlString with rendered content.</returns>
        public static MvcHtmlString RenderIncludesRegion(this HtmlHelper htmlHelper, string region, string view = null)
        {
            string includesUrl = DyndleConfig.IncludesUrl;

            return RenderIncludesByUrl(htmlHelper, includesUrl, region, view);
        }

        /// <summary>
        /// Renders the includes using the <see cref="PageController"/>.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="includeName">The include name to be used with IncludesUrlMask.</param>
        /// <param name="region">The region.</param>
        /// <param name="view">The view.</param>
        /// <returns>MvcHtmlString with rendered content.</returns>
        public static MvcHtmlString RenderIncludesRegion(this HtmlHelper htmlHelper, string includeName, string region, string view = null)
        {
            string includesUrl = string.Format(DyndleConfig.IncludesUrlMask, includeName);

            return RenderIncludesByUrl(htmlHelper, includesUrl, region, view);
        }

        /// <summary>
        /// Renders the includes by URL.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="includesUrl">The includes URL.</param>
        /// <param name="region">The region.</param>
        /// <param name="view">The view.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString RenderIncludesByUrl(this HtmlHelper htmlHelper, string includesUrl, string region, string view)
        {
            MvcHtmlString result = htmlHelper.Action("Includes", "Region", new { Page = includesUrl, Region = region, View = view });

            return result;
        }

        #endregion Render methods

        #region labels

        /// <summary>
        /// Gets the label from the <see cref="ISiteContext"/> .
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="key">The key.</param>
        /// <param name="nullIfMissing">If true return null if label missing, otherwise return missing label message</param>
        /// <returns>System.String with the value configured in Tridion.</returns>
        public static string GetLabel(this HtmlHelper helper, string key, bool nullIfMissing = false)
        {
            var siteContext = DependencyResolver.Current.GetService<ISiteContext>(); //does this make sense here? should we moved it to a razor base class?

            siteContext.ThrowIfNull(nameof(siteContext));

            return siteContext.GetLabel(key, nullIfMissing);
        }

        /// <summary>
        /// Gets the label from the <see cref="ISiteContext"/> .
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">Is returned if the label is missing</param>
        /// <returns>System.String with the value configured in Tridion.</returns>
        public static string GetLabel(this HtmlHelper helper, string key, string defaultValue)
        {
            var siteContext = DependencyResolver.Current.GetService<ISiteContext>(); //does this make sense here? should we moved it to a razor base class?

            siteContext.ThrowIfNull(nameof(siteContext));

            return siteContext.GetLabel(key, true) ?? defaultValue;
        }




        /// <summary>
        /// Gets the application setting from the <see cref="ISiteContext"/> .
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.String with the value configured in Tridion.</returns>
        public static string GetApplicationSetting(this HtmlHelper helper, string key)
        {
            var siteContext = DependencyResolver.Current.GetService<ISiteContext>(); //does this make sense here? should we moved it to a razor base class?

            siteContext.ThrowIfNull(nameof(siteContext));

            return siteContext.GetApplicationSetting(key);
        }

        /// <summary>
        /// Gets the application setting from the <see cref="ISiteContext"/> .
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">Is returned if the application setting is missing</param>
        /// <returns>System.String with the value configured in Tridion.</returns>
        public static string GetApplicationSetting(this HtmlHelper helper, string key, string defaultValue)
        {
            var siteContext = DependencyResolver.Current.GetService<ISiteContext>(); //does this make sense here? should we moved it to a razor base class?

            siteContext.ThrowIfNull(nameof(siteContext));

            return siteContext.GetApplicationSetting(key, false) ?? defaultValue;
        }

        /// <summary>
        /// Determines whether request is in debug mode.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>
        ///   <c>true</c> if is debug mode; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDebugMode(this HtmlHelper helper)
        {
            return DebugInfoFilterProvider.IsDebugMode(helper.ViewContext.HttpContext);
        }

        #endregion labels
    }
}