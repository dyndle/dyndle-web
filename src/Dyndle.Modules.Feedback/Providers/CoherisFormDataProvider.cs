using System;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Feedback.Coheris;
using Dyndle.Modules.Feedback.Contracts;

namespace Dyndle.Modules.Feedback.Providers
{
    /// <summary>
    /// Methods to retrieve values from Coheris using Coheris SOAP client
    /// </summary>
    public class CoherisFormDataProvider : IFeedbackFormDataProvider
    {
        private const string CacheKeyFormat = "Feedback({0}-{1})";
        private const string CacheRegion = "Feedback";

        private readonly ICacheAgent _cacheAgent;
        private readonly ILogger _logger;

        /// <summary>
        /// Injected dependencies
        /// </summary>
        /// <param name="cacheAgent"></param>
        /// <param name="logger"></param>
        public CoherisFormDataProvider(ICacheAgent cacheAgent,
            ILogger logger)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            logger.ThrowIfNull(nameof(logger));

            _cacheAgent = cacheAgent;
            _logger = logger;
        }

        /// <summary>
        /// store data in cache using key, and using cache region as constant
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        private void StoreInCache(string key, object data)
        {
            _logger.Debug($"Storing Coheris data in cache for key: {key}");
            _cacheAgent.Store(key, CacheRegion, data);
        }

        /// <summary>
        /// Load item from cache using the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IList<KeyValuePair<string, string>> LoadFromCache(string key)
        {
            var data = _cacheAgent.Load(key) as IList<KeyValuePair<string, string>>;
            if (data != null)
            {
                _logger.Debug($"Coheris data retrieved from cache for key: {key}");
            }
            else
            {
                _logger.Debug($"No Coheris data found in cache for key: {key}");
            }
            return data;
        } 

        /// <summary>
        /// Get age ranges from service
        /// </summary>
        public IList<KeyValuePair<string, string>> AgeRanges
        {
            get
            {
                var key = CacheKeyFormat.FormatString("AgeRanges", "");
                var data = GetData(key, () =>
                {
                    using (var client = new ServiceSoapClient())
                    {
                        return client.GetTrancheDAge().Select(Convert).ToList();
                    }
                });
                return data;
            }
        }

        /// <summary>
        /// Get contries from service
        /// </summary>
        public IList<KeyValuePair<string, string>> Countries
        {
            get
            {
                var key = CacheKeyFormat.FormatString("Countries", "");
                var data = GetData(key, () =>
                {
                    using (var client = new ServiceSoapClient())
                    {
                        return client.GetPays().Select(Convert).ToList();
                    }
                });
                return data;
            }
        }

        /// <summary>
        /// Get reasons from service
        /// </summary>
        public IList<KeyValuePair<string, string>> Reasons
        {
            get
            {
                var key = CacheKeyFormat.FormatString("Reasons", "");
                var data = GetData(key, () =>
                {
                    using (var client = new ServiceSoapClient())
                    {
                        return client.GetMotifDeContact().Select(Convert).ToList();
                    }
                });
                return data;
            }
        }

        /// <summary>
        /// Get titles from service
        /// </summary>
        public IList<KeyValuePair<string, string>> Titles
        {
            get
            {
                var key = CacheKeyFormat.FormatString("Titles", "");
                var data = GetData(key, () =>
                {
                    using (var client = new ServiceSoapClient())
                    {
                        return client.GetCivilite().Select(Convert).ToList();
                    }
                });
                return data;
            }
        }

        /// <summary>
        /// Get products from service
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public IList<KeyValuePair<string, string>> Products(int siteId)
        {
            var key = CacheKeyFormat.FormatString("Products", siteId);
            var data = GetData(key, () =>
            {
                using (var client = new ServiceSoapClient())
                {
                    return client.GetProduitBySite(siteId).Select(Convert).ToList();
                }
            });
            return data;
        }

        /// <summary>
        /// First check the cache for the requested cache key and if present, return the data
        /// if not present, make some service call and if the return data is not null, place it in the cache
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="serviceCall"></param>
        /// <returns></returns>
        private IList<KeyValuePair<string, string>> GetData(string cacheKey, Func<IList<KeyValuePair<string, string>>> serviceCall)
        {
            var data = LoadFromCache(cacheKey);
            if (data != null)
            {
                return data;
            }
            // execute service call
            data = serviceCall();
            if (data != null)
            {
                StoreInCache(cacheKey, data);
            }
            return data;
        }

        private static KeyValuePair<string, string> Convert(CleValeur cv)
        {
            return new KeyValuePair<string, string>(cv.cle.ToString(), cv.valeur);
        }

        private static KeyValuePair<string, string> Convert(CleValeurStr cv)
        {
            return new KeyValuePair<string, string>(cv.cle, cv.valeur);
        }

    }
}