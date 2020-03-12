using DD4T.Core.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Models.System
{
    /// <summary>
    /// Interface IMvcData
    /// Used to identify the MVC route data provided by Tridion templates
    /// </summary>
    /// <seealso cref="DD4T.Core.Contracts.ViewModels.IRenderData" />
    public interface IMvcData : IRenderData
    {
        /// <summary>
        /// Gets the area.
        /// </summary>
        /// <value>The area.</value>
        string Area { get; }
        /// <summary>
        /// Gets the route values.
        /// </summary>
        /// <value>The route values.</value>
        Dictionary<string, string> RouteValues { get; }
        /// <summary>
        /// Gets or sets the region route values.
        /// </summary>
        /// <value>The region route values.</value>
        Dictionary<string, string> RegionRouteValues { get; set; }

        /// <summary>
        /// Gets or sets the name of the region view.
        /// </summary>
        /// <value>The name of the region view.</value>
        string RegionViewName { get; set; }

        /// <summary>
        /// Gets or sets the size of the entity grid.
        /// </summary>
        /// <value>The size of the entity grid.</value>
        int EntityGridSize { get; set; }
        /// <summary>
        /// Gets or sets the size of the region grid.
        /// </summary>
        /// <value>The size of the region grid.</value>
        int RegionGridSize { get; set; }
    }
}