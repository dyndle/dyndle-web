using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Contracts.Entities;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Models.System;

namespace Dyndle.Modules.Core.Attributes.ViewModels
{
    /// <inheritdoc />
    public class RegionsAttribute : RegionsBaseAttribute
    {
        /// <inheritdoc />
        protected override IEnumerable GetRegions(IList<IRegion> tridionRegions, IModelProperty property, IViewModelFactory viewModelFactory)
        {
            var regionModels = new List<IRegionModel>();
            foreach (var region in tridionRegions)
            {
                var regionModel = new RegionModel(region)
                {
                    Entities = region.ComponentPresentations
                        .Select(cp => BuildViewModel(viewModelFactory, cp)).OfType<IEntityModel>().ToList()
                };

                regionModels.Add(regionModel);
            }

            return regionModels;
        }

        /// <inheritdoc />
        protected override IEnumerable GetRegionsFromComponentPresentations(IList<IComponentPresentation> cps, IModelProperty property,
            IViewModelFactory viewModelFactory)
        {

            var entities = cps.Select(cp => BuildViewModel(viewModelFactory, cp)).OfType<IEntityModel>();

            var result = new List<RegionModel>();
            foreach (var entity in entities)
            {
                var currentRegion = result.LastOrDefault();
                var constraint = GetRegionConstraints(entity.MvcData);
                if (currentRegion == null || currentRegion.Constraint != constraint)
                {
                    currentRegion = new RegionModel(entity.MvcData, constraint);
                    result.Add(currentRegion);
                }

                currentRegion.Entities.Add(entity);
            }
            return result;
        }
        /// <summary>
        /// Builds a View Model from a component presentation
        /// </summary>
        /// <param name="factory">The view model factory.</param>
        /// <param name="cp">The component presentation.</param>
        /// <returns></returns>
        private object BuildViewModel(IViewModelFactory factory, IComponentPresentation cp)
        {
            object model;
            if (ComplexTypeMapping != null)
            {
                model = factory.BuildMappedModel(cp, ComplexTypeMapping);
            }
            else model = factory.BuildViewModel((cp));

            return model;
        }

        /// <summary>
        /// Gets the region constraints.
        /// </summary>
        /// <param name="mvcData">The MVC data.</param>
        /// <returns>System.String.</returns>
        private string GetRegionConstraints(IMvcData mvcData)
        {
            var routeValues = mvcData.RegionRouteValues ?? new Dictionary<string, string>();

            return string.Join("|", mvcData.Region, mvcData.RegionViewName, string.Join("|", routeValues.Select(k => String.Join(":", k.Key, k.Value))));
        }
    }
}
