using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyndle.Modules.Logging
{
    public class LogAreaRegistration : BaseModuleAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Logging";
            }
        }


        protected override void RegisterTypes(TypeDescriptionList types)
        {
            types.Add(typeof(LogWrapper), typeof(ILogger));
        }
    }
}