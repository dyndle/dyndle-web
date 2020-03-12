using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Trivident.Modules.Core.Attributes.Caching;
using Trivident.Modules.Core.Attributes.Passive;
using Trivident.Modules.Core.Configuration;
using Trivident.Modules.Core.Contracts;
using Trivident.Modules.Core.Controllers.Base;
using Trivident.Modules.Core.Models;
using Trivident.Modules.Core.Models.System;
using Trivident.Modules.Core.Providers.Content;
using Trivident.Modules.Core.Services.Preview;
using Trivident.Modules.Core.Services.Redirection;

namespace Trivident.Modules.Core.Controllers
{
    /// <summary>
    /// Class PageController.
    /// Used to handle all default page requests
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Controllers.Base.ModuleControllerBase" />
    public class PageController : ModuleControllerBase
    {
        /// <summary>
        /// The enrichment providers
        /// </summary>
        private readonly IList<IWebPageEnrichmentProvider> _enrichmentProviders;
        /// <summary>
        /// The redirection service
        /// </summary>
        private readonly IRedirectionService _redirectionService;
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IDD4TConfiguration _configuration;
        /// <summary>
        /// The preview Cotnent  Service
        /// </summary>
        private readonly IPreviewContentService _previewContentService;
        private const string DEFAULT_INCLUDES_VIEW = "Includes";

        /// <summary>
        /// Initializes a new instance of the <see cref="PageController"/> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="enrichmentProviders">The enrichment providers.</param>
        /// <param name="previewContentService">The preview service.</param>
        /// <param name="redirectionService">The redirection service.</param>
        /// <param name="configuration">The configuration.</param>
        public PageController(IContentProvider contentProvider,
                                ILogger logger,
                                  IPreviewContentService previewContentService,
                                IList<IWebPageEnrichmentProvider> enrichmentProviders, IRedirectionService redirectionService, IDD4TConfiguration configuration) :
            base(contentProvider, logger)
        {
            enrichmentProviders.ThrowIfNull(nameof(enrichmentProviders));
            redirectionService.ThrowIfNull(nameof(redirectionService));
            configuration.ThrowIfNull(nameof(configuration));
            previewContentService.ThrowIfNull(nameof(previewContentService));

            _redirectionService = redirectionService;
            _enrichmentProviders = enrichmentProviders;
            _configuration = configuration;
            _previewContentService = previewContentService;
        }

        /// <summary>
        /// Enriches the page model using all registered implementations of <see cref="IWebPageEnrichmentProvider"/> 
        /// Renders the specified page using the view specified in Tridion.
        /// When no model found uses the <see cref="IRedirectionService"/> to redirect the page.
        /// If theres no way to render or redirect, renders the 404 page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>ActionResult.</returns>
        [OutputCache(CacheProfile = "Default")]
        public virtual ActionResult Page(IWebPage page)
        {
            if (page == null)
            {
                if (DyndleConfig.EnableRedirects)
                {
                    var redirectResult = _redirectionService.GetRedirectResult(Request.Url.PathAndQuery, true);
                    return redirectResult ?? HttpNotFound();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            // store uris of the page and all the CPs on the page in the HttpContext for use in the output cache provider
            var dependsOnUris = new List<string>();
            dependsOnUris.Add(page.Id.ToString());
            dependsOnUris.AddRange(((IPage)page.ModelData).ComponentPresentations?.Select(cp => cp.Component.Id) ?? Enumerable.Empty<string>());
            HttpContext.Items.Add(CoreConstants.General.DependentOnUris, dependsOnUris);

            PreProcess(page);
            BuildWebPageEnrichments(page);

            // if this page is the configured 404 page, set the status to 404
            if (HttpContext.Request.Url.LocalPath == DyndleConfig.GetErrorPageUrl(404))
            {
                HttpContext.Response.TrySkipIisCustomErrors = true;
                HttpContext.Response.StatusCode = 404;
            }
            return View(page.GetView(), page);
        }

        /// <summary>
        /// Pre-process the page before it is being enriched
        /// </summary>
        /// <param name="page"></param>
        protected virtual void PreProcess(IWebPage page)
        {
            // Don't do anything here, this is meant to be overridden in a child class
        }

        /// <summary>
        /// PreviewPage
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        [Preview]
        public ActionResult Preview(string data, string url)
        {
            var model = _previewContentService.GetPage(data, url);
            return View(model.GetView(), model);
        }


        /// <summary>
        /// Used to render specific components in a region of the includes page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="view">The view.</param>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly]
        public virtual ActionResult Includes(IncludePage page, string view)
        {
            if (page == null)
            {
                Logger.Warning($"trying to include an unpublished / non-existing page; returning an empty string");
                return new ContentResult() { Content = "", ContentEncoding = Encoding.UTF8, ContentType = "text/html" };
            }

            BuildWebPageEnrichments(page);
            return PartialView(view ?? IncludesView, page);
        }

        public ActionResult Blank()
        {
            return new EmptyResult();
        }

        /// <summary>
        /// Enriches the web page using the providers.
        /// </summary>
        /// <param name="webPage">The web page.</param>
        private void BuildWebPageEnrichments(IWebPage webPage)
        {
            Logger.Debug("about to Enrich IWebPage. IWebPageEnrichmentProvider count = {0}".FormatString(_enrichmentProviders.Count));
            foreach (var provider in _enrichmentProviders)
            {
                provider.EnrichWebPage(webPage);
            }
        }
        private string IncludesView
        {
            get
            {
                return DyndleConfig.DefaultIncludesView ?? DEFAULT_INCLUDES_VIEW;
            }
        }     
    }
}