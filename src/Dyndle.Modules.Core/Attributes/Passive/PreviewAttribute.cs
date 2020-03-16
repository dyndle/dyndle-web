using System;

namespace Dyndle.Modules.Core.Attributes.Passive
{
    /// <summary>
    /// Is a passive attribute, used to set metadata on a action used for preview.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PreviewAttribute : Attribute
    {
    }
}