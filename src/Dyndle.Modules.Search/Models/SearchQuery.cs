using Dyndle.Modules.Search.Extensions;
using Dyndle.Modules.Search.Utils;
using Menon.Me.ModelToQuerystring.Attributes;

namespace Dyndle.Modules.Search.Models
{
    /// <summary>
    /// Generic search query
    /// </summary>
    public class SearchQuery
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        [QueryParameter(IsQuerystring = false)]
        public int CurrentPage { get; set; }
        /// <summary>
        /// Number of leading documents to skip and number of documents to return after 'start'. (Integers)
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        [QueryParameter(PropertyName = "start")]
        public int Start { get; set; }

        /// <summary>
        /// Number of leading documents to skip. (Integer).
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        [QueryParameter(PropertyName = "rows")]
        public int PageSize { get; set; }

        /// <summary>
        /// Search query
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        [QueryParameter(IsQuerystring = false)]
        public string QueryText { get; set; }

        [QueryParameter(PropertyName = "q")]
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the field list. Comma seperated
        /// </summary>
        /// <value>
        /// The field list.
        /// </value>
        [QueryParameter(PropertyName = "fl")]
        public string FieldList { get; set; } = SearchUtil.ResponseModel.GetFieldListItemsAsString();

        /// <summary>
        /// The filter query.
        /// </summary>
        /// <value>
        /// The filter query.
        /// </value>
        [QueryParameter(PropertyName = "fq")]
        public string FilterQuery { get; set; }

        /// <summary>
        /// Sort field or function with asc|desc.
        /// </summary>
        /// <value>
        /// The sort.
        /// </value>
        [QueryParameter(PropertyName = "sort")]
        public string Sort { get; set; }

        /// <summary>
        /// The writer type (response format)
        /// </summary>
        /// <value>
        /// The type of the writer.
        /// </value>
        [QueryParameter(PropertyName = "wt")]
        public string WriterType { get; set; } = "json";


        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        [QueryParameter(PropertyName = "fq")]
        public string Filters { get; set; }


        /// <summary>
        /// Gets or sets the omit header.
        /// </summary>
        /// <value>
        /// The omit header.
        /// </value>
        [QueryParameter(PropertyName = "omitHeader")]
        public string OmitHeader { get; set; } = "true";

        [QueryParameter(IsQuerystring = false)]
        public bool IsGrouped { get; set; }
        [QueryParameter(IsQuerystring = false)]
        public string GroupByField { get; set; }
        [QueryParameter(IsQuerystring = false)]
        public int GroupingPageSize { get; set; }
    }
}