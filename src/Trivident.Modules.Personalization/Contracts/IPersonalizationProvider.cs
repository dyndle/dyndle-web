using System.Collections.Generic;
using System.Web;
using Dyndle.Modules.Core.Models;

namespace Trivident.Modules.Personalization.Contracts
{
    /// <summary>
    /// Interface to custom personalization implementations per brand
    /// </summary>
    public interface IPersonalizationProvider
    {
        /// <summary>
        /// Process an update to tracking/personalization data
        /// </summary>
        /// <param name="httpContext">The current Http Context- used to identify visitor / store data</param>
        /// <param name="updateData">Data to update</param>
        void ProcessUpdate(HttpContextBase httpContext, string updateData);
        
        /// <summary>
        /// Get a list of visitor segments for the current context
        /// </summary>
        /// <param name="httpContext">The current Http Context</param>
        /// <returns>A list of segments valid for the current context (visitor)</returns>
        List<string> GetSegments(HttpContextBase httpContext);
        
        /// <summary>
        /// Extract tracking data from a model
        /// </summary>
        /// <param name="model">The model to process for tracking data</param>
        /// <returns>String representation of data to be tracked</returns>
        string GetTrackingData(EntityModel model);
    }
}
