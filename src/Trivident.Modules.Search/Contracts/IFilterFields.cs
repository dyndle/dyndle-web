using System.Collections.Generic;

namespace Trivident.Modules.Search.Contracts
{
    public interface IFilterFields
    {
        List<string> FilterBySchemas { get; }
        Dictionary<string, string> FilterByFields { get; }
    }
}
