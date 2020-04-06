using System.Web.Mvc;
using Dyndle.Modules.Core.Attributes.Filter;
using Dyndle.Modules.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Modules
{
    /// <summary>
    /// Global Action Filter
    /// </summary>
    public class ActionFilterModule : IServiceCollectionModule
    {
	    public void RegisterTypes(IServiceCollection serviceCollection)
        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
	    {
			serviceCollection.AddSingleton(typeof(IActionFilter), typeof(AbsoluteUrlAttribute));
		}
    }
}