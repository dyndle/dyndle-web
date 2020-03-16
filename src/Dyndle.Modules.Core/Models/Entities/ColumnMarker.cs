using System.Collections.Generic;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;

namespace Dyndle.Modules.Core.Models.Entities
{
    /// <summary>
    /// Class ColumnMarker.
    /// Implementation for the <see cref="IColumnMarker" /> used to identify new columns
    /// </summary>
    /// <seealso cref="EntityModel" />
    /// <seealso cref="IColumnMarker" />
    [ContentModelBySchemaTitle("ColumnStartMarker", true)]
    public class ColumnMarker : EntityModel, IColumnMarker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnMarker"/> class.
        /// </summary>
        public ColumnMarker()
        {
            Entities = new List<IEntityModel>();
        }
        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public List<IEntityModel> Entities { get; set; }
    }
}