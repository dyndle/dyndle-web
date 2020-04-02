using DD4T.ContentModel.Contracts.Caching;

namespace Dyndle.Modules.Core.Cache
{
    /// <summary>
    /// Interface which extends ICacheAgent with the purpose to serialize and deserialize each cached item
    /// This is slower than the normal cache agent so it should only be used in cases where each object should be
    /// truly unique, for example if you are modifying a cached object for a particular visitor.
    /// </summary>
    public interface ISerializedCacheAgent : ICacheAgent
    {
        /// <summary>
        /// Generic overload of the regular Load(string key) method in ICacheAgent, needed to provide the type to deserialize into.
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="key">Key of the cached item</param>
        /// <returns></returns>
        T Load<T>(string key);
    }
}