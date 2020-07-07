using System;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using Dyndle.Providers.Resolvers;
using Tridion.ContentDelivery.Meta;
using PublicationMeta = Dyndle.Modules.Core.Models.PublicationMeta;


namespace Dyndle.Providers
{
    public class DefaultPublicationProvider : IPublicationProvider
    {
        private static readonly PublicationMetaFactory PublicationMetaFactory = new PublicationMetaFactory();
        private static readonly IExtendedPublicationResolver _publicationResolver;


        public IEnumerable<IPublicationMeta> GetAllPublicationMetadata(bool excludeCurrent)
        {
            var publicationMeta = PublicationMetaFactory.GetAllMeta();

            if (publicationMeta.IsNull())
            {
                return Enumerable.Empty<IPublicationMeta>();
            }

            var meta = publicationMeta.Select(p => new PublicationMeta()
            {
                Id = p.Id,
                Key = p.Key,
                MultimediaPath = p.MultimediaPath,
                MultimediaUrl = p.MultimediaUrl,
                PublicationPath = p.PublicationPath,
                PublicationUrl = p.PublicationUrl,
                Title = p.Title
            } as IPublicationMeta);

            return excludeCurrent ? meta.Where(p => new Uri(p.PublicationUrl) != _publicationResolver.GetBaseUri()) : meta;
        }
    }
}
