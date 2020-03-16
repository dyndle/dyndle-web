using System;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Contracts.Resolvers;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models.System;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Core.Providers.Configuration
{
    /// <summary>
    /// Class DefaultSiteConfigurationProvider.
    /// Used to get all setting from Tridion based on the current publication
    /// </summary>
    /// <seealso cref="ISiteConfigurationProvider" />
    public class DefaultSiteConfigurationProvider : ISiteConfigurationProvider
    {
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;
        private readonly IPublicationResolver _publicationResolver;
        private const string _cacheKeyFormat = "Configuration_{0}";
        private const string _cacheRegion = "Configuration";

        /// <summary>
        /// The URL
        /// </summary>
        private string URL = DyndleConfig.SiteConfigUrl;
        private readonly ICacheAgent _cacheAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSiteConfigurationProvider" /> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheAgent">The cache agent.</param>
        public DefaultSiteConfigurationProvider(IContentProvider contentProvider, IPublicationResolver publicationResolver, ILogger logger, ICacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            contentProvider.ThrowIfNull(nameof(contentProvider));
            logger.ThrowIfNull(nameof(logger));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));

            _contentProvider = contentProvider;
            _publicationResolver = publicationResolver;
            _logger = logger;
            _cacheAgent = cacheAgent;
        }

        /// <summary>
        /// Retrieves the configuration.
        /// </summary>
        /// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
        public IDictionary<string, string> RetrieveConfiguration()
        {
            var pubId = _publicationResolver.ResolvePublicationId();
            var key = _cacheKeyFormat.FormatString(pubId);

            var configuration = _cacheAgent.Load(key) as IDictionary<string, string>;

            if (configuration == null)
            {
                configuration = LoadConfiguration();
                if (configuration != null)
                {
                    _cacheAgent.Store(key, _cacheRegion, configuration);
                }
            }

            return configuration;
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        /// <exception cref="Exception">configuration page not found. Url: {0}; Is the page published?</exception>
        private Dictionary<string, string> LoadConfiguration()
        {           
            _logger.Debug("about to load configuration page. Url: {0}".FormatString(URL));

            var page = _contentProvider.GetPage(URL);
            if (page == null)
            {
                _logger.Error("configuration page is not published. Url : {0}".FormatString(URL));
                throw new Exception("configuration page not found. Url: {0}; Is the page published?".FormatString(URL));
            }

            List<Dictionary<string, string>> listOfdictionaries = new List<Dictionary<string, string>>();
            foreach (var cp in page.ComponentPresentations)
            {
                var model = _contentProvider.BuildViewModel(cp) as SystemSettings;
                if (model == null)
                {
                    continue;
                }
                listOfdictionaries.Add(model.KeyValuePairs?.ToDictionary(a => a.Key, a => a.Value));
            }

            var mergedValues = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            mergedValues = listOfdictionaries.SelectMany(dict => dict)
                       .ToLookup(pair => pair.Key, pair => pair.Value)
                       .ToDictionary(group => group.Key, group => group.First());

            return mergedValues;
        }
    }
}