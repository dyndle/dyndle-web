using System.Web.Mvc;
using DD4T.Core.Contracts.Resolvers;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Binders;
using Dyndle.Modules.Core.Environment;
using Dyndle.Modules.Core.Factories;
using Dyndle.Modules.Core.Interfaces;
using Dyndle.Modules.Core.Providers.Configuration;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.Core.Resolver;
using Dyndle.Modules.Core.Services.Taxonomy;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Modules
{
    /// <summary>
    /// Register types for various implementations used by Core Module.
    /// </summary>
    public class DefaultsModule : IServiceCollectionModule
    {
	    public void RegisterTypes(IServiceCollection serviceCollection)
        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
	    {
			serviceCollection.AddSingleton(typeof(IContentProvider), typeof(DefaultContentProvider));
		    serviceCollection.AddSingleton(typeof(IViewModelFactory), typeof(ViewModelFactory));
		    serviceCollection.AddSingleton(typeof(ILinkResolver), typeof(LinkResolver));
		    serviceCollection.AddSingleton(typeof(IRichTextResolver), typeof(RichTextFieldResolver));
		    serviceCollection.AddSingleton(typeof(ISiteConfigurationProvider), typeof(DefaultSiteConfigurationProvider));
		    serviceCollection.AddSingleton(typeof(ISiteContext), typeof(SiteContext));
		    serviceCollection.AddSingleton(typeof(IModelBinderProvider), typeof(TypedModelBinder));
		    serviceCollection.AddSingleton(typeof(IContentByUrlProvider), typeof(DefaultContentByUrlProvider));
            serviceCollection.AddSingleton(typeof(ITaxonomyService), typeof(TaxonomyService));
        }
    }
}