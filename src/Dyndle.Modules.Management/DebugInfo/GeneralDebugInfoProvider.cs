using System;
using System.Web.Mvc;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Providers;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Base;

namespace Dyndle.Modules.Management.DebugInfo
{
    public class GeneralDebugInfoProvider : BaseDebugInfoProvider
    {

        public override string Name => "general";
        public override string OverrideLocation => null;
        public override string IconBase64 => "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABkAAAAZCAYAAADE6YVjAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsIAAA7CARUoSoAAAAI/SURBVEhL3ZW7axRRFMZn9sUuZu1EzRqw0oiBgJ2FoFViZYpALAWJhaAWYin+EYIINnZir4WlECytlGCqiKQwbLW7s89Z1993c3fYe2dGFEyzHxzOme8875m7s+FkMgmOGwWrjxXz0yTznYRhaK002u32inS9Xv9iiAykaorwJQ+DweAu/skvEMfxHUunMFtLkiIMCaIo2qSomVoYDof3xuPxIX4D2fi3rVsDrPT7/U3ZuN16PmFIQNH3yDPZFNiCzwS+24rhZE+VIxvakdwXz3vZRy7KLhQKyYl8lEol4yN2WTmG9OF3lQij0WiNlbQ7nc7ZXq93AzvG50AccddZ7SJ2RzlZNXNvF3xI4gceF1nFE+zDcrl8k8lPKobVtODfVSqVBU76kpwD9DqaVLfmH68wl+gE/s8kX1AT+I/Id+MMgnP4rxWLxQa+PWKuIJEcf9WEl7nBhKdY0wL6MdM3rEuNP6FCCl49YsxLPyDnuWz4/Vqt9tY4LDKbgNfwSzLQl0k8bVjA3u9Ls7oXhgA0/skJv8pG/0C5vyE18WUK7JDdv0In4HTbEvtoYGOSzwS2I7lXWDeGCXfY+Ra6b+kU5LMxO8qxtIPcJtVq9Q0q4lt1np0/PGLTYNIHikF3bU4as8eaCr+NVW5MjD5jwwI+GY80NZxZl2zWlDRvNpsNOOWs+vWch6lQ8BbTf7P5Cbrd7lKr1VqWUNBcjFkw2C65G34952FGqgSbT8q/gOaXlOvVyr3C/xXz8/c7L02C4DcEVIM8fry+4gAAAABJRU5ErkJggg==";


        private IViewModel viewModel;
        private IPage page;

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// Starts the stopWatch
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;
            foreach (var parameter in filterContext.ActionParameters)
            {
                var v = parameter.Value;
                if (v != null && v is ViewModelBase)
                {
                    var vm = v as IViewModel;

                    if (vm != null && vm is IPage)
                    {
                        page = vm.ModelData as IPage;
                        if (page.LastPublishedDate == default(DateTime))
                        {
                            ((Page)page).LastPublishedDate = DependencyResolver.Current.GetService<IPageProvider>().GetLastPublishedDateByUri(page.Id);
                        }
                        viewModel = vm;
                    }
                }
            }
        }



        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// Stops the stopWatch and stores result
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
          
        }


        /// <summary>
        /// Called by the ASP.NET MVC framework before the action result executes.
        /// Renders fieldset opening to display debug info
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (page == null)
            {
                return;
            }
            filterContext.HttpContext.Response.Write("<div id=\"debuginfo-general\" style=\"background-color: lightgrey\">");
            filterContext.HttpContext.Response.Write("<style>td { vertical-align: top; }</style>");
            filterContext.HttpContext.Response.Write("<table style=\"width: 100%\">");
            filterContext.HttpContext.Response.Write($"<tr><th style=\"background-color:#aaa\">Page</th><th style=\"background-color:#aaa\">Component presentations</th></tr>");
            filterContext.HttpContext.Response.Write($"<tr><td><table>");
            filterContext.HttpContext.Response.Write($"<tr><th>Property</th><th>Value</th></tr>");
            filterContext.HttpContext.Response.Write($"<tr><td>Page ID</td><td>{page.Id}</td></tr>");
            filterContext.HttpContext.Response.Write($"<tr><td>Title</td><td>{page.Title}</td></tr>");
            filterContext.HttpContext.Response.Write($"<tr><td>Last published on</td><td>{page.LastPublishedDate}</td></tr>");
            filterContext.HttpContext.Response.Write($"<tr><td>Page Template ID</td><td>{page.PageTemplate.Id}</td></tr>");
            filterContext.HttpContext.Response.Write($"<tr><td>Page Template Title</td><td>{page.PageTemplate.Title}</td></tr>");
            filterContext.HttpContext.Response.Write($"<tr><td>Page uses ViewModel class</td><td>{viewModel.GetType()}</td></tr>");
            filterContext.HttpContext.Response.Write($"</table></td><td><table><tr><th>Component</th><th>Schema</th><th>Template</th></tr>");
            foreach (var cp in page.ComponentPresentations)
            {
                filterContext.HttpContext.Response.Write($"<tr><td>{cp.Component.Title}</td><td>{cp.Component.Schema.Title}</td><td>{cp.ComponentTemplate.Title}</td></tr>");
            }
            filterContext.HttpContext.Response.Write("</table></td></tr></table>");
            filterContext.HttpContext.Response.Write("</div>");
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// Renders fieldset closing to display debug info
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            
        }
    }
}