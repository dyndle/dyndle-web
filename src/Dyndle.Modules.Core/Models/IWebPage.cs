using System.Web;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Interface used to define common page properties
    /// </summary>
    public interface IWebPage : IRenderable
    {
        IHtmlString XpmPageTag { get; set; }
    }
}