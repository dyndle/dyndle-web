using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Utils.Resolver;
using Dyndle.Modules.Core;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.SDLWeb85.Providers;
using Dyndle.Modules.SDLWeb85.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.SDLWeb85
{
	/// <inheritdoc />
	public class TridionAreaRegistration : BaseModuleAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tridion";
            }
        }
        private readonly bool _isDevMode;

        public TridionAreaRegistration()
        {
            _isDevMode = !CoreConstants.Configuration.PublicationId.GetConfigurationValue().IsNullOrEmpty();
        }

        /// <inheritdoc />
        public override void RegisterTypes(IServiceCollection serviceCollection)
	    {
			serviceCollection.AddSingleton(typeof(ITaxonomyProvider), typeof(DefaultTaxonomyProvider));
            serviceCollection.AddSingleton(typeof(IContentQueryProvider), typeof(DefaultContentQueryProvider));
            if (_isDevMode)
            {
                serviceCollection.AddSingleton(typeof(IPublicationResolver), typeof(DefaultPublicationResolver));
                serviceCollection.AddSingleton(typeof(IExtendedPublicationResolver), typeof(DevPublicationResolver));
            }
            else
            {
                serviceCollection.AddSingleton(typeof(IPublicationResolver), typeof(PublicationResolver));
                serviceCollection.AddSingleton(typeof(IExtendedPublicationResolver), typeof(PublicationResolver));
            }
        }

	}
}