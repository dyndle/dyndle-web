using System.Web.Mvc;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Navigation.Models;
using Dyndle.Modules.Navigation.Providers;

namespace Dyndle.Modules.Navigation.Controllers
{
    public class SitemapController : Controller
    {
        private readonly INavigationProvider _navigationProvider;
        private readonly IExtendedPublicationResolver _publicationResolver;

        public SitemapController(INavigationProvider navigationProvider, IExtendedPublicationResolver publicationResolver)
        {
            _navigationProvider = navigationProvider;
            _publicationResolver = publicationResolver;
        }
      
        public virtual ActionResult Sitemap()
        {
            var uri = _publicationResolver.GetBaseUri();
            var doc = _navigationProvider.GetFullSitemap().GetSitemapXmlDocument(uri);
            return new XmlResult(doc);

        }
    }
}