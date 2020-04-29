using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Resolvers;
using Tridion.ContentDelivery.DynamicContent;

namespace Dyndle.Providers.Resolvers
{
    /// <summary>
    /// Resolve publication id by querying Tridion DiscoveryService;
    /// </summary>
    /// <seealso cref="IExtendedPublicationResolver" />
    public class PublicationResolver : PublicationResolverBase
    {
        private readonly DynamicMappingsRetriever _mappingsRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationResolver" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PublicationResolver(ILogger logger) : base(logger)
        {
            _mappingsRetriever = new DynamicMappingsRetriever();
        }

        protected override PublicationMapping RetrieveMapping(string url)
        {
            return ToNativePublicationMapping(_mappingsRetriever.GetPublicationMapping(url));
        }

        private PublicationMapping ToNativePublicationMapping(IPublicationMapping result)
        {
            if (result == null)
            {
                return null;
            }
            return new PublicationMapping()
            {
                Domain = result.Domain,
                NamespaceId = result.NamespaceId,
                PublicationId = result.PublicationId,
                Protocol = result.Protocol,
                Port = result.Port,
                Path = result.Path,
                PathScanDepth = result.PathScanDepth
            };
        }
    }
}