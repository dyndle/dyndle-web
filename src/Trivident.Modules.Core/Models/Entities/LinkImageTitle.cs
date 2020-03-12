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
