using Trivident.Modules.Core.Models;

namespace Trivident.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface ILink. Used to identify that a model can be used as a link 
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.IEntityModel" />
    public interface ILink : IEntityModel
    {
        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        string Target { get; }
        /// <summary>
        /// Gets the tooltip.
        /// </summary>
        /// <value>The tooltip.</value>
        string Tooltip { get; }
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        new string Url { get; }
    }
}