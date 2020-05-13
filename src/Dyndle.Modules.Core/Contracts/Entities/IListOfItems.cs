using System.Collections.Generic;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Defines a renderable entity that will contain Items
    /// </summary>
    /// <seealso cref="IRenderable" />
    public interface IListOfItems : IRenderable
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        List<EntityModel> Items { get; set; }
    }
}