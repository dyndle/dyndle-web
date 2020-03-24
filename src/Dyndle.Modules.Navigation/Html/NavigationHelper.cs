using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dyndle.Modules.Navigation.Models;
using Dyndle.Modules.Navigation.Services;

namespace Dyndle.Modules.Navigation.Html
{
    public static class NavigationHelper
    {
        private static INavigationService _navigationService = DependencyResolver.Current.GetService<INavigationService>();

        #region Render Navigation
        public static void RenderNavigation(this HtmlHelper htmlHelper, string viewName)
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.Navigation());
        }
        public static void RenderNavigationChildren(this HtmlHelper htmlHelper, string viewName, int navLevels = 0, int navStartLevel = -1, string navSubtype = "none")
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.NavigationChildren(navLevels, navStartLevel, navSubtype));
        }
        public static void RenderNavigationPath(this HtmlHelper htmlHelper, string viewName, string requestUrlPath = "")
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.NavigationPath(requestUrlPath));
        }
        public static void RenderNavigationSitemap(this HtmlHelper htmlHelper, string viewName)
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.NavigationSitemap());
        }
        #endregion

        #region Return Navigation Model

        public static ISitemapItem Navigation(this HtmlHelper htmlHelper)
        {
            return _navigationService.GetNavigationModel();
        }

        public static ISitemapItem NavigationChildren(this HtmlHelper htmlHelper, int navLevels=0, int navStartLevel=-1, string navSubtype = "none")
        {
            return _navigationService.GetNavigationModel(string.Empty,NavigationConstants.NavigationType.Children, navSubtype, navLevels,navStartLevel);
        }

        public static ISitemapItem NavigationPath(this HtmlHelper htmlHelper, string requestUrlPath="")
        {
            return _navigationService.GetNavigationModel(requestUrlPath, NavigationConstants.NavigationType.Path);
        }

        public static ISitemapItem NavigationSitemap(this HtmlHelper htmlHelper)
        {
            return _navigationService.GetNavigationModel(string.Empty, NavigationConstants.NavigationType.Sitemap);
        }

        #endregion

    }

}