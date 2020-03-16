using System.Collections.Generic;

namespace Dyndle.Modules.Search.Contracts
{
    public interface IFilterFields
    {
        List<string> FilterBySchemas { get; }
        Dictionary<string, string> FilterByFields { get; }
    }
}
