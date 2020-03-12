using DD4T.ContentModel.Contracts.Logging;
using Trivident.Modules.Core.Controllers.Base;
using Trivident.Modules.Core.Models;
using Trivident.Modules.Core.Providers.Content;
using System.Web.Mvc;
using System;
using System.Web.Compilation;
using System.Configuration;
using DD4T.ContentModel;

namespace Trivident.Modules.Core.Controllers
{
    /// <summary>
    /// Class EntityController.
    /// Used to render all default entities using the view configured on the Template in Tridion.
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Controllers.Base.ModuleControllerBase" />
    public class EntityController : ModuleControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityController"/> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="logger">The logger.</param>
        public EntityController(IContentProvider contentProvider, ILogger logger)
            : base(contentProvider, logger)
        {
        }

        /// <summary>
        /// Renders the specified entity using configured view
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly]
        public ActionResult Entity(IEntityModel entity)
        {
            var viewName = entity.GetView();
            return PartialView(viewName, entity);
        }
    }
}