using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Dyndle.Modules.Core.DebugInfo;

namespace Dyndle.Modules.Management.DebugInfo
{
    public abstract class BaseDebugInfoProvider : IDebugInfoProvider
    {
        private static readonly string DEBUG_INFO_RESOURCE_FILE_NAME_HTML = "debuginfo.html";

        public abstract string Name { get; }
        public abstract string OverrideLocation { get; }
        public abstract string IconBase64 { get; }

        public abstract void OnActionExecuted(ActionExecutedContext filterContext);
        public abstract void OnActionExecuting(ActionExecutingContext filterContext);
        public abstract void OnResultExecuted(ResultExecutedContext filterContext);
        public abstract void OnResultExecuting(ResultExecutingContext filterContext);
        public MvcHtmlString ShowButton(int position)
        {
            // disable output cache for the current request (otherwise the toggling of debug modes breaks)
            HttpContext.Current.Response.Cache.SetNoStore();


            // temporary hack to speed up development
            string html;
            if (File.Exists(HttpContext.Current.Server.MapPath("/" + DEBUG_INFO_RESOURCE_FILE_NAME_HTML)))
            {
                html = File.ReadAllText(HttpContext.Current.Server.MapPath("/" + DEBUG_INFO_RESOURCE_FILE_NAME_HTML));
            }
            else
            {
                html = EmbeddedResourceHelper.GetResourceAsString(DEBUG_INFO_RESOURCE_FILE_NAME_HTML);
            }
           

            // replace placeholders
            html = html.Replace("[right-offset]", Convert.ToString(16 + (position * 34)));
            html = html.Replace("[name]", Name);
            html = html.Replace("[location]", OverrideLocation ?? string.Empty);
            html = html.Replace("[icon-base64]", IconBase64);

            // return the html and the js as one MvcHtmlString
            return new MvcHtmlString(html);
        }
     

     
    }
}