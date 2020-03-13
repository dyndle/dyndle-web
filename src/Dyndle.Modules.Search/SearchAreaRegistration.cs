using System.Web.Mvc;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Search.Binders;
using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Providers;
using Dyndle.Modules.Search.Providers.Solr;
using Dyndle.Modules.Search.Resolver;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Search
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