using DD4T.Caching.ApacheMQ;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.Mvc.ViewModels.XPM;
using DD4T.Utils.Caching;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Cache;
using Trivident.Modules.Core.Contracts;
using Trivident.Modules.Core.Interfaces;
using Trivident.Modules.Core.XPM;

namespace Trivident.Modules.Core.Modules
{
    /// <summary>
    /// Register Types for Cache Module
    /// </summary>
    public class XpmModule : IServiceCollectionModule
    {
        private readonly bool _isStaging;

        /// <inheritdoc />
        public XpmModule(bool isStaging)
	    {
            _isStaging = isStaging;
        }

        /// <inheritdoc />
        public void RegisterTypes(IServiceCollection serviceCollection)
        {
            if (_isStaging)
            {
                serviceCollection.AddSingleton(typeof(IWebPageEnrichmentProvider), typeof(XpmPageTagEnrichmentProvider));
                serviceCollection.AddSingleton(typeof(IXpmMarkupService), typeof(XpmMarkupService));

            }
        }
    }
}