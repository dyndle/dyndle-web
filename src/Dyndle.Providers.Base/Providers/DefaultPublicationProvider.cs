using System.Collections.Generic;
using System.Linq;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Providers.Content;
using Tridion.ContentDelivery.Meta;


namespace Dyndle.Providers
{
    public class DefaultPublicationProvider : IPublicationProvider
    {
        private static readonly PublicationMetaFactory PublicationMetaFactory = new PublicationMetaFactory();

        public IEnumerable<string> GetAllPublicationMetadata()
        {
            var publicationMeta = PublicationMetaFactory.GetAllMeta();

            if (publicationMeta.IsNull())
            {
                return Enumerable.Empty<string>();
            }

            return publicationMeta.Select(p => p.ToString());

        }
    }
}
