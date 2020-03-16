using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface IServiceCollectionModule
	{
		void RegisterTypes(IServiceCollection serviceCollection);
	}
}
