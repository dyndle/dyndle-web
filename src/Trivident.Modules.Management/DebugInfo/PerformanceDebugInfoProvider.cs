using DD4T.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trivident.Modules.Core.Configuration;

namespace Trivident.Modules.Core.DebugInfo
{
    public class PerformanceDebugInfoProvider : BaseDebugInfoProvider
    {
        public override string Name => "performance";
        public override string OverrideLocation => null;
        public override string IconBase64 => "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAIAAABvFaqvAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAQYSURBVDhPhZR7bFN1FMfb3t7e2/Z2fawd3XiMue4hokMXBCKQ6YyEkBiNZkgEwSjgAxM1gMkiMT4W52JigkMTiHEBB9nAhCgYjH+Q6cjMXCaysQftWLd2q6yPrc/76L23nt+9d+vajfhJ7i/n/O75fe+5v985P/WDb/Wp7oPZgJUXkyU2Is2L0xHOE2BoTlTeLWEZIUyj2vmYtWGbY0OZUQvOPCDXMxLr6A5dH4gqU4vIF1q/xtC0r7SixAC2KIqzCb6wQMcLYpwWrBQux/R54o1nvf4wJ7syOUI7a61Ne0tJHTaXTJ/vCv7UG/GH2MHW2pkoV9c4UL1Kv2eb49lNNgJHAa997R720cpKlSqb+ZYqU8v+taAyFWZ2fTLUejUwGWQzKhW4o360YMRPf3RhcnfLqHeGthjx796pcBWT8lpAychsxH458ZDVpCTf1DnZ3hWU7aWY9NgP71fC77unUy98PsKL8Ln5jA7vcILKWIA+3jbO8eLDpWiP8tBq1MeeX3nmbRfDiW98Mwa7BlovbbfLb5FQgR7bvdUBxpeX/Vf+itR/OHCifVJ6m8Vi1J4+4nr1aef6UgOBawKz3OlfAzD/ypNFajUKQEJbqk16AvMFma7BGLjhOJ8WULYyUABQBxc/qN5cVQAVcKzNm2AEmO/sDjGcsMpOrluN0kdCG8ooGHtG4zDmcaC+qKel5vzR6pWFBLgnr0x3D6GPAfBrf9yOQYlA3YKLOTYeanjCXl6s12nVteVUJM5D2nIonPfJQy4druzjtf5I8yW/bMv8fjt6rX/u5ngSbBRE6tAIO7drY+Fzm20oRMJVrFcsiUs3Qoo1T4oVR6doHEObpIUnluJh/Lk3fPnP8JAvhUIkoCGiybTZiGri3iz79130ZZn6RyyPV1JQRxUlegOBvdg8jITcAQZGA6HJ2ybY9X1f3dmz3ZHmM2evzyzu2C8OrIX1iiOBfqr3Dlq/qcoEWtJkFuj4Tzt8zT/6ofuVKYmPL2TrA85uIsiilbe8yakwS5HavXVFcNgNW+3yicpQJFZi0ymOBMy8XIfqTsYXYjMZ6dSgZhK0UF9jgXp59AFq/1MrVljwq32zELTGTnQcr3r9Gac7QI/fY2EG+uPMEVdNGQVXQnvXDJxsrzvx2805pdegOqEJoeTATjD84VMe2FpQaXu3wmlFFSQI4tHvvTeGYwsqjecmoA2KzHiSFZKMmL1GbJT23HuVdrPuzW89/WOJBZV4iveF2XWrjWxamA5zZU79goq8UCbnPiJwNYlroilhscrBU+67/7JyIhCzrAqQc0xsOrNU5ZY3Bc11sNXzz3gCLoZlVYCcjIClKsoL6Roxkihfxc8lv3Dg7JdVAeACu58KgI5fMSXgSqdIzWedvsGJHJX/QaX6D8uE7y2huoFZAAAAAElFTkSuQmCC";


        /// <summary>
        /// The action execution time
        /// </summary>
        private long actionExecutionTime;
        /// <summary>
        /// The result execution time
        /// </summary>
        private long resultExecutionTime;

        /// <summary>
        /// The stop watch
        /// </summary>
        private Stopwatch stopWatch;

        private object model;

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// Starts the stopWatch
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            foreach (var parameter in filterContext.ActionParameters)
            {
                var v = parameter.Value;
                if (v != null && typeof(ViewModelBase).IsAssignableFrom(v.GetType()))
                {
                    model = v;
                }
            }

            stopWatch = new Stopwatch();
            stopWatch.Start();
        }



        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// Stops the stopWatch and stores result
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            stopWatch.Stop();
            actionExecutionTime = stopWatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Gets the description based on routevalues
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="result">The result.</param>
        /// <returns>System.String.</returns>
        private string GetDescription(ControllerContext filterContext, ActionResult result)
        {
            var area = filterContext.RouteData.DataTokens.Where(v => v.Key == "area").Select(v => v.Value).SingleOrDefault() ?? "Core";
            var controller = filterContext.RouteData.Values.Where(v => v.Key == "controller").Select(v => v.Value).SingleOrDefault();
            var action = filterContext.RouteData.Values.Where(v => v.Key == "action").Select(v => v.Value).SingleOrDefault();
            var modelName = model?.GetType().Name;

            string viewName = string.Empty;

            if (result != null)
            {
                var viewResult = result as ViewResultBase;
                if (viewResult != null)
                {
                    viewName = viewResult.ViewName;
                }
            }

            return string.Join(":", area, controller, action, modelName, viewName);
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action result executes.
        /// Renders fieldset opening to display debug info
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            filterContext.HttpContext.Response.Write("<fieldset style=\"border:1px solid #ccc;padding:5px;margin:5px;\">");
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// Renders fieldset closing to display debug info
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            stopWatch.Stop();
            resultExecutionTime = stopWatch.ElapsedMilliseconds;
            filterContext.HttpContext.Response.Write(string.Format("<legend title=\"area:controller:action:model:view, action execution time + view render time = total execution time\"><small>{0}({1} + {2} = {3})</small></legend></fieldset>", GetDescription(filterContext, filterContext.Result), actionExecutionTime, resultExecutionTime, actionExecutionTime + resultExecutionTime));
        }
    }
}