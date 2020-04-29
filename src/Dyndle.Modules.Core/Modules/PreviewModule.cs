using Dyndle.Modules.Core.Interfaces;
using Dyndle.Modules.Core.Services.Preview;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Modules
{
    /// <summary>
    /// Register Types for Preview.
    /// </summary>
    public class PreviewModule : IServiceCollectionModule
    {
        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public void RegisterTypes(IServiceCollection serviceCollection)
	    {
			serviceCollection.AddSingleton(typeof(IPreviewContentService), typeof(PreviewContentService));
		}
    }
}