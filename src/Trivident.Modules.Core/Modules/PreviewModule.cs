using System;
using System.Collections.Generic;
using DD4T.Core.Contracts.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Interfaces;
using Trivident.Modules.Core.Services.Preview;

namespace Trivident.Modules.Core.Modules
{
    /// <summary>
    /// Register Types for Preview.
    /// </summary>
    public class PreviewModule : IServiceCollectionModule
    {
	    public void RegisterTypes(IServiceCollection serviceCollection)
	    {
			serviceCollection.AddSingleton(typeof(IPreviewContentService), typeof(PreviewContentService));
		}
    }
}