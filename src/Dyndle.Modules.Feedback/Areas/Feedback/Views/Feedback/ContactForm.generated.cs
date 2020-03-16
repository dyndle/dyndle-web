﻿using Dyndle.Modules.Core.Html;

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
    using Dyndle.Modules.Feedback;
    
    #line 1 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
    using Dyndle.Modules.Feedback.Html;
    
    #line default
    #line hidden
    using Dyndle.Modules.Feedback.ViewModels;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Feedback/Views/Feedback/ContactForm.cshtml")]
    public partial class _Areas_Feedback_Views_Feedback_ContactForm_cshtml : System.Web.Mvc.WebViewPage<ContactFormViewModel>
    {
        public _Areas_Feedback_Views_Feedback_ContactForm_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"contact-form\"");

WriteLiteral(">\r\n");

            
            #line 5 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
    
            
            #line default
            #line hidden
            
            #line 5 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
     using (Html.BeginForm())
    {
        
            
            #line default
            #line hidden
            
            #line 7 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
   Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 7 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                                


            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"col-md-8 col-md-offset-2\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 13 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                   Write(Html.ValidationSummary());

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"col-xs-12 col-sm-6\"");

WriteLiteral(">\r\n\r\n");

WriteLiteral("                            ");

            
            #line 18 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                       Write(Html.TextBoxFor(m => m.Name, new {placeholder = Html.GetLabel(FeedbackConstants.Labels.Name)}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 669), Tuple.Create("\"", 699)
            
            #line 19 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
, Tuple.Create(Tuple.Create("", 675), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.Name)
            
            #line default
            #line hidden
, 675), false)
);

WriteLiteral(">");

            
            #line 19 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                                                             Write(Html.GetLabel(FeedbackConstants.Labels.Name));

            
            #line default
            #line hidden
WriteLiteral("*</label>\r\n                        </div>\r\n                        <div");

WriteLiteral(" class=\"col-xs-12 col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 22 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                       Write(Html.TextBoxFor(m => m.Email, new {placeholder = Html.GetLabel(FeedbackConstants.Labels.Email)}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 1008), Tuple.Create("\"", 1039)
            
            #line 23 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
, Tuple.Create(Tuple.Create("", 1014), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.Email)
            
            #line default
            #line hidden
, 1014), false)
);

WriteLiteral(">");

            
            #line 23 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                                                              Write(Html.GetLabel(FeedbackConstants.Labels.Email));

            
            #line default
            #line hidden
WriteLiteral("*</label>\r\n                        </div>\r\n                    </div>\r\n          " +
"          <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"col-xs-12 col-sm-12\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 28 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                       Write(Html.DropDownListFor(m => m.Subject, Html.CreateSelectList(Model.Subjects, Html.GetLabel(FeedbackConstants.Labels.Reason))));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 1444), Tuple.Create("\"", 1477)
            
            #line 29 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
, Tuple.Create(Tuple.Create("", 1450), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.Subject)
            
            #line default
            #line hidden
, 1450), false)
);

WriteLiteral(">");

            
            #line 29 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                                                                Write(Html.GetLabel(FeedbackConstants.Labels.Subject));

            
            #line default
            #line hidden
WriteLiteral("*</label>\r\n                        </div>\r\n                    </div>\r\n\r\n        " +
"            <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"col-xs-12\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 35 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                       Write(Html.TextAreaFor(m => m.Message, new { rows = "6", placeholder = @Html.GetLabel(FeedbackConstants.Labels.Message) }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                   " +
" <div");

WriteLiteral(" class=\"row checkbox-input\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"col-xs-12\"");

WriteLiteral(">\r\n                            <input");

WriteAttribute("id", Tuple.Create(" id=\"", 2032), Tuple.Create("\"", 2078)
            
            #line 40 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
, Tuple.Create(Tuple.Create("", 2037), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.AcceptTermsConditions)
            
            #line default
            #line hidden
, 2037), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 2079), Tuple.Create("\"", 2129)
            
            #line 40 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
        , Tuple.Create(Tuple.Create("", 2086), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.AcceptTermsConditions)
            
            #line default
            #line hidden
, 2086), false)
);

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" value=\"true\"");

WriteLiteral(" ");

            
            #line 40 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                                                                                                                                                              Write(Model.AcceptTermsConditions ? "checked" : "");

            
            #line default
            #line hidden
WriteLiteral(">\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 2244), Tuple.Create("\"", 2291)
            
            #line 41 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
, Tuple.Create(Tuple.Create("", 2250), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.AcceptTermsConditions)
            
            #line default
            #line hidden
, 2250), false)
);

WriteLiteral(">");

            
            #line 41 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                                                                              Write(Html.Raw(Html.GetLabel(FeedbackConstants.Labels.TermsConditions)));

            
            #line default
            #line hidden
WriteLiteral("*</label>\r\n                            <input");

WriteAttribute("name", Tuple.Create(" name=\"", 2404), Tuple.Create("\"", 2454)
            
            #line 42 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
, Tuple.Create(Tuple.Create("", 2411), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.AcceptTermsConditions)
            
            #line default
            #line hidden
, 2411), false)
);

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" value=\"false\"");

WriteLiteral(">\r\n\r\n                            ");

WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                   " +
" <div");

WriteLiteral(" class=\"contact-form__actions\"");

WriteLiteral(">\r\n                        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-pink-outline\"");

WriteLiteral(">");

            
            #line 50 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
                                                                      Write(Html.GetLabel(FeedbackConstants.Labels.Send));

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n                    </div>\r\n                </div>\r\n            </div>" +
"\r\n        </div>\r\n");

            
            #line 55 "..\..\Areas\Feedback\Views\Feedback\ContactForm.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>");

        }
    }
}
#pragma warning restore 1591
