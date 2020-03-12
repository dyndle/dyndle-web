using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivident.Modules.Core.Attributes.ViewModels;

namespace Trivident.Modules.Core.Models.System

{
    /// <summary>
    /// Class IncludePage.
    /// Used as page models that contains  all shared component that can be rendered using Includes
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.WebPage" />
    [PageViewModel(TemplateTitle = "Include Page")]
    public class IncludePage : WebPage
    {
        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        [ComponentPresentations]
        public List<IEntityModel> Entities { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        /// <value>The regions.</value>
        [ComponentPresentationRegions]
        public List<IRegionModel> Regions { get; set; }
    }
}