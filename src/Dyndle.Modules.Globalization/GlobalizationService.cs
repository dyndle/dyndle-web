using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Cache;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Providers.Content;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Dyndle.Modules.Globalization
{
    public class GlobalizationService : IGlobalizationService
    {
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;
        private readonly ISerializedCacheAgent _cacheAgent;
        private const string PublicationMetaPath = "Globalization.SourceUrl";

        public GlobalizationService(IContentProvider contentProvider, ILogger logger, ISerializedCacheAgent cacheAgent)
        {
            _contentProvider = contentProvider;
            _logger = logger;
            _cacheAgent = cacheAgent;
        }

        public Dictionary<string, string> GetCustomPublicationMetadata(int pubId)
        {
            Dictionary<string, IField> AllPublicationMetadata = null;
            AllPublicationMetadata = _cacheAgent.Load("AllPublicationMetaData") as Dictionary<string, IField>;

            if (AllPublicationMetadata == null)
            {
                var url = ConfigurationManager.AppSettings[PublicationMetaPath];
                var page = _contentProvider.GetPage(url);

                if (page == null)
                {
                    _logger.Error("PublicationMeta page not found. Is it published? url: {0}".FormatString(url));
                    return null;
                }

                if (page.ComponentPresentations.Count == 0)
                {
                    _logger.Error("PublicationMeta page has no component presentations. Is it using the correct template? url: {0}".FormatString(url));
                    return null;
                }

                AllPublicationMetadata = new Dictionary<string, IField>();

                foreach (var field in page.ComponentPresentations.FirstOrDefault().Component.Fields)
                {
                    AllPublicationMetadata.Add(field.Key, field.Value);
                }

                _cacheAgent.Store("AllPublicationMetaData", "Globalization", AllPublicationMetadata);
            }

            var result = new Dictionary<string, string>();
            var publicationMetadata = AllPublicationMetadata.Where(p => p.Key.Contains(pubId.ToString()));
            if (publicationMetadata.Any())
            {
                var embeddedValues = publicationMetadata.FirstOrDefault().Value.EmbeddedValues.FirstOrDefault();
                foreach (var item in embeddedValues)
                {
                    result.Add(item.Key, item.Value.Value);
                }
            }
            return result;
        }
    }
}