using System;
using System.Drawing;
using System.IO;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.ImageEnhancement.Models;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;

namespace Dyndle.Modules.ImageEnhancement.Services
{
    public class DefaultImageEnhancementService : IImageEnhancementService
    {

        private IConfiguration _configuration;

        public DefaultImageEnhancementService(IConfiguration configuration)
        {
            configuration.ThrowIfNull(nameof(configuration));
            _configuration = configuration;
        }

        public byte[] Crop(byte[] imageIn, int centerX, int centerY, int width, int height)
        {
            // TODO: implement using ImageProcessor API
            return imageIn;
        }

        public byte[] Enhance(byte[] imageIn, IEnhancementSettings enhancementSettings)
        {
            ISupportedImageFormat format = new JpegFormat { Quality = 70 };

            Size size = new Size(150, 0);
            using (MemoryStream inStream = new MemoryStream(imageIn))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream)
                                    .Enhance(enhancementSettings, imageFactory.Image.Size)
                                    .BackgroundColor(ColorTranslator.FromHtml(_configuration.BackgroundColor))
                                    .Save(outStream);
                    }
                    return outStream.ToArray();
                }
            }

            

        }

        public byte[] Resize(byte[] imageIn, int width = 0, int height = 0)
        {
            // TODO: implement using ImageProcessor API
            return imageIn;
        }
    }
    public static class ImageFactoryEnhancements
    {
        public static ImageFactory Enhance(this ImageFactory factory, IEnhancementSettings enhancementSettings, Size originalSize)
        {
            if (enhancementSettings.RequiresCropping)
            {
                // first, calculate the top/left/bottom/right coordinates from the percentual center point
                int centerX = enhancementSettings.RequiresPercentageCropping ? 
                    (originalSize.Width * enhancementSettings.CropXP.Value) / 100 :
                    enhancementSettings.CropX.Value;
                int centerY = enhancementSettings.RequiresPercentageCropping ?
                    (originalSize.Height * enhancementSettings.CropYP.Value) / 100:
                    enhancementSettings.CropY.Value;

                int cropWidth; int cropHeight;
                if (enhancementSettings.CropStyle == CropStyle.Greedy)
                {

                    // next, calculate the coordinates of the box that must be cropped
                    // we will take the largest possible box based on the desired aspect ratio, and then resize to the final size
                    float cropRatio = (float)enhancementSettings.Width / (float)enhancementSettings.Height;
                    float origRatio = (float)originalSize.Width / (float)originalSize.Height;
                    
                    if (cropRatio > origRatio)
                    {
                        cropWidth = originalSize.Width;
                        cropHeight = Convert.ToInt32(((float)1 / cropRatio) * (float)cropWidth);
                    }
                    else
                    {
                        cropHeight = originalSize.Height;
                        cropWidth = Convert.ToInt32(cropRatio * (float)cropHeight);
                    }
                }
                else
                {
                    cropWidth = enhancementSettings.Width.Value;
                    cropHeight = enhancementSettings.Height.Value;
                }
                int left = centerX - (cropWidth / 2);
                int right = cropWidth;
                int top = centerY - cropHeight / 2;
                int bottom = cropHeight;

                // next, make corrections in case the box falls outside the dimensions of the image
                left = left < 0 ? 0 : left;
                top = top < 0 ? 0 : top;
                right = left + right > originalSize.Width ? originalSize.Width - left : right;
                bottom = top + bottom > originalSize.Height? originalSize.Height - top : bottom;

                if (left < 0 || left + right > originalSize.Width || 
                    top < 0 || top + bottom > originalSize.Height)
                {
                    // TODO: log
                    // the specified dimensions are physically impossible
                    // we will leave the image untouched
                    return factory;
                }

                Size size = new Size(enhancementSettings.Width == null ? 0 : enhancementSettings.Width.Value, enhancementSettings.Height == null ? 0 : enhancementSettings.Height.Value);
                CropLayer cropLayer = new CropLayer(left, top, right, bottom, CropMode.Pixels);
                factory =  factory.Crop(cropLayer);

                if (enhancementSettings.CropStyle == CropStyle.NonGreedy)
                {
                    return factory;
                }

                // in greedy mode we have grabbed the biggest possible area to crop to
                // we still need to resize that to the desired size

            }

            if (enhancementSettings.RequiresResizing)
            {
                Size size = new Size(enhancementSettings.Width == null ? 0 : enhancementSettings.Width.Value, enhancementSettings.Height == null ? 0 : enhancementSettings.Height.Value);
                return factory.Resize(size);
            }
            return factory;
        }
    }
}
