using System;
using System.Web;
using DD4T.ViewModels.Base;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Models.System;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Class WebPage.
    /// Used as base class for all DD4T page models
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Base.ViewModelBase" />
    /// <seealso cref="IWebPage" />
    public abstract class WebPage : ViewModelBase, IWebPage
    {
        /// <summary>
        /// Gets or sets the XPM tag.
        /// </summary>
        /// <value>The XPM tag.</value>
        public IHtmlString XpmPageTag { get; set; }

        #region IRenderableViewModel

        /// <summary>
        /// Gets or sets the MVC data.
        /// </summary>
        /// <value>The MVC data.</value>
        [CustomRenderData]
        public IMvcData MvcData { get; set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetView()
        {
            return this.MvcData.View;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PageTcmUri]
        public TcmUri Id { get; set; }

        #endregion IRenderableViewModel

        #region Overrides

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string containing the type and identifier of the Entity.</returns>
        public override string ToString()
        {
            return (Id == null) ? GetType().Name : String.Format("{0}: {1}", GetType().Name, Id);
        }

        #endregion Overrides
    }
}