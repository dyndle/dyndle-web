using System.Web;
using Dyndle.Modules.Core.Cache;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Navigation.Binders;
using Dyndle.Modules.Navigation.Models;
using Dyndle.Modules.Navigation.Providers;

namespace Dyndle.Modules.Navigation.Services
{
    public class NavigationService : INavigationService
    {
        private readonly INavigationProvider _navigationProvider;
        private const string _cacheKeyFormat = "Navigation({0}-{1}-{2}-{3}-{4})";
        private const string _cacheRegion = "Navigation";
        private readonly ISerializedCacheAgent _cacheAgent;

        public NavigationService(INavigationProvider navigationProvider, ISerializedCacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            navigationProvider.ThrowIfNull(nameof(navigationProvider));
            _cacheAgent = cacheAgent;
            _navigationProvider = navigationProvider;
        }

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