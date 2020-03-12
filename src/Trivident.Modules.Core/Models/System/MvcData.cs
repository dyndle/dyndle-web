using DD4T.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Models.System
{
    /// <summary>
    /// Class MvcData.
    /// Used to hold MVC data configured using Templates in Tridion
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.RenderData" />
    /// <seealso cref="Trivident.Modules.Core.Models.System.IMvcData" />
    public class MvcData : RenderData, IMvcData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MvcData"/> class.
        /// </summary>
        public MvcData()
        {
            RouteValues = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            RegionRouteValues = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the area.
        /// </summary>
        /// <value>The area.</value>
        public string Area { get; set; }

        /// <summary>
        /// Gets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public Dictionary<string, string> RouteValues { get; set; }
        /// <summary>
        /// Gets or sets the region route values.
        /// </summary>
        /// <value>The region route values.</value>
        public Dictionary<string, string> RegionRouteValues { get; set; }

        /// <summary>
        /// Gets or sets the name of the region view.
        /// </summary>
        /// <value>The name of the region view.</value>
        public string RegionViewName { get; set; }
        /// <summary>
        /// Gets or sets the size of the entity grid.
        /// </summary>
        /// <value>The size of the entity grid.</value>
        public int EntityGridSize { get; set; }
        /// <summary>
        /// Gets or sets the size of the region grid.
        /// </summary>
        /// <value>The size of the region grid.</value>
        public int RegionGridSize { get; set; }
    }
}