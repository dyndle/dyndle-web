using System.Collections.Generic;
using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;

namespace Dyndle.Modules.Core.Models.Entities
{
    [ContentModelBySchemaTitle("List of Links", true)]
    public class ListOfLinks : EntityModel, IListOfLinks
    {
        [TextField]
        public string Heading { get; set; }

        [EmbeddedSchemaField(EmbeddedModelType = typeof(RichText))]
        public IRichText Subheading { get; set; }

        [EmbeddedSchemaField(EmbeddedModelType = typeof(LinkImageTitle))]
        public List<ILinkImageTitle> Links { get; set; }

        [EmbeddedSchemaField(FieldName = "call_to_action", EmbeddedModelType = typeof(LinkTitle))]
        public ILinkTitle CallToAction { get; set; }
    }
}