using System.Web;
using System.Web.Mvc;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Interface IRichText
    /// Used to provide access to the HTML content from Tridion
    /// </summary>
    /// <seealso cref="IEntityModel" />
    /// <seealso cref="IHtmlString" />
    public interface IRichText : IEntityModel, IHtmlString
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        MvcHtmlString Content { get; }
    }
}