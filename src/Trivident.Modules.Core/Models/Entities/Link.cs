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
