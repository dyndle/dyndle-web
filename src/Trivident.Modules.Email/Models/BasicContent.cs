using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Models;

namespace Trivident.Modules.Email.Models
{
    [ContentModel("BasicContent", false, ViewModelKeys = new[] { "Email" })]
    public class BasicContent : EntityModel
    {
        [TextField]
        public string Heading { get; set; }

        [EmbeddedSchemaField]
        public RichText Body { get; set; }

        [EmbeddedSchemaField]
        public EmbeddedImage Image { get; set; }
    }
}