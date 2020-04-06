using System;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Providers.Content
{
    /// <summary>
    /// Default implementation for IContentByUrlProvider.
    /// Implements the <see cref="Dyndle.Modules.Core.Providers.Content.IContentByUrlProvider" />
    /// </summary>
    /// <seealso cref="Dyndle.Modules.Core.Providers.Content.IContentByUrlProvider" />
    public class DefaultContentByUrlProvider : IContentByUrlProvider
    {
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContentByUrlProvider"/> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="logger">The logger.</param>
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
        /// <param name="urlOrTcmUri">The URL or TCM URI.</param>
        /// <param name="preferredModelType">Type of the preferred model.</param>
        /// <returns>IViewModel.</returns>
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