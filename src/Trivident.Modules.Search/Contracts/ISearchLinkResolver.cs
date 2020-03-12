using System.Collections.Generic;

namespace Trivident.Modules.Search.Contracts
{
    public interface ISearchLinkResolver
    {
        void Resolve(ISearchResultItem item);
    }
}
