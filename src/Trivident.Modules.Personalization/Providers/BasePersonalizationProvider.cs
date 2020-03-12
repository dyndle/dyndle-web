using DD4T.ContentModel.Contracts.Logging;
using Trivident.Modules.Personalization.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trivident.Modules.Core.Environment;
using Trivident.Modules.Core.Models;
using Trivident.Modules.Personalization.Model;
//using Tridion.ContentDelivery.AmbientData;

namespace Trivident.Modules.Personalization.Providers
{
    /// <summary>
    /// Base provider for personalization implementation. Segments are read from trivident_test_segment parameter
    /// on the current request query string. Calculated segments are cached for the scope of the current request
    /// Classes inheriting from this should override the CalculateSegments method to provide custom business logic
    /// </summary>
    public class BasePersonalizationProvider : IPersonalizationProvider
    {
        protected static readonly string _segmentCacheKey = "Trivident.Personalization.ContextSegments";
        protected readonly ILogger _logger;
        protected readonly IPersonalizationDataStore _dataStore;
        protected readonly ISiteContext _siteContext;

        public BasePersonalizationProvider(ILogger logger, 
            IPersonalizationDataStore dataStore, 
            ISiteContext siteContext)
        {
            logger.ThrowIfNull(nameof(logger));
            dataStore.ThrowIfNull(nameof(dataStore));
            siteContext.ThrowIfNull(nameof(siteContext));

            _logger = logger;
            _dataStore = dataStore;
            _siteContext = siteContext;
        }

        /// <summary>
        /// Gets the website visitor segmentation for the current request
        /// </summary>
        /// <returns>A list of segment strings (empty list if there are no segments)</returns>
        public virtual List<string> GetSegments(HttpContextBase httpContext)
        {
            var segments = ReadSegmentsFromCache(httpContext);
            if (segments==null)
            {
                if (AllowQueryStringSegmentOverride())
                {
                    segments = GetSegmentsFromQueryString(httpContext);
                }
                if (segments == null)
                {
                    segments = CalculateSegments(httpContext);
                }
                AddSegmentsToCache(httpContext, segments);
                AddSegmentsToAdf(segments);
            }
            return segments;
        }

        protected void AddSegmentsToAdf(List<string> segments)
        {
            // this method requires Tridion DLLs, and we don't want those in the modules
            // for now, we are uncommenting it
            // The module will still work, but it will no longer be possible to use the collected segment through the ADF
            //var claimstore = AmbientDataContext.CurrentClaimStore;
            ////Check if ADF is enabled (if not, do nothing)
            //if (claimstore != null && segments!=null)
            //{
            //    string[] data = segments.ToArray();
            //    var uri = new Uri("trivident:profile:segments");
            //    _logger.Debug("Adding trivident:profile:segments Claim in ADF with value {0}", String.Join(",",data));
            //    if (claimstore.Contains(uri))
            //    {
            //        claimstore.Remove(uri);
            //    }
            //    claimstore.Put(uri, data);
            //}
        }

        /// <summary>
        /// Process update of personalization data
        /// </summary>
        /// <param name="httpContext">The HttpContext for which data should be updated</param>
        /// <param name="updateData">The data for the update</param>
        public virtual void ProcessUpdate(HttpContextBase httpContext, string updateData)
        {
            IPersonalizationData data = _dataStore.Get(httpContext) ?? CreateNewData();
            _dataStore.Update(httpContext, this.ProcessData(data,updateData));
        }

        /// <summary>
        /// Get tracking data for an entity model
        /// </summary>
        /// <param name="model">The model in which to look for tracking data</param>
        /// <returns>A string representing tracking data to process for this model</returns>
        public virtual string GetTrackingData(EntityModel model)
        {
            //Default implementation is to return nothing, logic to be implemented by child class 
            return string.Empty;
        }

        /// <summary>
        /// Implementations should override this to specify a concrete IPersonalizationData class to store personalization data
        /// </summary>
        /// <returns>A new object in which personalization data can be stored</returns>
        protected virtual IPersonalizationData CreateNewData()
        {
            return new BasePersonalizationData();
        }

        /// <summary>
        /// Can be override to prevent query string overrides for segments
        /// </summary>
        /// <returns>true if query string segment override is allowed</returns>
        protected virtual bool AllowQueryStringSegmentOverride()
        {
            string configValue = _siteContext.GetApplicationSetting(PersonalisationConstants.Settings.DebugViaQueryString);
            // if not specifed, default to true
            return (configValue.IsNullOrEmpty() || configValue.Equals("true"));
        }

        /// <summary>
        /// Implementations should override this to specify how tracking data updates should be handled
        /// </summary>
        /// <param name="data">An existing set of personalization data</param>
        /// <param name="updateData">The data to update</param>
        /// <returns>Updated personalization data</returns>
        protected virtual IPersonalizationData ProcessData(IPersonalizationData data, string updateData)
        {
            //Default implementation is to return the data without modification, logic to be implemented by child class 
            return data;
        }

        /// <summary>
        /// Implementations should override this to implement business logic to calculate segments etc. from the raw personalization data
        /// </summary>
        /// <param name="data">The data to process</param>
        /// <returns>Updated personalization data</returns>
        protected virtual IPersonalizationData Recalculate(IPersonalizationData data)
        {
            //Default implementation is to do nothing, logic to be implemented by child class 
            return data;
        }
        
        protected virtual List<string> CalculateSegments(HttpContextBase httpContext)
        {
            IPersonalizationData data = _dataStore.Get(httpContext) ?? CreateNewData();
            if (!data.Recalculated)
            {
                data = this.Recalculate(data); 
                data.Recalculated = true;
                _dataStore.Update(httpContext, data);
            }
            return data.Segments ?? new List<string>();
        }

        protected virtual List<string> GetSegmentsFromQueryString(HttpContextBase httpContext)
        {
            var parameters = httpContext.Request.QueryString;
            if (parameters != null)
            {
                _logger.Debug("Reading segments from query string...");
                var vals = parameters.GetValues("trivident_test_segment");
                if (vals != null && vals.Length > 0)
                {
                    return vals.ToList();
                }
            }
            return null;
        }

        protected virtual List<string> ReadSegmentsFromCache(HttpContextBase httpContext)
        {
            return httpContext.Items.Contains(_segmentCacheKey) ? (List<string>)httpContext.Items[_segmentCacheKey] : null;
        }

        protected virtual void AddSegmentsToCache(HttpContextBase httpContext, List<string> segments)
        {
            _logger.Debug("Adding current request segments to cache: {0}", String.Join(", ", segments));
            httpContext.Items[_segmentCacheKey] = segments;
        }
    }
}
