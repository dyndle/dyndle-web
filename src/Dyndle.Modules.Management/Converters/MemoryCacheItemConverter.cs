using System.Collections.Generic;
using CacheItem = Dyndle.Modules.Management.Models.CacheItem;

namespace Dyndle.Modules.Management.Converters
{
    public class MemoryCacheItemConverter
    {
        public CacheItem Convert(System.Runtime.Caching.CacheItem sourceItem)
        {
            CacheItem targetItem = new CacheItem();
            targetItem.Key = sourceItem.Key;
            targetItem.Value = sourceItem.Value;
            targetItem.Region = sourceItem.RegionName;
            return targetItem;
        }
        public CacheItem Convert(KeyValuePair<string,object> sourceItem)
        {
            CacheItem targetItem = new CacheItem();
            targetItem.Key = sourceItem.Key;
            targetItem.Value = sourceItem.Value;
            return targetItem;
        }
    }
}
