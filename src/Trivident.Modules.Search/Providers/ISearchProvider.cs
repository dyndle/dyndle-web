using Trivident.Modules.Search.Contracts;
using Trivident.Modules.Search.Models;

namespace Trivident.Modules.Search.Providers
{
    /// <summary>
    /// Interface to provide abstraction of search functionality from underlying search technology
    /// </summary>
    public interface ISearchProvider
    {
        T ExecuteQuery<T>(SearchQuery query) where T : ISearchResults;

        T ExecuteFilterByQuery<T>(SearchQuery query) where T : ISearchFilterBy;

        object ExecuteFacetQuery(SearchQuery query);
    }
}