using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Providers.Configuration
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