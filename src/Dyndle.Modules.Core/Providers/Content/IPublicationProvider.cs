using Dyndle.Modules.Core.Models;
using System.Collections.Generic;

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
        IEnumerable<IPublicationMeta> GetAllPublicationMetadata(bool excludeCurrent);
    }
}
