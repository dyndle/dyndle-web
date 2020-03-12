using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Attributes.ViewModels;
using Trivident.Modules.Core.Contracts.Entities;
using Trivident.Modules.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Models.Entities
{
    /// <summary>
    /// Class ColumnMarker.
    /// Implementation for the <see cref="Trivident.Modules.Core.Contracts.Entities.IColumnMarker" /> used to identify new columns
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.EntityModel" />
    /// <seealso cref="Trivident.Modules.Core.Contracts.Entities.IColumnMarker" />
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