using System.Web.Mvc;

namespace Dyndle.Modules.Core.DebugInfo
{
    /// <summary>
    /// Interface IDebugInfoProvider
    /// </summary>
    public interface IDebugInfoProvider
    {
        /// <summary>
        /// Shows the button.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>MvcHtmlString.</returns>
        MvcHtmlString ShowButton(int position);
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets the icon base64.
        /// </summary>
        /// <value>The icon base64.</value>
        string IconBase64 { get; }
        /// <summary>
        /// Gets the override location.
        /// </summary>
        /// <value>The override location.</value>
        string OverrideLocation { get; }
        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        void OnActionExecuting(ActionExecutingContext filterContext);
        /// <summary>
        /// Called when [action executed].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        void OnActionExecuted(ActionExecutedContext filterContext);
        /// <summary>
        /// Called when [result executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        void OnResultExecuting(ResultExecutingContext filterContext);
        /// <summary>
        /// Called when [result executed].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        void OnResultExecuted(ResultExecutedContext filterContext);
    }
}