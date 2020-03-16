using System.Collections.Generic;
using Dyndle.Modules.Management.Models;

namespace Dyndle.Modules.Management.Contracts
{
    public interface ICacheProvider
    {
        IEnumerable<CacheItem> GetList();
        CacheItem GetItem(string key);
        void RemoveItem(string key);
        void ClearCache();
    }
}
