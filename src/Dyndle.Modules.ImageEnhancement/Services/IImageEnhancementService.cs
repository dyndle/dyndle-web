using Dyndle.Modules.ImageEnhancement.Models;

namespace Dyndle.Modules.ImageEnhancement.Services
{
    public interface IImageEnhancementService
    {
        byte[] Enhance(byte[] imageIn, IEnhancementSettings enhancementSettings);

        byte[] Crop(byte[] imageIn, int centerX, int centerY, int width, int height);

        byte[] Resize(byte[] imageIn, int width = 0, int height = 0);
    }
}
