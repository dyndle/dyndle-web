using System;
using System.Configuration;
using System.Linq;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Cache;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models.Defaults;
using Dyndle.Modules.Core.Models.System;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.Navigation.Models;

namespace Dyndle.Modules.Navigation.Providers
{
    /// <summary>
    /// Class NavigationProvider.
    /// Used to retrieve the navigation items from Tridion.
    /// Prepairs the collection for the specific needs like path or a limited levels to render
    /// </summary>
    /// <seealso cref="INavigationProvider" />
    public class NavigationProvider : INavigationProvider
    {
        private const string _cacheKeyFormat = "NavigationComplete({0})";
        private const string _cacheKeySpecificFormat = "NavigationSpecific({0},{1},{2},{3},{4},{5})";
        private const string _cacheRegion = "Navigation";
        private const string URL = "/system/navigation.json";
        private readonly ICacheAgent _cacheAgent;
        private readonly ISerializedCacheAgent _serializedCacheAgent;
        private readonly IDD4TConfiguration _configuration;
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;
        private readonly IPublicationResolver _publicationResolver;
        private readonly IViewModelFactory _viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationProvider" /> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        /// <param name="cacheAgent">The cache agent.</param>
        /// <param name="serializedCacheAgent">The serialized cache agent.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        public NavigationProvider(IContentProvider contentProvider, ILogger logger, IDD4TConfiguration configuration, IViewModelFactory viewModelFactory, ICacheAgent cacheAgent, ISerializedCacheAgent serializedCacheAgent, IPublicationResolver publicationResolver)
        {
            contentProvider.ThrowIfNull(nameof(contentProvider));
            logger.ThrowIfNull(nameof(logger));
            configuration.ThrowIfNull(nameof(configuration));
            viewModelFactory.ThrowIfNull(nameof(viewModelFactory));
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            serializedCacheAgent.ThrowIfNull(nameof(serializedCacheAgent));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));

            _configuration = configuration;
            _contentProvider = contentProvider;
            _logger = logger;
            _viewModelFactory = viewModelFactory;
            _cacheAgent = cacheAgent;
            _serializedCacheAgent = serializedCacheAgent;
            _publicationResolver = publicationResolver;
        }

        private string GetNavigationPath
        {
            get
            {
                string path = ConfigurationManager.AppSettings[NavigationConstants.Settings.NavigationPath];
                return path.IsNullOrEmpty() ? URL : path;
            }
        }

        private bool IncludeAllPagesInSitemap
        {
            get
            {
                string setting = ConfigurationManager.AppSettings[NavigationConstants.Settings.IncludeAllPagesInSitemap];
                return !string.IsNullOrEmpty(setting) && Convert.ToBoolean(setting);
            }
        }

        private bool SubNavDefaultsToMainNav
        {
            get
            {
                string setting = ConfigurationManager.AppSettings[NavigationConstants.Settings.SubNavDefaultsToMainNav];
                if (string.IsNullOrEmpty(setting))
                {
                    return false;
                }
                return Convert.ToBoolean(setting);
            }
        }

        /// <summary>
        /// Gets the Navigation Model (Sitemap) for a given Localization.
        /// </summary>
        /// <param name="levels">The number of levels to return.</param>
        /// <param name="navSubtype">Subtype of the navigation.</param>
        /// <returns>The Navigation Model (Sitemap root Item).</returns>
        public virtual ISitemapItem GetAll(int levels = 0, string navSubtype = null)
        {
            string key = _cacheKeySpecificFormat.FormatString(
                _publicationResolver.ResolvePublicationId(), "all", levels, navSubtype, null, null);
            var specificSitemap = _cacheAgent.Load(key) as ISitemapItem;
            if (specificSitemap != null)
            {
                return specificSitemap;
            }

            var result = Load();

            if (levels == 0) levels = 10;
            result.CleanUpSitemap(levels);

            // remove structure groups with 'visible' == false
            RemoveInvisbleStructureGroups(result);

            if (navSubtype != null)
            {
                result.FilterBySubtype(navSubtype);
            }

            _cacheAgent.Store(key, _cacheRegion, result);            
            return result;
        }

        /// <summary>
        /// Gets the children starting from an ancestor at specified level.
        /// If startLevel -1 is provided the current page level is used.
        /// </summary>
        /// <param name="requestUrlPath">The request URL path.</param>
        /// <param name="levels">The number of levels to return.</param>
        /// <param name="startLevel">The start level. If startLevel -1 is provided the current page level is used
        /// Otherwise starting from an ancestor of the current page at specified level.</param>
        /// <param name="navSubtype">The nav subtype.</param>
        /// <returns>ISitemapItem.</returns>
        public ISitemapItem GetChildren(string requestUrlPath, int levels = 0, int startLevel = -1, string navSubtype = null)
        {
            string key = _cacheKeySpecificFormat.FormatString(_publicationResolver.ResolvePublicationId(), "children", levels, navSubtype, requestUrlPath, startLevel);
            var specificSitemap = _cacheAgent.Load(key) as ISitemapItem;
            if (specificSitemap != null)
            {
                return specificSitemap;
            }

            var result = Load();

            if (startLevel != -1)
            {
                var urlWithoutPublication = requestUrlPath.Remove(0, result.Url.Length);
                var segments = urlWithoutPublication.Split('/').Where(s => !s.IsNullOrEmpty());
                if (segments.Count() < startLevel)
                {
                    if (SubNavDefaultsToMainNav)
                    {
                        return GetAll(levels, navSubtype);
                    }
                    return new SitemapItem();
                }
                segments = segments.Take(startLevel).ToArray();

                requestUrlPath = string.Concat(string.Concat(result.Url, string.Join("/", segments)).TrimEnd('/'), "/");
            }
            if (levels == 0) levels = 10;

            result = result.FindByUrl(requestUrlPath);
            result?.CleanUpSitemap(levels);

            _cacheAgent.Store(key, _cacheRegion, result);
            return result;
        }

        /// <summary>
        /// Loads complete sitemap from Tridion
        /// If IncludeAllPagesInSitemap is true, include all pages
        /// </summary>
        /// <returns>ISitemapItem.</returns>
        public virtual ISitemapItem GetFullSitemap()
        {
            string key = _cacheKeySpecificFormat.FormatString(_publicationResolver.ResolvePublicationId(), "full", null, null, null, null);
            var specificSitemap = _cacheAgent.Load(key) as ISitemapItem;
            if (specificSitemap != null)
            {
                return specificSitemap;
            }

            var result = Load();

            if (!IncludeAllPagesInSitemap)
            {
                // remove pages but don't enforce a maximum level
                result.CleanUpSitemap(-1);
            }

            _cacheAgent.Store(key, _cacheRegion, result);
            return result;
        }

        /// <summary>
        /// Gets the path for the current URL.
        /// </summary>
        /// <param name="requestUrlPath">The request URL path.</param>
        /// <returns>ISitemapItem.</returns>
        public virtual ISitemapItem GetPath(string requestUrlPath)
        {
            string key = _cacheKeySpecificFormat.FormatString(_publicationResolver.ResolvePublicationId(), "path", null, null, requestUrlPath, null);
            var specificSitemap = _cacheAgent.Load(key) as ISitemapItem;
            if (specificSitemap != null)
            {
                return specificSitemap;
            }

            var result = Load();
            result.PrepareBreadcrumb(requestUrlPath);

            _cacheAgent.Store(key, _cacheRegion, result);
            return result;
        }

        /// <summary>
        /// Loads all the data from Tridion.
        /// </summary>
        /// <returns>ISitemapItem.</returns>
        protected virtual ISitemapItem Load()
        {
            string key = _cacheKeyFormat.FormatString(_publicationResolver.ResolvePublicationId());
            var completeSitemap = _serializedCacheAgent.Load(key) as ISitemapItem;
            if (completeSitemap != null)
            {
                return completeSitemap;
            }
            var page = _contentProvider.GetPage(this.GetNavigationPath);
            if (page == null)
            {
                _logger.Error("Navigation not found. Is the navigation page published? url: {0}".FormatString(this.GetNavigationPath));
                return null;
            }

            if (page.ComponentPresentations.Count == 0)
            {
                _logger.Error("Navigation page has no component presentations. Is it using the correct template? url: {0}".FormatString(this.GetNavigationPath));
                return null;
            }

            var result = _viewModelFactory.BuildViewModel(page.ComponentPresentations.FirstOrDefault()) as ISitemapItem;
            if (result == null || result is ExceptionEntity || result is DefaultEntity)
            {
                result = _viewModelFactory.BuildViewModel<SitemapItem>(page.ComponentPresentations.FirstOrDefault());
            }
            result.CleanAllUrls(_configuration.WelcomeFile);

            _serializedCacheAgent.Store(key, _cacheRegion, result);
            return result;
        }
        private void RemoveInvisbleStructureGroups(ISitemapItem item)
        {
            item.Items?.RemoveAll(i => !i.Visible);
            item.Items?.ForEach(i => RemoveInvisbleStructureGroups(i));
        }
    }
}