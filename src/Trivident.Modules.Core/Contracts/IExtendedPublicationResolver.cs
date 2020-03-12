using DD4T.ContentModel.Contracts.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Contracts
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