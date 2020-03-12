using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivident.Modules.ImageEnhancement.Models;

namespace Trivident.Modules.ImageEnhancement.Services
{
    public interface IImageEnhancementService
    {
        byte[] Enhance(byte[] imageIn, IEnhancementSettings enhancementSettings);

        byte[] Crop(byte[] imageIn, int centerX, int centerY, int width, int height);

        byte[] Resize(byte[] imageIn, int width = 0, int height = 0);
    }
}
