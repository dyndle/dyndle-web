using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Interfaces;

namespace Dyndle.Modules.Core.Extensions
{
    /// <summary>
    /// Extension methods.
    /// Extending <seealso cref="string"/>
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public static string GetConfigurationValue(this string key, string defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (!setting.IsNullOrEmpty())
            {
                return setting;
            }

            if (key.StartsWith("Dyndle."))
            {
                return GetConfigurationValue(key.Replace("Dyndle.", "Trivident."), defaultValue);
            }
            else
            {
                return defaultValue ?? string.Empty;
            }

        }

        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static string GetConfigurationValue(this string key)
        {
            return GetConfigurationValue(key, null);
        }

        /// <summary>
        /// Gets the configuration value as boolean.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetConfigurationValueAsBoolean(this string key)
        {
            var val = key.GetConfigurationValue();
            return (!string.IsNullOrEmpty(val)) && (val.Equals("true", StringComparison.InvariantCultureIgnoreCase) || val.Equals("yes", StringComparison.InvariantCultureIgnoreCase));
        }



        /// <summary>
        /// Determines whether the specified value is null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [the specified value] [is null or empty]; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Formats the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="values">The values.</param>
        /// <returns>System.String.</returns>
        public static string FormatString(this string input, params object[] values)
        {
            if (values.IsNull() || !values.Any())
                return input;

            if (!input.IsNullOrEmpty())
                return string.Format(input, values);

            return string.Empty;
        }


        /// <summary>
        /// Base64 encodes the string.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>System.String.</returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base64 decodes the string.
        /// </summary>
        /// <param name="base64EncodedData">The base64 encoded data.</param>
        /// <returns>System.String.</returns>
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Converts a request URL into a CMS URL (for example adding default page name, and file extension)
        /// </summary>
        /// <param name="url">The request URL</param>
        /// <param name="contextPath">The context path.</param>
        /// <param name="locateIndex">if set to <c>true</c> [locate index].</param>
        /// <param name="defaultFileName">Default name of the file.</param>
        /// <returns>System.String.</returns>
        public static string ParseUrl(this string url, string contextPath = null, bool locateIndex = false, string defaultFileName = "index.html")
        {
            if (!IncludeFileExtensions)
            {
                return ParseUrlExtensionless(url, contextPath, locateIndex, defaultFileName);
            }
            if (string.IsNullOrEmpty(url))
            {
                url = defaultFileName;
            }

            // TODO: investigate what it takes to make urls case insensitive
            // url = url.ToLower();
            url = url.TrimEnd('/');

            if (!Path.HasExtension(url))
            {
                url = $"{url}/{defaultFileName}";
            }
            if (!url.StartsWith("/") && !url.StartsWith("http"))
            {
                url = string.Format("/{0}", url);
            }
            if (!contextPath.IsNullOrEmpty() && !contextPath.Equals("/", StringComparison.InvariantCultureIgnoreCase) && !url.StartsWith("http"))
            {
                if (contextPath.EndsWith("/"))
                {
                    contextPath = contextPath.TrimEnd('/');
                }
                if (!url.StartsWith(contextPath))
                {
                    url = string.Format("{0}{1}", contextPath, url);
                }
            }
            return Uri.EscapeUriString(url);
        }

        private static string ParseUrlExtensionless(this string url, string contextPath, bool locateIndex, string defaultFileName)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = defaultFileName;
            }

            // TODO: investigate what it takes to make urls case insensitive
            // url = url.ToLower();
            url = url.TrimEnd('/');

            if (locateIndex)
            {
                url = "{0}/".FormatString(url);
            }
            if (!url.StartsWith("/") && !url.StartsWith("http"))
            {
                url = string.Format("/{0}", url);
            }
            if (!contextPath.IsNullOrEmpty() && !contextPath.Equals("/", StringComparison.InvariantCultureIgnoreCase) && !url.StartsWith("http"))
            {
                if (contextPath.EndsWith("/"))
                {
                    contextPath = contextPath.Substring(0, contextPath.Length - 1);
                }
                if (!url.StartsWith(contextPath))
                {
                    url = string.Format("{0}{1}", contextPath, url);
                }
            }
            if (url.EndsWith("/"))
            {
                url = url + defaultFileName;
            }
            if (!Path.HasExtension(url))
            {
                var extension = Path.GetExtension(defaultFileName);
                url = string.Format("{0}{1}", url, extension);
            }

            return Uri.EscapeUriString(url);
        }

        /// <summary>
        /// Converts a CMS URL into a Request URL (for example removing default page name, and file extension)
        /// </summary>
        /// <param name="url">The CMS URL</param>
        /// <param name="defaultFileName">Default name of the file.</param>
        /// <returns>A Request URL</returns>
        /// <summary>
        /// Cleans the URL.
        /// </summary>
        public static string CleanUrl(this string url, string defaultFileName)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            if (IncludeFileExtensions)
            {
                if (url.StartsWith("/")|| url.StartsWith("http"))
                {
                    return url;
                }
                return "/" + url;
            }
            
            // make sure its a CMS URL 
            url = url.ParseUrl(defaultFileName: defaultFileName);


            var name = Path.GetFileName(url);
            var nameNoExt = Path.GetFileNameWithoutExtension(name);

            url = url.Replace(name, name == defaultFileName ? string.Empty : nameNoExt);

            if (!url.EndsWith("/"))
                url = string.Concat(url, "/");

            return url;
        }

        /// <summary>
        /// Use this extention to bind directly to objects.
        /// Sample: "The current member is {UserName}".FormatWith(member);
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string FormatWith(this string format, object source)
        {
            return FormatWith(format, null, source, false);
        }

        /// <summary>
        /// Use this extention to bind directly to objects.
        /// Sample: "The current member is {UserName}".FormatWith(member);
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="source">The source.</param>
        /// <param name="ignoreMissingProperties">if set to <c>true</c> [ignore missing properties].</param>
        /// <returns>System.String.</returns>
        public static string FormatWith(this string format, object source, bool ignoreMissingProperties)
        {
            return FormatWith(format, null, source, ignoreMissingProperties);
        }

        /// <summary>
        /// Use this extention to bind directly to objects.
        /// Sample: "The current member is {UserName}".FormatWith(member);
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string FormatWith(this string format, IFormatProvider provider, object source)
        {
            return FormatWith(format, null, source, false);
        }

        /// <summary>
        /// Use this extention to bind directly to objects.
        /// Sample: "The current member is {UserName}".FormatWith(member);
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="source">The source.</param>
        /// <param name="ignoreMissingProperties">if set to <c>true</c> [ignore missing properties].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentNullException">format</exception>
        public static string FormatWith(this string format, IFormatProvider provider, object source, bool ignoreMissingProperties)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            if (!format.Contains('{'))
                return format;

            Regex r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            List<object> values = new List<object>();
            string rewrittenFormat = r.Replace(format, delegate (Match m)
            {
                Group startGroup = m.Groups["start"];
                Group propertyGroup = m.Groups["property"];
                Group formatGroup = m.Groups["format"];
                Group endGroup = m.Groups["end"];
                if (propertyGroup.Value == "0")
                {
                    values.Add(source);
                }
                else
                {
                    object value;
                    try
                    {
                        value = DataBinder.Eval(source, propertyGroup.Value);
                        values.Add(value);
                    }
                    catch (Exception)
                    {
                        if (!ignoreMissingProperties)
                            throw;

                        return m.Value;
                    }
                }
                return (new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value + new string('}', endGroup.Captures.Count)).Replace("{", "[[[").Replace("}", "]]]");
            });
            rewrittenFormat = rewrittenFormat.Replace("{", "{{").Replace("}", "}}");
            rewrittenFormat = rewrittenFormat.Replace("[[[", "{").Replace("]]]", "}");

            return string.Format(provider, rewrittenFormat, values.ToArray());
        }

        /// <summary>
        /// Method to get the keyword id
        /// </summary>
        /// <param name="categoryXmlName">Category Xml Name</param>
        /// <param name="keywordTitle">Keyword Title</param>
        /// <returns>Returns keyword ID</returns>
        public static int? GetKeywordId(this string categoryXmlName, string keywordTitle)
        {
            var taxonomyService = DependencyResolver.Current.GetService<ITaxonomyService>();
            
            var keywords = taxonomyService?.GetKeywords(categoryXmlName);
            var keywordId = keywords?.FirstOrDefault(k => k.Title.Equals(keywordTitle, StringComparison.OrdinalIgnoreCase))?.Id;

            if (!keywordId.IsNull()) return new TcmUri(keywordId).ItemId;

            var logger = DependencyResolver.Current.GetService<ILogger>();
            logger.Debug($"keyword {keywordTitle} in category {categoryXmlName} not found.");
            return null;

        }

        private static bool IncludeFileExtensions
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["DD4T.IncludeFileExtensions"];
                if (setting.IsNullOrEmpty())
                {
                    return false;
                }
                if (Boolean.TryParse(setting, out bool result))
                {
                    return result;
                }
                return false;
            }
        }
    }
}