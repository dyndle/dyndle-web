using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivident.Modules.Management.Models;

namespace Trivident.Modules.Management.Contracts
{
    public interface ICacheProvider
    {
        IEnumerable<CacheItem> GetList();
        CacheItem GetItem(string key);
        void RemoveItem(string key);
        void ClearCache();
    }
}
