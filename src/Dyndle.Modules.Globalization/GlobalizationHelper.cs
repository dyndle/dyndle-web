using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Dyndle.Modules.Globalization
{
    /// <summary>
    /// Adds HtmlHelper methods for retrieving publication metadata.
    /// </summary>
    public static class GlobalizationHelper
    {
        private static readonly IPublicationProvider PublicationProvider = DependencyResolver.Current.GetService<IPublicationProvider>();
        private static IGlobalizationService GlobalizationService = DependencyResolver.Current.GetService<IGlobalizationService>();

        public static IEnumerable<IPublicationMeta> Publications(this HtmlHelper htmlHelper, bool excludeCurrent = true)
        {
            var publicationMeta = PublicationProvider.GetAllPublicationMetadata(excludeCurrent);
            foreach (var item in publicationMeta)
            {
                item.CustomMeta = GlobalizationService.GetCustomPublicationMetadata(item.Id);
            }

            return publicationMeta;
        }
    }
}