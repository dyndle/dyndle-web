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
    using Trivident.Modules.Navigation.Models;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Navigation/Views/Navigation/MultiLevelTopNavigation.cshtml")]
    public partial class _Areas_Navigation_Views_Navigation_MultiLevelTopNavigation_cshtml : System.Web.Mvc.WebViewPage<ISitemapItem>
    {

#line 7 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
public System.Web.WebPages.HelperResult RenderLevel(ISitemapItem level)
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {

#line 8 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
 
    foreach (var item in level.Items)
    {
        if (item.Type.Equals("page", StringComparison.InvariantCultureIgnoreCase)) { continue; }

        var cssClass = (Request.Url.LocalPath.StartsWith(item.Url) ? "active" : null);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "        <li");

WriteAttributeTo(__razor_helper_writer, "class", Tuple.Create(" class=\"", 369), Tuple.Create("\"", 386)

#line 14 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
, Tuple.Create(Tuple.Create("", 377), Tuple.Create<System.Object, System.Int32>(cssClass

#line default
#line hidden
, 377), false)
);

WriteLiteralTo(__razor_helper_writer, ">\n            <a");

WriteAttributeTo(__razor_helper_writer, "href", Tuple.Create(" href=\"", 403), Tuple.Create("\"", 419)

#line 15 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
, Tuple.Create(Tuple.Create("", 410), Tuple.Create<System.Object, System.Int32>(item.Url

#line default
#line hidden
, 410), false)
);

WriteLiteralTo(__razor_helper_writer, ">");


#line 15 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
  WriteTo(__razor_helper_writer, item.Title);


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "</a>\n            <ul>\n");

WriteLiteralTo(__razor_helper_writer, "                ");


#line 17 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
WriteTo(__razor_helper_writer, RenderLevel(item));


#line default
#line hidden
WriteLiteralTo(__razor_helper_writer, "\n            </ul>\n        </li>\n");


#line 20 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
    }


#line default
#line hidden
});

#line 21 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
}
#line default
#line hidden

        public _Areas_Navigation_Views_Navigation_MultiLevelTopNavigation_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<ul");

WriteLiteral(" class=\"navigation multilevel\"");

WriteLiteral(">\n");

WriteLiteral("    ");

            
            #line 4 "..\..\Areas\Navigation\Views\Navigation\MultiLevelTopNavigation.cshtml"
Write(RenderLevel(Model));

            
            #line default
            #line hidden
WriteLiteral("\n</ul>\n\n");

        }
    }
}
#pragma warning restore 1591
