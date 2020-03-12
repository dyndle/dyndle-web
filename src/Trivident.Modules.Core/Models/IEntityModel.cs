using DD4T.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Models
{
    /// <summary>
    /// holds common properties of a Entity (Tridion Component or ComponentPresentation)
    /// </summary>
    public interface IEntityModel : IRenderable
    {
        string Url { get; }
    }
}