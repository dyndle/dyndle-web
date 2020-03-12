using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Trivident.Modules.Management.Contracts;
using Trivident.Modules.Management.Converters;
using TrividentModel = Trivident.Modules.Management.Models;

namespace Trivident.Modules.Management.Providers
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

        public TrividentModel.CacheItem GetItem(string key)
        {
            return converter.Convert(MemoryCache.Default.GetCacheItem(key));
        }

        public IEnumerable<TrividentModel.CacheItem> GetList()
        {
            return MemoryCache.Default.Select(a => converter.Convert(a));
        }

        public void RemoveItem(string key)
        {
            MemoryCache.Default.Remove(key);
        }
    }
}
