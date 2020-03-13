using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyndle.Modules.Core.Models;

namespace Trivident.Modules.Personalization.Contracts
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
