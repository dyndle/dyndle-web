using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Contracts.Entities;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Models.System;

namespace Dyndle.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// Component Presentations grouped IRegionModels, the same region can occur multiple times (based on cp order)
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Attributes.ComponentPresentationsAttributeBase" />
    public class ComponentPresentationRegions : ComponentPresentationsAttributeBase
    {
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
            var entities = cps.Select(cp =>
            {
                object model = null;
                if (ComplexTypeMapping != null)
                {
                    model = factory.BuildMappedModel(cp, ComplexTypeMapping);
                }
                else model = factory.BuildViewModel((cp));
                return model;
            }).OfType<IEntityModel>();

            entities = GroupByColumnMarker(entities);

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
        /// Groups the by column marker.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>IEnumerable&lt;IEntityModel&gt;.</returns>
        private static IEnumerable<IEntityModel> GroupByColumnMarker(IEnumerable<IEntityModel> entities)
        {
            var resetMarker = false;
            IColumnMarker marker = null;

            foreach (var entity in entities)
            {
                if (resetMarker)
                {
                    marker = null;
                    resetMarker = false;
                }

                if (entity is IColumnMarker columnMarker)
                {
                    if (marker != null && marker.Entities.Any())
                    {
                        resetMarker = true;
                        yield return marker;
                    } 

                    marker = columnMarker;
                }
                else
                {
                    if (marker != null)
                    {
                        marker.Entities.Add(entity);
                    }
                    else
                    {
                        yield return entity;
                    }
                }
            }
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
