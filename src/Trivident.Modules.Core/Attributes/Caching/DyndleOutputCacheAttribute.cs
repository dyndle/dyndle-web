using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trivident.Modules.Core.Configuration;

namespace Trivident.Modules.Core.Attributes.Caching
{
    public class DyndleOutputCacheAttribute : OutputCacheAttribute
    {
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
