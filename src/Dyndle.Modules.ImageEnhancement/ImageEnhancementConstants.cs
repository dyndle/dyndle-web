using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dyndle.Modules.ImageEnhancement
{
    /// <summary>
    /// Constants used in the Image Enhancement module
    /// </summary>
    public static class ImageEnhancementConstants
    {
        private static readonly IEnumerable<FieldInfo> Fields = typeof(ParameterNames).GetFields();

        public static string Parameters => string.Concat(string.Join("=,", Fields.Select(x => x.GetValue(null))), "=");

        /// <summary>
        /// Names of settings in the config file
        /// </summary>
        public static class Settings
        {
            /// <summary>
            /// Setting that contains the local path which holds the enhanced images
            /// </summary>
            public static readonly string LocalPath = "ImageEnhancement.LocalPath";

            /// <summary>
            /// Setting that contains the background color to be used when resizing images
            /// </summary>
            public static readonly string BackgroundColor = "ImageEnhancement.BackgroundColor";

            /// <summary>
            /// Setting that contains the maximum time to keep the cached version of the enhanced images
            /// </summary>
            public static readonly string CacheSeconds = "ImageEnhancement.CacheSeconds";

        }

        /// <summary>
        /// Possible entries in the RouteValues table (these are the supported parameters in the URL)
        /// </summary>
        public static class ParameterNames
        {
            /// <summary>
            /// The width
            /// </summary>
            public static readonly string Width = "width";

            /// <summary>
            /// The height
            /// </summary>
            public static readonly string Height = "height";

            /// <summary>
            /// The crop center X coordinate (in absolute pixels)
            /// </summary>
            public static readonly string CropX = "cropx";

            /// <summary>
            /// The crop center Y coordinate (in absolute pixels)
            /// </summary>
            public static readonly string CropY = "cropy";

            /// <summary>
            /// The crop center X coordinate (in percentage)
            /// </summary>
            public static readonly string CropXP = "cropxp";

            /// <summary>
            /// The crop center Y coordinate (in percentage)
            /// </summary>
            public static readonly string CropYP = "cropyp";

            /// <summary>
            /// The crop style (greedy or non-greedy)
            /// </summary>
            public static readonly string CropStyle = "cropstyle";

        }
    }
}
