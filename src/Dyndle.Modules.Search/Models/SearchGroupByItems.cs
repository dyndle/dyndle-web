using System.Collections.Generic;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Search.Models
{
    public class SearchGroupByItems
    {
        public string Name { get; set; }
        public List<EntityModel> Items { get; set; }
        public int Page { get; set; } = 1;
    }
}
