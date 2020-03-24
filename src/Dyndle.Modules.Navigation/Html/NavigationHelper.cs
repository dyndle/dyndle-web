using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dyndle.Modules.Navigation.Models;
using Dyndle.Modules.Navigation.Services;

namespace Dyndle.Modules.Navigation.Html
{
    /// <summary>
    /// Adds HtmlHelper methods to handle Navigation
    /// </summary>
    public static class NavigationHelper
    {
        private static readonly INavigationService NavigationService = DependencyResolver.Current.GetService<INavigationService>();

        #region Render Navigation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper">The current HtmlHelper</param>
        /// <param name="viewName">The view to render</param>
        /// <param name="navLevels">The number of levels of the navigation to fetch</param>
        /// <param name="navSubtype">Subtype of the navigation that is being requested (default is "none")</param>
        public static void RenderNavigation(this HtmlHelper htmlHelper, string viewName, int navLevels = 0, string navSubtype = "none")
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.Navigation(navLevels, navSubtype));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper">The current HtmlHelper</param>
        /// <param name="viewName">The view to render</param>
        /// <param name="navLevels">The number of levels of the navigation to fetch</param>
        /// <param name="navStartLevel">The starting level of the navigation</param>
        /// <param name="navSubtype">Subtype of the navigation that is being requested (default is "none")</param>
        public static void RenderNavigationChildren(this HtmlHelper htmlHelper, string viewName, int navLevels = 0, int navStartLevel = -1, string navSubtype = "none")
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.NavigationChildren(navLevels, navStartLevel, navSubtype));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper">The current HtmlHelper</param>
        /// <param name="viewName">The view to render</param>
        /// <param name="requestUrlPath">The request path to use when rendering the navigation</param>
        public static void RenderNavigationPath(this HtmlHelper htmlHelper, string viewName, string requestUrlPath = "")
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.NavigationPath(requestUrlPath));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper">The current HtmlHelper</param>
        /// <param name="viewName">The view to render</param>
        public static void RenderNavigationSitemap(this HtmlHelper htmlHelper, string viewName)
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.NavigationSitemap());
        }
        #endregion

        #region Return Navigation Model

        /// <summary>
        /// Get Navigation Default Model
        /// </summary>
        /// <param name="htmlHelper">The current HtmlHelper</param>
        /// <param name="navLevels">The number of levels of the navigation to fetch</param>
        /// <param name="navSubtype">Subtype of the navigation that is being requested (default is "none")</param>
        /// <returns></returns>
        public static ISitemapItem Navigation(this HtmlHelper htmlHelper, int navLevels=0, string navSubtype = "none")
        {
            return NavigationService.GetNavigationModel("", NavigationConstants.NavigationType.Default, navSubtype, navLevels);
        }

        /// <summary>
        /// Get Navigation Children Model
        /// </summary>
        /// <param name="htmlHelper">The current HtmlHelper</param>
        /// <param name="navLevels">The number of levels of the navigation to fetch</param>
        /// <param name="navStartLevel">The starting level of the navigation</param>
        /// <param name="navSubtype">Subtype of the navigation that is being requested (default is "none")</param>
        /// <returns></returns>
        public static ISitemapItem NavigationChildren(this HtmlHelper htmlHelper, int navLevels=0, int navStartLevel=-1, string navSubtype = "none")
        {
            return NavigationService.GetNavigationModel(string.Empty,NavigationConstants.NavigationType.Children, navSubtype, navLevels,navStartLevel);
        }

        /// <summary>
        /// Get Navigation Path Model
        /// </summary>
        /// <param name="htmlHelper">The current HtmlHelper</param>
        /// <param name="requestUrlPath">The request path to use when fetching the navigation</param>
        /// <returns></returns>
        public static ISitemapItem NavigationPath(this HtmlHelper htmlHelper, string requestUrlPath="")
        {
            return NavigationService.GetNavigationModel(requestUrlPath, NavigationConstants.NavigationType.Path);
        }

        /// <summary>
        /// Get Navigation Sitemap Model
        /// </summary>
        /// <param name="htmlHelper">The current HtmlHelper</param>
        /// <returns></returns>
        public static ISitemapItem NavigationSitemap(this HtmlHelper htmlHelper)
        {
            return NavigationService.GetNavigationModel(string.Empty, NavigationConstants.NavigationType.Sitemap);
        }
        #endregion
    }
}