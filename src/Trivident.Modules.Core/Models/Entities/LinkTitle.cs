using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Attributes.ViewModels;
using Trivident.Modules.Core.Contracts.Entities;
using Trivident.Modules.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Models.Entities
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
