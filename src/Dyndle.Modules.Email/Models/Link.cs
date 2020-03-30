using System;
using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Email.Models
{
    [ContentModelBySchemaTitle("Link", true)]
    public class Link : EntityModel, ILink
    {
        private string _target;

        [TextField(FieldName = "tool_tip")]
        public string Tooltip { get; set; }

        public new string Url
        {
            get { return this.ComponentLink ?? this.ExternalUrl; }
        }

        [TextField]
        public string Target {
            get
            {
                if (String.IsNullOrWhiteSpace(_target)) return "";

                switch (_target.ToLower())
                {
                    case "new tab":
                        return "_blank";
                    case "same tab":
                        return "_self";
                    default:
                        return _target;
                }
            }
            set { _target = value; }
        }

        [ResolvedUrlField(FieldName = "internal_link")]
        public string ComponentLink { get; set; }

        [TextField(FieldName = "external_link")]
        public string ExternalUrl { get; set; }
    }
}
