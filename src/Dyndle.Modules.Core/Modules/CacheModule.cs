using DD4T.ContentModel.Contracts.Caching;
using DD4T.Utils.Caching;
using Dyndle.Modules.Core.Cache;
using Dyndle.Modules.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core.Modules
{
    /// <summary>
    /// Register Types for Cache Module
    /// </summary>
    public class CacheModule : IServiceCollectionModule
    {
        private readonly bool _isCacheEnabled;
        private readonly bool _isStaging;

        /// <inheritdoc />
        public CacheModule(bool isCacheEnabled, bool isStaging)
	    {
		    _isCacheEnabled = isCacheEnabled;
            _isStaging = isStaging;
        }

	    /// <inheritdoc />
	    public void RegisterTypes(IServiceCollection serviceCollection)
	    {
            if (_isCacheEnabled)
            {
                if (_isStaging)
                {
                    serviceCollection.AddSingleton(typeof(ICacheAgent), typeof(PreviewAwareCacheAgent));
                }
                else
                {
                    serviceCollection.AddSingleton(typeof(ICacheAgent), typeof(DefaultCacheAgent));
                }
                serviceCollection.AddSingleton(typeof(ISerializedCacheAgent), typeof(SerializedCacheAgent));
            }
            else
            {
                serviceCollection.AddSingleton(typeof(ICacheAgent), typeof(NullCacheAgent));
                serviceCollection.AddSingleton(typeof(ISerializedCacheAgent), typeof(NullSerializedCacheAgent));
            }
        }
    }
}