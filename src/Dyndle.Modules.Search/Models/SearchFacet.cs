using System.Collections.Generic;
using Dyndle.Modules.Search.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dyndle.Modules.Search.Models
{
    public class SearchFacetItem : ISearchFacet
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> Items { get; set; }

        public Dictionary<string, int> Facets { get; set; }
    }

    public class SearchFacetResponse
    {
        public int NumFound { get; set; }
        public int Start { get; set; }
        public List<object> Docs { get; set; }
    }

    public class SearchFacetCounts
    {
        [JsonProperty("facet_fields")]
        public object Fields { get; set; }
    }

    public class SearchFacet
    {
        [JsonProperty("response")]
        public SearchFacetResponse Response { get; set; }

        [JsonProperty("facet_counts")]
        public SearchFacetCounts Facets { get; set; }
    }
}
