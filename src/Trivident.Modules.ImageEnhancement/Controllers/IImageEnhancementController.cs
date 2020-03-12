using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Trivident.Modules.ImageEnhancement.Models;

namespace Trivident.Modules.ImageEnhancement.Controllers
{
    public interface IImageEnhancementController
    {
        FileResult EnhanceImage(string url, IEnhancementSettings enhancementSettings);
    }
}
