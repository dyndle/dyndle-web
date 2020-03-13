using Trivident.Modules.Personalization.Contracts;
using System.Web.Mvc;
using System.Web;
using System;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;

namespace Trivident.Modules.Personalization.Extensions
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Generate HTML Data attribute containing personalization/tracking data for this model
        /// </summary>
        /// <param name="model">The model to process</param>
        /// <returns>HTML string with data-trivident-tracking-data attribute and value</returns>
        public static MvcHtmlString TrackingDirective(this EntityModel model)
        {
            if (PersonalizationProvider != null)
            {
                string trackingData = PersonalizationProvider.GetTrackingData(model);
                if (!trackingData.IsNullOrEmpty())
                {
                    return new MvcHtmlString(string.Format("data-trivident-tracking-data=\"{0}\"", HttpContext.Current.Server.HtmlEncode(trackingData)));
                }
            }
            return null;
        }

        private static IPersonalizationProvider _personalizationProvider;
        private static IPersonalizationProvider PersonalizationProvider
        {
            get
            {
                if (_personalizationProvider==null)
                {
                    _personalizationProvider = DependencyResolver.Current.GetService<IPersonalizationProvider>();
                }
                return _personalizationProvider;
            }
        }
    }
}
