using System.Collections.Generic;

namespace Dyndle.Modules.Search.Contracts
{
    public interface ISearchFilterBy
    {
        List<object> Items { get; set; }
        int Total { get; set; }
        string ErrorMessage { get; set; }
    }
}
