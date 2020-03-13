using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Core.Contracts.Resolvers;
using DD4T.Utils.Resolver;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Resolver
{
    /// <summary>
    /// Richtext resolver extended for ecl metadata fields
    /// </summary>
	[Obsolete("TODO: remove from modules")]
    public class RichTextFieldResolver : DefaultRichTextResolver
    {
        /// <summary>
        /// ctor RichTextFieldResolver
        /// </summary>
        /// <param name="linkResolver"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public RichTextFieldResolver(ILinkResolver linkResolver, ILogger logger, IDD4TConfiguration configuration) : base(linkResolver, logger, configuration)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pageUri"></param>
        /// <returns></returns>
        public override object Resolve(string input, string pageUri = null)
        {
            var output = (string)base.Resolve(input, pageUri);

            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);

            doc.PreserveWhitespace = true;
            doc.LoadXml(string.Format("<xhtmlroot>{0}</xhtmlroot>", output));
            // resolve links which haven't been resolved
            try
            {
                foreach (XmlNode link in doc.SelectNodes("//a", nsmgr))
                {
                    // TODO: find out if this is really generic, or something copied from an implementation somewhere? (QS)
                    if (link.Attributes["data-eclDisplayTypeId"] == null)
                        continue;

                    if (link.Attributes["data-CDNPaths"] == null)
                        continue;

                    var displayType = link.Attributes["data-eclDisplayTypeId"].Value;
                    var dic = this.GetCDNPaths(link.Attributes["data-CDNPaths"].Value);

                    if (displayType.Equals("document", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var url = string.Empty;
                        if (dic.TryGetValue("original", out url))
                            link.Attributes["href"].Value = url;
                    }
                    else
                    {
                        var url = string.Empty;
                        if (dic.TryGetValue("original", out url))
                            link.Attributes["src"].Value = url;
                    }
                }
            }
            catch (Exception)
            {
                return output;
            }

            return doc.DocumentElement.InnerXml;
        }

        private Dictionary<string, string> GetCDNPaths(string cdnPaths)
        {
            var paths = cdnPaths.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in paths.Where(s => !s.IsNullOrEmpty()))
            {
                var fileName = item.Split(':').First();

                if (!dictionary.ContainsKey(fileName))
                {
                    dictionary[fileName] = string.Join(":", item.Split(':').Skip(1));
                }
            }
            return dictionary;
        }
    }
}