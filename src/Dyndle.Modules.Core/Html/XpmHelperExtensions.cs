using System.Web.Mvc;

namespace Dyndle.Modules.Core.Html
{
    /// <summary>
    /// Helper extensions to work with Tridion Experience Manager (XPM)
    /// </summary>
    /// <remarks>The functionality contained in this class extends the functionality in DD4T.MVC</remarks>
    public static class XpmHelperExtensions
    {
        private static readonly string RemovePreviewCookieScript = "<script type=\"text/javascript\">window.onload = function () { document.cookie = \"preview-session-token= ; domain=\" + window.location.hostname + \";path=/; expires=\" + (new Date(Date.now() + 1000)).toUTCString();};</script>";

        /// <summary>
        /// Delete the preview-session-token cookie which is set by XPM when the user clicks on 'update preview'. This cookie is used by Dyndle to turn off
        /// caching for selected objects (like the page).
        /// If you don't clean up the cookie, the cache will stay disabled until the user restarts their browser. The clean-up is executed through JavaScript
        /// and takes place after the entire page (including images etc) has been loaded.
        /// </summary>
        /// <param name="helper">The HtmlHelper</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString CleanupPreviewCookie(this HtmlHelper helper)
        {
            return new MvcHtmlString(RemovePreviewCookieScript);
        }
    }
}
