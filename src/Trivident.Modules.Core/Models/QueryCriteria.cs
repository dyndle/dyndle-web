using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Models
{
    /// <summary>
    /// Class QueryCriteria.
    /// Provides all criteria needed to query for content from broker
    /// </summary>
    public class QueryCriteria
    {
        public string GetUniqueKey()
        {
            var categorySearches = string.Join("|", CategorySearches.Select(c => c.GetUniqueKey()));
            var metaSearches = string.Join("|", MetaSearches.Select(c => c.GetUniqueKey()));
            var taxonomyKeywordNameSearches = string.Join("|", TaxonomyKeywordNameSearches.Select(c => c.GetUniqueKey()));
            var sortParameters = string.Join("|", SortParameters.Select(c => string.Join("|", c.SortColumn.GetHashCode(), c.SortDirection.GetHashCode())));

            // Note: if the first value parameter is null, string.Join always returns an empty string
            // So we must make sure the first one (SchemaRootElementName) is never null
            // See https://davidzych.com/fun-times-with-string-join-in-c/

            return string.Join("|", SchemaRootElementName ?? "", SchemaTitle, Type, BySchemaFromAttribute, ViewTitle, categorySearches, metaSearches, taxonomyKeywordNameSearches, sortParameters);
        }
        /// <summary>
        /// Enum ItemType, the type of content to search for
        /// </summary>
        public enum ItemType
        {
            /// <summary>
            /// The component
            /// </summary>
            Component = 16,
            /// <summary>
            /// The page
            /// </summary>
            Page = 64
        }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public ItemType Type { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to search by schema from attribute.
        /// </summary>
        /// <value><c>true</c> if [by schema from attribute]; otherwise, <c>false</c>.</value>
        public bool BySchemaFromAttribute { get; set; }
        /// <summary>
        /// Gets or sets the view title to search for.
        /// </summary>
        /// <value>The view title.</value>
        public string ViewTitle { get; set; }
        /// <summary>
        /// Gets or sets the categories to search for.
        /// </summary>
        /// <value>The category searches.</value>
        public List<CategorySearch> CategorySearches { get; set; }
        /// <summary>
        /// Gets or sets the meta searches to search for.
        /// </summary>
        /// <value>The meta searches.</value>
        public List<MetaSearch> MetaSearches { get; set; }

        /// <summary>
        /// Gets or sets the taxonomy keyword names to search for.
        /// </summary>
        /// <value>The taxonomy keyword name searches.</value>
        public List<TaxonomyKeywordNameSearch> TaxonomyKeywordNameSearches { get; set; }

        /// <summary>
        /// Gets or sets the sort parameters to order by.
        /// </summary>
        /// <value>The sort parameters.</value>
        public List<SortParameter> SortParameters { get; set; }
        /// <summary>
        /// Gets or sets the name of the schema element.
        /// </summary>
        /// <value>
        /// The name of the schema element.
        /// </value>
        public string SchemaRootElementName { get; set; }
        /// <summary>
        /// Gets or sets the schema title.
        /// </summary>
        /// <value>
        /// The schema title.
        /// </value>
        public string SchemaTitle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCriteria"/> class.
        /// Using default query criteria
        /// </summary>
        public QueryCriteria()
        {
            Type = ItemType.Component;
            BySchemaFromAttribute = true;
            SortParameters = new List<SortParameter>();
            CategorySearches = new List<CategorySearch>();
            TaxonomyKeywordNameSearches = new List<TaxonomyKeywordNameSearch>();
            MetaSearches = new List<MetaSearch>();
        }

        /// <summary>
        /// Adds the meta field and value to search for.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <returns>QueryCriteria.</returns>
        public QueryCriteria AddMetaSearch(string fieldName, string fieldValue)
        {
            fieldName.ThrowIfNull(nameof(fieldName));
            fieldValue.ThrowIfNull(nameof(fieldValue));

            MetaSearches.Add(new MetaSearch() { FieldName = fieldName, FieldValue = fieldValue });

            return this;
        }

        /// <summary>
        /// Adds a category to search for.
        /// </summary>
        /// <param name="xmlName">Name of the XML.</param>
        /// <param name="keywords">The keywords.</param>
        /// <returns>QueryCriteria.</returns>
        public QueryCriteria AddCategorySearch(string xmlName, string[] keywords)
        {
            xmlName.ThrowIfNull(nameof(xmlName));
            keywords.ThrowIfNull(nameof(keywords));

            CategorySearches.Add(new CategorySearch() { XmlName = xmlName, Keywords = keywords });

            return this;
        }
        /// <summary>
        /// Adds the taxonomy keyword name to search for.
        /// </summary>
        /// <param name="xmlName">Name of the XML.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="includeKeywordBranches">if set to <c>true</c> [include keyword branches].</param>
        /// <returns>QueryCriteria.</returns>
        public QueryCriteria AddTaxonomyKeywordNameSearch(string xmlName, string keyword, bool includeKeywordBranches)
        {
            xmlName.ThrowIfNull(nameof(xmlName));
            keyword.ThrowIfNull(nameof(keyword));

            TaxonomyKeywordNameSearches.Add(new TaxonomyKeywordNameSearch() { XmlName = xmlName, Keyword = keyword, IncludeKeywordBranches = includeKeywordBranches });

            return this;
        }
        /// <summary>
        /// Adds the sorting.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortingDirection">The sorting direction.</param>
        /// <returns>QueryCriteria.</returns>
        public QueryCriteria AddSorting(SortColumn sortColumn, SortDirection sortingDirection)
        {
            SortParameters.Add(new SortParameter(sortColumn, sortingDirection));
            return this;
        }

        /// <summary>
        /// Class MetaSearch.
        /// Holds info to search for a meta field
        /// </summary>
        public class MetaSearch
        {
            internal string GetUniqueKey()
            {
                return string.Join("|", FieldName, FieldValue);
            }
            /// <summary>
            /// Gets or sets the name of the field.
            /// </summary>
            /// <value>The name of the field.</value>
            public string FieldName { get; set; }
            /// <summary>
            /// Gets or sets the field value.
            /// </summary>
            /// <value>The field value.</value>
            public string FieldValue { get; set; }
        }

        /// <summary>
        /// Class CategorySearch.
        /// Holds info to search for a category
        /// </summary>
        public class CategorySearch
        {
            internal string GetUniqueKey()
            {
                return string.Join("|", XmlName, string.Join("|", Keywords));
            }
            /// <summary>
            /// Gets or sets the name of the XML.
            /// </summary>
            /// <value>The name of the XML.</value>
            public string XmlName { get; set; }
            /// <summary>
            /// Gets or sets the keywords.
            /// </summary>
            /// <value>The keywords.</value>
            public string[] Keywords { get; set; }
        }

        /// <summary>
        /// Class TaxonomyKeywordNameSearch.
        /// Holds info to search for a Taxonomy Keyword Name
        /// </summary>
        public class TaxonomyKeywordNameSearch
        {
            internal string GetUniqueKey()
            {
                return string.Join("|", XmlName, Keyword, IncludeKeywordBranches);
            }
            /// <summary>
            /// Gets or sets the name of the XML.
            /// </summary>
            /// <value>The name of the XML.</value>
            public string XmlName { get; set; }
            /// <summary>
            /// Gets or sets the keyword.
            /// </summary>
            /// <value>The keyword.</value>
            public string Keyword { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether [include keyword branches].
            /// </summary>
            /// <value><c>true</c> if [include keyword branches]; otherwise, <c>false</c>.</value>
            public bool IncludeKeywordBranches { get; set; }
        }
    }

    public enum SortColumn
    {
        ItemReferenceId,
        ItemUrl,
        ItemPageTemplateId,
        ItemSchemaId,
        ItemFileName,
        ItemModificationDate,
        ItemLastPublishedDate,
        ItemTrustee,
        ItemCreationDate,
        ItemTitle,
        ItemItemType,
        ItemOwningPublicationId,
        ItemMinorVersion,
        ItemMajorVersion,
        ItemPublicationId,
        ItemInitialPublicationDate
    }
    public enum SortDirection
    {
        Descending, Ascending
    }

    public class SortParameter
    {
        public SortParameter(SortColumn sortColumn, SortDirection sortDirection)
        {
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }
        public SortColumn SortColumn { get; set; }
        public SortDirection SortDirection { get; set; }
    }

}
