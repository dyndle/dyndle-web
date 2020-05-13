using System.Collections.Generic;
using DD4T.ContentModel.Contracts.Caching;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Cache
{
    /// <summary>
    /// Simple wrapper for the original DD4T DefaultCacheAgent 
    /// It only adds the generic Load method, but does not do any (de)serialization (regardless of the name of the interface)
    /// </summary>
    /// <seealso cref="ISerializedCacheAgent" />
    public class NonSerializedCacheAgent : ISerializedCacheAgent
    {
        private readonly ICacheAgent _cacheAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="NonSerializedCacheAgent"/> class.
        /// </summary>
        /// <param name="cacheAgent">The DD4T cache agent.</param>
        public NonSerializedCacheAgent(ICacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            _cacheAgent = cacheAgent;
        }

        /// <summary>
        /// Loads the specified key (generic mode).
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="key">Key of the cached item</param>
        /// <returns></returns>
        public T Load<T>(string key)
        {
            return (T)Load(key);
        }

        /// <summary>
        /// Loads the specified key
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <returns></returns>
        public object Load(string key)
        {
            return _cacheAgent.Load(key);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        public void Remove(string key)
        {
            _cacheAgent.Remove(key);
        }

        /// <summary>
        /// Store item in cache
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <param name="item">Item to store</param>
        public void Store(string key, object item)
        {
            _cacheAgent.Store(key, item);
        }

        /// <summary>
        /// Store item in cache with dependency on Tridion items
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <param name="item">Item to store</param>
        /// <param name="dependOnTcmUris">List of TCM URIs of Tridion items</param>
        public void Store(string key, object item, List<string> dependOnTcmUris)
        {
            _cacheAgent.Store(key, item, dependOnTcmUris);
        }

        /// <summary>
        /// Store item in cache using a specific region
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <param name="region">Region to use (this corresponds with an appSetting 'DD4T.CacheSettings.[region]' in the Web.config</param>
        /// <param name="item">Item to store</param>
        public void Store(string key, string region, object item)
        {
            _cacheAgent.Store(key, region, item);
        }

        /// <summary>
        /// Store item in cache using a specific region
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <param name="region">Region to use (this corresponds with an appSetting 'DD4T.CacheSettings.[region]' in the Web.config</param>
        /// <param name="item">Item to store</param>
        /// <param name="dependOnTcmUris">List of TCM URIs of Tridion items</param>
        public void Store(string key, string region, object item, List<string> dependOnTcmUris)
        {
            _cacheAgent.Store(key, region, item, dependOnTcmUris);
        }
    }
}