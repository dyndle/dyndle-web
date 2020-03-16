using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Search.Builders;
using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Models;

namespace Dyndle.Modules.Search.Binders
{
    /// <summary>
    /// Base search query binder, to build a query object from request parameters. 
    /// Can be extended/replaced as appropriate in specific brand implementations
    /// </summary>
    public class SearchQueryBinder : DefaultModelBinder, IModelBinderProvider
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var routeData = controllerContext.RouteData.Values.FirstOrDefault(x => x.Key.Equals("entity"));
            EntityModel entityModel = (EntityModel)routeData.Value;
            var routeValues = entityModel.MvcData.RouteValues.ToDictionary(x => x.Key, y => y.Value);
            var resultsPerPage = (entityModel as IHasPageSize).ResultsPerPage.ToString(CultureInfo.InvariantCulture);
            var groupingRowSize = ConfigurationManager.AppSettings[SearchConstants.Settings.SearchGroupingPageSize];

            if (typeof(IFilterFields).IsInstanceOfType(entityModel))
            {
                IFilterFields filterFields = entityModel as IFilterFields;

                routeValues = filterFields?.FilterByFields != null
                    ? routeValues.Union(filterFields.FilterByFields).ToDictionary(x => x.Key, x => x.Value)
                    : routeValues;

                if (filterFields?.FilterBySchemas != null)
                {
                    var filterBySchema = filterFields.FilterBySchemas.Select(x => new KeyValuePair<string, string>("schemaid", x))
                        .ToDictionary(x => x.Key, x => x.Value);

                    routeValues = routeValues.Union(filterBySchema).ToDictionary(x => x.Key, x => x.Value);
                }
            }

            bool isGroupBySearch = false;
            var groupByField = string.Empty;

            if (typeof(ISearchGroupBy).IsInstanceOfType(entityModel))
            {
                ISearchGroupBy groupByFields = entityModel as ISearchGroupBy;
                if (groupByFields != null && !string.IsNullOrWhiteSpace(groupByFields.GroupByField))
                {
                    routeValues.Add("groupByField", groupByFields.GroupByField);
                    groupByField = groupByFields.GroupByField;
                    isGroupBySearch = true;
                }
            }

            if (typeof(IHasSortOrder).IsInstanceOfType(entityModel))
            {
                routeValues.Add("sort", (entityModel as IHasSortOrder).SortOrder);
            }

            routeValues.Add("hits", isGroupBySearch ? groupingRowSize : resultsPerPage);

            var searchQueryBuilder = new SearchQueryBuilder(controllerContext, bindingContext, routeValues);
            var isDynamicList = typeof(IDynamicList).IsInstanceOfType(entityModel);
            if (string.IsNullOrWhiteSpace(searchQueryBuilder.SearchQueryString) && !isDynamicList) return null;

            var query = searchQueryBuilder.SearchQuery;
            if (isGroupBySearch)
            {
                query.GroupingPageSize = int.Parse(resultsPerPage);
            }

            query.IsGrouped = isGroupBySearch;
            query.GroupByField = groupByField;
            return query;
        }

        /// <summary>
        /// Get the binder for a particular model type
        /// </summary>
        /// <param name="modelType">The model type to check</param>
        /// <returns>This model binder if the model has the appropriate type, null otherwise</returns>
        public IModelBinder GetBinder(Type modelType)
        {
            if (typeof(SearchQuery).IsAssignableFrom(modelType) || typeof(IDynamicList).IsAssignableFrom(modelType)) return this;

            return null;
        }
    }
}
