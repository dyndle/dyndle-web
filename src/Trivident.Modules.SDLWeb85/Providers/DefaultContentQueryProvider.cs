﻿using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Core.Contracts.ViewModels;
using Trivident.Modules.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using Tridion.ContentDelivery.DynamicContent.Query;
using Tridion.ContentDelivery.Taxonomies;
using DD4T.ContentModel.Contracts.Caching;
using DD4TModels = Trivident.Modules.Core.Models;

namespace Trivident.Modules.Core.Providers.Content
{
    /// <summary>
    /// Class DefaultContentQueryProvider.
    /// Provides a generic way to Query the broker for dynamic components, pages or keywords.
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Providers.Content.IContentQueryProvider" />
    public class DefaultContentQueryProvider : IContentQueryProvider
    {
        // Tridion template item IDs
        private const int ComponentTemplateItemId = 32;
        private const int PageTemplateItemId = 128;

        // cache keys
        private const string _cacheKeyFormat = "ContentQuery_{0}";
        private const string _cacheRegion = "ContentQuery";

        private readonly ISiteContext _context;
        private readonly ILogger _logger;
        private readonly IContentProvider _contentProvider;
        private readonly IPublicationResolver _publicationResolver;
        private readonly IViewModelResolver _viewModelResolver;
        private readonly ICacheAgent _cacheAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContentQueryProvider" /> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The context.</param>
        /// <param name="viewModelResolver">The view model resolver.</param>
        /// <param name="cacheAgent">The cache agent.</param>
        public DefaultContentQueryProvider(IContentProvider contentProvider, IPublicationResolver publicationResolver, ILogger logger, ISiteContext context, IViewModelResolver viewModelResolver, ICacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            context.ThrowIfNull(nameof(context));
            contentProvider.ThrowIfNull(nameof(contentProvider));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));
            logger.ThrowIfNull(nameof(logger));
            viewModelResolver.ThrowIfNull(nameof(viewModelResolver));

            _cacheAgent = cacheAgent;
            _context = context;
            _logger = logger;
            _publicationResolver = publicationResolver;
            _contentProvider = contentProvider;
            _viewModelResolver = viewModelResolver;
        }

        /// <summary>
        /// Finds the schema identifier by model type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>TcmUri.</returns>
        private TcmUri FindSchemaIdByModelType<T>() where T : IViewModel
        {
            var meta = _viewModelResolver.GetCustomAttribute<IContentModelAttribute>(typeof(T));
            var schemaId = _context.GetSchemaIdByRootElementName(meta.SchemaRootElementName);
            if (schemaId.IsNull())
                schemaId = _context.GetSchemaIdByTitle(meta.SchemaRootElementName);

            schemaId.ThrowIfNull(nameof(schemaId));

            return schemaId;
        }


        /// <summary>
        /// Finds the category title by XML.
        /// </summary>
        /// <param name="xmlName">Name of the XML.</param>
        /// <returns>System.String.</returns>
        private string FindCategoryTitleByXmlName(string xmlName)
        {
            xmlName.ThrowIfNull(nameof(xmlName));
            return _context.GetCategoryTitleByXmlName(xmlName);
        }

        /// <summary>
        /// Finds the category identifier by XML.
        /// </summary>
        /// <param name="xmlName">Name of the XML.</param>
        /// <returns>System.String.</returns>
        private string FindCategoryIdByXmlName(string xmlName)
        {
            xmlName.ThrowIfNull(nameof(xmlName));
            return _context.GetCategoryIdByXmlName(xmlName);
        }

        /// <summary>
        /// Queries the broker for dynamic components or pages.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> Query<T>(int skip, int take) where T : IViewModel
        {
            return Query<T>(skip, take, new DD4TModels.QueryCriteria());
        }

        /// <summary>
        /// Determine the template criterium/ID to use based on the viewname
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private Criteria GetTemplateCriterium(DD4TModels.QueryCriteria.ItemType itemType, string viewName)
        {
            var templateId = _context.GetTemplateIdByTitle(viewName);
            if (templateId.IsNull())
            {
                throw new Exception("ViewName {0} not found.".FormatString(viewName));
            }

            switch (itemType)
            {
                case DD4TModels.QueryCriteria.ItemType.Component:
                    if (ComponentTemplateItemId == templateId.ItemTypeId) 
                    {
                        return new ItemTemplateCriteria(templateId.ItemId);
                    }
                    break;
                case DD4TModels.QueryCriteria.ItemType.Page:
                    if (PageTemplateItemId == templateId.ItemTypeId) 
                    {
                        return new PageTemplateCriteria(templateId.ItemId);
                    }
                    break;
            }
            throw new Exception($"The template ID {templateId} found for viewname {viewName} does not match the itemtype {itemType} criterium."); 
        }

        /// <summary>
        /// Queries the broker for dynamic components or pages.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        /// <exception cref="Exception">ViewName {0} not found.".FormatString(criteria.ViewTitle)</exception>
        /// <exception cref="InvalidCastException"></exception>
        public IEnumerable<T> Query<T>(int skip, int take, DD4TModels.QueryCriteria criteria) where T : IViewModel
        {
            var publicationId = _publicationResolver.ResolvePublicationId();

            string cacheKey = string.Format(_cacheKeyFormat, string.Join("|", typeof(T).FullName, skip, take, publicationId, criteria.GetUniqueKey()));

            var result = (List<T>)_cacheAgent.Load(cacheKey);

            if (result != null)
            {
                return result;
            }

            var listCriteria = new List<Criteria>();

            listCriteria.Add(new PublicationCriteria(publicationId));
            listCriteria.Add(new ItemTypeCriteria((int)criteria.Type));

            if (!criteria.ViewTitle.IsNullOrEmpty())
            {
                // add a template criterium using the viewtitle
                listCriteria.Add(GetTemplateCriterium(criteria.Type, criteria.ViewTitle));
            }
            if (criteria.MetaSearches.Any())
            {
                foreach (var metaSearch in criteria.MetaSearches)
                {
                    listCriteria.Add(new CustomMetaValueCriteria(new CustomMetaKeyCriteria(metaSearch.FieldName), metaSearch.FieldValue));
                }
            }
            if (criteria.CategorySearches.Any())
            {
                foreach (var categorySearch in criteria.CategorySearches)
                {
                    var categoryTitle = FindCategoryTitleByXmlName(categorySearch.XmlName);
                    if (categoryTitle.IsNull())
                        throw new Exception("Category {0} not found.".FormatString(categorySearch.XmlName));
                    listCriteria.Add(new CategorizationCriteria(publicationId, categoryTitle, categorySearch.Keywords));
                }
            }
            if (criteria.TaxonomyKeywordNameSearches.Any())
            {
                foreach (var categorySearch in criteria.TaxonomyKeywordNameSearches)
                {
                    var categoryId = FindCategoryIdByXmlName(categorySearch.XmlName);
                    if (categoryId.IsNull())
                        throw new Exception("Category {0} not found.".FormatString(categorySearch.XmlName));
                    listCriteria.Add(new TaxonomyKeywordNameCriteria(categoryId, categorySearch.Keyword, categorySearch.IncludeKeywordBranches));
                }
            }
            if (criteria.BySchemaFromAttribute)
            {
                var schemaId = FindSchemaIdByModelType<T>();
                if (schemaId.IsNull())
                    throw new Exception("Schema for {0} not found.".FormatString(typeof(T).Name));
                listCriteria.Add(new ItemSchemaCriteria(schemaId.ItemId));
            }
            if (!string.IsNullOrWhiteSpace(criteria.SchemaTitle))
            {
                var schemaId = _context.GetSchemaIdByTitle(criteria.SchemaTitle);
                if (schemaId.IsNull())
                    throw new Exception("Schema {0} not found.".FormatString(criteria.SchemaTitle));
                listCriteria.Add(new ItemSchemaCriteria(schemaId.ItemId));
            }
            if (!string.IsNullOrWhiteSpace(criteria.SchemaRootElementName))
            {
                var schemaId = _context.GetSchemaIdByRootElementName(criteria.SchemaRootElementName);
                if (schemaId.IsNull())
                    throw new Exception("Schema {0} not found.".FormatString(criteria.SchemaRootElementName));
                listCriteria.Add(new ItemSchemaCriteria(schemaId.ItemId));
            }

            var query = new Query();
            query.Criteria = CriteriaFactory.And(listCriteria.ToArray());

            if (criteria.SortParameters.Any())
            {
                criteria.SortParameters.ForEach(sp => AddSorting(sp, query));
            }

            PagingFilter filter = new PagingFilter(skip, take);
            query.SetResultFilter(filter);

            var allComponentUris = query.ExecuteQuery();
            _logger.Information("component found. count: {0}".FormatString(allComponentUris.Count()));

            result = new List<T>();

            foreach (var item in allComponentUris)
            {
                var viewModel = _contentProvider.BuildViewModel(new TcmUri(item));
                if (viewModel == null)
                    continue;

                if (!(viewModel is T))
                    throw new InvalidCastException(string.Format("Query for '{1}' returns items of the wrong type '{0}'", viewModel.GetType().FullName, typeof(T).FullName));

                result.Add((T)viewModel);
            }

            _cacheAgent.Store(cacheKey, _cacheRegion, result);

            return result;
        }

        private void AddSorting(Models.SortParameter sortParameter, Query query)
        {
            SortColumn sortColumn;
            switch (sortParameter.SortColumn)
            {
                case DD4TModels.SortColumn.ItemCreationDate:
                    sortColumn = SortParameter.ItemCreationDate;
                    break;
                case DD4TModels.SortColumn.ItemFileName:
                    sortColumn = SortParameter.ItemFileName;
                    break;
                case DD4TModels.SortColumn.ItemInitialPublicationDate:
                    sortColumn = SortParameter.ItemInitialPublicationDate;
                    break;
                case DD4TModels.SortColumn.ItemItemType:
                    sortColumn = SortParameter.ItemItemType;
                    break;
                case DD4TModels.SortColumn.ItemLastPublishedDate:
                    sortColumn = SortParameter.ItemLastPublishedDate;
                    break;
                case DD4TModels.SortColumn.ItemMajorVersion:
                    sortColumn = SortParameter.ItemMajorVersion;
                    break;
                case DD4TModels.SortColumn.ItemMinorVersion:
                    sortColumn = SortParameter.ItemMinorVersion;
                    break;
                case DD4TModels.SortColumn.ItemModificationDate:
                    sortColumn = SortParameter.ItemModificationDate;
                    break;
                case DD4TModels.SortColumn.ItemOwningPublicationId:
                    sortColumn = SortParameter.ItemOwningPublicationId;
                    break;
                case DD4TModels.SortColumn.ItemPageTemplateId:
                    sortColumn = SortParameter.ItemPageTemplateId;
                    break;
                case DD4TModels.SortColumn.ItemPublicationId:
                    sortColumn = SortParameter.ItemPublicationId;
                    break;
                case DD4TModels.SortColumn.ItemReferenceId:
                    sortColumn = SortParameter.ItemReferenceId;
                    break;
                case DD4TModels.SortColumn.ItemSchemaId:
                    sortColumn = SortParameter.ItemSchemaId;
                    break;
                case DD4TModels.SortColumn.ItemTitle:
                    sortColumn = SortParameter.ItemTitle;
                    break;
                case DD4TModels.SortColumn.ItemTrustee:
                    sortColumn = SortParameter.ItemTrustee;
                    break;
                case DD4TModels.SortColumn.ItemUrl:
                    sortColumn = SortParameter.ItemUrl;
                    break;
                default:
                    sortColumn = SortParameter.ItemTitle;
                    break;
            }
            SortDirection sortDirection;
            switch (sortParameter.SortDirection)
            {
                case DD4TModels.SortDirection.Descending:
                    sortDirection = SortParameter.Descending;
                    break;
                default:
                    sortDirection = SortParameter.Ascending;
                    break;
            }

            query.AddSorting(new SortParameter(sortColumn, sortDirection));
        }

        /// <summary>
        /// Gets the keyword names for a given category.
        /// </summary>
        /// <param name="categoryXmlName">Name of the category XML.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetKeywordNames(string categoryXmlName)
        {
            string cacheKey = String.Format(_cacheKeyFormat, "KeywordNames=" + categoryXmlName);

            var keywords = (List<string>)_cacheAgent.Load(cacheKey);

            if (keywords != null)
                return keywords;

            TaxonomyFactory taxonomyFactory = new TaxonomyFactory();

            var categoryUri = FindCategoryIdByXmlName(categoryXmlName);
            var taxonomy = taxonomyFactory.GetTaxonomyKeywords(categoryUri);

            keywords = taxonomy.KeywordChildren.OfType<Keyword>().Select(k => k.KeywordName).ToList();

            _cacheAgent.Store(cacheKey, _cacheRegion, (object)keywords);

            return keywords;
        }

        /// <summary>
        /// Gets the keyword name key dictionary.
        /// </summary>
        /// <param name="categoryXmlName">Name of the category XML.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeywordNameKeyDictionary(string categoryXmlName)
        {
            string cacheKey = String.Format(_cacheKeyFormat, "KeywordDictionary=" + categoryXmlName);

            var keywords = (Dictionary<string, string>)_cacheAgent.Load(cacheKey);

            if (keywords != null)
                return keywords;

            TaxonomyFactory taxonomyFactory = new TaxonomyFactory();

            var categoryUri = FindCategoryIdByXmlName(categoryXmlName);
            var taxonomy = taxonomyFactory.GetTaxonomyKeywords(categoryUri);

            keywords = taxonomy.KeywordChildren.OfType<Keyword>().ToDictionary(k => k.KeywordName, k => k.KeywordKey);

            _cacheAgent.Store(cacheKey, _cacheRegion, (object)keywords);

            return keywords;
        }

        /// <summary>
        /// Gets the keyword name key dictionary.
        /// </summary>
        /// <param name="categoryXmlName">Name of the category XML.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeywordKeyNameDictionary(string categoryXmlName)
        {
            string cacheKey = String.Format(_cacheKeyFormat, "KeywordDictionary=" + categoryXmlName);

            var keywords = (Dictionary<string, string>)_cacheAgent.Load(cacheKey);

            if (keywords != null)
                return keywords;

            TaxonomyFactory taxonomyFactory = new TaxonomyFactory();

            var categoryUri = FindCategoryIdByXmlName(categoryXmlName);
            var taxonomy = taxonomyFactory.GetTaxonomyKeywords(categoryUri);

            keywords = taxonomy.KeywordChildren.OfType<Keyword>().ToDictionary(k => k.KeywordKey, k => k.KeywordName);

            _cacheAgent.Store(cacheKey, _cacheRegion, (object)keywords);

            return keywords;
        }

    }


}