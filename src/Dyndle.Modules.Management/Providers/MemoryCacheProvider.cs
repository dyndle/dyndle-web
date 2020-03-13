using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Dyndle.Modules.Management.Contracts;
using Dyndle.Modules.Management.Converters;
using CacheItem = Dyndle.Modules.Management.Models.CacheItem;

namespace Dyndle.Modules.Management.Providers
{
    public class MemoryCacheProvider : ICacheProvider
    {
        MemoryCacheItemConverter converter;
        public MemoryCacheProvider()
        {
            converter = new MemoryCacheItemConverter();
        }

        public void ClearCache()
        {
            var allKeys = MemoryCache.Default.Select(o => o.Key);
            Parallel.ForEach(allKeys, key => MemoryCache.Default.Remove(key));
        }

        public CacheItem GetItem(string key)
        {
            return converter.Convert(MemoryCache.Default.GetCacheItem(key));
        }

        public IEnumerable<CacheItem> GetList()
        {
            return MemoryCache.Default.Select(a => converter.Convert(a));
        }

        public void RemoveItem(string key)
        {
            MemoryCache.Default.Remove(key);
        }
    }
}
