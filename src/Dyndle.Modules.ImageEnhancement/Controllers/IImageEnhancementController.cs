using System.Web.Mvc;
using Dyndle.Modules.ImageEnhancement.Models;

namespace Dyndle.Modules.ImageEnhancement.Controllers
{
    public interface IImageEnhancementController
    {
        FileResult EnhanceImage(string url, IEnhancementSettings enhancementSettings);
    }
}
