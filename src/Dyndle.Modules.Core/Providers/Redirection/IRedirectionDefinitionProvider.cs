using System.Collections.Generic;

namespace Dyndle.Modules.Core.Providers.Redirection
{
    /// <summary>
    /// Interface IRedirectionDefinitionProvider
    /// Used to get all redirect definitions 
    /// </summary>
    public interface IRedirectionDefinitionProvider
    {
        /// <summary>
        /// Gets the definitions.
        /// </summary>
        /// <param name="cleanUrls"><c>true</c> if the URLs should be cleaned; otherwise, <c>false</c></param>
        /// <returns>List&lt;IRedirectionDefinition&gt;.</returns>
        List<IRedirectionDefinition> GetDefinitions(bool cleanUrls);
    }
}