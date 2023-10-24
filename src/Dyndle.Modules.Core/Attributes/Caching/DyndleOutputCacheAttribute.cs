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
    public class DyndleOutputCacheAttribute : ActionFilterAttribute
    {

        public static string ENABLE_OUTPUT_CACHE = "enable-output-cache";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (DyndleConfig.DisableOutputCaching || DisableOutputCachingForRequest())
            {
                return;
            }
            filterContext.RequestContext.HttpContext.Items.Add(ENABLE_OUTPUT_CACHE, true);
        }


        private static Regex reDisableOutputCachingForUrls = null;
        private static bool DisableOutputCachingForRequest()
        {
            if (string.IsNullOrEmpty(DyndleConfig.DisableOutputCachingForUrls))
            {
                return false;
            }

            if (reDisableOutputCachingForUrls == null)
            {
                reDisableOutputCachingForUrls = new Regex(DyndleConfig.DisableOutputCachingForUrls);
            }
            return reDisableOutputCachingForUrls.IsMatch(HttpContext.Current.Request.Url.LocalPath);
         }

    }
}
