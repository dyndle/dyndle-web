using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Core.Controllers
{
    /// <summary>
    /// Class EntityController.
    /// Used to render all default entities using the view configured on the Template in Tridion.
    /// </summary>
    /// <seealso cref="ModuleControllerBase" />
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