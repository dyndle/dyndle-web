using System;
using System.Linq;
using System.Web.Mvc;
using Trivident.Modules.Core.Attributes.Passive;
using Trivident.Modules.Core.Contracts;

namespace Trivident.Modules.Core.Attributes.Filter
{
    /// <summary>
    /// Global filter registered for all actions on all controllers
    /// </summary>
    public class AbsoluteUrlAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// local boolean to determine is an Action has the 
        /// needed decoration to enable this ActionFilter
        /// </summary>
        private bool _active;

        /// <summary>
        /// publication resolver.
        /// </summary>
        private readonly IExtendedPublicationResolver _publicationResolver;

        /// <summary>
        /// create an instance of AbsoluteUrlAttribute
        /// </summary>
        /// <param name="publicationResolver"></param>
        public AbsoluteUrlAttribute(IExtendedPublicationResolver publicationResolver)
        {
            publicationResolver.ThrowIfNull(nameof(publicationResolver));

            _publicationResolver = publicationResolver;
        }


        /// <summary>
        /// overrides the Result action.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (_active && !filterContext.IsChildAction)
            {
                var response = filterContext.HttpContext.Response;
                var baseUri = _publicationResolver.GetBaseUri();
                response.Filter = new AbsoluteUrlFilter(response.Filter, baseUri);
            }
        }
        /// <summary>
        /// determines based on ActionDescriptor to enable the Actionilter
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction) return;

            var previewAttribute = filterContext
                .ActionDescriptor
                .GetCustomAttributes(typeof(PreviewAttribute), false)
                .SingleOrDefault();

            _active = previewAttribute != null;
        }
    }
}