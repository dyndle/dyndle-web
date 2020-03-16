using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Results
{
    /// <summary>
    /// XmlResult implementation of FileResult
    /// </summary>
    /// <seealso cref="System.Web.Mvc.FileResult" />
    public class XmlResult : FileResult
    {
        private XmlDocument _document;

        /// <summary>
        /// calls base ctor and set content type
        /// </summary>
        /// <param name="document"></param>
        public XmlResult(XmlDocument document)
            : base("application/xml")
        {
            _document = document;
        }

        /// <summary>
        /// calls base ctor and set content type
        /// </summary>
        /// <param name="document"></param>
        public XmlResult(XDocument document)
            : base("application/xml")
        {
            _document = document.ToXmlDocument();
        }

        /// <summary>
        /// calls base ctor and set content type
        /// </summary>
        /// <param name="content"></param>
        public XmlResult(string content)
            : base("application/xml")
        {
            _document = new XmlDocument();
            _document.LoadXml(content);
        }

        /// <summary>
        /// write file
        /// </summary>
        /// <param name="response"></param>
        protected override void WriteFile(HttpResponseBase response)
        {
            using (XmlWriter writer = XmlWriter.Create(response.OutputStream))
            {
                _document.WriteTo(writer);
            }
        }
    }
}