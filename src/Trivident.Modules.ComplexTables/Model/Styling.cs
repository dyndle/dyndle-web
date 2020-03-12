using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.ComplexTables.Model
{
    public enum HAlign { Left, Center, Right }
    public enum VAlign { Top, Middle, Bottom }

    public class Styling 
    {

        /// <summary>
        /// Background color
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// True if the text is bold
        /// </summary>
        public bool IsBold { get; set; }

        /// <summary>
        /// Number of pixels for the top border
        /// </summary>
        public BorderStyle BorderTop { get; set; }

        /// <summary>
        /// Number of pixels for the left border
        /// </summary>
        public BorderStyle BorderLeft { get; set; }

        /// <summary>
        /// Number of pixels for the right border
        /// </summary>
        public BorderStyle BorderRight { get; set; }

        /// <summary>
        /// Number of pixels for the bottom border
        /// </summary>
        public BorderStyle BorderBottom { get; set; }

        /// <summary>
        /// Width in pixels
        /// </summary>
        public double Width { get; set; }

       
        /// <summary>
        /// Text color
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Font family
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// Font size
        /// </summary>
        public double FontSize { get; set; }
        
        /// <summary>
        /// Number of columns to take up
        /// </summary>
        public int Colspan { get; set; }

        /// <summary>
        /// Number of rows to take up
        /// </summary>
        public int Rowspan { get; set; }

        /// <summary>
        /// Horizontal alignment
        /// </summary>
        public HAlign HAlign { get; set; }

        /// <summary>
        /// Vertical alignment
        /// </summary>
        public VAlign VAlign { get; set; }

    }
}
