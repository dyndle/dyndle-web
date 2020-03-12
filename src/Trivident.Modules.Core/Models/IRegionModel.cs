using System.Collections.Generic;

namespace Trivident.Modules.Core.Models
{
    /// <summary>
    /// Interface IRegionModel
    /// Can provide data needed to render a region
    /// </summary>
    public interface IRegionModel
    {
        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        List<IEntityModel> Entities { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets the route values.
        /// </summary>
        /// <value>The route values.</value>
        Dictionary<string, string> RouteValues { get; }
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        string ViewName { get; }
        /// <summary>
        /// Gets or sets the size of the grid.
        /// </summary>
        /// <value>The size of the grid.</value>
        int GridSize { get; set; }
    }
}