using DD4T.Core.Contracts.ViewModels;
using Trivident.Modules.Core.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Trivident.Modules.Core.Models
{
    /// <summary>
    /// Interface used to define common page properties
    /// </summary>
    public interface IWebPage : IRenderable
    {
        IHtmlString XpmPageTag { get; set; }
    }
}