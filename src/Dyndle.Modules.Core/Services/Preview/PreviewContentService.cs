using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Factories;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Core.Services.Preview
{
    /// <summary>
    /// Create Viewmodel based on json data.
    /// </summary>
    public class PreviewContentService : IPreviewContentService
    {
        private readonly IPageFactory _pageFactory;
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="DyndleOutputCacheAttribute"/> class.
        /// </summary>
        /// <param name="pageFactory">The Page Factory</param>
        /// <param name="contentProvider">The Content Provider</param>
        /// <param name="logger">The Logger</param>
        public PreviewContentService(IPageFactory pageFactory, IContentProvider contentProvider, ILogger logger)
        {
            pageFactory.ThrowIfNull(nameof(pageFactory));
            contentProvider.ThrowIfNull(nameof(contentProvider));
            logger.ThrowIfNull(nameof(logger));

            _pageFactory = pageFactory;
            _contentProvider = contentProvider;
            _logger = logger;
        }

        /// <summary>
        /// use _pageFactory to deserialize data into a IViewModel.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IWebPage GetPage(string data)
        {
            _logger.Debug("previewing page based on JSON of length " + data.Length);
            var page = _pageFactory.GetIPageObject(data);
            _logger.Debug($"found page with URI {page.Id}");
            var model = _contentProvider.BuildViewModel(page) as IWebPage;
            return model;
        }
    }
}