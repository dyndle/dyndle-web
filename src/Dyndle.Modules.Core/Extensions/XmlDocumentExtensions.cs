using System.Xml;
using System.Xml.Linq;

namespace Dyndle.Modules.Core.Extensions
{
    /// <summary>
    /// Extension methods.
    /// Extending <seealso cref="XmlDocument" />
    /// </summary>
    public static class XmlDocumentExtensions
    {
        /// <summary>
        /// convert XmlDocumX into Documentent
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <returns>XDocument.</returns>
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}