using System.Text;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Management.DebugInfo;

namespace Dyndle.Modules.Management.Controllers
{
    /// <summary>
    /// Class PageController.
    /// Used to handle all default page requests
    /// </summary>
    /// <seealso cref="ModuleControllerBase" />
    public class DebugController : Controller
    {

        private static readonly string DEBUG_INFO_RESOURCE_FILE_NAME_JS = "debuginfo.js";


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public DebugController(ILogger logger, IDD4TConfiguration configuration)
        {
            logger.ThrowIfNull(nameof(logger));
            configuration.ThrowIfNull(nameof(configuration));
        }

        /// <summary>
        /// List all items in the cache
        /// </summary>
        /// <returns>ActionResult.</returns>
        public virtual ActionResult Javascript()
        {
            string js;
            if (System.IO.File.Exists(HttpContext.Server.MapPath("/" + DEBUG_INFO_RESOURCE_FILE_NAME_JS)))
            {
                js = System.IO.File.ReadAllText(HttpContext.Server.MapPath("/" + DEBUG_INFO_RESOURCE_FILE_NAME_JS));
            }
            else
            {
                js = EmbeddedResourceHelper.GetResourceAsString(DEBUG_INFO_RESOURCE_FILE_NAME_JS);
            }
            return new FileContentResult(Encoding.UTF8.GetBytes(js), "application/javascript");
        }



    }
}