﻿using System.Web.WebPages;

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

namespace Dyndle.Modules.Management.Areas.Management.Views.Shared.DisplayTemplates
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [PageVirtualPath("~/Areas/Management/Views/Shared/DisplayTemplates/WebPage.cshtml")]
    public partial class _Areas_Management_Views_Shared_DisplayTemplates_WebPage_cshtml_ : System.Web.Mvc.WebViewPage<Dyndle.Modules.Core.Models.WebPage>
    {
        public _Areas_Management_Views_Shared_DisplayTemplates_WebPage_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<tr>\r\n    <td>ID</td>\r\n    <td>");

            
            #line 5 "..\..\Areas\Management\Views\Shared\DisplayTemplates\WebPage.cshtml"
   Write(Model.Id);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n</tr>\r\n<tr>\r\n    <td>View</td>\r\n    <td>");

            
            #line 9 "..\..\Areas\Management\Views\Shared\DisplayTemplates\WebPage.cshtml"
   Write(Model.GetView());

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n</tr>");

        }
    }
}
#pragma warning restore 1591
