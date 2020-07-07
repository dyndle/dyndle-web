using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyndle.Modules.Core.Providers.Content
{
    /// <summary>
    /// Interface IPublicationProvider
    /// Creates a model that contains all publication metadata
    /// </summary>
    public interface IPublicationProvider
    {
        /// <summary>
        /// Loads all metadata from all publications
        /// </summary>
        /// <returns>IEnumerable<string>.</returns>
        IEnumerable<string> GetAllPublicationMetadata();
    }
}
