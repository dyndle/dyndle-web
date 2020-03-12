using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Trivident.Modules.Core.DebugInfo;
using Trivident.Modules.Management.Contracts;
using Trivident.Modules.Management.Models;

namespace Trivident.Modules.Management.Controllers
{
    /// <summary>
    /// Class PageController.
    /// Used to handle all default page requests
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Controllers.Base.ModuleControllerBase" />
    public class DebugController : Controller
    {

        private static readonly string DEBUG_INFO_RESOURCE_FILE_NAME_JS = "debuginfo.js";

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IDD4TConfiguration _configuration;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public DebugController(ILogger logger, IDD4TConfiguration configuration)
        {
            logger.ThrowIfNull(nameof(logger));
            configuration.ThrowIfNull(nameof(configuration));

            _logger = logger;
            _configuration = configuration;
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