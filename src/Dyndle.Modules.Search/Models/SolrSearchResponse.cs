using System.Collections.Generic;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Utils;
using Dyndle.Modules.Core.Models.Query;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Extensions;
using Newtonsoft.Json;

namespace Dyndle.Modules.Search.Models
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

        public ISearchSummary GetSearchSummaryEntity()
        {
            var contentProvider = DependencyResolver.Current.GetService<IContentProvider>();
            if (ItemType == (int) QueryCriteria.ItemType.Component)
            {
                var viewModel = contentProvider.BuildViewModel(Id);
                return viewModel as ISearchSummary;
            }

            if (ItemType == (int) QueryCriteria.ItemType.Page)
            {
                var page = contentProvider.GetPage(new TcmUri(Id));
                var pubId = DependencyResolver.Current.GetService<IPublicationResolver>().ResolvePublicationId();
                if (page?.ComponentPresentations != null)
                {
                    foreach (var componentPresentation in page.ComponentPresentations)
                    {
                        var viewModel = contentProvider.BuildViewModel(componentPresentation);
                        if(viewModel is ISearchSummary searchSummary)
                        {
                            return searchSummary;
                        }
                    }
                    Logger.Warning($"Could not find ISearchSummary component presentation for page {Id}");
                }
                else
                {
                    Logger.Warning($"Could not find page with id {Id}");
                }
            }
            return null;
        } 
    }
}