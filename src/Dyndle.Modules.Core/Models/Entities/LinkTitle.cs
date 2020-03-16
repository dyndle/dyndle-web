using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;

namespace Dyndle.Modules.Core.Models.Entities
{
    [ContentModelBySchemaTitle("LinkTitle", false)]
    public class LinkTitle : EntityModel, ILinkTitle
    {
        [TextField(FieldName = "link_text")]
        public string LinkText { get; set; }

        [EmbeddedSchemaField(EmbeddedModelType = typeof(Link))]
        public ILink Link { get; set; }
    }
}
