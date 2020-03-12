using DD4T.ContentModel.Contracts.Logging;
using Trivident.Modules.Core.Controllers.Base;
using Trivident.Modules.Core.Models;
using Trivident.Modules.Core.Providers.Content;
using System.Web.Mvc;
using Trivident.Modules.Core.Models.System;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Net.Mime;
using Trivident.Modules.Core.Configuration;

namespace Trivident.Modules.Core.Controllers
{
    /// <summary>
    /// Class RegionController.
    /// Used to render all default regions using the view configured on the Template in Tridion.
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Controllers.Base.ModuleControllerBase" />
    public class RegionController : ModuleControllerBase
    {
        private const string DEFAULT_REGION_VIEW = "Region";
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionController"/> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="logger">The logger.</param>
        public RegionController(IContentProvider contentProvider, ILogger logger)
            : base(contentProvider, logger)
        {
        }

        /// <summary>
        /// Renders the specified region using configured view
        /// </summary>
        /// <param name="region">The Region.</param>
        /// <param name="regionView">The view.</param>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly]
        public ActionResult Region(IRegionModel region, string regionView = "")
        {
            if (!regionView.IsNullOrEmpty())
            {
                return PartialView(regionView, region);
            }
            string view = region.Name;
            if (!string.IsNullOrEmpty(region.ViewName))
            {
                view = region.ViewName;
            }
            return PartialView(ViewExists(view) ? view : DefaultRegionView, region);
        }
        /// <summary>
        /// Used to render specific components in a region of the includes page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="region">The region.</param>
        /// <param name="view">The view.</param>
        /// <returns>ActionResult.</returns>
        [ChildActionOnly]
        public virtual ActionResult Includes(IncludePage page, string region, string view = "")
        {
            if (page == null)
            {
                Logger.Warning($"trying to include region {region} from an unpublished / non-existing page; returning an empty string");
                return new ContentResult() { Content = "", ContentEncoding = Encoding.UTF8, ContentType = "text/html" };
            }

            // Work around for issue with grouping of same regions with a seperate region inbetween
            // Example: Combination of Utility,Social,Utility,CookieOptIn regions
            // will cause issue with missing entities in the first RegionModel
            var regionModels = page.Regions.FindAll(r => r.Name.Equals(region, StringComparison.InvariantCultureIgnoreCase));
            var regionModel = regionModels.FirstOrDefault();

            if (regionModel != null && regionModels.Count > 1)
            {
                foreach (var model in regionModels.Skip(1)) // Skip the first one since we will use that as our base RegionModel
                {
                    regionModel.Entities.AddRange(model.Entities);
                }
            }
            if (regionModel == null)
            {
                // trying to include a non-existing region, let's return an empty string
                return new ContentResult() { Content = "", ContentEncoding = Encoding.UTF8, ContentType = "text/html" };
            }
            return Region(regionModel, view);
        }

        private bool ViewExists(string name)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, name, null);
            return (result.View != null);
        }
        private string DefaultRegionView
        {
            get
            {
                return DyndleConfig.DefaultRegionView ?? DEFAULT_REGION_VIEW;
            }
        }
    }
}