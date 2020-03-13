using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Models.System;

namespace Dyndle.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// Component Presentations grouped IRegionModels, the same region can occur multiple times (based on cp order)
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Attributes.ComponentPresentationsAttributeBase" />
    public class RegionByEntityViewsAttribute : ComponentPresentationsAttributeBase
    {
        private string[] _viewNames;
        public RegionByEntityViewsAttribute(string[] viewNames)
        {
            _viewNames = viewNames;
        }
        /// <summary>
        /// Gets the presentation values and optionally groups entities based on a marker 
        /// and then groups them in regions based on the region constrains configured in Tridion
        /// </summary>
        /// <param name="cps">The CPS.</param>
        /// <param name="property">The property.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="contextModel">The context model.</param>
        /// <returns>IEnumerable.</returns>
        public override IEnumerable GetPresentationValues(IList<IComponentPresentation> cps, IModelProperty property, IViewModelFactory factory, IContextModel contextModel)
        {
            var selection = cps.Where(cp => IsMatch(cp));
            var entities = selection.Select(cp =>
            {
                object model = null;
                if (ComplexTypeMapping != null)
                {
                    model = factory.BuildMappedModel(cp, ComplexTypeMapping);
                }
                else model = factory.BuildViewModel((cp));
                return model;
            }).OfType<IEntityModel>();

            // TODO: make this configurable
            var regionModel = new RegionModel(new MvcData() { Region = "Region", RegionViewName = "Region" }, string.Empty);
            regionModel.Entities = entities.ToList();
            return new List<IRegionModel>() { regionModel };
        }

      /// <summary>
      /// If true, include all component presentations with a view name that starts with the specified string. If false, only include if view name is an exact match.
      /// </summary>
        public bool AllowPartialMatch
        {
            get; set;
        }

        private bool IsMatch(IComponentPresentation cp)
        {
            string viewToMatch = CustomRenderDataAttribute.GetMvcData(cp).View.ToLower();
            if (AllowPartialMatch)
            {
                return _viewNames.Any(v => viewToMatch.StartsWith(v.ToLower()));
            }
            return _viewNames.Any(v => viewToMatch.Equals(v, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the expected type of the return.
        /// </summary>
        /// <value>The expected type of the return.</value>
        public override Type ExpectedReturnType
        {
            get { return typeof(IList<IRegionModel>); }
        }
    }
}
