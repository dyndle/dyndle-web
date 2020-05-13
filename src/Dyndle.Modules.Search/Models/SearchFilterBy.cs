using System.Collections.Generic;
using Dyndle.Modules.Search.Contracts;

namespace Dyndle.Modules.Search.Models
{
    public class SearchFilterBy : ISearchFilterBy
    {
        public List<object> Items { get; set; }
        public int Total { get; set; }
        public string ErrorMessage { get; set; }
    }
}
