using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface ILink. Used to identify that a model can be used as a link 
    /// </summary>
    /// <seealso cref="IEntityModel" />
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