namespace System.Xml.Linq
{
    /// <summary>
    /// Extension methods.
    /// Extending <seealso cref="XDocument"/>
    /// </summary>
    public static class XDocumentExtensions
    {
        /// <summary>
        /// convert XDocument into XmlDocument
        /// </summary>
        /// <param name="xDocument"></param>
        /// <returns></returns>
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }
    }
}