using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyndle.Modules.Core.Models;

namespace Trivident.Modules.Search.Models
{
    public class SearchGroupByItem
    {
        public string Name { get; set; }
        public EntityModel Item { get; set; }
    }
}
