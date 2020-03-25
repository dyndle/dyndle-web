using System.Web;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Factories;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Services.Redirection;

namespace Dyndle.Modules.Core.Controllers
{
    /// <summary>
    /// Serve binary content from Tridion
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class BinaryController : Controller
    {
        private readonly IBinaryFactory _binaryFactory;
        private readonly ILogger _logger;
        private readonly IRedirectionService _redirectionService;
        private const string DEFAULT_BINARY_CACHE_PATH = "/BinaryData";

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryController"/> class.
        /// </summary>
        /// <param name="binaryFactory">The DD4T binary factory.</param>
        /// <param name="redirectionService">The DD4T redirection service.</param>
        /// <param name="logger">The logger.</param>
        public BinaryController(IBinaryFactory binaryFactory, IRedirectionService redirectionService, ILogger logger)
        {
            binaryFactory.ThrowIfNull(nameof(binaryFactory));
            logger.ThrowIfNull(nameof(logger));
            redirectionService.ThrowIfNull(nameof(redirectionService));

            _binaryFactory = binaryFactory;
            _redirectionService = redirectionService;
            _logger = logger;
        }

        private string BinaryCachePath
        {
            get
            {
                var binaryCachePath = CoreConstants.Configuration.BinaryCacheFolder.GetConfigurationValue(DEFAULT_BINARY_CACHE_PATH);
                if (binaryCachePath.StartsWith("~") || binaryCachePath.StartsWith("/"))
                {
                    return Server.MapPath(binaryCachePath);
                }
                return binaryCachePath;
            }
        }


        /// <summary>
        /// Binaries the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <exception cref="HttpException">404 - File '/" + url + "' cannot be found</exception>
        public ActionResult Binary(string url)
        {
            var physicalPath = BinaryCachePath + @"\" + url.Replace("/", @"\");
            var found = _binaryFactory.FindAndStoreBinary("/" + url, physicalPath);
            if (found)
            {
                return File(physicalPath, MimeMapping.GetMimeMapping(physicalPath));
            }
            if (!DyndleConfig.EnableRedirects)
            {
                throw new HttpException(404, "File '/" + url + "' cannot be found");
            }
            var redirectResult = _redirectionService.GetRedirectResult("/" + url, false);
            return redirectResult ?? throw new HttpException(404, "File '/" + url + "' cannot be found");
        }
    }
}