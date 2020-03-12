using Trivident.Modules.Core.Models;
using System.Collections.Generic;

namespace Trivident.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface IColumnMarker, used to identify entities used for the grouping in columns 
    /// handled by <see cref="Trivident.Modules.Core.Attributes.ViewModels.ComponentPresentationRegions" />
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.IEntityModel" />
    public interface IColumnMarker : IEntityModel
    {
        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>The entities.</value>
        List<IEntityModel> Entities { get; }
    }
}