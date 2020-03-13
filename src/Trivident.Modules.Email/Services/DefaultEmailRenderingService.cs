using DD4T.Core.Contracts.Resolvers;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;

namespace Trivident.Modules.Email.Services
{
    /// <summary>
    /// Class DefaultEmailRenderingService.
    /// Default implementation used for rendering email based on tridion pages
    /// </summary>
    /// <seealso cref="Trivident.Modules.Email.Services.IEmailRenderingService" />
    public class DefaultEmailRenderingService : IEmailRenderingService
    {
        private readonly ILinkResolver _linkResolver;
        private readonly IExtendedPublicationResolver _publicationResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEmailRenderingService"/> class.
        /// </summary>
        /// <param name="linkResolver">The link resolver.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        public DefaultEmailRenderingService(ILinkResolver linkResolver, IExtendedPublicationResolver publicationResolver)
        {
            linkResolver.ThrowIfNull(nameof(linkResolver));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));

            _publicationResolver = publicationResolver;
            _linkResolver = linkResolver;
        }

        /// <summary>
        /// Renders the email body based on a page in Tridion.
        /// </summary>
        /// <param name="itemId">The component identifier used to determine the url of the page.</param>
        /// <param name="routeValues">The route values to use during rendering of the actions on the page and also used for placeholders.</param>
        /// <returns>System.String. Full page rendered and ready to send as email</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public string RenderEmailBody(int itemId, object routeValues)
        {
            //Try get a component url
            string url = _linkResolver.ResolveUrl(string.Format("tcm:{0}-{1}", _publicationResolver.ResolvePublicationId(), itemId));

            if (url == null)
            {
                throw new ArgumentException(string.Format("Could not resolve link for item '{0}' from publication '{1}'", itemId, _publicationResolver.ResolvePublicationId()));
            }

            //Call overload that takes localUri
            var localUri = new Uri(_publicationResolver.GetBaseUri(), url);
            return RenderEmailBody(localUri, routeValues);
        }

        /// <summary>
        /// Renders the email body based on a page in Tridion.
        /// </summary>
        /// <param name="localUri">The local URI to render.</param>
        /// <param name="routeValues">The route values to use during rendering of the actions on the page and also used for placeholders.</param>
        /// <returns>System.String. Full page rendered and ready to send as email</returns>
        public string RenderEmailBody(Uri localUri, object routeValues)
        {
            //Render the local page using mvc with routevalues
            var rawBody = RenderUrlToString(localUri, routeValues);

            // Tridion escapes {} characters in links, so revert this
            rawBody = rawBody.Replace("%7B", "{");
            rawBody = rawBody.Replace("%7D", "}");

            //Replace all placeholders with routevalues
            var emailBody = rawBody.FormatWith(routeValues, true);
            return emailBody;
        }

        private static string RenderUrlToString(Uri localUri, object routeValues)
        {
            try
            {
                using (var sw = new StringWriter())
                {
                    // Build a sub request pipeline
                    var request = new HttpRequest(null, localUri.ToString(), null);
                    var response = new HttpResponse(sw);
                    var context = new HttpContext(request, response);
                    var wrapper = new HttpContextWrapper(context);
                    var routeData = RouteTable.Routes.GetRouteData(wrapper);
                    var controllerName = routeData.Values["controller"].ToString();
                    var controllerFactory = ControllerBuilder.Current.GetControllerFactory();

                    // Add provided routeValues to the MVC route data
                    foreach (var item in new RouteValueDictionary(routeValues))
                    {
                        routeData.Values.Add(item.Key, item.Value);
                    }

                    // Setup request and controller
                    var requestContext = new RequestContext(wrapper, routeData);
                    var controller = controllerFactory.CreateController(requestContext, controllerName);

                    // Execute page
                    controller.Execute(requestContext);

                    // Return the rendered content
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Rendering email for Url '{0}' failed", localUri), ex);
            }
        }
    }
}
