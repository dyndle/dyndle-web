using DD4T.ContentModel.Contracts.Logging;
using Trivident.Modules.Core.Controllers.Base;
using Trivident.Modules.Core.Models;
using Trivident.Modules.Core.Providers.Content;
using System.Web.Mvc;

namespace Trivident.Modules.Email.Controllers
{
    /// <summary>
    /// Class EmailController.
    /// Used for the rendering of entities in emails
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Controllers.Base.ModuleControllerBase" />
    public class EmailController : ModuleControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailController"/> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="logger">The logger.</param>
        public EmailController(IContentProvider contentProvider, ILogger logger)
            : base(contentProvider, logger)
        {
        }

        /// <summary>
        /// Map and render an entity model
        /// </summary>
        /// <param name="entity">The entity model</param>
        /// <returns>Rendered entity model</returns>
        public virtual ActionResult Email(EntityModel entity)
        {
            return View(entity.GetView(), entity);
        }
    }
}