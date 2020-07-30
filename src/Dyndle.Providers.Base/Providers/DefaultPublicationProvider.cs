using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tridion.ContentDelivery.DynamicContent;
using Tridion.ContentDelivery.Meta;
using PublicationMeta = Dyndle.Modules.Core.Models.PublicationMeta;

namespace Dyndle.Providers
{
    public class DefaultPublicationProvider : IPublicationProvider
    {
        private static readonly PublicationMetaFactory _publicationMetaFactory = new PublicationMetaFactory();
        private readonly DynamicMappingsRetriever _mappingsRetriever = new DynamicMappingsRetriever();
        private static readonly IExtendedPublicationResolver _publicationResolver = DependencyResolver.Current.GetService<IExtendedPublicationResolver>();

        public IEnumerable<IPublicationMeta> GetAllPublicationMetadata(bool excludeCurrent)
        {
            var publicationMeta = _publicationMetaFactory.GetAllMeta();

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
                PublicationUrl = _mappingsRetriever.GetPublicationUrls(p.Id).FirstOrDefault(),
                Title = p.Title
            });

            return excludeCurrent ? meta.Where(p => p.Id != _publicationResolver.ResolvePublicationId()).ToList() : meta.ToList();
        }
    }
}