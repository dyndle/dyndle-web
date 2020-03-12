using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trivident.Modules.Core.Attributes.Passive
{
    /// <summary>
    /// Is a passive attribute, used to set metadata on a action used for preview.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PreviewAttribute : Attribute
    {
    }
}