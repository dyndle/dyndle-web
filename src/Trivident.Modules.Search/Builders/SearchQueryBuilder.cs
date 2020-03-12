using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Trivident.Modules.Search.Extensions;
using Trivident.Modules.Search.Contracts;
using Trivident.Modules.Search.Models;

namespace Trivident.Modules.Search.Builders
{
    public class SearchQueryBuilder
    {
        private ControllerContext ControllerContext { get; set; }
        private ModelBindingContext BindingContext { get; set; }
        private Dictionary<string, string> RouteValues { get; set; }
        private SearchAdditionalFilter AdditionalFilter { get; set; }

        /// <inheritdoc />
        public SearchQueryBuilder(ControllerContext controllerContext, ModelBindingContext bindingContext, Dictionary<string, string> routeValues) : this(controllerContext, bindingContext, routeValues, null)
        {
        }


        /// <inheritdoc />
        public SearchQueryBuilder(ControllerContext controllerContext, Dictionary<string, string> routeValues, SearchAdditionalFilter additionalFilter) : this(controllerContext, null, routeValues, additionalFilter)
        {
        }

        /// <inheritdoc />
        public SearchQueryBuilder(ControllerContext controllerContext, Dictionary<string, string> routeValues) : this(controllerContext, null, routeValues, null)
        {
        }

        /// <inheritdoc />
        public SearchQueryBuilder(ControllerContext controllerContext, ModelBindingContext bindingContext, Dictionary<string, string> routeValues, SearchAdditionalFilter additionalFilter)
        {
            this.ControllerContext = controllerContext;
            this.BindingContext = bindingContext;
            RouteValues = routeValues ?? throw new ArgumentNullException(nameof(routeValues));
            AdditionalFilter = additionalFilter;
        }

        public SearchQuery SearchQuery
        {
            get
            {
                var hasGrouping = RouteValues.Any(x => x.Key.Equals("groupByField", StringComparison.Ordinal));
                if (hasGrouping)
                {
                    RouteValues.Remove("groupByField");
                }

                var pageHits = RouteValues.FirstOrDefault(x => x.Key.Equals("hits", StringComparison.Ordinal));
                int.TryParse(ConfigurationManager.AppSettings[SearchConstants.Settings.SearchPageSize], out int pageSize);
                pageSize = this.BindIntValue("hits", pageSize);

                if (!pageHits.Equals(new KeyValuePair<string, string>()))
                {
                    int.TryParse(pageHits.Value, out pageSize);
                    RouteValues.Remove("hits");
                }

                pageSize = pageSize > 0 ? pageSize : 10;

                var page = this.BindIntValue("p");
                var offset = this.BindIntValue("offset") + 1;

                var sort = RouteValues.FirstOrDefault(x => x.Key.Equals("sort", StringComparison.Ordinal));
                if (!sort.Equals(new KeyValuePair<string, string>()))
                {
                    RouteValues.Remove("sort");
                }

                return new SearchQuery()
                {
                    QueryText = this.SearchQueryString,
                    Query = this.BuildQuery(),
                    Start = hasGrouping ? 1 : page > 1 ? ((page - 1) * pageSize) + 1 : offset,
                    PageSize = pageSize,
                    Filters = this.BindFilters(),
                    CurrentPage = page,
                    Sort = sort.Value
                };
            }
        }

        public string SearchQueryString => BindStringValue("q") ?? BindStringValue("query");
        public string BindFilters()
        {
            return BindFilters(false);
        }

        public string BindFilters(bool ignoreQuerystring)
        {
            if (ignoreQuerystring)
            {
                return RouteValues != null && RouteValues.Any()
                    ? string.Join("&fq=", RouteValues.Select(x => $"{x.Key}:{x.Value}"))
                    : string.Empty;
            }

            var parameters = ControllerContext.HttpContext.Request.Params;

            var filters = parameters.AllKeys.Where(x => x.StartsWith("facet_"))
                .Select(p => new SearchFilter {Name = p.Replace("facet_", string.Empty), Value = parameters[p]})
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Value)).ToList();

            filters.AddRange(RouteValues.Select(r => new SearchFilter() { Name = r.Key, Value = r.Value }));

            var result = string.Join("&fq=", filters.Select(x => $"{x.Name}:{x.Value}"));

            return AdditionalFilter != null ? $"{result}&fq={AdditionalFilter.Query.JoinFields(AdditionalFilter.Separator)}" : result;
        }

        public Dictionary<string, string> BindAdditionalQueryParameters()
        {
            var parameters = new Dictionary<string, string>();
            var type = BindIntValue("type");
            if (type == 0) return parameters;
            parameters.Add("type", type.ToString());
            return parameters;
        }

        public int BindIntValue(string parameter, int defaultValue = 0)
        {
            int res = defaultValue;
            if (BindingContext == null) return res;

            var result = BindingContext.ValueProvider.GetValue(parameter);
            if (result?.AttemptedValue == null) return res;
            Int32.TryParse(result.AttemptedValue, out res);
            return res;
        }

        public string BindStringValue(string parameter, string defaultValue = null)
        {
            string res = defaultValue;
            if (BindingContext == null) return res;

            var result = BindingContext.ValueProvider.GetValue(parameter);
            if (result?.AttemptedValue == null) return res;
            res = result.AttemptedValue;
            return res;
        }
        /// <summary>
        /// Query Builder
        /// Default Fields: title,body
        /// </summary>
        /// <returns></returns>
        public string BuildQuery()
        {
            var fields = SearchConstants.Settings.BaseFields.GetConfigurationValue() ?? "title,body";
            return BuildQuery(fields);
        }

        /// <summary>
        /// Query Builder
        /// </summary>
        /// <param name="fields">The Fields</param>
        /// <param name="wildcard">Wildcard Accepted</param>
        /// <param name="proximityQuery">A proximity query, is like a phrase query with a tilda (~) followed by a slop that specifies the number of term position moves (edits) allowed.</param>
        /// <param name="queryOperator">The default operator is “OR”, meaning that clauses are optional.</param>
        /// <returns></returns>
        public string BuildQuery(string fields, bool wildcard = true, int proximityQuery = 0, string queryOperator = "or")
        {
            var oOperator = $" {queryOperator.ToUpperInvariant()} ";

            if (string.IsNullOrWhiteSpace(SearchQueryString) && RouteValues == null) return SearchQueryString;
            if (string.IsNullOrWhiteSpace(SearchQueryString) && RouteValues != null) return $"{string.Join(oOperator, fields.Split(',').Select(f => $"+{f}:*"))}";

            var hasMultiple = SearchQueryString.Contains(" ");
            var wildCardA = wildcard && !hasMultiple ? "*" : string.Empty;
            var multiTerm = hasMultiple ? "\"" : string.Empty;
            var pharseQuery = hasMultiple && proximityQuery > 0
                ? $"~{proximityQuery}"
                : string.Empty;

            return $"{string.Join(oOperator, fields.Split(',').Select(f => $"+{f}:({multiTerm}{SearchQueryString}{multiTerm}{pharseQuery}{wildCardA})"))}";
        }
    }
}
