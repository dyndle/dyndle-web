using DD4T.ContentModel.Contracts.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Trivident.Modules.Core.Attributes.Filter
{
    /// <summary>
    /// Class HandleSectionErrorAttribute.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.HandleErrorAttribute" />
    internal class HandleSectionErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleSectionErrorAttribute" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public HandleSectionErrorAttribute(ILogger logger)
        {
            logger.ThrowIfNull(nameof(logger));
            _logger = logger;

            View = "SectionError";
        }

        /// <summary>
        /// Called when [exception] occurs and displays the error without breaking the entire page.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.IsChildAction)
                return;

            base.OnException(filterContext);

            _logger.Error(filterContext.Exception.ToString());

            ViewDataDictionary data = new ViewDataDictionary(new HandleErrorInfo(filterContext.Exception, (string)filterContext.RouteData.Values["controller"], (string)filterContext.RouteData.Values["action"]));
            filterContext.Result = new ViewResult { ViewName = View, ViewData = data };
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.HttpContext.Response.StatusCode = 500;
        }
    }
}