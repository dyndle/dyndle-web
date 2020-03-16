using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Dyndle.Modules.Search.Models;

namespace Dyndle.Modules.Search.Utils
{
    public static class SearchUtil
    {
        /// <summary>
        /// Gets the search assembly.
        /// </summary>
        /// <value>
        /// The search assembly.
        /// </value>
        public static string SearchAssembly => ConfigurationManager.AppSettings[SearchConstants.Settings.Assembly];

        /// <summary>
        /// Gets the response item model.
        /// </summary>
        /// <value>
        /// The response item model.
        /// </value>
        public static string ResponseItemModel => ConfigurationManager.AppSettings[SearchConstants.Settings.ResponseItemModel];

        /// <summary>
        /// Get the Assembly specified in the AppSettings.
        /// </summary>
        /// <value>
        /// The get assembly.
        /// </value>
        public static Assembly GetAssembly
        {
            get
            {
                try
                {
                    return (!string.IsNullOrWhiteSpace(SearchAssembly))
                        ? Assembly.Load(SearchAssembly)
                        : null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Check whether the Assembly Exsits
        /// <returns>true/false</returns>
        /// </summary>
        public static bool IsAssemblyExsits => GetAssembly != null;

        /// <summary>
        /// Gets the search response model.
        /// </summary>
        /// <value>
        /// The search response model.
        /// </value>
        private static Type SearchResponseModel => GetAssembly != null && !string.IsNullOrWhiteSpace(ResponseItemModel)
            ? GetAssembly.GetTypes()
                .FirstOrDefault(x => x.Name.Equals(ResponseItemModel))
            : typeof(SearchResultItem);

        /// <summary>
        /// Gets the response model.
        /// </summary>
        /// <value>
        /// The response model.
        /// </value>
        public static Type ResponseModel => SearchResponseModel ?? typeof(SearchResultItem);
    }
}
