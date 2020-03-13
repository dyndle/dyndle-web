using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Dyndle.Modules.Core.DebugInfo;

namespace Dyndle.Modules.Core.Attributes.Filter
{
    /// <summary>
    /// Class DebugInfoAttribute.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public class DebugInfoAttribute : ActionFilterAttribute
    {
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

        private List<IDebugInfoProvider> debugInfoProviders;

        public DebugInfoAttribute(IEnumerable<string> debugInfoNames) : base()
        {
                debugInfoProviders = DebugInfoProviderFactory.Providers.Where(p => debugInfoNames.Any(n => n == p.Name)).ToList();
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// Starts the stopWatch
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            debugInfoProviders.ForEach(p => p.OnActionExecuting(filterContext));

            //foreach (var parameter in filterContext.ActionParameters)
            //{
            //    var v = parameter.Value;
            //    if (v != null && typeof(ViewModelBase).IsAssignableFrom(v.GetType()))
            //    {
            //        model = v;
            //    }
            //}

            //stopWatch = new Stopwatch();
            //stopWatch.Start();
        }



        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// Stops the stopWatch and stores result
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            debugInfoProviders.ForEach(p => p.OnActionExecuted(filterContext));
            //stopWatch.Stop();
            //actionExecutionTime = stopWatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action result executes.
        /// Renders fieldset opening to display debug info
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            debugInfoProviders.ForEach(p => p.OnResultExecuting(filterContext));
            //stopWatch = new Stopwatch();
            //stopWatch.Start();
            //filterContext.HttpContext.Response.Write("<fieldset style=\"border:1px solid #ccc;padding:5px;margin:5px;\">");
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// Renders fieldset closing to display debug info
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            debugInfoProviders.ForEach(p => p.OnResultExecuted(filterContext));
            //stopWatch.Stop();
            //resultExecutionTime = stopWatch.ElapsedMilliseconds;
            //filterContext.HttpContext.Response.Write(string.Format("<legend title=\"area:controller:action:model:view, action execution time + view render time = total execution time\"><small>{0}({1} + {2} = {3})</small></legend></fieldset>", GetDescription(filterContext, filterContext.Result), actionExecutionTime, resultExecutionTime, actionExecutionTime + resultExecutionTime));
        }
    }
}
