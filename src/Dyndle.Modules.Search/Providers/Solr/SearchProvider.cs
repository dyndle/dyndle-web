using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Environment;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Convertors;
using Dyndle.Modules.Search.Extensions;
using Dyndle.Modules.Search.Models;
using Menon.Me.ModelToQuerystring;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dyndle.Modules.Search.Providers.Solr
{
    /// <summary>
    /// Solr search provider
    /// </summary>
    public class SearchProvider : ISearchProvider
    {
        private readonly ILogger _logger;
        private readonly ISiteContext _context;
        protected readonly IDD4TConfiguration _configuration;
        public SearchProvider(ISiteContext context, ILogger logger, IDD4TConfiguration configuration)
        {
            logger.ThrowIfNull(nameof(logger));
            _logger = logger;
            context.ThrowIfNull(nameof(context));
            _context = context;
            _configuration = configuration;
        }


        /// <summary>
        /// Execute a search against Solr
        /// </summary>
        /// <typeparam name="T">The type of ISearchResults object to return</typeparam>
        /// <param name="query">The search query</param>
        /// <returns>ISearchResults object containing results</returns>
        public T ExecuteQuery<T>(SearchQuery query) where T : ISearchResults
        {
            ISearchResults results = (ISearchResults)Activator.CreateInstance(typeof(T));

            try
            {
                var searchUrl = SearchConstants.Settings.BaseUrl.GetConfigurationValue();
                query.Start = query.Start - 1;

                //Type type = typeof(TestResultItem);
                //ISolrSearchResultItem resultItem = (ISolrSearchResultItem) Activator.CreateInstance(typeof(TestResultItem));

                searchUrl = $"{searchUrl}?{query.ToQueryString()}";
                _logger.Information("SearchProvider.Solr.ExecuteQuery URL: {0}", searchUrl);
                var content = ExecuteRequest(searchUrl);

                List<string> errors = new List<string>();

                var data = JsonConvert.DeserializeObject<SolrSearchResponse>(content, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>() { new TConverter() },
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    }
                    
                });

                _logger.Warning("Solr SearchProvider DeserializeObject Warnings: {0}", string.Join(",", errors.Select(x => x)));

                results.Total = data.Response.Total;
                results.Items = data.Response.Items.Select(x =>
                {
                    x.Body = x.Body?.Select(y => y.TruncateAtWord(200)).ToList();
                    x.Summary = x.Summary ?? x.Body;
                    x.Urls = x.Urls?.Select(y => y.CleanUrl(_configuration.WelcomeFile)).ToList();
                    return x;
                }).ToList();

                _logger.Debug("Search returned {0} results for URL: {1}", results.Total, searchUrl);
            }
            catch (Exception ex)
            {
                //TODO - potentially handle fixable errors (invalid query syntax etc.) differently
                if (ex is AggregateException)
                {
                    foreach (var e1 in ((AggregateException)ex).InnerExceptions)
                    {
                        _logger.Warning("exception while searching: " + e1.Message + Environment.NewLine + e1.StackTrace);
                    }
                }
                _logger.Error("Error executing search: " + ex.Message + Environment.NewLine + ex.StackTrace);
                results.ErrorMessage = _context.GetLabel(SearchConstants.Labels.SearchError);
            }
            //Store search state in results for reuse
            results.Query = query;
            return (T)results;
        }

        public T ExecuteFilterByQuery<T>(SearchQuery query) where T : ISearchFilterBy
        {
            ISearchFilterBy results = (ISearchFilterBy)Activator.CreateInstance(typeof(T));

            try
            {
                var searchUrl = SearchConstants.Settings.BaseUrl.GetConfigurationValue();
                query.Start = query.Start - 1;

                searchUrl = $"{searchUrl}?{(query.PageSize > 0 ? query.ToQueryString() : query.ToQueryString().Replace("&rows=", "&row="))}";
                _logger.Information("SearchProvider.Solr.ExecuteFilterByQuery URL: {0}", searchUrl);

                var content = ExecuteRequest(searchUrl);

                List<string> errors = new List<string>();

                var data = JsonConvert.DeserializeObject<SolrSearchFilterByResponse>(content, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>() { new TConverter() },
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    }

                });

                _logger.Warning("Solr SearchProvider DeserializeObject Warnings: {0}", string.Join(",", errors.Select(x => x)));

                results.Items = data.Response.Items;

                _logger.Debug("Search returned {0} results for URL: {1}", results.Total, searchUrl);
            }
            catch (Exception ex)
            {
                //TODO - potentially handle fixable errors (invalid query syntax etc.) differently
                if (ex is AggregateException)
                {
                    foreach (var e1 in ((AggregateException)ex).InnerExceptions)
                    {
                        _logger.Warning("exception while searching: " + e1.Message + Environment.NewLine + e1.StackTrace);
                    }
                }
                _logger.Error("Error executing search: " + ex.Message + Environment.NewLine + ex.StackTrace);
                results.ErrorMessage = _context.GetLabel(SearchConstants.Labels.SearchError);
            }

            return (T)results;
        }

        public object ExecuteFacetQuery(SearchQuery query)
        {
            object results = null;
            try
            {
                var searchUrl = SearchConstants.Settings.BaseUrl.GetConfigurationValue();
                query.Start = query.Start - 1;

                searchUrl = $"{searchUrl}?{query.ToQueryString()}";
                _logger.Information("SearchProvider.Solr.ExecuteFacetQuery URL: {0}", searchUrl);

                var content = ExecuteRequest(searchUrl);

                List<string> errors = new List<string>();

                var data = JsonConvert.DeserializeObject<SearchFacet>(content, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>() { new TConverter() },
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        errors.Add(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    }

                });

                results = data.Facets.Fields;

                _logger.Warning("Solr SearchProvider DeserializeObject Warnings: {0}", string.Join(",", errors.Select(x => x)));

                _logger.Debug("Search returned results for URL: {0}", searchUrl);
            }
            catch (Exception ex)
            {
                //TODO - potentially handle fixable errors (invalid query syntax etc.) differently
                if (ex is AggregateException)
                {
                    foreach (var e1 in ((AggregateException)ex).InnerExceptions)
                    {
                        _logger.Warning("exception while searching: " + e1.Message + Environment.NewLine + e1.StackTrace);
                    }
                }
                _logger.Error("Error executing search: " + ex.Message + Environment.NewLine + ex.StackTrace);
                //results.ErrorMessage = _context.GetLabel(SearchConstants.Labels.SearchError);
            }

            return results;
        }

        /// <summary>
        /// Generate a query string to be used to provide paging functionality
        /// </summary>
        /// <param name="query">The search query to execute</param>
        /// <param name="pageNumber">The page number to display results for</param>
        /// <returns></returns>
        public static string GetPageQueryString(SearchQuery query, int pageNumber)
        {
            int offset = query.PageSize * (pageNumber - 1);
            return BuildQueryString(query, null, null, offset);
        }

        /// <summary>
        /// Generate a query string to be used to provide filtering functionality
        /// </summary>
        /// <param name="query">The search query to execute (without the filter applied)</param>
        /// <param name="filterName">The filter name to add to the query</param>
        /// <param name="filterValue">The filter value to add to the query</param>
        /// <returns></returns>
        public static string GetFilterQueryString(SearchQuery query, String filterName, String filterValue)
        {
            return BuildQueryString(query, filterName, filterValue, 0);
        }

        protected String ExecuteRequest(String uri)
        {
 
            using (var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            })
            {
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = GetTimeout();
                    _logger.Debug("Running query with timeout of {0} milliseconds and URL: {1}", client.Timeout.TotalMilliseconds, uri);
                    var response = client.GetAsync(uri).Result;
                    //will throw an exception if not successful
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        protected TimeSpan GetTimeout()
        {
            var timeoutConfig = _context.GetApplicationSetting(SearchConstants.Settings.TimeoutMilliseconds);
            int timeout = 0;
            if (!Int32.TryParse(timeoutConfig, out timeout))
            {
                timeout = 5000;
                _logger.Warning("No Search.TimeoutMilliseconds configuration found. Defaulting to " + timeout);
            }
            return TimeSpan.FromMilliseconds(timeout);
        }

        protected virtual List<SearchFilter> ParseFilterData(dynamic filterdata, SearchQuery originalQuery)
        {
            var filters = new List<SearchFilter>();
            var queryString = BuildQueryString(originalQuery);
            foreach (var facet in filterdata)
            {
                foreach (var item in facet.selectableItems)
                {
                    var filter = new SearchFilter
                    {
                        DisplayName = item.displayName,
                        NumberOfHits = item.count
                    };
                    foreach (var param in item.@params)
                    {
                        //Only feasible way to work out the actual filter name/value is to check if its missing from the querystring
                        //This is due to the weird way filters are modelled in the Findwise JSON results
                        if (queryString.Contains($"{param.name}={param.values[0]}")) continue;

                        filter.Name = param.name;
                        filter.Value = HttpUtility.UrlDecode((string)param.values[0]);
                        break;
                    }
                    filter.Selected = false;
                    filters.Add(filter);
                }
                foreach (var item in facet.appliedItems)
                {
                    var filter = new SearchFilter
                    {
                        DisplayName = item.displayName,
                        NumberOfHits = item.count
                    };
                    foreach (var param in item.@params)
                    {
                        filter.Name = param.name;
                        filter.Value = HttpUtility.UrlDecode((String)param.values[0]);
                        break;
                    }
                    filter.Selected = true;
                    filters.Add(filter);
                }
            }
            return filters;
        }

        protected static String BuildQueryString(SearchQuery query, string filterName = null, string filterValue = null, int offsetOverride = -1)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["q"] = query.QueryText;
            queryString["hits"] = (query.PageSize > 0 ? query.PageSize : 10).ToString();
            queryString["offset"] = (offsetOverride >= 0 ? offsetOverride : (query.Start - 1)).ToString();

            if (query.Filters == null) return queryString.ToString();

            if (!string.IsNullOrEmpty(filterName) && !string.IsNullOrEmpty(filterValue))
            {
                queryString[filterName] = filterValue;
            }
            return queryString.ToString();
        }
    }
}