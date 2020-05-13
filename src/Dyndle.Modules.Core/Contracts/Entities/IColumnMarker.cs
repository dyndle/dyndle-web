using System.Collections.Generic;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface IColumnMarker, used to identify entities used for the grouping in columns 
    /// </summary>
    /// <seealso cref="IEntityModel" />
    public interface IColumnMarker : IEntityModel
    {
        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>The entities.</value>
        List<IEntityModel> Entities { get; }
    }
}