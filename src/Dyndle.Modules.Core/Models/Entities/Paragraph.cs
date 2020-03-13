using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;

namespace Dyndle.Modules.Core.Models.Entities
{
    [ContentModelBySchemaTitle("Paragraph", true)]
    public class Paragraph : EntityModel, IParagraph
    {
        [TextField]
        public string Heading { get; set; }

        [EmbeddedSchemaField(FieldName = "paragraph_text", EmbeddedModelType = typeof(RichText))]
        public IRichText Text { get; set; }

        [EmbeddedSchemaField(FieldName = "paragraph_image", EmbeddedModelType = typeof(MediaAltText))]
        public IMediaAltText Image { get; set; }

        [EmbeddedSchemaField(FieldName = "paragraph_video", EmbeddedModelType = typeof(MediaAltText))]
        public IMediaAltText Video { get; set; }

        [EmbeddedSchemaField(FieldName = "paragraph_quotation", EmbeddedModelType = typeof(RichText))]
        public IRichText Quotation { get; set; }
    }
}
