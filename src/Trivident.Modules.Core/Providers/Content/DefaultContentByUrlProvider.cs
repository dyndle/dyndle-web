using DD4T.ContentModel.Contracts.Logging;
using DD4T.Core.Contracts.ViewModels;
using System;

namespace Trivident.Modules.Core.Providers.Content
{
    /// <summary>
    /// default implemenation for IContentByUrlProvider
    /// </summary>
    public class DefaultContentByUrlProvider : IContentByUrlProvider
    {
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// constructor for DefaultContentProviderByUrl
        /// </summary>
        /// <param name="contentProvider"></param>
        /// <param name="logger"></param>
        public DefaultContentByUrlProvider(IContentProvider contentProvider, ILogger logger)
        {
            contentProvider.ThrowIfNull(nameof(contentProvider));
            logger.ThrowIfNull(nameof(logger));

            _contentProvider = contentProvider;
            _logger = logger;
        }

        /// <summary>
        /// Load a Viewmodel based on a URL or TcmUri
        /// </summary>
        /// <param name="urlOrTcmUri"></param>
        /// <returns></returns>
        public virtual IViewModel Retrieve(string urlOrTcmUri, Type preferredModelType = null)
        {
            IViewModel model = null;
            _logger.Debug("About to load page with url: {0}", urlOrTcmUri);
            if (urlOrTcmUri.StartsWith("tcm:"))
            {
                model = _contentProvider.BuildViewModel(new TcmUri(urlOrTcmUri), preferredModelType);
            }
            else
            {
                model = _contentProvider.BuildViewModel(urlOrTcmUri, preferredModelType);
            }

            return model;
        }
    }
}