using System;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Environment;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Models.Query;
using Dyndle.Modules.Core.Providers.Content;
using Tridion.ContentDelivery.DynamicContent.Query;
using Tridion.ContentDelivery.Taxonomies;
using SortColumn = Tridion.ContentDelivery.DynamicContent.Query.SortColumn;
using SortDirection = Tridion.ContentDelivery.DynamicContent.Query.SortDirection;
using SortParameter = Tridion.ContentDelivery.DynamicContent.Query.SortParameter;

namespace Dyndle.Providers
{
    /// <summary>
    /// Class DefaultContentQueryProvider.
    /// Provides a generic way to Query the broker for dynamic components, pages or keywords.
    /// </summary>
    /// <seealso cref="IContentQueryProvider" />
    public class DefaultContentQueryProvider : IContentQueryProvider
    {
        // Tridion template item IDs
        private const int ComponentTemplateItemId = 32;
        private const int PageTemplateItemId = 128;


        private readonly ISiteContext _context;
        private readonly ILogger _logger;
        private readonly IPublicationResolver _publicationResolver;
        private readonly IViewModelResolver _viewModelResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContentQueryProvider" /> class.
        /// </summary>
        /// <param name="publicationResolver">The publication resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The context.</param>
        /// <param name="viewModelResolver">The view model resolver.</param>
        public DefaultContentQueryProvider(IPublicationResolver publicationResolver, ILogger logger, ISiteContext context, IViewModelResolver viewModelResolver)
        {
            context.ThrowIfNull(nameof(context));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));
            logger.ThrowIfNull(nameof(logger));
            viewModelResolver.ThrowIfNull(nameof(viewModelResolver));

            _context = context;
            _logger = logger;
            _publicationResolver = publicationResolver;
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
        /// Determine the template criterium/ID to use based on the viewname
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private Criteria GetTemplateCriterium(QueryCriteria.ItemType itemType, string viewName)
        {
            var templateId = _context.GetTemplateIdByTitle(viewName);
            if (templateId.IsNull())
            {
                throw new Exception("ViewName {0} not found.".FormatString(viewName));
            }

            switch (itemType)
            {
                case QueryCriteria.ItemType.Component:
                    if (ComponentTemplateItemId == templateId.ItemTypeId)
                    {
                        return new ItemTemplateCriteria(templateId.ItemId);
                    }
                    break;
                case QueryCriteria.ItemType.Page:
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
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public string[] Query<T>(int skip, int take) where T : IViewModel
        {
            return Query<T>(skip, take, new QueryCriteria());
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
        public string[] Query<T>(int skip, int take, QueryCriteria criteria) where T : IViewModel
        {
            var publicationId = _publicationResolver.ResolvePublicationId();

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

            var query = new Query
            {
                Criteria = CriteriaFactory.And(listCriteria.ToArray())
            };

            if (criteria.SortParameters.Any())
            {
                criteria.SortParameters.ForEach(sp => AddSorting(sp, query));
            }

            PagingFilter filter = new PagingFilter(skip, take);
            query.SetResultFilter(filter);

            var allComponentUris = query.ExecuteQuery();
            _logger.Information("component found. count: {0}".FormatString(allComponentUris.Count()));

            return allComponentUris;
        }

        private void AddSorting(Modules.Core.Models.Query.SortParameter sortParameter, Query query)
        {
            SortColumn sortColumn = sortParameter.SortColumn switch
            {
                Modules.Core.Models.Query.SortColumn.ItemReferenceId => SortParameter.ItemReferenceId,
                Modules.Core.Models.Query.SortColumn.ItemUrl => SortParameter.ItemUrl,
                Modules.Core.Models.Query.SortColumn.ItemPageTemplateId => SortParameter.ItemPageTemplateId,
                Modules.Core.Models.Query.SortColumn.ItemSchemaId => SortParameter.ItemSchemaId,
                Modules.Core.Models.Query.SortColumn.ItemFileName => SortParameter.ItemFileName,
                Modules.Core.Models.Query.SortColumn.ItemModificationDate => SortParameter.ItemModificationDate,
                Modules.Core.Models.Query.SortColumn.ItemLastPublishedDate => SortParameter.ItemLastPublishedDate,
                Modules.Core.Models.Query.SortColumn.ItemTrustee => SortParameter.ItemTrustee,
                Modules.Core.Models.Query.SortColumn.ItemCreationDate => SortParameter.ItemCreationDate,
                Modules.Core.Models.Query.SortColumn.ItemTitle => SortParameter.ItemTitle,
                Modules.Core.Models.Query.SortColumn.ItemItemType => SortParameter.ItemItemType,
                Modules.Core.Models.Query.SortColumn.ItemOwningPublicationId => SortParameter.ItemOwningPublicationId,
                Modules.Core.Models.Query.SortColumn.ItemMinorVersion => SortParameter.ItemMinorVersion,
                Modules.Core.Models.Query.SortColumn.ItemMajorVersion => SortParameter.ItemMajorVersion,
                Modules.Core.Models.Query.SortColumn.ItemPublicationId => SortParameter.ItemPublicationId,
                Modules.Core.Models.Query.SortColumn.ItemInitialPublicationDate => SortParameter.ItemInitialPublicationDate,
                _ => SortParameter.ItemTitle
            };
            SortDirection sortDirection;
            switch (sortParameter.SortDirection)
            {
                case Modules.Core.Models.Query.SortDirection.Descending:
                    sortDirection = SortParameter.Descending;
                    break;
                default:
                    sortDirection = SortParameter.Ascending;
                    break;
            }

            query.AddSorting(new SortParameter(sortColumn, sortDirection));
        }


    }


}