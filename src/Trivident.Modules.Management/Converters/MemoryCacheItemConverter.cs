using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using TrividentModel = Trivident.Modules.Management.Models;

namespace Trivident.Modules.Management.Converters
{
    public class MemoryCacheItemConverter
    {
        public TrividentModel.CacheItem Convert(CacheItem sourceItem)
        {
            TrividentModel.CacheItem targetItem = new TrividentModel.CacheItem();
            targetItem.Key = sourceItem.Key;
            targetItem.Value = sourceItem.Value;
            targetItem.Region = sourceItem.RegionName;
            return targetItem;
        }
        public TrividentModel.CacheItem Convert(KeyValuePair<string,object> sourceItem)
        {
            TrividentModel.CacheItem targetItem = new TrividentModel.CacheItem();
            targetItem.Key = sourceItem.Key;
            targetItem.Value = sourceItem.Value;
            return targetItem;
        }
    }
}
