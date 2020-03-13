using System;
using System.Web.Mvc;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Html
{
    /// <summary>
    /// Class ViewContextHelpers.
    /// Provides easy access to route values and view data from inside a razor view
    /// </summary>
    public static class ViewContextHelpers
    {
        /// <summary>
        /// Gets the route value from the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewContext">The view context.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value when the value is not found.</param>
        /// <returns>T.</returns>
        public static T GetRouteValue<T>(this ViewContext viewContext, string name, T defaultValue = default(T))
        {
            var value = viewContext.RouteData.Values[name];
            if (value.IsNull())
                return defaultValue;

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Gets the view data value from the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewContext">The view context.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value when the value is not found.</param>
        /// <returns>T.</returns>
        public static T GetViewDataValue<T>(this ViewContext viewContext, string name, T defaultValue = default(T))
        {
            var value = viewContext.ViewData[name];
            if (value.IsNull())
                return defaultValue;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}