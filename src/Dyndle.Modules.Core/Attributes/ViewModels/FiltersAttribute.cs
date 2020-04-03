using System;
using System.Collections;
using System.Collections.Generic;
using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// Used to add more target group data to the entities 
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Attributes.ModelPropertyAttributeBase" />
    public class FiltersAttribute : ModelPropertyAttributeBase
    {
        /// <summary>
        /// Get Filters set on a CP (for now just Target Groups, but could be other logic here later)
        /// not using the RenderDataAttribute within DD4T.Mvc; because that class has a hidden dependency on DD4TConfiguration.
        /// </summary>
        /// <param name="modelData">The model data.</param>
        /// <param name="property">The property.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>IEnumerable.</returns>
        public override IEnumerable GetPropertyValues(IModel modelData, IModelProperty property, IViewModelFactory factory)
        {
            if (modelData is IComponentPresentation cp)
            {
                if (cp.TargetGroupConditions != null)
                {
                    var filters = GetTargetGroupFilters(cp.TargetGroupConditions);
                    if (filters!=null && filters.Count>0)
                    {
                        return filters;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the expected type of the return.
        /// </summary>
        /// <value>The expected type of the return.</value>
        public override Type ExpectedReturnType
        {
            get
            {
                return typeof(List<IFilter>);
            }
        }

        private List<IFilter> GetTargetGroupFilters(IList<ITargetGroupCondition> conditions)
        {
            var filters = new List<IFilter>();
            foreach (var condition in conditions)
            {
                filters.Add(new SegmentFilter() { IsExclusion = condition.Negate, Id = condition.TargetGroup.Id, Title = condition.TargetGroup.Title });
            }
            return filters;
        }
    }
}