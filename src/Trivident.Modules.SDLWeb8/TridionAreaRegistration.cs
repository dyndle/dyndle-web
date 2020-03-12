using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Utils.Resolver;
using Microsoft.Extensions.DependencyInjection;
using System;
using Trivident.Modules.Core;
using Trivident.Modules.Core.Configuration;
using Trivident.Modules.Core.Contracts;
using Trivident.Modules.Core.Interfaces;
using Trivident.Modules.Core.Providers.Content;
using Trivident.Modules.Core.Resolvers;
using Trivident.Modules.Core.Services.Taxonomy;

namespace Trivident.Modules.Email
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