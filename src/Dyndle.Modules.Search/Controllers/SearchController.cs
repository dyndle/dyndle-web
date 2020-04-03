using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Attributes.Filter;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.Search.Contracts;
using Dyndle.Modules.Search.Extensions;
using Dyndle.Modules.Search.Models;
using Dyndle.Modules.Search.Providers;
using SearchResults = Dyndle.Modules.Search.Models.SearchResults;

namespace Dyndle.Modules.Search.Controllers
{
    /// <summary>
    /// Base Search controller, which can be used to execute and render searches
    /// </summary>
    public class SearchController : ModuleControllerBase
    {
        private readonly ISearchLinkResolver _searchLinkResolver;
        private readonly ISearchProvider _searchProvider;
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// Initialize the controller with appropriate providers
        /// </summary>
        /// <param name="contentProvider">The content provider</param>
        /// <param name="logger">The logger implementation</param>
        /// <param name="searchProvider">The search provider</param>
        /// <param name="viewModelFactory"></param>
        public SearchController(IContentProvider contentProvider, ILogger logger, ISearchProvider searchProvider, IViewModelFactory viewModelFactory, ISearchLinkResolver searchLinkResolver)
            : base(contentProvider, logger)
        {
            searchProvider.ThrowIfNull(nameof(searchProvider));
            _searchProvider = searchProvider;
            _searchLinkResolver = searchLinkResolver;
            _contentProvider = contentProvider;
            _logger = logger;
        }

        /// <summary>
        /// Populate and render a search results entity model for a given query
        /// </summary>
        /// <param name="entity">The search results configuration entity</param>
        /// <param name="query">The search query (typically bound from data in the request using the SearchQueryBinder)</param>
        /// <returns>Rendered search results</returns>
        public virtual ActionResult SiteSearch(ISearchResults entity, SearchQuery query)
        {
            var results = _searchProvider.ExecuteQuery<SearchResults>(query);
            MergeResults(entity, results);
            return View(entity.GetView(), entity);
        }

        public ActionResult List(IEntityModel entity, SearchQuery query = null)
        {
            if (query == null)
            {
                return View(entity.GetView(), entity);
            }

            var model = _searchProvider.ExecuteQuery<SearchResults>(query);

            IDynamicList dynamicList = (IDynamicList) entity;
            dynamicList.Total = model.Total;
            dynamicList.Query = model.Query;
            dynamicList.CurrentPage = model.CurrentPage;
            dynamicList.End = model.End;
            dynamicList.HasMore = model.HasMore;


            var items = model.Items?.Where(x => !string.IsNullOrWhiteSpace(x.Urls?.FirstOrDefault()))
                .Select(i => _contentProvider.BuildViewModel(new TcmUri(i.Urls.FirstOrDefault()))).Cast<EntityModel>().ToList();
            dynamicList.Items = items;

            if (query.IsGrouped)
            {
                var groupedItems = items.GroupByField(query.GroupByField, query.GroupingPageSize);
                var currentPage = query.CurrentPage > 0 ? query.CurrentPage : 1;
                dynamicList.GroupedItems = groupedItems.Where(x => x.Page.Equals(currentPage)).ToList();
                dynamicList.TotalPages = groupedItems.GroupBy(x => x.Page).Count();
                dynamicList.TotalItems = dynamicList.TotalPages * query.GroupingPageSize;

                dynamicList.Total = dynamicList.TotalItems > dynamicList.TotalPages ? model.Total : dynamicList.TotalItems;
            }

            return View(entity.GetView(), dynamicList);
        }

        /// <summary>
        /// Normal entity rendering - can be used in the case where we require only to write
        /// out search parameters/config from the source entity and not actually execute a search
        /// </summary>
        /// <param name="entity">The search configuration entity to render</param>
        /// <param name="query"></param>
        /// <param name="display"></param>
        /// <returns>Rendered search configuration content</returns>
        public virtual ActionResult Entity(IEntityModel entity, SearchQuery query, string display = null)
        {
            var view = entity.GetView();
            var results = (SearchResults)entity;

            if (query == null) return View(view, entity);

            // execute query and add results to the model
            var model = _searchProvider.ExecuteQuery<SearchResults>(query);

            if (model == null) return View(view, results);

            if (model.Items != null)
            {
                foreach (var item in model.Items)
                {
                    _searchLinkResolver.Resolve(item);
                }

                results.Items = model.Items;
            }
            results.Query = model.Query;
            results.Total = model.Total;

            return View(view, results);
        }

        /// <summary>
        /// Populate and render a search results entity model
        /// </summary>
        /// <param name="query">The search query (typically bound from data in the request using the SearchQueryBinder)</param>
        /// <param name="display">The view to use to render (if none specified JSON data will be returned)</param>
        /// <returns>Rendered search results (in HTML or JSON format, depending on use of the display parameter)</returns>
        [AjaxEnabled]
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Client, VaryByParam = "*")]
        public virtual ActionResult Index(SearchQuery query, string display = null)
        {
            var model = _searchProvider.ExecuteQuery<SearchResults>(query);
            if (display != null)
            {
                return View(display, model);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        private void MergeResults(ISearchResults entity, SearchResults results)
        {
            entity.Query = results.Query;
            entity.ErrorMessage = results.ErrorMessage;
            //entity.AvailableFilters = results.AvailableFilters;
            entity.Items = results.Items;
            entity.Total = results.Total;
        }
    }
}