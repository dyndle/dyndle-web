using Trivident.Modules.Core.Models;
using System;
using System.Collections.Generic;
using Trivident.Modules.Search.Models;

namespace Trivident.Modules.Search.Contracts
{
    /// <summary>
    /// Interface for generic search results, containing both query and result data
    /// </summary>
    public interface ISearchResults : IEntityModel
    {
        SearchQuery Query { get; set; }
        List<ISearchResultItem> Items { get; set; }
        int Total { get; set; }
        int End { get; }
        int CurrentPage { get; }
        bool HasMore { get; }
        String ErrorMessage { get; set; }
    }
}