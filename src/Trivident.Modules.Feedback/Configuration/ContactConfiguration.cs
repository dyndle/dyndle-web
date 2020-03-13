using System;
using Dyndle.Modules.Core.Environment;
using Dyndle.Modules.Core.Extensions;
using Trivident.Modules.Feedback.Contracts;

namespace Trivident.Modules.Feedback.Configuration
{
    /// <summary>
    /// Contains configuration for the contact form
    /// </summary>
    public class ContactConfiguration : IContactConfiguration
    {
        private readonly ISiteContext _siteContext;

        /// <summary>
        /// Injected dependencies
        /// </summary>
        /// <param name="siteContext"></param>
        public ContactConfiguration(ISiteContext siteContext)
        {
            siteContext.ThrowIfNull(nameof(siteContext));
            _siteContext = siteContext;
        }

        public string FromAddress
        {
            get { return _siteContext.GetApplicationSetting(FeedbackConstants.Settings.ContactFormEmailFromAddress, true); }
        }

        public string ToAddress
        {
            get { return _siteContext.GetApplicationSetting(FeedbackConstants.Settings.ContactFormEmailToAddress, true); }
        }

        public string FromName
        {
            get { return _siteContext.GetApplicationSetting(FeedbackConstants.Settings.ContactFormEmailFromName, true); }
        }

        public string Subject
        {
            get { return _siteContext.GetApplicationSetting(FeedbackConstants.Settings.ContactFormEmailSubject, true); }
        }

        public string ThankYouUrl
        {
            get { return _siteContext.GetApplicationSetting(FeedbackConstants.Settings.ContactFormEmailThankYouUrl, true); }
        }

        public int EmailComponentId
        {
            get
            {
                var setting = _siteContext.GetApplicationSetting(FeedbackConstants.Settings.ContactFormEmailEmailComponentId, true);
                return int.Parse(setting);
            }
        }
    }
}