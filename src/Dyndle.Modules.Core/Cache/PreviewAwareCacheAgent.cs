using System;
using System.Collections.Generic;
using System.Web;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Utils.Caching;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Cache
{
    /// <summary>
    /// Simple wrapper for the original DD4T DefaultCacheAgent 
    /// It only adds the generic Load method, but does not do any (de)serialization (regardless of the name of the interface)
    /// </summary>
    /// <seealso cref="ISerializedCacheAgent" />
    public class PreviewAwareCacheAgent : ICacheAgent
    {
        private readonly ICacheAgent _wrappedCacheAgent;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NonSerializedCacheAgent"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The DD4T configuration.</param>
        public PreviewAwareCacheAgent(IDD4TConfiguration configuration, ILogger logger)
        {
            configuration.ThrowIfNull(nameof(configuration));
            logger.ThrowIfNull(nameof(logger));
            _logger = logger;

            // note: we cannot use DI for this, because the signature of the PreviewAwareCacheAgent is the same as for the DefaultCacheAgent
            _wrappedCacheAgent = new DefaultCacheAgent(configuration, logger);
        }

        /// <summary>
        /// Set a MessageProvider that will listen to incoming messages (e.g. from JMS)
        /// </summary>
        public virtual IObservable<ICacheEvent> MessageProvider
        {
            set
            {
                if (_wrappedCacheAgent is DefaultCacheAgent defaultCacheAgent) 
                {
                    defaultCacheAgent.Subscribe(value);
                }
            }
        }

        /// <summary>
        /// Loads the specified key of generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
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
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                Remove(key);
                return null;
            }
            return _wrappedCacheAgent.Load(key);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        public void Remove(string key)
        {
            _wrappedCacheAgent.Remove(key);
        }

        /// <summary>
        /// Store item in cache
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <param name="item">Item to store</param>
        public void Store(string key, object item)
        {
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                return;
            }
            _wrappedCacheAgent.Store(key, item);
        }

        /// <summary>
        /// Store item in cache with dependency on Tridion items
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <param name="item">Item to store</param>
        /// <param name="dependOnTcmUris">List of TCM URIs of Tridion items</param>
        public void Store(string key, object item, List<string> dependOnTcmUris)
        {
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                return;
            }
            _wrappedCacheAgent.Store(key, item, dependOnTcmUris);
        }

        /// <summary>
        /// Store item in cache using a specific region
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <param name="region">Region to use (this corresponds with an appSetting 'DD4T.CacheSettings.[region]' in the Web.config</param>
        /// <param name="item">Item to store</param>
        public void Store(string key, string region, object item)
        {
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                return;
            }
            _wrappedCacheAgent.Store(key, region, item);
        }

        /// <summary>
        /// Store item in cache using a specific region
        /// </summary>
        /// <param name="key">Key of the cached item</param>
        /// <param name="region">Region to use (this corresponds with an appSetting 'DD4T.CacheSettings.[region]' in the Web.config</param>
        /// <param name="item">Item to store</param>
        /// <param name="dependOnTcmUris">List of TCM URIs of Tridion items</param>
        public void Store(string key, string region, object item, List<string> dependOnTcmUris)
        {
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                return;
            }
            _wrappedCacheAgent.Store(key, region, item, dependOnTcmUris);
        }

        private bool IsPreviewActive
        {
            get
            {
                if (HttpContext.Current.Items.Contains("preview-active"))
                {
                    return ((bool?)HttpContext.Current.Items["preview-active"]).Value;
                }
                bool isActive;
                if (HttpContext.Current.Request.Cookies["preview-session-token"] != null)
                {
                    isActive = true;
                    var value = HttpContext.Current.Request.Cookies["preview-session-token"].Value;
                    _logger.Debug($"preview-session-token is set ({value}), assuming we are in XPM preview mode. Pages, DCPs, Models and Output will not be read from cache");
                }
                else
                {
                    isActive = false;
                    _logger.Debug($"preview-session-token is not set, all items will be cached normally");
                }
                HttpContext.Current.Items.Add("preview-active", new bool?(isActive));
                return isActive;
            }
        }
    }
}