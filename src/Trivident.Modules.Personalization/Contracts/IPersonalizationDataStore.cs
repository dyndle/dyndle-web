using System.Web;

namespace Trivident.Modules.Personalization.Contracts
{
    /// <summary>
    /// Interface for storing/retrieving personalization data
    /// </summary>
    public interface IPersonalizationDataStore
    {
        /// <summary>
        /// Update personalization data for the current context (visitor)
        /// </summary>
        /// <param name="httpContext">The current HTTP context</param>
        /// <param name="data">The data to update</param>
        void Update(HttpContextBase httpContext, IPersonalizationData data);

        /// <summary>
        /// Get personalization data for the current context (visitor)
        /// </summary>
        /// <param name="httpContext">The current HTTP context</param>
        /// <returns>The lastest personalization data stored for this context</returns>
        IPersonalizationData Get(HttpContextBase httpContext);
    }
}
