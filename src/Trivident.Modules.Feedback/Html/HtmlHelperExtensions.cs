using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Trivident.Modules.Feedback.Html
{
    /// <summary>
    /// Some helper methods
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Create a select list from a IList<KeyValuePair<string, string>>, adding a default value as the first list element
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="list"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> CreateSelectList(this HtmlHelper helper, IList<KeyValuePair<string, string>> list, string defaultValue)
        {
            var items = list.Select(f => new SelectListItem
            {
                Value = f.Key,
                Text = f.Value
            }).ToList();
            items.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = defaultValue
            });
            return items;
        }
    }
}