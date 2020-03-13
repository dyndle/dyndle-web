using System.Collections.Generic;
using System.Web.Mvc;
using DD4T.ContentModel;

namespace Dyndle.Modules.Core.Interfaces
{
    /// <summary>
    /// Taxonomy service to get keyword info
    /// </summary>
    public interface ITaxonomyService
    {
        /// <summary>
        /// Method to get keywords from category xml name
        /// </summary>
        /// <param name="categoryXmlName">Category XML name</param>
        /// <returns>Returns list of keywords present in the category</returns>
        // IEnumerable<Keyword> GetKeywords(string categoryXmlName);
        IEnumerable<IKeyword> GetKeywords(string categoryXmlName);

        /// <summary>
        /// Method to get keyword value from category xml name
        /// </summary>
        /// <param name="keyword">Keyword</param>
        /// <param name="categoryXmlName">Category name</param>
        /// <returns>Returns keyword value in the category</returns>
        string GetKeywordValue(IKeyword keyword, string categoryXmlName);

        /// <summary>
        /// Method to get keyword ID from category xml name and keyword title
        /// </summary>
        /// <param name="keywordTitle">Keyword Title</param>
        /// <param name="categoryXmlName">Category name</param>
        /// <returns>Returns keyword ID in the category</returns>
        string GetKeywordId(string keywordTitle, string categoryXmlName);

        /// <summary>
        /// Method to get keyword list items
        /// </summary>
        /// <param name="categoryXmlName">Category XML name</param>
        /// <returns>Returns selected items list with keyword name and keyword id</returns>
        List<SelectListItem> GetKeywordListItems(string categoryXmlName);

    }
}
