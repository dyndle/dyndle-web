using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;

namespace Dyndle.Modules.Core.Models.Entities
{
    [ContentModelBySchemaTitle("Link Image Title", true)]
    public class LinkImageTitle : EntityModel, ILinkImageTitle
    {

        [EmbeddedSchemaField(FieldName = "link", EmbeddedModelType = typeof(LinkImage))]
        public ILinkImage LinkImage { get; set; }

        [EmbeddedSchemaField(FieldName = "Media", EmbeddedModelType = typeof(MediaAltText))]
        public IMediaAltText Media { get; set; }

        [TextField(FieldName = "link_text")]
        public string LinkText { get; set; }
    }
}
