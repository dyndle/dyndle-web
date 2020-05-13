using Dyndle.Modules.ComplexTables.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dyndle.Modules.ComplexTables.Html
{
    public static class TableHelper
    {
        public static string ToCss(this Styling styling)
        {
            return styling.ToCss(StylingConfiguration.Default);
        }
        public static string ToCss(this Styling styling, StylingConfiguration config)
        {
            IList<string> styleDefinitions = new List<string>();
            if (config.Fill && !string.IsNullOrEmpty(styling.Background))
            {
                styleDefinitions.Add("background-color: " + styling.Background);
            }
            if (config.FontColor && !string.IsNullOrEmpty(styling.Color))
            {
                styleDefinitions.Add("color: " + styling.Color);
            }
            if (config.FontFamily && !string.IsNullOrEmpty(styling.FontFamily))
            {
                styleDefinitions.Add("font-family: " + styling.FontFamily);
            }
            if (config.FontSize && styling.FontSize > 0)
            {
                styleDefinitions.Add("font-size: " + styling.FontSize.ToPoints());
            }
            if (config.TextAlign && styling.HAlign != HAlign.Left)
            {
                styleDefinitions.Add("text-align: " + styling.HAlign.ToString().ToLower());
            }
            if (config.Width && styling.Width > 0)
            {
                styleDefinitions.Add("width: " + styling.Width.ToEm());
                styleDefinitions.Add("max-width: " + styling.Width.ToEm());
            }
            if (config.FontWeight && styling.IsBold)
            {
                styleDefinitions.Add("font-weight: bold");
            }
            if (config.Borders && styling.BorderTop != null)
            {
                styleDefinitions.Add("border-top: " + styling.BorderTop.ToCss());
            }
            if (config.Borders && styling.BorderRight != null)
            {
                styleDefinitions.Add("border-right: " + styling.BorderRight.ToCss());
            }
            if (config.Borders && styling.BorderBottom != null)
            {
                styleDefinitions.Add("border-bottom: " + styling.BorderBottom.ToCss());
            }
            if (config.Borders && styling.BorderLeft != null)
            {
                styleDefinitions.Add("border-left: " + styling.BorderLeft.ToCss());
            }
            if (styleDefinitions.Count == 0)
            {
                return "";
            }

            return "style=\"" + styleDefinitions.Aggregate((a, b) => a + "; " + b) + "\"";
        }

        public static string GetTableID(this Table table)
        {
            return table.SheetName.ToLower().Replace(" ", "-");
        }

        private static string ToCss(this BorderStyle borderStyle)
        {
            var color = borderStyle.Color == null ? "black" : borderStyle.Color;
            return $"{borderStyle.Width.ToPixels()} {borderStyle.LineStyle} {color}";
        }

        private static string ToPixels(this double value)
        {
            return $"{Math.Round(value, 0)}px";
        }

        private static string ToPoints(this double value)
        {
            var numval = Convert.ToString(Math.Round(value, 1));
            return $"{numval.Replace(",",".")}pt";
        }

        private static string ToEm(this double value)
        {
            var numval = Convert.ToString(Math.Round(value, 2));
            return $"{numval.Replace(",", ".")}em";
        }

    }
}
