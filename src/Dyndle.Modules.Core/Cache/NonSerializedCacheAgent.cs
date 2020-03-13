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
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The DD4T configuration.</param>
        public NonSerializedCacheAgent(ICacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            _cacheAgent = cacheAgent;
        }

        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Load<T>(string key)
        {
            return (T)Load(key);
        }

        public object Load(string key)
        {
            return _cacheAgent.Load(key);
        }

        public void Remove(string key)
        {
            _cacheAgent.Remove(key);
        }

        public void Store(string key, object item)
        {
            _cacheAgent.Store(key, item);
        }

        public void Store(string key, object item, List<string> dependOnTcmUris)
        {
            _cacheAgent.Store(key, item, dependOnTcmUris);
        }

        public void Store(string key, string region, object item)
        {
            _cacheAgent.Store(key, region, item);
        }

        public void Store(string key, string region, object item, List<string> dependOnTcmUris)
        {
            _cacheAgent.Store(key, region, item, dependOnTcmUris);
        }
    }
}