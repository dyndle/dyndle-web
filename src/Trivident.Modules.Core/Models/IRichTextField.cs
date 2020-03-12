using System.Web;
using System.Web.Mvc;

namespace Trivident.Modules.Core.Models
{
    /// <summary>
    /// Interface IRichText
    /// Used to provide access to the HTML content from Tridion
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.IEntityModel" />
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