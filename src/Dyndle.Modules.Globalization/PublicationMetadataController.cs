using Dyndle.Modules.Core.Providers.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dyndle.Modules.Globalization
{
    /// <summary>
    /// Publication metadata controller
    /// </summary>
    public class PublicationMetadataController : Controller
    {
        private readonly IPublicationProvider _publicationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationMetadataController" /> class.
        /// </summary>
        /// <param name="publicationProvider">The publicationprovider.</param>
        public PublicationMetadataController(IPublicationProvider publicationProvider)
        {
            _publicationProvider = publicationProvider;
        }

        /// <summary>
        /// Sitemap action
        /// </summary>
        /// <returns>XmlResult.</returns>
        public virtual ActionResult PublicationMetadata()
        {
            string content = "";
            _publicationProvider.GetAllPublicationMetadata().Select(p => content+= p);
            return Content(content);
        }
    }
}
