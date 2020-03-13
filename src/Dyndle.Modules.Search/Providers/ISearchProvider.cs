using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Models;

namespace Dyndle.Modules.Search.Providers
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