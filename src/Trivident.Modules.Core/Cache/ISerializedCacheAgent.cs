using DD4T.ContentModel.Contracts.Caching;

namespace Trivident.Modules.Core.Cache
{
    public interface ISerializedCacheAgent : ICacheAgent
    {
        T Load<T>(string key);
    }
}