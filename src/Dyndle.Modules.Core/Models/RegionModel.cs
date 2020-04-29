using System;
using System.Collections.Generic;
using DD4T.ContentModel;
using Dyndle.Modules.Core.Models.System;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Class RegionModel.
    /// Holds data needed to render a region including the entities
    /// </summary>
    /// <seealso cref="IRegionModel" />
    public class RegionModel : IRegionModel
    {
        /// <summary>
        /// Gets or sets the constraint.
        /// </summary>
        /// <value>The constraint.</value>
        public string Constraint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionModel"/> class.
        /// </summary>
        /// <param name="mvcData">The MVC data.</param>
        /// <param name="constraint">The constraint.</param>
        public RegionModel(IMvcData mvcData, string constraint)
        {
            Constraint = constraint;

            if (mvcData != null)
            {
                Name = mvcData.Region;
                RouteValues = mvcData.RegionRouteValues;
                ViewName = mvcData.RegionViewName;
                GridSize = mvcData.RegionGridSize;
            }

            Entities = new List<IEntityModel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionModel"/> class based on a DD4T representation of Tridion region.
        /// </summary>
        /// <param name="region">The region</param>
        public RegionModel(IRegion region)
        {
            Name = region.Name;
            ViewName = Configuration.DyndleConfig.DefaultRegionView;
            GridSize = 12;

            Constraint = Name;
            RouteValues = new Dictionary<string, string>();

        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the size of the grid.
        /// </summary>
        /// <value>The size of the grid.</value>
        public int GridSize { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public List<IEntityModel> Entities { get; set; }

        /// <summary>
        /// Gets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public Dictionary<string, string> RouteValues { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        public string ViewName { get; set; }
    }
}
