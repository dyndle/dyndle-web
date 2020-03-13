using DD4T.ContentModel.Contracts.Configuration;
using DD4T.Core.Contracts.Resolvers;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Extensions;

namespace Dyndle.Modules.Search.Resolver
{
    public class DefaultSearchLinkResolver : ISearchLinkResolver
    {
        /// <summary>
        /// The LinkResolver 
        /// </summary>
        private readonly ILinkResolver _linkResolver;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IDD4TConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSearchLinkResolver" /> class.
        /// </summary>
        public DefaultSearchLinkResolver(ILinkResolver linkResolver, IDD4TConfiguration configuration)
        {
            linkResolver.ThrowIfNull(nameof(linkResolver));
            _linkResolver = linkResolver;
            _configuration = configuration;
        }

        public virtual void Resolve(ISearchResultItem item)
        {
            var url = item.Urls.GetValue();
            switch (item.ItemType)
            {
                case 64:
                    item.Url = url.CleanUrl(_configuration.WelcomeFile);
                    break;
                default:
                    item.Url = _linkResolver.ResolveUrl(url);
                    break;
            }
        }
    }
}