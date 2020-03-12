using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using System;
using System.Collections;

namespace Trivident.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// The TcmUri of the page (TcmUri)
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Attributes.PageAttributeBase" />
    public class PageTcmUriAttribute : PageAttributeBase
    {
        /// <summary>
        /// Parses the PageId as TcmUri into the decorated property
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>IEnumerable which contains the TcmUri</returns>
        public override IEnumerable GetPropertyValues(IPage page, Type propertyType, IViewModelFactory factory)
        {
            return page == null ? null : new TcmUri[] { new TcmUri(page.Id) };
        }

        /// <summary>
        /// Gets the expected type of the return.
        /// </summary>
        /// <value>The expected type of the return.</value>
        public override Type ExpectedReturnType
        {
            get { return typeof(TcmUri); }
        }
    }
}