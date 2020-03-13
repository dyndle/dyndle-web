using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Personalization.Contracts;
using System;
using System.Linq;
using System.Web;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Personalization.Providers
{
    /// <summary>
    /// Processes page model and removes any entities for which there are target group conditions
    /// which do not match the current visitor segmentation
    /// </summary>
    public class TargetGroupPageEnrichmentProvider : PersonalizedPageEnrichmentProvider
    {
        public TargetGroupPageEnrichmentProvider(ILogger logger, IPersonalizationProvider personalizationProvider) : base(logger, personalizationProvider)
        {
        }

        protected override bool ShowEntity(IEntityModel entity)
        {
            //Default is to show the entity
            bool show = true;
            if (entity is IFilterable)
            {
                var filterableEntity = (IFilterable)entity;
                //Get visitor's segments from the provider
                var segments = this.PersonalizationProvider.GetSegments(new HttpContextWrapper(HttpContext.Current));
                //Get inclusion/exclusion filters for the content
                if (filterableEntity.Filters!=null)
                {
                    var inclusionFilters = filterableEntity.Filters.Where(f => f is SegmentFilter && !((SegmentFilter)f).IsExclusion);
                    var exclusionFilters = filterableEntity.Filters.Where(f => f is SegmentFilter && ((SegmentFilter)f).IsExclusion);
                    if (exclusionFilters != null)
                    {
                        //hide the entity if ANY exclusion filter is found matching the visitors segments
                        foreach (var filter in exclusionFilters)
                        {
                            if (segments.Contains(filter.Title.ToLower()))
                            {
                                show = false;
                                break;
                            }
                        }
                    }
                    if (inclusionFilters != null)
                    {
                        //if there are any inclusion filters, the behaviour is not to show
                        //UNLESS the visitor is in a segment included in the filters
                        show = false;
                        foreach (var filter in inclusionFilters)
                        {
                            if (segments.Contains(filter.Title.ToLower()))
                            {
                                show = true;
                                break;
                            }
                        }
                    }
                }
            }
            return show;
        }
    }
}
