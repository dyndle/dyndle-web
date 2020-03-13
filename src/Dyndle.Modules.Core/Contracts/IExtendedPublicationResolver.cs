using System;
using DD4T.ContentModel.Contracts.Resolvers;

namespace Dyndle.Modules.Core.Contracts
{
    /// <summary>
    /// Interface IExtendedPublicationResolver. 
    /// Identifies a <see cref="DD4T.ContentModel.Contracts.Resolvers.IPublicationResolver" /> that can also return the base uri of the publication
    /// </summary>
    /// <seealso cref="DD4T.ContentModel.Contracts.Resolvers.IPublicationResolver" />
    public interface IExtendedPublicationResolver : IPublicationResolver
    {
        /// <summary>
        /// Gets the base URI.
        /// </summary>
        /// <returns>Uri.</returns>
        Uri GetBaseUri();
    }
}