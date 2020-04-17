using System.Collections.Generic;
using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Attributes.ViewModels;

namespace Dyndle.Modules.Core.Models.System

{
    /// <summary>
    /// Class IncludePage.
    /// Used as page models that contains  all shared component that can be rendered using Includes
    /// </summary>
    /// <seealso cref="WebPage" />
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
        [Regions]
        public List<IRegionModel> Regions { get; set; }
    }
}