using DD4T.ContentModel.Contracts.Logging;
using Trivident.Modules.Core.Contracts;
using Trivident.Modules.Core.Models;
using Trivident.Modules.Personalization.Contracts;
using System;
using System.Collections.Generic;

namespace Trivident.Modules.Personalization.Providers
{
    /// <summary>
    /// Base class for implementing personalization logic when processing a page
    /// </summary>
    public abstract class PersonalizedPageEnrichmentProvider : IWebPageEnrichmentProvider
    {
        protected readonly ILogger _logger;
        protected readonly IPersonalizationProvider PersonalizationProvider;


        public PersonalizedPageEnrichmentProvider(ILogger logger, IPersonalizationProvider personalizationProvider)
        {
            logger.ThrowIfNull(nameof(logger));
            personalizationProvider.ThrowIfNull(nameof(personalizationProvider));
            _logger = logger;
            PersonalizationProvider = personalizationProvider;
        }

        /// <summary>
        /// Modify the page model according to personalization logic
        /// </summary>
        /// <param name="webPage">The (unprocessed) page model</param>
        public void EnrichWebPage(IWebPage webPage)
        {
            if (webPage is IPersonalizablePage)
            {
                var removed = new List<IRegionModel>();
                _logger.Debug("Page model for page {0} implements IPersonalizablePage - seeing if there is anything to personalize...", webPage.Id);
                var personalizedWebPage = (IPersonalizablePage)webPage;
                foreach (var region in personalizedWebPage.Regions)
                {
                    ProcessRegion(personalizedWebPage, region);
                    if (region.Entities != null && region.Entities.Count == 0)
                    {
                        _logger.Debug("Region {0} is empty post-personalization, removing it from page model", region.Name);
                        removed.Add(region);
                    }
                }
                personalizedWebPage.Regions.RemoveAll(x => removed.Contains(x));
            }
        }

        protected virtual void ProcessRegion(IPersonalizablePage page, IRegionModel region)
        {
            _logger.Debug("Processing entities in region {0} to see if there is anything to personalize...", region.Name);
            var removed = new List<IEntityModel>();
            foreach (var entity in region.Entities)
            {
                if (!ShowEntity(entity))
                {
                    _logger.Debug("Removing Entity {0} from region '{1}' due to personalization logic", entity.Id, region.Name);
                    removed.Add(entity);
                }
            }
            if (removed.Count>0)
            {
                region.Entities.RemoveAll(x => removed.Contains(x));
            }
        }

        protected abstract bool ShowEntity(IEntityModel entity);
    }
}
