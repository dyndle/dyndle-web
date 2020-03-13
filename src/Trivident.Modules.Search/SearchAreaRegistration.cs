using System.Web.Mvc;
using Dyndle.Modules.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Trivident.Modules.Search.Binders;
using Trivident.Modules.Search.Contracts;
using Trivident.Modules.Search.Providers;
using Trivident.Modules.Search.Providers.Solr;
using Trivident.Modules.Search.Resolver;

namespace Trivident.Modules.Search
{
    public class SearchAreaRegistration : BaseModuleAreaRegistration
    {
        public override string AreaName => "Search";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            base.RegisterArea(context);
        }

	    public override void RegisterTypes(IServiceCollection serviceCollection)
	    {
		    serviceCollection.AddSingleton(typeof(ISearchProvider), typeof(SearchProvider));
		    serviceCollection.AddSingleton(typeof(IModelBinderProvider), typeof(SearchQueryBinder));
	        serviceCollection.AddSingleton(typeof(ISearchLinkResolver), typeof(DefaultSearchLinkResolver));
	    }
    }

}