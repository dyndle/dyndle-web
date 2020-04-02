using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Contracts.Providers;
using DD4T.ContentModel.Contracts.Resolvers;
using System;

namespace Tridion.TestProviders
{
    public class ProviderCommonServices : IProvidersCommonServices
    {
        public IPublicationResolver PublicationResolver
        {
            get { throw new NotImplementedException(); }
        }

        public ILogger Logger
        {
            get { throw new NotImplementedException(); }
        }

        public IDD4TConfiguration Configuration
        {
            get { throw new NotImplementedException(); }
        }
    }
}
