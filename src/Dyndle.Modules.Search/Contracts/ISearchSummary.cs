using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dyndle.Modules.Search.Contracts
{
    public interface ISearchSummary
    {
        string SearchResultTitle { get; }
        string SearchResultSummary { get; }
        string SearchResultImageUrl { get; }
    }
}