using System;
using System.Collections.Generic;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Search.Contracts;

namespace Dyndle.Modules.Search.Models
{
    /// <summary>
    /// Example implementation of ISearchResults which can be used in example views.
    /// Typically you would implement your own version with CMS specific fields for heading and other content/configuration
    /// </summary>
    public class SearchResults : EntityModel, ISearchResults
    {
        public SearchQuery Query { get; set; }

        public List<ISearchResultItem> Items { get; set; }

        public List<SearchFilter> AvailableFilters { get; set; }

        public int Total { get; set; }

        public int End
        {
            get
            {
                return this.Total > 0 && this.Query!=null ? this.Query.Start + this.Items.Count - 1 : 0;
            }
        }
        public String ErrorMessage { get; set; }

        public int CurrentPage
        {
            get
            {
                return this.Query==null ? 1 : (this.Query.Start - 1)  / this.Query.PageSize + 1;
            }
        }

        public bool HasMore => (this.End < this.Total);
    }
}