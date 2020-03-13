using System.Web.Mvc;
using DD4T.Mvc.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Class RichText.
    /// Used to provide access to the HTML content from a Tridion Rich Text Field
    /// </summary>
    /// <seealso cref="EntityModel" />
    /// <seealso cref="IRichText" />
    [ContentModelBySchemaTitle("RichTextField", false)]
    public class RichText : EntityModel, IRichText
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [RichTextField(FieldName = "RTF")]
        public MvcHtmlString Content { get; set; }

        /// <summary>
        /// Returns an HTML-encoded string.
        /// </summary>
        /// <returns>An HTML-encoded string.</returns>
        public string ToHtmlString()
        {
            return Content.ToHtmlString();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Content.ToString();
        }
    }
}