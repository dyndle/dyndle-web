using DD4T.ContentModel.Contracts.Logging;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Providers.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyndle.Modules.Globalization
{
    public class GlobalizationService : IGlobalizationService
    {
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;
        private readonly IViewModelFactory _viewModelFactory;
        public GlobalizationService(IContentProvider contentProvider, ILogger logger, IViewModelFactory viewModelFactory)
        {
            _contentProvider = contentProvider;
            _logger = logger;
            _viewModelFactory = viewModelFactory;
        }
        public Dictionary<string, string> GetCustomPublicationMetadata()
        {
            var page = _contentProvider.GetPage("/system/publication-meta.html");
            if (page == null)
            {
                _logger.Error("PublicationMeta page not found. Is it published? url: {0}".FormatString(""));
                return null;
            }

            if (page.ComponentPresentations.Count == 0)
            {
                _logger.Error("Navigation page has no component presentations. Is it using the correct template? url: {0}".FormatString(""));
                return null;
            }

            return new Dictionary<string, string>();
        }
    }
}
