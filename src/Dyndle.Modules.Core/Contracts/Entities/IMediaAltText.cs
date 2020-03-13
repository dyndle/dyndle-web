using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface IMediaAltText
    /// </summary>
    /// <seealso cref="IEntityModel" />
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