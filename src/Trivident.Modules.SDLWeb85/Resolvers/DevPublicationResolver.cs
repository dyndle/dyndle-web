using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Utils.Resolver;
using Trivident.Modules.Core.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tridion.ContentDelivery.DynamicContent;
using Trivident.Modules.Core.Configuration;

namespace Trivident.Modules.Core.Resolvers
{
    /// <summary>
    /// Resolve publication id by app key setting in web.config!
    /// Use only on local development machines.
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Contracts.IExtendedPublicationResolver" />
    public class DevPublicationResolver : DefaultPublicationResolver, IExtendedPublicationResolver
    {
        private readonly DynamicMappingsRetriever _mappingsRetriever;
        private readonly ILogger _logger;

        /// <summary>
        /// holds a list of Resolved IPublicationMappings
        /// </summary>
        private ConcurrentDictionary<string, IPublicationMapping> _publicationMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationResolver" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">DD4T Configuration.</param>
        public DevPublicationResolver(ILogger logger, IDD4TConfiguration configuration) : base(configuration)
        {
            logger.ThrowIfNull(nameof(logger));

            _logger = logger;
        }

        /// <summary>
        /// Gets the base URI of the current publication.
        /// </summary>
        /// <returns>Uri.</returns>
        public Uri GetBaseUri()
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = HttpContext.Current.Request.Url.Scheme;
            uriBuilder.Host = HttpContext.Current.Request.Url.Host;
            uriBuilder.Path = DyndleConfig.PublicationBaseUrl;
            uriBuilder.Port = HttpContext.Current.Request.Url.Port;
            return uriBuilder.Uri;
        }
    }
}