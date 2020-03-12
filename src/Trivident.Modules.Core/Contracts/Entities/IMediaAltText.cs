using Trivident.Modules.Core.Models;

namespace Trivident.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface IMediaAltText
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.IEntityModel" />
    public interface IMediaAltText : IEntityModel
    {
        /// <summary>
        /// Gets the alt text.
        /// </summary>
        /// <value>The alt text.</value>
        string AltText { get; }
        /// <summary>
        /// Gets the media.
        /// </summary>
        /// <value>The media.</value>
        IMedia Media { get; }
    }
}