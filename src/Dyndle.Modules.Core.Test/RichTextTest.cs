using System.Web.Mvc;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dyndle.Modules.Test
{
    [TestClass]
    public class RichTextTest 
    {

        [TestMethod]
        public void TestEntities()
        {
            MvcHtmlString mvcHtmlString = new MvcHtmlString("<p>This &amp; that</p>");
            var s = mvcHtmlString.ToString();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            MvcHtmlString mhs2 = new MvcHtmlString(doc.InnerXml);
        }
    }
}
