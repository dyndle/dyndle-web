using System.Collections.Generic;
using Dyndle.Modules.Core.Models;
using Trivident.Modules.Search.Models;

namespace Trivident.Modules.Search.Contracts
{
    public interface IDynamicList : IEntityModel
    {
        int ItemsPerPage { get; set; }
        Dictionary<string,string> Facets { get; set; }
        List<EntityModel> Items { get; set; }
        List<SearchGroupByItems> GroupedItems { get; set; }
        SearchQuery Query { get; set; }
        int Total { get; set; }

        /// <summary>
        /// Gets or sets the total items based on the GroupingPageSize and TotalPages
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the total pages based on the grouping.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        int TotalPages { get; set; }
        int End { get; set; }
        string ErrorMessage { get; set; }
        int CurrentPage { get; set; }
        bool HasMore { get; set; }
    }
}
