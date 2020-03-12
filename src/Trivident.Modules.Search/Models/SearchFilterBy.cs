using System.Collections.Generic;
using Trivident.Modules.Search.Contracts;

namespace Trivident.Modules.Search.Models
{
    public class SearchFilterBy : ISearchFilterBy
    {
        public List<object> Items { get; set; }
        public int Total { get; set; }
        public string ErrorMessage { get; set; }
    }
}
