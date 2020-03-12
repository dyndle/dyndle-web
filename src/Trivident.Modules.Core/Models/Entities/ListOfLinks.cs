using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Attributes.ViewModels;
using Trivident.Modules.Core.Contracts.Entities;
using Trivident.Modules.Core.Models;
using System.Collections.Generic;
using System.Web;

namespace Trivident.Modules.Core.Models.Entities
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