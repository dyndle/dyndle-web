using System.Web.Mvc;
using DD4T.Core.Contracts.Resolvers;
using DD4T.Core.Contracts.ViewModels;
using Trivident.Modules.Core.Binders;
using Trivident.Modules.Core.Environment;
using Trivident.Modules.Core.Factories;
using Trivident.Modules.Core.Providers.Configuration;
using Trivident.Modules.Core.Providers.Content;
using Trivident.Modules.Core.Resolver;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Interfaces;
using Trivident.Modules.Core.Services.Taxonomy;

namespace Trivident.Modules.Core.Modules
{
    /// <summary>
    /// Register types for various implementations used by Core Module.
    /// </summary>
    public class DefaultsModule : IServiceCollectionModule
    {
	    public void RegisterTypes(IServiceCollection serviceCollection)
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