using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DD4T.ContentModel;
using Dyndle.Modules.Core.Models;
using Newtonsoft.Json;
using Trivident.Modules.Search.Contracts;
using Trivident.Modules.Search.Models;

namespace Trivident.Modules.Search.Extensions
{
    /// <summary>
    /// Field List Extension
    /// </summary>
    public static class FieldListExtension
    {
        /// <summary>
        /// Field List
        /// </summary>
        /// <param name="url">The Url.</param>
        /// <param name="type">The Type.</param>
        /// <returns></returns>
        public static string AsFieldList(this string url, Type type)
        {
            return $"{url.UrlSeparator()}fl={GetFieldListItemsAsString(type)}";
        }

        /// <summary>
        /// Get Field List Items as String
        /// </summary>
        /// <param name="type">The Type.</param>
        /// <returns></returns>
        public static string GetFieldListItemsAsString(this Type type)
        {
            return string.Join(",", GetFieldListItems(type).Select(x => x));
        }

        /// <summary>
        /// Get Field List Items as String
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public static string GetFacetFields(this string fields)
        {
            return $"&facet=on&facet.field={string.Join("&facet.field=", fields.Split(',').Select(x => x))}";
        }

        /// <summary>
        /// Get Field List Items
        /// </summary>
        /// <param name="type">The Type.</param>
        /// <returns></returns>
        public static List<string> GetFieldListItems(this Type type)
        {
            var properties = type.GetProperties();

            return properties.Select(prop => prop.GetFieldListValue()).ToList();
        }

        /// <summary>
        /// Get Field List Value
        /// </summary>
        /// <param name="prop">The PropertyInfo.</param>
        /// <returns></returns>
        private static string GetFieldListValue(this PropertyInfo prop)
        {
            var jsonPropertyAttribute = prop.GetCustomAttributes(typeof(JsonPropertyAttribute), false);

            var value = prop.Name;

            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            JsonPropertyAttribute attr = jsonPropertyAttribute.FirstOrDefault() as JsonPropertyAttribute;

            return !string.IsNullOrWhiteSpace(attr?.PropertyName) ? attr.PropertyName : value;
        }

        public static List<SearchGroupByItems> GroupByField(this List<EntityModel> list, string field, int pageSize)
        {
            int searchCount = 0;
            int page = 1;

            List<SearchGroupByItems> items = list.Select(item =>
            {
                var itemProperty = item.GetType().GetProperty(field.ToTitleCase());
                var itemValue = itemProperty?.GetValue(item);
                string groupName = "General";
                if (itemProperty != null && (typeof(List<IKeyword>) == itemProperty.PropertyType && (itemValue as List<IKeyword>) != null))
                {
                    groupName = (itemValue as List<IKeyword>).FirstOrDefault()?.Title;
                }
                else if (itemProperty != null && (typeof(IKeyword) == itemProperty.PropertyType && (itemValue as IKeyword) != null))
                {
                    groupName = (itemValue as IKeyword)?.Title;
                }

                return new SearchGroupByItem()
                {
                    Name = groupName?.ToString() ?? "General",
                    Item = item
                };
            }).GroupBy(x => x.Name, x => x.Item, (key, g) => new SearchGroupByItems()
            {
                Name = key,
                Items = g.ToList()
            }).ToList();

            for (var index = 0; index < items.Count; index++)
            {
                var item = items[index];
                var difInItems = pageSize - searchCount;

                if (difInItems <= 0)
                {
                    page++;
                    searchCount = 0;
                }

                //update the page number
                item.Page = page;

                searchCount = searchCount + item.Items.Count;
                searchCount = searchCount > pageSize ? pageSize : searchCount;
            }

            return items;
        }

    }
}
