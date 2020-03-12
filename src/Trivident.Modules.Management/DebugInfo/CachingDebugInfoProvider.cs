using DD4T.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trivident.Modules.Core.Configuration;
using Trivident.Modules.Core.DebugInfo;

namespace Trivident.Modules.Management.DebugInfo
{
    public class CachingDebugInfoProvider : BaseDebugInfoProvider
    {

        public override string Name => "caching";
        public override string OverrideLocation => "/admin/cache";
        public override string IconBase64 => "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAIAAABvFaqvAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAK9SURBVDhPY9TMOsNADYDPIC52pjhHcTZWRiD7y/e/i/a/+vP3P0QKE6AbxMbCyMIM0vnz939DJe5FheoQcSBwq7/y4v0voAIg+/ef/79RDWWB0jCwtkJTWZITyEifevv7738QQQgAGuBnJtQSowBkH7vxKWXybYg4BDBBaTBQk+KUE+VoXfWoacWjOy9+QEWRwIV7X4FSfRufmqvxCvGiOALFIDdDwQv3vyw9+Hrd8TdACRE+VqgEGIgLsP78/W/10Tdzd7149Pqnq4EgVAIMEAYxMjJ4Ggvuv/wRyHbSE9jdrNuXpASRggBgeO1q0jVS5gaGzYHLHz2NcBikIsmpKM5x4PIHIHvHuffz97yEiCOD7vVPTt/+AmQcuPLBSJkH2XcIg7yMBe+9+PHg1U8It2fDk5WHX0PYEDBly7MFe6Gmn7/39euPv+6GCEdBDQL6CxhAB66A/AUB//8zNK98tOnUWwh33p4X07Y/h7CBAJigDl39CAwKKB9ukCrYX/vB/oKDf/8ZapY83Hnu/fJDr3vXP4WKwgDQVkMlHniEMIuapgEpMzVeFibGBXtfAR2CDIBm7brw/iCSS+Hg1cffmrJcT978evwGFBqglB1gIawhDUqE5IEzd77sufiBwbDg3N9///5TAN5++g10DdOvP/+A2QpqOFng0/c/QJLp3z8GYFaECJEHXn74DSRBsYZpUMvKRz3rn0A5SKBy0YPpSIkAAl5+AGkHJU2IkXDw49e/DSffKktwwJMfHFx68PX07c8ZHpLAdAcHEO0gg9Bc9P7LH2AiuProGxBBhZAAHxfLj9//ONkQWQKiHRT94bai9RFyEFEIACZcYArCCpgYGSAlHxxkz7gDzOoggyGeRAZApcCSECtCMwUIXryHBfblh9+A2fXz979koCsPv95/BSoCqVaLIMKMIsDAAAAXtpnetAgQcAAAAABJRU5ErkJggg==";


      

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// Starts the stopWatch
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// Stops the stopWatch and stores result
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action result executes.
        /// Renders fieldset opening to display debug info
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// Renders fieldset closing to display debug info
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}