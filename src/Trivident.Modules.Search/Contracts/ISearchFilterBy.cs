using System;
using System.Collections.Generic;

namespace Trivident.Modules.Search.Contracts
{
    public interface ISearchFilterBy
    {
        List<object> Items { get; set; }
        int Total { get; set; }
        string ErrorMessage { get; set; }
    }
}
