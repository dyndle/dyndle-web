using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;

namespace Dyndle.Modules.Core.Models.Entities
{
    [ContentModelBySchemaTitle("Media_Alt_Text", true)]
    public class MediaAltText : EntityModel, IMediaAltText
    {
        [TextField(FieldName = "Alt_Text")]
        public string AltText { get; set; }

        [LinkedComponentField(FieldName = "ImageVideo", LinkedComponentTypes = new[] { typeof(Image), typeof(Video), typeof(EclMedia) })]
        public IMedia Media { get; set; }
    }
}