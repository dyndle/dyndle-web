using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Caching;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Environment;
using Dyndle.Modules.Core.Interfaces;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Core.Services.Taxonomy
{
    /// <summary>
    /// Taxonomy service
    /// </summary>
    public class TaxonomyService : ITaxonomyService
    {
        private readonly ISiteContext _siteContext;
        private readonly ICacheAgent _cacheAgent;
        private readonly IExtendedPublicationResolver _extendedPublicationResolver;
        private readonly ITaxonomyProvider _taxonomyProvider;
        private const string CacheKeyFormat = "Keywords_{0}_{1}";
        private const string CacheRegion = "Keywords";

        /// <summary>
        /// Taxonomy service constructor
        /// </summary>
        /// <param name="siteContext">Sitecontext</param>
        public TaxonomyService(ISiteContext siteContext, ICacheAgent cacheAgent, IExtendedPublicationResolver extendedPublicationResolver, ITaxonomyProvider taxonomyProvider)
        {
            _siteContext = siteContext;
            _cacheAgent = cacheAgent;
            _extendedPublicationResolver = extendedPublicationResolver;
            _taxonomyProvider = taxonomyProvider;
        }

        /// <summary>
        /// Method to get keywords from category xml name
        /// </summary>
        /// <param name="categoryXmlName">Category XML name</param>
        /// <returns>Returns list of keywords present in the category</returns>
        public IEnumerable<IKeyword> GetKeywords(string categoryXmlName)
        {
            var cacheKey = string.Format(CacheKeyFormat, categoryXmlName, _extendedPublicationResolver.ResolvePublicationId());
            var keywords = _cacheAgent.Load(cacheKey) as IEnumerable<DD4T.ContentModel.IKeyword>;
            if (keywords != null)
            {
                return keywords;
            }
            var categoryId = _siteContext.GetCategoryIdByXmlName(categoryXmlName);
            keywords = _taxonomyProvider.GetKeywords(categoryId);
            _cacheAgent.Store(cacheKey, CacheRegion, keywords);
            return keywords;
        }

        /// <summary>
        /// Method to get keyword value from category xml name
        /// </summary>
        /// <param name="keyword">Keyword</param>
        /// <param name="categoryXmlName">Category name</param>
        /// <returns>Returns keyword value in the category</returns>
        public string GetKeywordValue(IKeyword keyword, string categoryXmlName)
        {
            var keywords = GetKeywords(categoryXmlName);

            var firstOrDefault = keywords?.FirstOrDefault(x => x.Key.Equals(keyword.Key, StringComparison.OrdinalIgnoreCase));
            return (firstOrDefault != null && String.IsNullOrEmpty(firstOrDefault.Title)) ? firstOrDefault.Title: string.Empty;
        }

        /// <summary>
        /// Method to get keyword ID from category xml name and keyword title
        /// </summary>
        /// <param name="keywordTitle">Keyword Title</param>
        /// <param name="categoryXmlName">Category name</param>
        /// <returns>Returns keyword ID in the category</returns>
        public string GetKeywordId(string keywordTitle, string categoryXmlName)
        {
            var keywords = GetKeywords(categoryXmlName);

            var firstOrDefault = keywords?.FirstOrDefault(x => x.Key.Equals(keywordTitle, StringComparison.OrdinalIgnoreCase));
            return firstOrDefault?.Id != null ? new TcmUri(firstOrDefault.Id).ItemId.ToString() : string.Empty;
        }

        /// <summary>
        /// Method to get keyword list items
        /// </summary>
        /// <param name="categoryXmlName">Category XML name</param>
        /// <returns>Returns selected items list with keyword name and keyword id</returns>
        public List<SelectListItem> GetKeywordListItems(string categoryXmlName)
        {
            return GetKeywords(categoryXmlName).Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = (new TcmUri(x.Id)).ItemId.ToString()
            }).ToList();
        }

        public List<string> GetKeywordNames(string categoryXmlName)
        {
            var keywords = GetKeywords(categoryXmlName);

            return keywords.Select(k => k.Title).ToList();
        }

        public Dictionary<string, string> GetKeywordNameKeyDictionary(string categoryXmlName)
        {
            var keywords = GetKeywords(categoryXmlName);

            return keywords.ToDictionary(k => k.Title, k => k.Key);
        }

        public Dictionary<string, string> GetKeywordKeyNameDictionary(string categoryXmlName)
        {
            var keywords = GetKeywords(categoryXmlName);

            return keywords.ToDictionary(k => k.Key, k => k.Title);
        }
    }
}
