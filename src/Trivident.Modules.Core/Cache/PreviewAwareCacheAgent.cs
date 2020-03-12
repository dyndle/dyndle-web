using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Utils.Caching;
using System;
using System.Collections.Generic;
using System.Web;
using Trivident.Modules.Core.Extensions;

namespace Trivident.Modules.Core.Cache
{
    /// <summary>
    /// Simple wrapper for the original DD4T DefaultCacheAgent 
    /// It only adds the generic Load method, but does not do any (de)serialization (regardless of the name of the interface)
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Cache.ISerializedCacheAgent" />
    public class PreviewAwareCacheAgent : ICacheAgent
    {
        private readonly ICacheAgent _wrappedCacheAgent;
        private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NonSerializedCacheAgent"/> class.
        /// </summary>
        /// <param name="cacheAgent">The DD4T cache agent.</param>
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

        public virtual IObservable<ICacheEvent> MessageProvider
        {
            set
            {
                if (_wrappedCacheAgent is DefaultCacheAgent)
                {
                    ((DefaultCacheAgent)_wrappedCacheAgent).Subscribe(value);
                }
            }
        }

        /// <summary>
        /// Loads the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Load<T>(string key)
        {
            return (T)Load(key);
        }

        public object Load(string key)
        {
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                Remove(key);
                return null;
            }
            return _wrappedCacheAgent.Load(key);
        }

        public void Remove(string key)
        {
            _wrappedCacheAgent.Remove(key);
        }

        public void Store(string key, object item)
        {
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                return;
            }
            _wrappedCacheAgent.Store(key, item);
        }

        public void Store(string key, object item, List<string> dependOnTcmUris)
        {
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                return;
            }
            _wrappedCacheAgent.Store(key, item, dependOnTcmUris);
        }

        public void Store(string key, string region, object item)
        {
            if (key.SkipKeyWhilePreviewing() && IsPreviewActive)
            {
                return;
            }
            _wrappedCacheAgent.Store(key, region, item);
        }

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