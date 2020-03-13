using Trivident.Modules.Personalization.Contracts;
using Newtonsoft.Json;
using System.Web;
using Newtonsoft.Json.Serialization;
using System;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Environment;
using Dyndle.Modules.Core.Extensions;

namespace Trivident.Modules.Personalization.Providers
{
    /// <summary>
    /// Use cookie storage for storing personalization data between requests/sessions
    /// </summary>
    public class CookieDataStore : IPersonalizationDataStore
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
        private readonly IExtendedPublicationResolver _publicationResolver;
        private readonly ISiteContext _siteContext;
        private readonly ILogger _logger;

        public CookieDataStore(IExtendedPublicationResolver publicationResolver,
            ISiteContext siteContext,
            ILogger logger)
        {
            publicationResolver.ThrowIfNull(nameof(publicationResolver));
            siteContext.ThrowIfNull(nameof(siteContext));
            logger.ThrowIfNull(nameof(logger));

            _publicationResolver = publicationResolver;
            _siteContext = siteContext;
            _logger = logger;
        }

        /// <summary>
        /// Update the personalization data cookie
        /// </summary>
        /// <param name="httpContext">The current HTTP context</param>
        /// <param name="data">The data to overwrite the cookie with</param>
        public void Update(HttpContextBase httpContext, IPersonalizationData data)
        {
            var cookie = new HttpCookie(PersonalisationConstants.Constants.CookieName);
            cookie.Secure = false;
            cookie.Path = _publicationResolver.GetBaseUri().AbsolutePath;
            cookie.Shareable = false;
            cookie.HttpOnly = false;
            cookie.Value = JsonConvert.SerializeObject(data, _settings).Base64Encode();            
            int cookieExpirationMonths = int.Parse(_siteContext.GetApplicationSetting(PersonalisationConstants.Settings.CookieExpirationInMonths, true));
            cookie.Expires = DateTime.Now.AddMonths(cookieExpirationMonths);
            httpContext.Response.SetCookie(cookie);
        }

        /// <summary>
        /// Get the current personalization data from the cookie
        /// </summary>
        /// <param name="httpContext">The current Http context</param>
        /// <returns>The deserialized personalization data</returns>
        public IPersonalizationData Get(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies[PersonalisationConstants.Constants.CookieName];
            if (cookie != null)
            {
                try
                {
                    var data = JsonConvert.DeserializeObject(cookie.Value.Base64Decode(), _settings);
                    if (data is IPersonalizationData)
                    {
                        return (IPersonalizationData)data;
                    }
                    _logger.Information("Personalisation cookie data is not IPersonalizationData");
                    return null;
                }
                catch (Exception ex)
                {
                    //If the cookie data is corrupt, or somehow otherwise no longer useable, we just ignore it
                    _logger.Error($"Personalisation cookie data seems corrupt: {ex.Message}");
                    return null;
                }
            }
            _logger.Information("Personalisation cookie cannot be found");
            return null;
        }
    }
}
