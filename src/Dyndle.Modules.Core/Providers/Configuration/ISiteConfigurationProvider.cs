using System.Collections.Generic;

namespace Dyndle.Modules.Core.Providers.Configuration
{
    /// <summary>
    /// Interface ISiteConfigurationProvider
    /// Used to get all setting from Tridion based on the current publication
    /// </summary>
    public interface ISiteConfigurationProvider
    {
        /// <summary>
        /// Retrieves the configuration.
        /// </summary>
        /// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
        IDictionary<string, string> RetrieveConfiguration();
    }
}