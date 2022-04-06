using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Caching;
using Dyndle.Modules.Core.Extensions;
using Newtonsoft.Json;

namespace Dyndle.Modules.Core.Cache
{
    /// <summary>
    /// CacheAgent that serializes and deserializes the objects
    /// User for cache that needs to have a different instance of the object (clone)
    /// </summary>
    /// <seealso cref="ISerializedCacheAgent" />
    public class SerializedCacheAgent : ISerializedCacheAgent
    {
        private readonly ICacheAgent _cacheAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializedCacheAgent"/> class.
        /// </summary>
        /// <param name="cacheAgent">The cache agent.</param>
        public SerializedCacheAgent(ICacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            _cacheAgent = cacheAgent;
        }

        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="key">Key of the cached item</param>
        /// <returns></returns>
        public T Load<T>(string key)
        {
            return (T)Load(key);
        }

        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Load(string key)
        {
            return DeserializeObject(_cacheAgent.Load(key));
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            _cacheAgent.Remove(key);
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void Store(string key, object item)
        {
            _cacheAgent.Store(key, SerializeObject(item));
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="region">The region.</param>
        /// <param name="item">The item.</param>
        public void Store(string key, string region, object item)
        {
            _cacheAgent.Store(key, region, SerializeObject(item));
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <param name="dependOnTcmUris">The depend on TCM uris.</param>
        public void Store(string key, object item, List<string> dependOnTcmUris)
        {
            _cacheAgent.Store(key, SerializeObject(item), dependOnTcmUris);
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="region">The region.</param>
        /// <param name="item">The item.</param>
        /// <param name="dependOnTcmUris">The depend on TCM uris.</param>
        public void Store(string key, string region, object item, List<string> dependOnTcmUris)
        {
            _cacheAgent.Store(key, region, SerializeObject(item), dependOnTcmUris);
        }

        private object SerializeObject(object item)
        {
            return JsonConvert.SerializeObject(item, GetSettings());
        }

        private JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            settings.Converters.Add(new TcmUriConverter());
            settings.Converters.Add(new MvcHtmlStringConverter());
            
            return settings;
        }

        private object DeserializeObject(object item)
        {
            if (item == null)
                return null;

            return JsonConvert.DeserializeObject((string)item, GetSettings());
        }

        private class TcmUriConverter : Newtonsoft.Json.Converters.CustomCreationConverter<TcmUri>
        {
            public override TcmUri Create(Type objectType)
            {
                return TcmUri.NullUri;
            }
        }

        private class MvcHtmlStringConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(MvcHtmlString).IsAssignableFrom(objectType);
            }

            public override object ReadJson(
                JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var value = reader.Value as string;
                return Activator.CreateInstance(objectType, value);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var htmlString = value as IHtmlString;
                if (htmlString == null)
                {
                    return;
                }

                writer.WriteValue(htmlString.ToString());
            }
        }
    }
}