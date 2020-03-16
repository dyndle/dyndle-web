using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface ILinkImageTitle
    /// </summary>
    /// <seealso cref="IEntityModel" />
    public interface ILinkImageTitle : IEntityModel
    {
        /// <summary>
        /// Gets the link image.
        /// </summary>
        /// <value>The link image.</value>
        ILinkImage LinkImage { get; }
        /// <summary>
        /// Gets the link text.
        /// </summary>
        /// <value>The link text.</value>
        string LinkText { get; }

        /// <summary>
        /// 
        /// </summary>
        IMediaAltText Media { get; set; }
    }
}