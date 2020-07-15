using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

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
            IEnumerable<IPublicationMeta> publicationMeta = PublicationProvider.GetAllPublicationMetadata(excludeCurrent);
            //publicationMeta.Select(p => p.CustomMeta = GlobalizationService.GetCustomPublicationMetadata());
            foreach (var item in publicationMeta)
            {
                item.CustomMeta = GlobalizationService.GetCustomPublicationMetadata();
            }

            return publicationMeta;
        }
    }
}
