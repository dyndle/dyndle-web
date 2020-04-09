using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Exceptions;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Json;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.Core.Providers.Filter;

namespace Dyndle.Modules.Core.Controllers.Base
{
    /// <summary>
    /// Class ModuleControllerBase.
    /// Used as base class for all controllers, provides shared logic for exception handling and logging
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ModuleControllerBase : Controller
    {
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleControllerBase"/> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="logger">The logger.</param>
        public ModuleControllerBase(IContentProvider contentProvider,
                            ILogger logger)
        {
            contentProvider.ThrowIfNull(nameof(contentProvider));
            logger.ThrowIfNull(nameof(logger));

            _logger = logger;
            _contentProvider = contentProvider;
        }

        /// <summary>
        /// Returns the rendered result of a partial view to string.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public string PartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Gets the content provider.
        /// </summary>
        /// <value>The content provider.</value>
        protected IContentProvider ContentProvider { get { return _contentProvider; } }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        protected ILogger Logger { get { return _logger; } }

        /// <summary>
        /// HTTPs the not found.
        /// </summary>
        /// <param name="statusDescription">The status description.</param>
        /// <returns>ActionResult.</returns>
        protected new ActionResult HttpNotFound(string statusDescription = null)
        {
            return new HttpNotFoundResult(_contentProvider,
                            _logger, statusDescription);
        }

        /// <summary>
        /// HTTPs the unauthorized.
        /// </summary>
        /// <param name="statusDescription">The status description.</param>
        /// <returns>ActionResult.</returns>
        protected ActionResult HttpUnauthorized(string statusDescription = null)
        {
            return new HttpUnauthorizedResult(_contentProvider,
                            _logger, statusDescription);
        }

        /// <summary>
        /// HTTPs the exception.
        /// </summary>
        /// <param name="statusDescription">The status description.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult HttpException(string statusDescription = null)
        {
            return new HttpExceptionResult(_contentProvider,
                            _logger, statusDescription);
        }

        /// <summary>
        /// Override of standard method to serialize data as JSON to response ensuring that the data is camelCased and null items are omitted
        /// </summary>
        /// <param name="data">Object to serialize</param>
        /// <param name="contentType">The content type</param>
        /// <param name="contentEncoding">The content encoding</param>
        /// <param name="behavior">The JSON request behaviour</param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        /// <summary>
        /// Override of standard method to serialize data as JSON to response ensuring that the data is camelCased and null items are omitted
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Underlying Exception</param>
        /// <returns></returns>
        protected JsonResult JsonError(string message, Exception exception = null)
        {
            Logger.Error("{0} : {1}", message, exception != null ? exception.Message : "");
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Class HttpNotFoundResult.
        /// </summary>
        /// <seealso cref="HttpStatusCodeResult" />
        protected class HttpNotFoundResult : HttpStatusCodeResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HttpNotFoundResult"/> class.
            /// </summary>
            /// <param name="contentProvider">The content provider.</param>
            /// <param name="logger">The logger.</param>
            public HttpNotFoundResult(IContentProvider contentProvider,
                            ILogger logger) : this(contentProvider, logger, null) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="HttpNotFoundResult"/> class.
            /// </summary>
            /// <param name="contentProvider">The content provider.</param>
            /// <param name="logger">The logger.</param>
            /// <param name="statusDescription">The status description.</param>
            public HttpNotFoundResult(IContentProvider contentProvider,
                            ILogger logger, string statusDescription) : base(contentProvider, logger, 404, statusDescription) { }
        }

        /// <summary>
        /// Class HttpUnauthorizedResult.
        /// </summary>
        /// <seealso cref="HttpStatusCodeResult" />
        protected class HttpUnauthorizedResult : HttpStatusCodeResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HttpUnauthorizedResult"/> class.
            /// </summary>
            /// <param name="contentProvider">The content provider.</param>
            /// <param name="logger">The logger.</param>
            /// <param name="statusDescription">The status description.</param>
            public HttpUnauthorizedResult(IContentProvider contentProvider,
                            ILogger logger, string statusDescription) : base(contentProvider, logger, 401, statusDescription) { }
        }

        /// <summary>
        /// Class HttpExceptionResult.
        /// </summary>
        /// <seealso cref="HttpStatusCodeResult" />
        protected class HttpExceptionResult : HttpStatusCodeResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HttpExceptionResult"/> class.
            /// </summary>
            /// <param name="contentProvider">The content provider.</param>
            /// <param name="logger">The logger.</param>
            /// <param name="statusDescription">The status description.</param>
            public HttpExceptionResult(IContentProvider contentProvider,
                            ILogger logger, string statusDescription) : base(contentProvider, logger, 500, statusDescription) { }
        }

        /// <summary>
        /// Class HttpStatusCodeResult.
        /// </summary>
        /// <seealso cref="System.Web.Mvc.ViewResult" />
        protected class HttpStatusCodeResult : ViewResult
        {
            /// <summary>
            /// Gets the status code.
            /// </summary>
            /// <value>The status code.</value>
            public int StatusCode { get; private set; }

            /// <summary>
            /// Gets the status description.
            /// </summary>
            /// <value>The status description.</value>
            public string StatusDescription { get; private set; }

            /// <summary>
            /// The content provider
            /// </summary>
            private readonly IContentProvider _contentProvider;

            /// <summary>
            /// The logger
            /// </summary>
            private readonly ILogger _logger;

            /// <summary>
            /// Initializes a new instance of the <see cref="HttpStatusCodeResult"/> class.
            /// </summary>
            /// <param name="contentProvider">The content provider.</param>
            /// <param name="logger">The logger.</param>
            /// <param name="statusCode">The status code.</param>
            public HttpStatusCodeResult(IContentProvider contentProvider,
                            ILogger logger, int statusCode) : this(contentProvider, logger, statusCode, null) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="HttpStatusCodeResult"/> class.
            /// </summary>
            /// <param name="contentProvider">The content provider.</param>
            /// <param name="logger">The logger.</param>
            /// <param name="statusCode">The status code.</param>
            /// <param name="statusDescription">The status description.</param>
            public HttpStatusCodeResult(IContentProvider contentProvider,
                            ILogger logger, int statusCode, string statusDescription)
            {
                contentProvider.ThrowIfNull(nameof(contentProvider));
                logger.ThrowIfNull(nameof(logger));

                this._contentProvider = contentProvider;
                this._logger = logger;

                this.StatusCode = statusCode;
                this.StatusDescription = statusDescription;
            }

            /// <summary>
            /// When called by the action invoker, renders the view to the response.
            /// </summary>
            /// <param name="context">The context that the result is executed in.</param>
            /// <exception cref="HttpException"></exception>
            /// <exception cref="Exception"></exception>
            public override void ExecuteResult(ControllerContext context)
            {
                if (!context.HttpContext.IsCustomErrorEnabled || DebugInfoFilterProvider.IsDebugMode(context.HttpContext) || DyndleConfig.ThrowNotFound)
                {
                    throw new HttpException(this.StatusCode, this.StatusDescription);
                }

                try
                {
                    try
                    {
                        context.ThrowIfNull(nameof(context));
                        context.HttpContext.Response.Clear();
                        context.HttpContext.Response.ClearContent();
                        context.HttpContext.Response.StatusCode = this.StatusCode;
                        context.HttpContext.Response.TrySkipIisCustomErrors = true;

                        if (this.StatusDescription != null)
                        {
                            context.HttpContext.Response.StatusDescription = this.StatusDescription;
                        }

                        this.ViewBag.Message = context.HttpContext.Response.StatusDescription;

                        var errorPageUrl = DyndleConfig.GetErrorPageUrl(this.StatusCode);

                        // first check if the page exists locally
                        if (System.IO.File.Exists(context.HttpContext.Server.MapPath(errorPageUrl)))
                        {
                            // redirect to error page
                            context.HttpContext.Server.ClearError();
                            string baseUrl = $"{context.HttpContext.Request.Url.Scheme}://{context.HttpContext.Request.Url.Host}";
                            if (!context.HttpContext.Request.Url.IsDefaultPort)
                            {
                                baseUrl += ":" + context.HttpContext.Request.Url.Port;
                            }
                            //context.HttpContext.Response.RedirectPermanent(baseUrl + errorPageUrl + errorQuerystring);
                            context.HttpContext.Response.Write(System.IO.File.ReadAllText(context.HttpContext.Server.MapPath(errorPageUrl)));
                            return;
                        }

                        // page does not exist locally, so we will assume it is published from Tridion
                        var page = _contentProvider.BuildViewModel(errorPageUrl) as IWebPage;

                        if (page == null)
                        {
                            throw new PageNotFoundException(string.Format("Could not find page for error '{0}'", errorPageUrl));
                        }

                        context.RouteData.DataTokens.Clear();
                        context.RouteData.Values.Clear();
                        context.RouteData.DataTokens["area"] = page.MvcData.Area;
                        context.RouteData.Values["controller"] = page.MvcData.Controller;

                        this.ViewData.Model = page;
                        this.ViewName = page.GetView();
                        base.ExecuteResult(context);
                    }
                    catch (Exception fallbackEx)
                    {
                        _logger.Critical("Handling custom errors using Tridion page failed, initiating IO fall back!", LoggingCategory.System, fallbackEx);
                        context.HttpContext.Response.Clear();
                        context.HttpContext.Response.StatusCode = this.StatusCode;
                        context.HttpContext.Response.ContentType = "text/html";
                        context.HttpContext.Response.WriteFile(context.HttpContext.Server.MapPath(string.Format("\\errors\\{0}.html", this.StatusCode)));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Critical("IO fall back of custom errors failed, just writing a line.", LoggingCategory.System, ex);
                    context.HttpContext.Response.Clear();
                    context.HttpContext.Response.StatusCode = this.StatusCode;
                    context.HttpContext.Response.ContentType = "text/html";
                    context.HttpContext.Response.Write("We are unable to process this request, please try again later.");
                }
            }
        }
    }
}