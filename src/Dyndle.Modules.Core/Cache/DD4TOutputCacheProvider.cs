using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Configuration;

namespace Dyndle.Modules.Core.Cache
{
    /// <summary>
    /// Overrides Microsoft's built-in OutputCacheProvider to decache the output as soon as the corresponding Tridion page is republished or unpublished
    /// </summary>
    public class DD4TOutputCacheProvider : OutputCacheProvider
    {       
        private ILogger Logger { get; set; }
        private int MaximumCacheTime { get; set; }
        private static bool MustCacheOutput 
        { 
            get 
            {
                if (HttpContext.Current.Response.Cache.GetNoStore())
                {
                    return false;
                }
                if (! string.IsNullOrEmpty(DyndleConfig.DisableOutputCachingForUrls))
                {
                    return ! reDisableOutputCachingForUrls.IsMatch(HttpContext.Current.Request.Path);
                }
                return true;
            }
        }


        private static Regex reDisableOutputCachingForUrls = new Regex(DyndleConfig.DisableOutputCachingForUrls, RegexOptions.Compiled);

        private ICacheAgent _cacheAgent;
        /// <summary>
        /// Format of the cache key as used by the DD4T cache agent
        /// </summary>
        public const string CACHE_KEY_FORMAT = "Output_{0}";
        /// <summary>
        /// Region used by the DD4T cache agent to store output in the cache. You can configure the duration
        /// using the appSetting DD4T.CacheSettings.Output. Since the output is automatically decached, you
        /// can use a high value (3600 seconds or more).
        /// </summary>
        public const string CACHE_REGION = "Output";

        /// <summary>
        /// Constructor for the DD4TOutputCacheProvider
        /// </summary>
        public DD4TOutputCacheProvider()
        {
            MaximumCacheTime = 10; 

            if (CacheSettingsManagerConfiguration.CurrentOutputCachingProfile != null)
            {
                MaximumCacheTime = CacheSettingsManagerConfiguration.CurrentOutputCachingProfile.Duration;
            }

            Logger = DependencyResolver.Current.GetService<ILogger>();
        }

        #region OutputCacheProvider
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        /// <param name="utcExpiry">Note: this parameter is IGNORED</param>
        /// <returns></returns>
        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            if (! MustCacheOutput)
            {
                return null;
            }

            Initialize();
            var cacheKey = string.Format(CACHE_KEY_FORMAT, key);
            var currentEntry = _cacheAgent.Load(cacheKey);
            if (currentEntry != null)
            {
                return currentEntry;
            }

            var dependsOnUris = (List<string>) HttpContext.Current.Items[CoreConstants.General.DependentOnUris];
            if (dependsOnUris == null)
            {
                _cacheAgent.Store(cacheKey, CACHE_REGION, entry);
            }
            else
            {
                _cacheAgent.Store(cacheKey, CACHE_REGION, entry, dependsOnUris);
            }
            return entry;
        }

        /// <summary>
        /// Get the cached output by key
        /// </summary>
        /// <param name="key">key of item to get</param>
        /// <returns>Returns the cached output or null if the item isn't cached</returns>
        public override object Get(string key)
        {
            Initialize();
            Logger.Debug("returning item from cache with key " + key);
            var cacheKey = string.Format(CACHE_KEY_FORMAT, key);
            return _cacheAgent.Load(cacheKey);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">key of item to remove</param>
        public override void Remove(string key)
        {
            Initialize();
            var cacheKey = string.Format(CACHE_KEY_FORMAT, key);
            Logger.Debug("removing item from cache with key " + key);
            _cacheAgent.Remove(cacheKey);
        }

        /// <summary>
        /// Add item to the cache
        /// </summary>
        /// <param name="key">>key of item to cache</param>
        /// <param name="entry">item to cache</param>
        /// <param name="utcExpiry">expiration date/time. Note that this value is passed in by ASP.NET but we will ignore it.
        /// Instead, we are using the value which is configured in the Web.config using the key DD4T.CacheSettings.Output (in seconds)</param>
        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            if (! MustCacheOutput)
            {
                return;
            }
            Initialize();
            var cacheKey = string.Format(CACHE_KEY_FORMAT, key);
            //Added to check since, throwing error for multiple request
            if (_cacheAgent.Load(cacheKey)!= null)
            {
                _cacheAgent.Remove(cacheKey);
            }
            var dependsOnUris = (List<string>)HttpContext.Current.Items[CoreConstants.General.DependentOnUris];
            if (dependsOnUris == null)
            {
                _cacheAgent.Store(cacheKey, CACHE_REGION, entry);
            }
            else
            {
                _cacheAgent.Store(cacheKey, CACHE_REGION, entry, dependsOnUris);
            }
        }
        #endregion

        private void Initialize()
        {
            if (_cacheAgent == null)
            {
                _cacheAgent = DependencyResolver.Current.GetService<ICacheAgent>();
            }
        }


        /// <summary>
        /// Class to encapsulate the configuration
        /// </summary>
        public static class CacheSettingsManagerConfiguration
        {
            private static OutputCacheProfile _currentOutputCachingProfile;
            public static OutputCacheProfile CurrentOutputCachingProfile
            {
                get
                {
                    if (_currentOutputCachingProfile == null)
                    {
                        try
                        {
                            OutputCacheSettingsSection outputCacheSettingsSection;
                            outputCacheSettingsSection = (OutputCacheSettingsSection)ConfigurationManager.GetSection("system.web/caching/outputCacheSettings");
                            if (outputCacheSettingsSection != null && outputCacheSettingsSection.OutputCacheProfiles.Count > 0)
                            {
                                _currentOutputCachingProfile = outputCacheSettingsSection.OutputCacheProfiles["default"];
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Caught exception while trying to read outputCache configuration from the Web.config", e);
                        }
                    }
                    return _currentOutputCachingProfile;
                }
            }

            private static ILogger _logger;
            private static ILogger Logger => _logger ?? (_logger = DependencyResolver.Current.GetService<ILogger>());
        }
    }
}

