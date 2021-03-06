﻿using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Email.Models
{
    /// <summary>
    /// Rich Content viewmodel
    /// </summary>
    [ContentModel("CallToAction", false, ViewModelKeys = new[] { "Email" })]
    public class CTA : EntityModel
    {
        [TextField]
        public string Heading { get; set; }

        [TextField]
        public string Subheading { get; set; }

        [TextField]
        public string Body { get; set; }

        [EmbeddedSchemaField]
        public EmbeddedImage Image { get; set; }

        [EmbeddedSchemaField]
        public LinkTitle Link { get; set; }
    }
}