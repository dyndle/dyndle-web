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
    [PageVirtualPath("~/Areas/Management/Views/Shared/DisplayTemplates/object.cshtml")]
    public partial class _Areas_Management_Views_Shared_DisplayTemplates_object_cshtml_ : System.Web.Mvc.WebViewPage<object>
    {
        public _Areas_Management_Views_Shared_DisplayTemplates_object_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("    <tr>\r\n        <td>Value</td>\r\n        <td>");

            
            #line 5 "..\..\Areas\Management\Views\Shared\DisplayTemplates\object.cshtml"
       Write(Model.ToString());

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    </tr>\r\n");

        }
    }
}
#pragma warning restore 1591