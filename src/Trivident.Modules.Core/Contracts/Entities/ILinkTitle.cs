using Trivident.Modules.Core.Models;

namespace Trivident.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface ILinkTitle
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.IEntityModel" />
    public interface ILinkTitle : IEntityModel
    {
        /// <summary>
        /// Gets the link.
        /// </summary>
        /// <value>The link.</value>
        ILink Link { get; }
        /// <summary>
        /// Gets the link text.
        /// </summary>
        /// <value>The link text.</value>
        string LinkText { get; }
    }
}