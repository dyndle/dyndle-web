using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Environment;
using Newtonsoft.Json;
using Trivident.Modules.Search.Builders;
using Trivident.Modules.Search.Extensions;
using Trivident.Modules.Search.Contracts;
using Trivident.Modules.Search.Models;
using Trivident.Modules.Search.Providers;
using Trivident.Modules.Search.Providers.Solr;

namespace Trivident.Modules.Search.Services
{
    public class SearchQueryService
    {
        private ControllerContext ControllerContext { get; }
        private readonly ISearchProvider _searchProvider;
        public SearchQueryService(ControllerContext controllerContext)
        {
            ControllerContext = controllerContext;
            ISiteContext siteContext = DependencyResolver.Current.GetService(typeof(ISiteContext)) as ISiteContext;
            ILogger logger = DependencyResolver.Current.GetService(typeof(ILogger)) as ILogger;
            IDD4TConfiguration dd4TConfiguration = DependencyResolver.Current.GetService(typeof(IDD4TConfiguration)) as IDD4TConfiguration;

            _searchProvider = new SearchProvider(siteContext, logger, dd4TConfiguration);
        }

        public List<T> FilterBy<T>(SearchQuery query)
        {
            var result = _searchProvider.ExecuteFilterByQuery<SearchFilterBy>(query)?.Items;

            var jsonData = JsonConvert.SerializeObject(result);
            var data = JsonConvert.DeserializeObject<List<T>>(jsonData);

            return data;
        }

        /// <summary>
        /// Filters the by.
        /// Default Page Size is 0
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        /// <returns>SearchResultItem</returns>
        public List<SearchResultItem> FilterBy(Dictionary<string, string> routeValues)
        {
            return FilterBy(routeValues, null);
        }

        /// <summary>
        /// Filters the by.
        /// Default Page Size is 0
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        /// <param name="additionalFilters">The additional filters.</param>
        /// <returns>SearchResultItem</returns>
        public List<SearchResultItem> FilterBy(Dictionary<string, string> routeValues, Dictionary<string, string> additionalFilters)
        {
            return FilterBy(routeValues, additionalFilters, null);
        }

        /// <summary>
        /// Filters the by.
        /// Default Page Size is 0
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        /// <param name="additionalFilters">The additional filters.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>SearchResultItem</returns>
        public List<SearchResultItem> FilterBy(Dictionary<string, string> routeValues, Dictionary<string, string> additionalFilters, string separator)
        {
            return FilterBy(routeValues, additionalFilters, separator, 0);
        }

        /// <summary>
        /// Filters the by.
        /// Default Page Size is 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routeValues">The route values.</param>
        /// <param name="additionalFilters">The additional filters.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>SearchResultItem</returns>
        public List<SearchResultItem> FilterBy(Dictionary<string, string> routeValues, Dictionary<string, string> additionalFilters, string separator, int pageSize)
        {
            return FilterBy<SearchResultItem>(routeValues, additionalFilters, separator, pageSize);
        }

        /// <summary>
        /// Filters the by.
        /// Default Page Size is 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routeValues">The route values.</param>
        /// <returns>T</returns>
        public List<T> FilterBy<T>(Dictionary<string, string> routeValues)
        {
            return FilterBy<T>(routeValues, null);
        }

        /// <summary>
        /// Filters the by.
        /// Default Page Size is 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routeValues">The route values.</param>
        /// <param name="additionalFilters">The additional filters.</param>
        /// <returns>T</returns>
        public List<T> FilterBy<T>(Dictionary<string, string> routeValues, Dictionary<string, string> additionalFilters)
        {
            return FilterBy<T>(routeValues, additionalFilters, "AND");
        }

        /// <summary>
        /// Filters the by.
        /// Default Page Size is 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routeValues">The route values.</param>
        /// <param name="additionalFilters">The additional filters.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>T</returns>
        public List<T> FilterBy<T>(Dictionary<string, string> routeValues, Dictionary<string, string> additionalFilters, string separator)
        {
            return FilterBy<T>(routeValues, additionalFilters, separator, 0);
        }

        /// <summary>
        /// Filters the by.
        /// Default Page Size is 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routeValues">The route values.</param>
        /// <param name="additionalFilters">The additional filters.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>T</returns>
        public List<T> FilterBy<T>(Dictionary<string, string> routeValues, Dictionary<string, string> additionalFilters, string separator, int pageSize)
        {
            var additionalFilter = new SearchAdditionalFilter() { Query = additionalFilters, Separator = separator };

            var queryBuilder = new SearchQueryBuilder(this.ControllerContext, routeValues, additionalFilter);
            var query = queryBuilder.SearchQuery;
            query.QueryText = "*";
            query.Query = queryBuilder.BuildQuery("*");
            query.PageSize = pageSize;
            query.FieldList = typeof(T).GetFieldListItemsAsString();
            var result = FilterBy<T>(query);

            var jsonData = JsonConvert.SerializeObject(result);
            var data = JsonConvert.DeserializeObject<List<T>>(jsonData);

            return data;
        }

        public SearchFacetItem FilterByFacets(Dictionary<string, string> routeValues, string fields)
        {
            var jsonData = GetFacetsData(routeValues, fields);
            return JsonConvert.DeserializeObject<SearchFacetItem>(jsonData);
        }

        public ISearchFacet FilterByFacets<T>(Dictionary<string, string> routeValues, string fields)
        {
            var jsonData = GetFacetsData(routeValues, fields);
            return JsonConvert.DeserializeObject<T>(jsonData) as ISearchFacet;
        }

        private string GetFacetsData(Dictionary<string, string> routeValues, string fields)
        {
            var queryBuilder = new SearchQueryBuilder(this.ControllerContext, routeValues);

            var query = queryBuilder.SearchQuery;
            query.QueryText = "*";
            query.Query = queryBuilder.BuildQuery("*");
            query.PageSize = 0;
            query.FieldList = "";
            query.Start = 1;

            var filters = queryBuilder.BindFilters(true);
            query.Filters = !string.IsNullOrWhiteSpace(fields)
                ? $"{filters}{fields.GetFacetFields()}"
                : query.Filters; // //todo model to querystring, instead of string fields for Facet Fields generation
            var result = _searchProvider.ExecuteFacetQuery(query);
            return JsonConvert.SerializeObject(result);
        }
    }
}
