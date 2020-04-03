using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Models;
using Dyndle.Modules.Search.Providers;

namespace Dyndle.Modules.Search.Html
{
    /// <summary>
    /// Html Helpers to directly make Search API Calls
    /// </summary>
    public static class SearchHelpers
    {
        private static readonly ISearchLinkResolver SearchLinkResolver = DependencyResolver.Current.GetService<ISearchLinkResolver>();
        private static readonly ISearchProvider SearchProvider = DependencyResolver.Current.GetService<ISearchProvider>();

        /// <summary>
        /// Uses Search API to fetch SearchResults
        /// </summary>
        /// <param name="htmlHelper">current HtmlHelper</param>
        /// <param name="query">search query to execute</param>
        /// <returns></returns>
        public static SearchResults Search(this HtmlHelper htmlHelper, SearchQuery query)
        {
            var result = SearchProvider.ExecuteQuery<SearchResults>(query);
            if (result != null && result.Items.Count > 0)
            {
                result.Items.ForEach(i => SearchLinkResolver.Resolve(i));
            }
            return result;
        }

        /// <summary>
        /// Renders search results for the given search query with the given view
        /// </summary>
        /// <param name="htmlHelper">current HtmlHelper</param>
        /// <param name="viewName">view name to render</param>
        /// <param name="query">search query to execute</param>
        public static void RenderSearchResults(this HtmlHelper htmlHelper, string viewName, SearchQuery query)
        {
            htmlHelper.RenderPartial(viewName, htmlHelper.Search(query));
        }
    }
}