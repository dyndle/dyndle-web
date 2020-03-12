using System.Collections.Generic;
using Trivident.Modules.Core.Models;

namespace Trivident.Modules.Search.Models
{
    public class SearchGroupByItems
    {
        public string Name { get; set; }
        public List<EntityModel> Items { get; set; }
        public int Page { get; set; } = 1;
    }
}
