using System.Web;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Interface used to define common page properties
    /// </summary>
    public interface IWebPage : IRenderable
    {
        /// <summary>
        /// Gets or sets the XPM page tag.
        /// </summary>
        /// <value>The XPM page tag.</value>
        IHtmlString XpmPageTag { get; set; }
    }
}