using System.Xml.Linq;

namespace System.Xml
{
    /// <summary>
    /// Extension methods.
    /// Extending <seealso cref="XmlDocument"/>
    /// </summary>
    public static class XmlDocumentExtensions
    {
        /// <summary>
        /// convert XmlDocumX into Documentent
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
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