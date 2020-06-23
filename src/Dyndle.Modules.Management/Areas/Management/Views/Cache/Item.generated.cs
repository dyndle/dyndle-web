﻿using System;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using Dyndle.Modules.Management.Models;

#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dyndle.Modules.Management.Areas.Management.Views.Cache
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [PageVirtualPath("~/Areas/Management/Views/Cache/Item.cshtml")]
    public partial class _Areas_Management_Views_Cache_Item_cshtml : System.Web.Mvc.WebViewPage<CacheItem>
    {
        public _Areas_Management_Views_Cache_Item_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<h1>Cached item</h1>\r\n<table");

WriteLiteral(" class=\"pure-table pure-table-horizontal\"");

WriteLiteral(">\r\n    <tr>\r\n        <th>Property</th>\r\n        <th>Value</th>\r\n    </tr>\r\n    <t" +
"r>\r\n        <td>Cache Key</td>\r\n        <td>");

            
            #line 11 "..\..\Areas\Management\Views\Cache\Item.cshtml"
       Write(Model.Key);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    </tr>\r\n    <tr>\r\n        <td>Type</td>\r\n        <td>");

            
            #line 15 "..\..\Areas\Management\Views\Cache\Item.cshtml"
       Write(Model.Value.GetType().FullName);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    </tr>\r\n");

            
            #line 17 "..\..\Areas\Management\Views\Cache\Item.cshtml"
    
            
            #line default
            #line hidden
            
            #line 17 "..\..\Areas\Management\Views\Cache\Item.cshtml"
     if (Model.Value.GetType().IsGenericType)
    {
        Type type = Model.Value.GetType().GetGenericArguments()[0];
        var templateName = "ListOf" + type.Name;
        
            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\Management\Views\Cache\Item.cshtml"
   Write(Html.DisplayFor(m => m.Value, templateName, new { isDependencyList = Model.Key.StartsWith("Dependencies:") }));

            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\Management\Views\Cache\Item.cshtml"
                                                                                                                      
    }
    else
    {
        
            
            #line default
            #line hidden
            
            #line 25 "..\..\Areas\Management\Views\Cache\Item.cshtml"
   Write(Html.DisplayFor(m => m.Value));

            
            #line default
            #line hidden
            
            #line 25 "..\..\Areas\Management\Views\Cache\Item.cshtml"
                                      
    }

            
            #line default
            #line hidden
WriteLiteral("</table>\r\n<br/><br/>\r\nClick <a");

WriteLiteral(" href=\"/admin/cache\"");

WriteLiteral(">here</a> to return to the cache list.");

        }
    }
}
#pragma warning restore 1591