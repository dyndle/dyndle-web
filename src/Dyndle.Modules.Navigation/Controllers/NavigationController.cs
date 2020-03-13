using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Modules.Core.Results;
using Dyndle.Modules.Navigation.Binders;
using Dyndle.Modules.Navigation.Models;

namespace Dyndle.Modules.Navigation.Controllers
{
    /// <summary>
    /// Class NavigationController.
    /// Used to render different types of navigation views
    /// </summary>
    /// <seealso cref="ModuleControllerBase" />
    public class NavigationController : ModuleControllerBase
    {
        /// <summary>
        /// publication resolver.
        /// </summary>
        private readonly IExtendedPublicationResolver _publicationResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationController"/> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        public NavigationController(IContentProvider contentProvider, ILogger logger, IExtendedPublicationResolver publicationResolver)
            : base(contentProvider, logger)
        {
            publicationResolver.ThrowIfNull(nameof(publicationResolver));
            _publicationResolver = publicationResolver;
        }

        /// <summary>
        /// Populate and render a navigation entity model
        /// </summary>
        /// <param name="entity">The navigation entity from DD4T</param>
        /// <param name="model">The <see cref="ISitemapItem"/> model constructed by the <see cref="SitemapItemModelBinder"/> </param>
        /// <returns>ActionResult.</returns>
        public virtual ActionResult Navigation(IEntityModel entity, ISitemapItem model)
        {
            return View(entity.GetView(), model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("sitemap.xml")]
        [Route("{publication}/sitemap.xml")]
        public virtual ActionResult Sitemap(ISitemapItem model)
        {
            var uri = _publicationResolver.GetBaseUri();
            var doc = model.GetSitemapXmlDocument(uri);
            return new XmlResult(doc);
        }
    }
}