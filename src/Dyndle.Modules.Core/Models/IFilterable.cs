using System.Collections.Generic;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Interface IFilterable
    /// </summary>
    public interface IFilterable
    {
        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        List<IFilter> Filters { get; set; }
    }
}
