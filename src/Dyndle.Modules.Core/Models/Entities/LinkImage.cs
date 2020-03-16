using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;

namespace Dyndle.Modules.Core.Models.Entities
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