using Dyndle.Modules.Personalization.Contracts;
using System.Collections.Generic;

namespace Dyndle.Modules.Personalization.Model
{
    /// <summary>
    /// Base class for defining personalization data used as a default by the module, 
    /// but will be typically replaced by brand implementations
    /// </summary>
    public class BasePersonalizationData : IPersonalizationData
    {
        /// <summary>
        /// A list of segments
        /// </summary>
        public List<string> Segments { get; set; }
        /// <summary>
        /// If false then segments and other data need to be recaculated by the Personalization Provider
        /// </summary>
        public bool Recalculated { get; set; }
    }
}
