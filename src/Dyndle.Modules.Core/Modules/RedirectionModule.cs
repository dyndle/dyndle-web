using Dyndle.Modules.Core.Interfaces;
using Dyndle.Modules.Core.Providers.Redirection;
using Dyndle.Modules.Core.Services.Redirection;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Modules
{
    /// <summary>
    /// Register Types for Redirection module.
    /// </summary>
    public class RedirectionModule : IServiceCollectionModule
    {
	    public void RegisterTypes(IServiceCollection serviceCollection)
	    {
			serviceCollection.AddSingleton(typeof(IRedirectionDefinitionProvider), typeof(DefaultRedirectionDefinitionProvider));
		    serviceCollection.AddSingleton(typeof(IRedirectionService), typeof(RedirectionService));
		}
    }
}