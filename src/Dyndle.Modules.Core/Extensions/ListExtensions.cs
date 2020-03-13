using System;
using System.Collections.Generic;

namespace Dyndle.Modules.Core.Extensions
{
    /// <summary>
    /// Extension methods.
    /// Extending <seealso cref="List{T}"/>
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Split a list into specific size
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="sliceSize"></param>
        /// <returns></returns>
        public static List<List<T>> Split<T>(this List<T> items, int sliceSize)
        {
            List<List<T>> list = new List<List<T>>();
            for (int i = 0; i < items.Count; i += sliceSize)
                list.Add(items.GetRange(i, Math.Min(sliceSize, items.Count - i)));
            return list;
        }
    }
}