using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.DebugInfo;

namespace Dyndle.Modules.Management.Html
{
    /// <summary>
    /// Helper extensions to work with Tridion Experience Manager (XPM)
    /// </summary>
    /// <remarks>The functionality contained in this class extends the functionality in DD4T.MVC</remarks>
    public static class DebugHelperExtensions
    {
        /// <summary>
        /// Return HTML to show a debug button at the top right of the screen, to reload the page with debug=true
        /// </summary>
        /// <param name="helper">The HtmlHelper</param>
        /// <returns></returns>
        public static MvcHtmlString ShowDebugInfo(this HtmlHelper helper)
        {
            if (!DyndleConfig.IsStagingSite)
            {
                return MvcHtmlString.Empty;
            }

            var sb = new StringBuilder();
            int i = 1;
            foreach (var debugInfoProvider in DebugInfoProviderFactory.Providers)
            {
                sb.Append(debugInfoProvider.ShowButton(i++).ToString());
            }
            
            return new MvcHtmlString($"{sb.ToString()}<script type=\"text/javascript\" src=\"/admin/debug.js\"></script>");

        }

        private static string GetResourceAsString(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string result = string.Empty;
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
    }
}
