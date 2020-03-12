
using Trivident.Modules.Core.Contracts;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trivident.Modules.Core.Html
{
    /// <summary>
    /// Extension methods for UrlHelper
    /// </summary>
    public static class UrlHelperExtensions
    {
        private static readonly string _urlPrefixCacheKey = "cached-url-prefix";
        /// <summary>
        /// Get the base URL of the current site context (eg /en etc)
        /// </summary>
        /// <param name="helper">The UrlHelper</param>
        /// <returns>Site root base URL</returns>
        public static string GetBaseUrl(this UrlHelper helper)
        {
            return helper.GetBaseUrl(false);
        }

        /// <summary>
        /// Get the base URL of the current site context (eg /en etc)
        /// </summary>
        /// <param name="helper">The UrlHelper</param>
        /// <param name="absolute">if set to <c>true</c> [absolute].</param>
        /// <returns>
        /// Site root base URL
        /// </returns>
        public static string GetBaseUrl(this UrlHelper helper, bool absolute)
        {
            if (absolute)
            {
                return PublicationResolver.GetBaseUri().ToString().TrimEnd('/');
            }
            else
            {
                return PublicationResolver.GetBaseUri().AbsolutePath.TrimEnd('/');
            }
        }

        /// <summary>
        /// Process a URL to (optionally) prefix the protocol domain if a 'full-link' query string parameter is found on the current request
        /// </summary>
        /// <param name="helper">The UrlHelper</param>
        /// <param name="url">The URL to prefix</param>
        /// <returns>full URL</returns>
        public static string GetUrl(this UrlHelper helper, string url)
        {
            if (url!=null && !url.ToLower().StartsWith("http"))
            {
                return GetUrlPrefix() + url;
            }
            return url;
        }

        private static string GetUrlPrefix()
        {
            if (!HttpContext.Current.Items.Contains(_urlPrefixCacheKey))
            {
                var prefix = "";
                if (HttpContext.Current.Request.Params.AllKeys.Contains("full-url"))
                {
                    prefix = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                }
                HttpContext.Current.Items.Add(_urlPrefixCacheKey, prefix);
            }
            return (string)HttpContext.Current.Items[_urlPrefixCacheKey];
        }

        private static IExtendedPublicationResolver _publicationResolver;
        private static IExtendedPublicationResolver PublicationResolver
        {
            get
            {
                if (_publicationResolver == null)
                {
                    _publicationResolver = DependencyResolver.Current.GetService<IExtendedPublicationResolver>();
                }
                return _publicationResolver;
            }
        }
    }
}
