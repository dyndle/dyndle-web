using System.Collections.Generic;

namespace Trivident.Modules.Core.Models
{
    public interface IFilterable
    {
        List<IFilter> Filters { get; set; }
    }
}
