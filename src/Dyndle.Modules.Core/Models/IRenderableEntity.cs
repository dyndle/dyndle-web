using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Models.System;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Interface IRenderable
    /// Provides data needed to use a model in MVC.
    /// </summary>
    /// <seealso cref="DD4T.Core.Contracts.ViewModels.IViewModel" />
    public interface IRenderable : IViewModel
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        TcmUri Id { get; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetView();

        /// <summary>
        /// Gets or sets the MVC data.
        /// </summary>
        /// <value>The MVC data.</value>
        IMvcData MvcData { get; set; }
    }
}