using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface IParagraph
    /// </summary>
    /// <seealso cref="IEntityModel" />
    public interface IParagraph : IEntityModel
    {
        /// <summary>
        /// Gets the heading.
        /// </summary>
        /// <value>The heading.</value>
        string Heading { get; }
        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>The image.</value>
        IMediaAltText Image { get; }
        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The text.</value>
        IRichText Text { get; }
        /// <summary>
        /// Gets the video.
        /// </summary>
        /// <value>The video.</value>
        IMediaAltText Video { get; }
        /// <summary>
        /// Gets the quotation.
        /// </summary>
        /// <value>The quotation.</value>
        IRichText Quotation { get; }
    }
}