using System.Collections.Generic;

namespace Dyndle.Modules.Core.Models
{
    public interface IFilterable
    {
        List<IFilter> Filters { get; set; }
    }
}
