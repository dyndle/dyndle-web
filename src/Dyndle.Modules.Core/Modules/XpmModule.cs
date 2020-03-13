using DD4T.Mvc.ViewModels.XPM;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Interfaces;
using Dyndle.Modules.Core.XPM;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Modules
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