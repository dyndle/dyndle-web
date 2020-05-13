using System.Collections.Generic;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Personalization.Contracts
{
    /// <summary>
    /// Interface to enable personalization on a page
    /// </summary>
    public interface IPersonalizablePage
    {
        /// <summary>
        /// Regions of the page
        /// </summary>
        List<IRegionModel> Regions { get; set; }
    }
}
