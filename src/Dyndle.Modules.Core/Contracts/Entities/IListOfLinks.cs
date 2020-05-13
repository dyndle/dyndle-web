using System.Collections.Generic;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface IListOfLinks
    /// </summary>
    /// <seealso cref="IEntityModel" />
    public interface IListOfLinks : IEntityModel
    {
        /// <summary>
        /// Gets the call to action.
        /// </summary>
        /// <value>The call to action.</value>
        ILinkTitle CallToAction { get; }
        /// <summary>
        /// Gets the heading.
        /// </summary>
        /// <value>The heading.</value>
        string Heading { get; }
        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <value>The links.</value>
        List<ILinkImageTitle> Links { get; }
        /// <summary>
        /// Gets the subheading.
        /// </summary>
        /// <value>The subheading.</value>
        IRichText Subheading { get; }
    }
}