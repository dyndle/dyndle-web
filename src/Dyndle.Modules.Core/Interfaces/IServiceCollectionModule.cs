using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <summary>
    /// Interface IServiceCollectionModule
    /// </summary>
    public interface IServiceCollectionModule
	{
        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        void RegisterTypes(IServiceCollection serviceCollection);
	}
}
