using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dyndle.Modules.Search.Models
{
    public class SolrSearchFilterByResponse
    {
        [JsonProperty("response")]
        public SearchFilterByResponse Response { get; set; }
    }

    public class SearchFilterByResponse
    {
        [JsonProperty("docs")]
        public List<object> Items { get; set; }

        [JsonProperty("numFound")]
        public int Total { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }
    }
}
