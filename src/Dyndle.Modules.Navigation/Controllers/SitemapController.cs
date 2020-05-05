using System.Web.Mvc;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Navigation.Models;
using Dyndle.Modules.Navigation.Providers;

namespace Dyndle.Modules.Navigation.Controllers
{
    /// <summary>
    /// Sitemap controller
    /// </summary>
    public class SitemapController : Controller
    {
        private readonly INavigationProvider _navigationProvider;
        private readonly IExtendedPublicationResolver _publicationResolver;



        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapController" /> class.
        /// </summary>
        /// <param name="navigationProvider">The navigationprovider.</param>
        /// <param name="publicationResolver">The publicationresolver.</param>
        public SitemapController(INavigationProvider navigationProvider, IExtendedPublicationResolver publicationResolver)
        {
            _navigationProvider = navigationProvider;
            _publicationResolver = publicationResolver;
        }

        /// <summary>
        /// Sitemap action
        /// </summary>
        /// <returns>XmlResult.</returns>
        public virtual ActionResult Sitemap()
        {
            var uri = _publicationResolver.GetBaseUri();
            var doc = _navigationProvider.GetFullSitemap().GetSitemapXmlDocument(uri);
            return new XmlResult(doc);

        }
    }
}