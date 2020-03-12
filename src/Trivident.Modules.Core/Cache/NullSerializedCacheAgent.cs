using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Cache
{
    /// <summary>
    /// Null CacheAgent that will be registered when Cache is disabled
    /// User for cache that needs to have a different instance of the object (clone)
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Cache.ISerializedCacheAgent" />
    public class NullSerializedCacheAgent : ISerializedCacheAgent
    {
        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Load(string key)
        {
            return null;
        }

        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
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
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void Store(string key, object item)
        {
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="region">The region.</param>
        /// <param name="item">The item.</param>
        public void Store(string key, string region, object item)
        {
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="dependOnTcmUris">The depend on TCM uris.</param>
        public void Store(string key, object item, List<string> dependOnTcmUris)
        {
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
        }
    }
}