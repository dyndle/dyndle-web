using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Mvc;
using Trivident.Modules.Core;
using Trivident.Modules.Core.Configuration;

namespace Trivident.Modules.Core.Cache
{
    public class DD4TOutputCacheProvider : OutputCacheProvider
    {       
        private ILogger Logger { get; set; }
        public int MaximumCacheTime { get; set; }
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
        public static string CACHE_KEY_FORMAT = "Output_{0}";
        public static string CACHE_REGION = "Output";


        public DD4TOutputCacheProvider()
        {
            MaximumCacheTime = 10; // DynConfig.ItemAsInt("Caching/Default");

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


        public override object Get(string key)
        {
            Initialize();
            Logger.Debug("returning item from cache with key " + key);
            var cacheKey = string.Format(CACHE_KEY_FORMAT, key);
            return _cacheAgent.Load(cacheKey);
        }


        public override void Remove(string key)
        {
            Initialize();
            var cacheKey = string.Format(CACHE_KEY_FORMAT, key);
            Logger.Debug("removing item from cache with key " + key);
            _cacheAgent.Remove(cacheKey);
        }

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
                        catch (SecurityException)
                        {
                            //Logger.Error("CustomCacheAgent SecurityException: ", ex);
                        }
                        catch (Exception)
                        {
                            //Logger.Error("CustomCacheAgent Exception: ", ex);
                        }
                    }
                    return _currentOutputCachingProfile;
                }
            }


        }
    }
}

