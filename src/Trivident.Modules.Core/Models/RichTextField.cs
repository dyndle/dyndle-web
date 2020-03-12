using DD4T.Mvc.ViewModels.Attributes;
using Trivident.Modules.Core.Attributes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Trivident.Modules.Core.Models
{
    /// <summary>
    /// Class RichText.
    /// Used to provide access to the HTML content from a Tridion Rich Text Field
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.EntityModel" />
    /// <seealso cref="Trivident.Modules.Core.Models.IRichText" />
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Content.ToString();
        }
    }
}