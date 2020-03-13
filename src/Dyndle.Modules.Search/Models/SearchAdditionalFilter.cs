using System.Collections.Generic;

namespace Dyndle.Modules.Search.Models
{
    /// <summary>
    /// Generic search filter
    /// </summary>
    public class SearchAdditionalFilter
    {
        public Dictionary<string,string> Query { get; set; }
        public string Separator { get; set; } = "OR";
    }
}