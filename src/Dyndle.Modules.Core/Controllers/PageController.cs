﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Attributes.Caching;
using Dyndle.Modules.Core.Attributes.Filter;
using Dyndle.Modules.Core.Attributes.Passive;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Exceptions;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Models.System;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.Core.Services.Preview;
using Dyndle.Modules.Core.Services.Redirection;

namespace Dyndle.Modules.Core.Controllers
{
    /// <summary>
    /// Class PageController.
    /// Used to handle all default page requests
    /// </summary>
    /// <seealso cref="ModuleControllerBase" />
    public class PageController : ModuleControllerBase
    {
        private readonly IList<IWebPageEnrichmentProvider> _enrichmentProviders;
        private readonly IRedirectionService _redirectionService;
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
        [DyndleOutputCache]
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
        /// Preview a page from the Tridion CME
        /// </summary>
        /// <param name="data">JSON representation of the page (normally generated by the DD4T template and posted to this action by the Dyndle Preview Page TBB)</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Preview]
        [AbsoluteUrl]
        public ActionResult Preview(string data)
        {
            try
            {
                var model = _previewContentService.GetPage(data);
                return View(model.GetView(), model);
            }
            catch (CacheAgentMismatchException e)
            {
                return new ContentResult() { Content = e.Message, ContentEncoding = Encoding.UTF8, ContentType = "text/html" };
            }
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

        /// <summary>
        /// Blanks this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Blank()
        {
            return new EmptyResult();
        }

        /// <summary>Called when an unhandled exception occurs in the action.</summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext filterContext)
        {

            //Log the error!!
            Logger.Error($"{filterContext.Exception.GetType()} - message: {filterContext.Exception.Message}\r\n{filterContext.Exception.StackTrace}");

            if (!HttpContext.IsCustomErrorEnabled) return;

            var routeData = new RouteData();

            routeData.Values.Add("controller", "Page");
            routeData.Values.Add("area", "Core");
            routeData.Values.Add("action", "HttpException");

            IController controller = DependencyResolver.Current.GetService<PageController>();

            controller.Execute(new RequestContext(HttpContext, routeData));
            filterContext.ExceptionHandled = true;

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