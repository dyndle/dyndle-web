using System;
using System.Collections.Generic;
using System.Linq;

namespace Trivident.Modules.Search.Extensions
{
    public static class StringExtension
    {
        public static string UrlSeparator(this string url)
        {
            return url.Contains("?") ? $"{url}&" : $"{url}?";
        }

        /// <summary>
        /// Truncate the paragrah text after completing the entire word.
        /// </summary>
        /// <param name="input">The input text.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string TruncateAtWord(this string input, int length)
        {
            return TruncateAtWord(input, 0, length, "...");
        }

        /// <summary>
        /// Truncate the paragrah text after completing the entire word.
        /// </summary>
        /// <param name="input">The input text.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string TruncateAtWord(this string input, int startIndex, int length)
        {
            return TruncateAtWord(input, startIndex, length, "...");
        }

        /// <summary>
        /// Truncate the paragrah text after completing the entire word. 
        /// </summary>
        /// <param name="input">The input text.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="suffix">...</param>
        /// <returns></returns>
        public static string TruncateAtWord(this string input, int startIndex, int length, string suffix)
        {
            if (input == null || input.Length < length)
                return input;

            var iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);

            return $"{input.Substring(startIndex, (iNextSpace > 0) ? iNextSpace : length).Trim()}{suffix}";
        }

        /// <summary>
        /// Camel Case the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string text)
        {
            return new string(text.CharsToTitleCase().ToArray());
        }

        public static string ToTitleCase(this string text, bool titleCase)
        {
            return titleCase ? ToTitleCase(text) : text;
        }

        private static IEnumerable<char> CharsToTitleCase(this string s)
        {
            var newWord = true;

            foreach (var c in s)
            {
                if (newWord) { yield return Char.ToUpper(c); newWord = false; }
                else if (char.IsUpper(c)) yield return c;
                else yield return Char.ToLower(c);
                if (c == ' ') newWord = true;
            }
        }

        public static string GetValue(this List<string> list)
        {
            return list != null ? list.FirstOrDefault() : string.Empty;
        }

        public static Dictionary<string, string> ToDictionary(this string value)
        {
            var list = value.Split(',').ToList();
            return list.ToDictionary(x => x.Split(':')[0], x => x.Split(':')[1]);
        }

        public static string JoinFields(this Dictionary<string, string> fields, string seperator)
        {
            seperator = $" {seperator} ";
            return $"{string.Join(seperator, fields.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(f => $"{f.Key}:{f.Value}"))}";
        }

        public static string JoinFields(this Dictionary<string, string> fields)
        {
            return fields.JoinFields("OR");
        }

        public static Dictionary<string, int> ToFacetDictionary(this List<object> list)
        {
            var dict = new Dictionary<string, int>();

            for (int i = 0; i < list.Count; i+=2)
            {
                dict.Add(list[i].ToString(), int.Parse(list[i+1].ToString()));
            }

            return dict;
        }
    }
}
