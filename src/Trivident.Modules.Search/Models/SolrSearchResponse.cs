using System.Collections.Generic;
using Newtonsoft.Json;
using Trivident.Modules.Search.Contracts;
using Trivident.Modules.Search.Extensions;

namespace Trivident.Modules.Search.Models
{
    public class SolrSearchResponse
    {
        [JsonProperty("response")]
        public SearchResponse Response { get; set; }
    }

    public class SearchResponse
    {
        [JsonProperty("docs")]
        public List<ISearchResultItem> Items { get; set; }

        [JsonProperty("numFound")]
        public int Total { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }
    }


    public class SearchResultItem : ISearchResultItem
    {
        [JsonProperty("title")]
        public List<string> Title { get; set; }

        [JsonProperty("body")]
        public List<string> Body { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public List<long> Type { get; set; }

        [JsonProperty("url")]
        public List<string> Urls { get; set; }

        [JsonProperty("imageurl")]
        public List<string> ImageUrl { get; set; }

        [JsonProperty("summary")]
        public List<string> Summary { get; set; }

        [JsonProperty("itemtype")]
        public List<string> ItemTypes { get; set; }

        public string Url { get; set; }
        public string HrefTarget { get; set; } = "_self";

        public int ItemType
        {
            get
            {
                int.TryParse(ItemTypes.GetValue(), out var result);
                return result;
            }
        }
    }
}