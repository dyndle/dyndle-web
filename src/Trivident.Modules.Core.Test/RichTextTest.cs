using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD4T.ContentModel.Factories;
using Ninject.Modules;
using DD4T.Factories;
using Ninject;
using System.Reflection;
using DD4T.ContentModel.Contracts.Providers;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Serializing;
using DD4T.Serialization;
using System.Collections.Generic;
using DD4T.Utils.Caching;
using System.Web.Mvc;
using System.Xml;

namespace Trivident.Modules.Core.Test
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
