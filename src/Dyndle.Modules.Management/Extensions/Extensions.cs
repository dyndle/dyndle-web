using System.Web;

namespace Dyndle.Modules.Management.Extensions
{
    public static class Extensions
    {
        public static string Summarize(this string value)
        {
            return value.Length > 50 ? value.Substring(0,50) + "..." : value;
        }

        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }
    }
}
