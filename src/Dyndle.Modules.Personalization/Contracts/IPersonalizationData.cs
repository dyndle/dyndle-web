using System.Collections.Generic;

namespace Dyndle.Modules.Personalization.Contracts
{
    /// <summary>
    /// Interface to enable generic manipulation of personalization data
    /// </summary>
    public interface IPersonalizationData
    {
        /// <summary>
        /// If true, then the segments and other data is up to date, if false it requires recalculation by the personalization provider
        /// </summary>
        bool Recalculated { get; set; }

        /// <summary>
        /// Get a list of vistor segments
        /// </summary>
        List<string> Segments { get; set; }
    }
}
