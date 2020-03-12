using System;
using System.Collections.Generic;
using DD4T.Core.Contracts.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Interfaces;
using Trivident.Modules.Core.Providers.Redirection;
using Trivident.Modules.Core.Services.Redirection;

namespace Trivident.Modules.Core.Modules
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