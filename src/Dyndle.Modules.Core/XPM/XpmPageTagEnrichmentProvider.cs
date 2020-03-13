using System.Web.Mvc;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Mvc.ViewModels.XPM;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.XPM
{
    /// <summary>
    /// Adds XPM tag (i.e. the XPM comments plus the bootstrap javascript) to the IWebPage model so it can be written out in the page view.
    /// </summary>
    public class XpmPageTagEnrichmentProvider : IWebPageEnrichmentProvider
    {
        IDD4TConfiguration _configuration;
        ILogger _logger;

        /// <summary>
        /// Constructor that loads dependencies through the DI framework
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public XpmPageTagEnrichmentProvider(ILogger logger, IDD4TConfiguration configuration)
        {
            logger.ThrowIfNull(nameof(logger));
            configuration.ThrowIfNull(nameof(configuration));

            _logger = logger;
            _configuration = configuration;

            Enabled = _configuration.IsPreview && !string.IsNullOrEmpty(DyndleConfig.ContentManagerUrl);

            _logger.Debug($"loaded XpmPageTagEnrichmentProvider (enabled = {Enabled})");
        }

        /// <summary>
        /// Add XPM page tag to the web page
        /// </summary>
        /// <param name="webPage"></param>
        public void EnrichWebPage(IWebPage webPage)
        {
            if (webPage.ModelData != null && typeof(IPage).IsAssignableFrom(webPage.ModelData.GetType()))
            {
                SetXpmPageTag((IPage)webPage.ModelData, webPage);
            }
        }


        private void SetXpmPageTag(IPage page, IWebPage webPage)
        {
            if (Enabled)
            {
                var xpmPageTag = new MvcHtmlString(XpmExtensions.XpmMarkupService.RenderXpmMarkupForPage(
                   page, DyndleConfig.ContentManagerUrl));
                webPage.XpmPageTag = xpmPageTag;
            }
        }


        private bool Enabled { get; set; }
    }
}
