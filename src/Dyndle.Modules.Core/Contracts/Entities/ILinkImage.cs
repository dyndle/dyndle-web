﻿using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface ILinkImage. Used to identify models that can be used as an image link
    /// </summary>
    /// <seealso cref="IEntityModel" />
    public interface ILinkImage : IEntityModel
    {
        /// <summary>
        /// Gets the link.
        /// </summary>
        /// <value>The link.</value>
        ILink Link { get; }
        /// <summary>
        /// Gets the media.
        /// </summary>
        /// <value>The media.</value>
        IMediaAltText Media { get; }
    }
}