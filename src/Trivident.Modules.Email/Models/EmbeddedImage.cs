﻿using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Contracts.Entities;
using Trivident.Modules.Core.Models;

namespace Trivident.Modules.Email.Models
{
    //[ContentModel("EmbedImage", false, ViewModelKeys = new[] { "Email" })]
    public class EmbeddedImage : EntityModel
    {
        [LinkedComponentField]
        public IMedia Image { get; set; }

        [TextField]
        public string AltText { get; set; }
    }
}
