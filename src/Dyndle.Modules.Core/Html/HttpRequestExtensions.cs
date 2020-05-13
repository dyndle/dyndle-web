using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Dyndle.Modules.Core.Html
{
    /// <summary>
    /// Class HttpRequestExtensions.
    /// Contains extensions for Url modification
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Appends the current querystring to URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>System.String. the Url with current querystring appended.</returns>
        public static string AppendCurrentQuerystringToUrl(this HttpRequestBase request, string absolutePath)
        {
            var nameValues = HttpUtility.ParseQueryString(request.QueryString.ToString());

            return request.FormatUrl(absolutePath, nameValues);
        }
        /// <summary>
        /// Gets the URL with the specified parameter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>System.String. the URL with the specified parameter.</returns>
        public static string GetURLWith(this HttpRequestBase request, string name, object value, string absolutePath = null)
        {
            var nameValues = HttpUtility.ParseQueryString(request.QueryString.ToString());

            if (!string.IsNullOrWhiteSpace(name) && value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                nameValues.Set(name.ToLower(), value.ToString().ToLower());

            return request.FormatUrl(absolutePath, nameValues);
        }

        /// <summary>
        /// Gets the URL with only the specified parameter. All other parameters will be removed
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>System.String. the URL with only the specified parameter. All other parameters will be removed</returns>
        public static string GetURLWithOnly(this HttpRequestBase request, string name, object value, string absolutePath = null)
        {
            var nameValues = HttpUtility.ParseQueryString(request.QueryString.ToString());
            nameValues.Clear();

            if (!string.IsNullOrWhiteSpace(name) && value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                nameValues.Set(name.ToLower(), value.ToString().ToLower());

            return request.FormatUrl(absolutePath, nameValues);
        }

        /// <summary>
        /// Gets the URL without the specified parameter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="name">The name.</param>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>System.String the URL without the specified parameter.</returns>
        public static string GetURLWithout(this HttpRequestBase request, string name, string absolutePath = null)
        {
            var nameValues = HttpUtility.ParseQueryString(request.QueryString.ToString());

            if (!string.IsNullOrWhiteSpace(name))
                nameValues.Remove(name.ToLower());

            return request.FormatUrl(absolutePath, nameValues);
        }

        /// <summary>
        /// Gets the URL without any parameters.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>System.String the URL without any parameters.</returns>
        public static string GetURLWithoutAny(this HttpRequestBase request, string absolutePath = null)
        {
            var nameValues = HttpUtility.ParseQueryString(request.QueryString.ToString());
            nameValues.Clear();
            return request.FormatUrl(absolutePath, nameValues);
        }

        /// <summary>
        /// Formats the URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="absolutePath">The absolute path.</param>
        /// <param name="nameValues">The name values.</param>
        /// <returns>System.String.</returns>
        private static string FormatUrl(this HttpRequestBase request, string absolutePath, NameValueCollection nameValues)
        {
            Clean(nameValues);
            return string.Format("{0}{1}{2}", absolutePath ?? request.Url.AbsolutePath, nameValues.Count > 0 ? "?" : "", nameValues);
        }

        /// <summary>
        /// Cleans the specified name values.
        /// </summary>
        /// <param name="nameValues">The name values.</param>
        private static void Clean(System.Collections.Specialized.NameValueCollection nameValues)
        {
            List<string> toDelete = new List<string>();

            foreach (string item in nameValues.Keys)
                if (string.IsNullOrWhiteSpace(nameValues[item]))
                    toDelete.Add(item);

            foreach (var item in toDelete)
                nameValues.Remove(item);

            Array.Sort(nameValues.AllKeys);
        }
    }
}
