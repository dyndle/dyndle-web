using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Dyndle.Modules.Core.Configuration;

namespace Dyndle.Modules.Core.Attributes.Caching
{
    /// <summary>
    /// Enables DyndleOutputCache.
    /// Implements the <see cref="System.Web.Mvc.OutputCacheAttribute" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.OutputCacheAttribute" />
    public class DyndleOutputCacheAttribute : OutputCacheAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DyndleOutputCacheAttribute"/> class.
        /// </summary>
        public DyndleOutputCacheAttribute()
        {
            if (DyndleConfig.DisableOutputCaching || DisableOutputCachingForRequest())
            {
                this.VaryByParam = "*";
                this.Duration = 0;
                this.NoStore = true;
            }
        }

        private static Regex reDisableOutputCachingForUrls = null;
        private static bool DisableOutputCachingForRequest()
        {
            if (reDisableOutputCachingForUrls == null)
            {
                reDisableOutputCachingForUrls = new Regex(DyndleConfig.DisableOutputCachingForUrls);
            }
            return reDisableOutputCachingForUrls.IsMatch(HttpContext.Current.Request.Url.LocalPath);
         }
    }
}
