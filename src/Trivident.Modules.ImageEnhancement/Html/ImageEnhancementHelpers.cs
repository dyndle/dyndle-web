using Trivident.Modules.ImageEnhancement.Models;

namespace Trivident.Modules.ImageEnhancement.Html
{
    public static class ImageEnhancementHelpers
    {
       
        public static string SetWidth(this string url, int width)
        {
            return url.AddParameter(width, ImageEnhancementConstants.ParameterNames.Width);
        }

        public static string SetHeight(this string url, int height)
        {
            return url.AddParameter(height, ImageEnhancementConstants.ParameterNames.Height);
        }

        public static string SetWidthAndHeight(this string url, int width, int height)
        {
            return url.SetWidth(width).SetHeight(height);
        }

        public static string SetCropCenter(this string url, int x, int y)
        {
            return url.AddParameter(x, ImageEnhancementConstants.ParameterNames.CropX)
                .AddParameter(y, ImageEnhancementConstants.ParameterNames.CropY);
        }

        public static string SetCropCenterPercentage(this string url, int x, int y)
        {
            return url.AddParameter(x, ImageEnhancementConstants.ParameterNames.CropXP)
                .AddParameter(y, ImageEnhancementConstants.ParameterNames.CropYP);
        }

        public static string SetCropStyle(this string url, CropStyle cropStyle)
        {
            return url.AddParameter(cropStyle.ToString(), ImageEnhancementConstants.ParameterNames.CropStyle);
        }

        private static string AddParameter(this string url, int val, string paramName)
        {
            return url + (url.Contains("?") ? "&" : "?") + paramName + "=" + val;
        }
        private static string AddParameter(this string url, string val, string paramName)
        {
            return url + (url.Contains("?") ? "&" : "?") + paramName + "=" + val;
        }
    }
}
