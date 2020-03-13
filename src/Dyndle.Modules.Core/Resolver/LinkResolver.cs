using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Factories;
using DD4T.Utils.Resolver;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Resolver
{
    /// <summary>
    /// Class LinkResolver.
    /// Overrides default <see cref="DefaultLinkResolver"/> so we can cleanup the URL's that are being resolved from Tridion
    /// </summary>
    /// <seealso cref="DD4T.Utils.Resolver.DefaultLinkResolver" />
    public class LinkResolver : DefaultLinkResolver
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IDD4TConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkResolver" /> class.
        /// </summary>
        /// <param name="linkFactory">The link factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="binaryFactory">The binary factory.</param>
        /// <param name="configuration">The configuration.</param>
        public LinkResolver(ILinkFactory linkFactory, ILogger logger, IBinaryFactory binaryFactory, IDD4TConfiguration configuration)
        : base(linkFactory, logger, binaryFactory, configuration)
        {
            configuration.ThrowIfNull(nameof(configuration));
            _configuration = configuration;
        }

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="tcmUri">The TCM URI.</param>
        /// <param name="pageId">The page identifier.</param>
        /// <returns>System.String.</returns>
        public override string ResolveUrl(string tcmUri, string pageId = null)
        {
            var url = base.ResolveUrl(tcmUri, pageId);

            if (url.IsNullOrEmpty())
                return url;

            return url.CleanUrl(_configuration.WelcomeFile);
        }
    }
}
