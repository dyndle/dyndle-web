using System.Web;
using Dyndle.Modules.Core.Cache;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Navigation.Models;
using Dyndle.Modules.Navigation.Providers;

namespace Dyndle.Modules.Navigation.Services
{
    /// <summary>
    /// Used to query the navigation provider and return a navigation model
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly INavigationProvider _navigationProvider;
        private const string _cacheKeyFormat = "Navigation({0}-{1}-{2}-{3}-{4})";
        private const string _cacheRegion = "Navigation";
        private readonly ISerializedCacheAgent _cacheAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="navigationProvider">The navigation provider.</param>
        /// <param name="cacheAgent">The cache agent.</param>
        public NavigationService(INavigationProvider navigationProvider, ISerializedCacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            navigationProvider.ThrowIfNull(nameof(navigationProvider));
            _cacheAgent = cacheAgent;
            _navigationProvider = navigationProvider;
        }

        /// <summary>
        /// Gets the navigation model.
        /// </summary>
        /// <param name="requestUrlPath">The request path to use when fetching the navigation</param>
        /// <param name="navType">Type of navigation that is requested</param>
        /// <param name="navSubtype">Subtype of the navigation that is being requested (default is "none")</param>
        /// <param name="navLevels">The number of levels of the navigation to fetch</param>
        /// <param name="navStartLevel">The starting level of the navigation</param>
        /// <returns>ISitemapItem</returns>
        public ISitemapItem GetNavigationModel(string requestUrlPath="", NavigationConstants.NavigationType navType=NavigationConstants.NavigationType.Default, string navSubtype="none", int navLevels = 0, int navStartLevel = -1)
        {
            ISitemapItem model = null;

            if (string.IsNullOrEmpty(requestUrlPath))
                requestUrlPath = HttpContext.Current?.Request.Url.LocalPath ?? "";

            //generate a Cache key for the current model.
            //try to get model out of HttpContext.Items[], incase the same model is request multiple times during
            //a single request.
            string key = _cacheKeyFormat.FormatString(navType, requestUrlPath, navLevels, navStartLevel, navSubtype);

            model = _cacheAgent.Load(key) as ISitemapItem;

            if (model == null)
            {
                switch (navType)
                {
                    case NavigationConstants.NavigationType.Children:
                        model = _navigationProvider.GetChildren(requestUrlPath, navLevels, navStartLevel, navSubtype);
                        break;

                    case NavigationConstants.NavigationType.Path:
                        model = _navigationProvider.GetPath(requestUrlPath);
                        break;

                    case NavigationConstants.NavigationType.Sitemap:
                        model = _navigationProvider.GetFullSitemap();
                        break;

                    default:
                        model = _navigationProvider.GetAll(navLevels, navSubtype);
                        break;
                }
                if (model == null)
                {
                    model = new SitemapItem();
                }

                _cacheAgent.Store(key, _cacheRegion, model);
            }

            return model;
        }
    }
}