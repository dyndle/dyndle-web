﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Dyndle.Modules.Management.Models;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Management/Views/Cache/RemoveItems.cshtml")]
    public partial class _Areas_Management_Views_Cache_RemoveItems_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_Management_Views_Cache_RemoveItems_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Areas\Management\Views\Cache\RemoveItems.cshtml"
  
    var items = ViewData["items"] as List<string> ?? new List<string>();

            
            #line default
            #line hidden
WriteLiteral("\r\n<h1>Cache removal</h1>\r\nItems with the following keys were removed. Click <a");

WriteLiteral(" href=\"/admin/cache\"");

WriteLiteral(">here</a> to return to the cache listing. \r\n\r\n");

            
            #line 7 "..\..\Areas\Management\Views\Cache\RemoveItems.cshtml"
 if (items.Count > 0)
{

            
            #line default
            #line hidden
WriteLiteral("    <ul>\r\n");

            
            #line 10 "..\..\Areas\Management\Views\Cache\RemoveItems.cshtml"
        
            
            #line default
            #line hidden
            
            #line 10 "..\..\Areas\Management\Views\Cache\RemoveItems.cshtml"
         foreach (var item in items)
        {

            
            #line default
            #line hidden
WriteLiteral("            <li>item</li>\r\n");

            
            #line 13 "..\..\Areas\Management\Views\Cache\RemoveItems.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </ul>\r\n");

            
            #line 15 "..\..\Areas\Management\Views\Cache\RemoveItems.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591