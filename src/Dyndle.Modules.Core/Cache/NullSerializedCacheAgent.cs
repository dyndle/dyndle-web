using System.Collections.Generic;

namespace Dyndle.Modules.Core.Cache
{
    /// <summary>
    /// Null CacheAgent that will be registered when Cache is disabled
    /// User for cache that needs to have a different instance of the object (clone)
    /// </summary>
    /// <seealso cref="ISerializedCacheAgent" />
    public class NullSerializedCacheAgent : ISerializedCacheAgent
    {
        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        public object Load(string key)
        {
            return null;
        }

        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public T Load<T>(string key)
        {
            return default(T);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            // nothing to do, this cache agent doesn't really store anything so it doesn't have to remove anything either
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void Store(string key, object item)
        {
            // nothing to do, this cache agent doesn't really store anything 
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="region">The region.</param>
        /// <param name="item">The item.</param>
        public void Store(string key, string region, object item)
        {
            // nothing to do, this cache agent doesn't really store anything 
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="dependOnTcmUris">The depend on TCM uris.</param>
        public void Store(string key, object item, List<string> dependOnTcmUris)
        {
            // nothing to do, this cache agent doesn't really store anything
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="region">The region.</param>
        /// <param name="item">The item.</param>
        /// <param name="dependOnTcmUris">The depend on TCM uris.</param>
        public void Store(string key, string region, object item, List<string> dependOnTcmUris)
        {
            // nothing to do, this cache agent doesn't really store anything
        }
    }
}