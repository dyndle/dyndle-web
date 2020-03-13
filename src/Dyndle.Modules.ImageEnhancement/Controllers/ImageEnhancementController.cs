using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Exceptions;
using DD4T.ContentModel.Factories;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.ImageEnhancement.Models;
using Dyndle.Modules.ImageEnhancement.Services;

namespace Dyndle.Modules.ImageEnhancement.Controllers
{
    public class ImageEnhancementController : Controller, IImageEnhancementController
    {
        private IBinaryFactory _binaryFactory;
        private IImageEnhancementService _imageEnhancementService;
        private ILogger _logger;
        private IConfiguration _configuration;

        public ImageEnhancementController(IBinaryFactory binaryFactory, IImageEnhancementService imageEnhancementService, ILogger logger, IConfiguration configuration)
        {
            binaryFactory.ThrowIfNull(nameof(binaryFactory));
            imageEnhancementService.ThrowIfNull(nameof(imageEnhancementService));
            logger.ThrowIfNull(nameof(logger));
            configuration.ThrowIfNull(nameof(configuration));
            _binaryFactory = binaryFactory;
            _imageEnhancementService = imageEnhancementService;
            _logger = logger;
            _configuration = configuration;
        }

        public FileResult EnhanceImage(string url, IEnhancementSettings enhancementSettings)
        {
            var filename = GetEnhancedFilename(url, enhancementSettings);
            var physicalPath = HttpContext.Server.MapPath(Path.Combine(EnhancedImagesPath, filename));

            if (System.IO.File.Exists(physicalPath))
            {
                TimeSpan difference = DateTime.UtcNow - System.IO.File.GetLastWriteTimeUtc(physicalPath);
                if (difference.TotalSeconds > CacheSeconds)
                {
                    // enhanced file is too old, we will refresh
                    System.IO.File.Delete(physicalPath);
                }
                else
                {
                    return File(physicalPath, MimeMapping.GetMimeMapping(physicalPath));
                }
            }
            try
            {
                var bytes = _binaryFactory.FindBinaryContent("/" + url);

                var enhancedBytes = _imageEnhancementService.Enhance(bytes, enhancementSettings);

                if (WriteBinaryToFile(enhancedBytes, physicalPath))
                {
                    return File(physicalPath, MimeMapping.GetMimeMapping(physicalPath));
                }
                throw new HttpException(404, $"File /{url} cannot be found");
            }
            catch (BinaryNotFoundException)
            {
                CleanupImageEnhancements(url);
                throw new HttpException((int)HttpStatusCode.NotFound, $"No image exists at {url}");
            }

        }

        private void CleanupImageEnhancements(string url)
        {
            var baseFilename = Path.GetFileNameWithoutExtension(url);
            var cacheDir = HttpContext.Server.MapPath(EnhancedImagesPath);
            foreach (var file in Directory.EnumerateFiles(cacheDir).Where(f => f.StartsWith(baseFilename)))
            {
                System.IO.File.Delete(file);
            }
        }

        private static string GetEnhancedFilename(string url, IEnhancementSettings enhancementSettings)
        {
            var filename = Path.GetFileNameWithoutExtension(url);
            string extension = Path.GetExtension(url);
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}.{8}", filename, enhancementSettings.Width, enhancementSettings.Height, enhancementSettings.CropX, enhancementSettings.CropY, enhancementSettings.CropXP, enhancementSettings.CropYP, enhancementSettings.CropStyle.ToString(), extension);
        }

        private string EnhancedImagesPath => _configuration.LocalPath;
        private string BackgroundColor => _configuration.BackgroundColor;
        private int CacheSeconds => _configuration.CacheSeconds;

        private bool WriteBinaryToFile(byte[] bytes, String physicalPath)
        {
            bool result = true;
            FileStream fileStream = null;
            try
            {
                lock (physicalPath)
                {
                    if (System.IO.File.Exists(physicalPath))
                    {
                        fileStream = new FileStream(physicalPath, FileMode.Create);
                    }
                    else
                    {
                        FileInfo fileInfo = new FileInfo(physicalPath);
                        if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                        {
                            fileInfo.Directory.Create();
                        }
                        fileStream = System.IO.File.Create(physicalPath);
                    }

                    byte[] buffer = bytes;

                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Exception while writing enhanced image to disk: {0}\r\n{1}", e.Message, e.StackTrace);
                result = false;
            }
            finally
            {
                fileStream?.Close();
            }

            return result;
        }
    }
}
