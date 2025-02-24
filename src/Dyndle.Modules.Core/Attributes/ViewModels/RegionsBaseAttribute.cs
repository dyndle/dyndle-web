﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// Component Presentations grouped into IRegionModels, the same region can occur multiple times (based on cp order). Tridion 9 regions are used if available, otherwise regions are defined on component template metadata.
    /// </summary>
    public abstract class RegionsBaseAttribute : ModelPropertyAttributeBase
    {
        /// <summary>
        /// Returns a list of regions based on the provided modeldata and property. Will try to use Tridion 9 regions before getting the regions from component presentations.
        /// </summary>
        /// <param name="modelData">The model data.</param>
        /// <param name="property">The property.</param>
        /// <param name="factory">The view model factory.</param>
        /// <returns>IEnumerable.</returns>
        public override IEnumerable GetPropertyValues(IModel modelData, IModelProperty property, IViewModelFactory factory)
        {
            IEnumerable result = null;
            if (modelData is IPage page)
            {
                if (PageUsesTridionRegions(page))
                {
                    return GetRegions(page.Regions, property, factory);
                }
                result = GetRegionsFromComponentPresentations(page.ComponentPresentations, property, factory);
            }
            return result;
        }

        private static bool PageUsesTridionRegions(IPage page)
        {
            return page.Regions?.Any() ?? false;
        }

        /// <summary>
        /// Gets region models based on regions defined in Tridion.
        /// </summary>
        /// <param name="tridionRegions">Tridion regions.</param>
        /// <param name="property">The property.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        /// <returns></returns>
        protected abstract IEnumerable GetRegions(IList<IRegion> tridionRegions, IModelProperty property,
            IViewModelFactory viewModelFactory);

        /// <summary>
        /// Gets the presentation values and optionally groups entities based on a marker 
        /// and then groups them in regions based on the region constrains configured in Tridion
        /// </summary>
        /// <param name="cps">The CPS.</param>
        /// <param name="property">The property.</param>
        /// <param name="viewModelFactory">The factory.</param>
        /// <returns>IEnumerable.</returns>
        protected abstract IEnumerable GetRegionsFromComponentPresentations(IList<IComponentPresentation> cps,
            IModelProperty property, IViewModelFactory viewModelFactory);

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
