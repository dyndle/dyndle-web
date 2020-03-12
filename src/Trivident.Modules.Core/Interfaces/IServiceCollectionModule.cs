using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Trivident.Modules.Core.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface IServiceCollectionModule
	{
		void RegisterTypes(IServiceCollection serviceCollection);
	}
}
