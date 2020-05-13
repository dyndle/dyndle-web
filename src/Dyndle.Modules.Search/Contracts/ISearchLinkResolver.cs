namespace Dyndle.Modules.Search.Contracts
{
    public interface ISearchLinkResolver
    {
        void Resolve(ISearchResultItem item);
    }
}
