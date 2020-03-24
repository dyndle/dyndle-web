using System;
using System.Collections.Concurrent;
using System.Web;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Utils.Resolver;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;
using Tridion.ContentDelivery.DynamicContent;

namespace Dyndle.Modules.SDLWeb8.Resolvers
{
    /// <summary>
    /// Resolve publication id by app key setting in web.config!
    /// Use only on local development machines.
    /// </summary>
    /// <seealso cref="IExtendedPublicationResolver" />
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
            uriBuilder.Path = DyndleConfig.PublicationBasePath;
            uriBuilder.Port = HttpContext.Current.Request.Url.Port;
            return uriBuilder.Uri;
        }
    }
}