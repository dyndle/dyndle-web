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
    [ContentModelBySchemaTitle("Media_Alt_Text", true)]
    public class MediaAltText : EntityModel, IMediaAltText
    {
        [TextField(FieldName = "Alt_Text")]
        public string AltText { get; set; }

        [LinkedComponentField(FieldName = "ImageVideo", LinkedComponentTypes = new[] { typeof(Image), typeof(Video), typeof(EclMedia) })]
        public IMedia Media { get; set; }
    }
}