using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Attributes.ViewModels;
using Trivident.Modules.Core.Contracts.Entities;
using Trivident.Modules.Core.Models;

namespace Trivident.Modules.Core.Models.Entities
{
    [ContentModelBySchemaTitle("Link Image", true)]
    public class LinkImage : EntityModel, ILinkImage
    {
        [EmbeddedSchemaField(EmbeddedModelType = typeof(Link))]
        public ILink Link { get; set; }

        [EmbeddedSchemaField(EmbeddedModelType = typeof(MediaAltText))]
        public IMediaAltText Media { get; set; }
    }
}