using System.Collections.Generic;

namespace Dyndle.Modules.Core.Providers.Content
{
    /// <summary>
    /// Interface ITaxonomyProvider
    /// </summary>
    public interface ITaxonomyProvider
    {
        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>IEnumerable&lt;DD4T.ContentModel.Keyword&gt;.</returns>
        IEnumerable<DD4T.ContentModel.Keyword> GetKeywords(string categoryId);
    }
}
