using System;
using Trivident.Modules.Core.Environment;
using Trivident.Modules.Feedback.Contracts;

namespace Trivident.Modules.Feedback.Configuration
{
    /// <summary>
    /// Contains configuration for the feedback form
    /// </summary>
    public class FeedbackConfiguration : IFeedbackConfiguration
    {
        private readonly ISiteContext _siteContext;

        /// <summary>
        /// Injected dependencies
        /// </summary>
        /// <param name="siteContext"></param>
        public FeedbackConfiguration(ISiteContext siteContext)
        {
            siteContext.ThrowIfNull(nameof(siteContext));
            _siteContext = siteContext;
        }

        /// <summary>
        /// Get site Id from app config
        /// </summary>
        /// <returns></returns>
        public int SiteId
        {
            get
            {
                var setting = _siteContext.GetApplicationSetting(FeedbackConstants.Settings.CoherisSiteId, true);
                return int.Parse(setting);
            }
        }

        /// <summary>
        /// Get thankyou Url from app config
        /// </summary>
        /// <returns></returns>
        public string ThankYouUrl
        {
            get { return _siteContext.GetApplicationSetting(FeedbackConstants.Settings.ThankYouUrl, true); }
        }
    }
}