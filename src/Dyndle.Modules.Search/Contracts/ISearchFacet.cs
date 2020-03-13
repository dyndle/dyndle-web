using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Dyndle.Modules.Search.Contracts
{
    public interface ISearchFacet
    {
        Dictionary<string, JToken> Items { get; set; }

        Dictionary<string, int> Facets { get; }
    }
}
