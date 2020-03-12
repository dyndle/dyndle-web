using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Attributes.Filter;
using Trivident.Modules.Core.Interfaces;

namespace Trivident.Modules.Core.Modules
{
    /// <summary>
    /// Global Action Filter
    /// </summary>
    public class ActionFilterModule : IServiceCollectionModule
    {
	    public void RegisterTypes(IServiceCollection serviceCollection)
	    {
			serviceCollection.AddSingleton(typeof(IActionFilter), typeof(AbsoluteUrlAttribute));
		}
    }
}