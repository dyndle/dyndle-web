using System.Collections.Generic;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Models.Query;

namespace Dyndle.Modules.Core.Providers.Content
{
    /// <summary>
    /// Interface IContentQueryProvider
    /// </summary>
    public interface IContentQueryProvider
    {
        /// <summary>
        /// Queries the specified skip.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> Query<T>(int skip, int take) where T : IViewModel;
        /// <summary>
        /// Queries the specified skip.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> Query<T>(int skip, int take, QueryCriteria criteria) where T : IViewModel;

        /// <summary>
        /// Gets the keyword names.
        /// </summary>
        /// <param name="categoryXmlName">Name of the category XML.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        IEnumerable<string> GetKeywordNames(string categoryXmlName);

        /// <summary>
        /// Gets the keyword name key dictionary.
        /// </summary>
        /// <param name="categoryXmlName">Name of the category XML.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        Dictionary<string, string> GetKeywordNameKeyDictionary(string categoryXmlName);

        /// <summary>
        /// Gets the keyword key name dictionary.
        /// </summary>
        /// <param name="categoryXmlName">Name of the category XML.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        Dictionary<string, string> GetKeywordKeyNameDictionary(string categoryXmlName);
    }
}