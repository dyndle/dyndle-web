using System;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Personalization.Contracts;
using System.Web.Mvc;
using Dyndle.Modules.Core.Attributes.Filter;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Personalization.Controllers
{
    /// <summary>
    /// Controller to enable tracking of data to update personalization data for the context visitor. 
    /// It is intended to be called by an AJAX call to {siteroot}/ajax/tracking?data={trackingdata}
    /// </summary>
    public class TrackingController : ModuleControllerBase
    {
        private readonly IPersonalizationProvider _personalizationProvider;

        public TrackingController(IContentProvider contentProvider, ILogger logger, IPersonalizationProvider personalizationProvider) : base(contentProvider, logger)
        {
            personalizationProvider.ThrowIfNull(nameof(personalizationProvider));
            _personalizationProvider = personalizationProvider;
        }
        
        /// <summary>
        /// Process tracking data to update segments
        /// </summary>
        /// <param name="data">A string containing tracking data to be used to update segments</param>
        /// <returns>An updated list of the current visitor's segments</returns>
        [AjaxEnabled]
        [HttpPost]
        public virtual ActionResult Index(string data)
        {
            try
            {
                _personalizationProvider.ProcessUpdate(HttpContext, data);
                return Json("OK",JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonError($"Error processing tracking data : {ex.Message}", ex);
            }
        }
    }
}
