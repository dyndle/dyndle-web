using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Management.Models
{
    public class CacheItem
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public string Region { get; set; }
    }
}
